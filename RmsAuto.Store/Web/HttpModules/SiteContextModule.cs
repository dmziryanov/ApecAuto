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
			
			}

			//проверка того, что регион заданный в куке, отсутвует в списке регионов
			if (HttpContext.Current.Request.Cookies["InternalFranchName"] != null)
			{


            }


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
