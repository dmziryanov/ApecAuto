using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using RmsAuto.Common.Web;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
	public class SiteContextModule : IHttpModule
	{
		#region IHttpModule Members

		public void Init(HttpApplication context)
		{
			context.BeginRequest += OnEnter;
			context.AcquireRequestState += OnEnter;
		}

		public void Dispose()
		{
		}

		#endregion


		//Нужно проверять куку на наличие в справочнике, иначе выпадет все
		private void OnEnter(object source, EventArgs eventArgs)
		{
			var context = ((HttpApplication)source).Context;

			//Если пришли по активационной ссылке, то меняем регион на  тот который в ней пришел посредством куки. Возможно в дальнейшем это стоит делать более гибко
			if (context.Request.Url.OriginalString.Contains("Activation"))
			{
				HttpCookie coockie;

				if
					(HttpContext.Current.Request.Cookies["InternalFranchName"] == null)
				{ coockie = new HttpCookie("InternalFranchName"); }
				else
				{ coockie = HttpContext.Current.Request.Cookies["InternalFranchName"]; }

				coockie.Value = (string)context.Request.QueryString[UrlKeys.Activation.FranchCode];

				
				//string franchKey = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.ToString()))[UrlKeys.Activation.FranchCode].ToString();
                //Получаем ключ франча из QueryString, если нет - берем из конфига
                string franchKey = string.IsNullOrEmpty(context.Request.QueryString[UrlKeys.Activation.FranchCode]) ? ConfigurationManager.AppSettings["InternalFranchName"] : context.Request.QueryString[UrlKeys.Activation.FranchCode];

				coockie.Value = franchKey;
				coockie.Domain = "rmsauto.ru";
				coockie.Path = "/";

				HttpContext.Current.Request.Cookies.Add(coockie);
				HttpContext.Current.Response.Cookies.Add(coockie);


				if (HttpContext.Current.Request.Cookies["cityName"] == null)
				{ coockie = new HttpCookie("cityName"); }
				else
				{ coockie = HttpContext.Current.Request.Cookies["cityName"]; }


				IEnumerable<City> cities;
				using (var dcCommon = new RmsAuto.Store.Entities.dcCommonDataContext())
				{
					//Извлекаем наборы данных в списки, так как LINQ to SQL не дает выполнять запросы к различным контекстам
					//TODO: сделать AcctgRefCatalog.Cities, вынести в справочник, чтобы не лезть в базу каждый раз
					cities = dcCommon.Cities.Select(x => x).ToList();
				}
				//var regionId = AcctgRefCatalog.RmsFranches[(string)context.Request.QueryString[UrlKeys.Activation.FranchCode]].RegionID;


				if (context.Handler is System.Web.SessionState.IRequiresSessionState && context.Session != null)
				{

					if ((string)context.Session["InternalFranchName"] != franchKey)
					{ context.Session["InternalFranchName"] = franchKey; }
				}

				var regionId = AcctgRefCatalog.RmsFranches[franchKey].RegionID;
				coockie.Value = HttpUtility.UrlEncodeUnicode(cities.Where(x => x.CityID == regionId).Select(x => x.Name).FirstOrDefault());
				coockie.Domain = "rmsauto.ru";
				coockie.Path = "/";

				HttpContext.Current.Request.Cookies.Add(coockie);
				HttpContext.Current.Response.Cookies.Add(coockie);
			}

			//проверка того, что регион заданный в куке, отсутвует в списке регионов
			if (HttpContext.Current.Request.Cookies["InternalFranchName"] != null)
			{

				var InternalFranchName = (string)HttpContext.Current.Request.Cookies["InternalFranchName"].Value;
				if (AcctgRefCatalog.RmsFranches[InternalFranchName] == null)
				{
					HttpContext.Current.Request.Cookies.Remove("InternalFranchName");
					HttpContext.Current.Request.Cookies.Remove("сityName");

					var coockie = new HttpCookie("InternalFranchName");

					coockie.Value = "rmsauto";
					coockie.Domain = "rmsauto.ru";
					coockie.Path = "/";

					HttpContext.Current.Response.Cookies.Add(coockie);
					HttpContext.Current.Request.Cookies.Add(coockie);



					coockie = new HttpCookie("cityName");
					coockie.Value = "Москва";
					coockie.Domain = "rmsauto.ru";
					coockie.Path = "/";

					if (context.Handler is System.Web.SessionState.IRequiresSessionState && context.Session != null)
					{
						{ context.Session["InternalFranchName"] = "rmsauto"; }
					}

					HttpContext.Current.Response.Cookies.Add(coockie);
					HttpContext.Current.Request.Cookies.Add(coockie);
				}
			}

			context = ((HttpApplication)source).Context;
			SiteContext.AttachFranchName(context);

			if (context.Handler is System.Web.SessionState.IRequiresSessionState && context.Session != null)
			{
				SiteContext.Attach(context);
				if (context.Session["InternalFranchName"] == null)
				{ context.Session["InternalFranchName"] = SiteContext.Current.InternalFranchName; }
			}
		}

		//Пока не надо
		private void CheckInternalFranchName(object source, EventArgs eventArgs)
		{
			var context = ((HttpApplication)source).Context;

			SiteContext.AttachFranchName(context);

			context.Handler.ProcessRequest(context);

			if (context.Handler is System.Web.SessionState.IRequiresSessionState && context.Session != null)
				SiteContext.Attach(context);
		}
	}
}
