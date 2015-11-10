using System;
using System.Web;
using System.Web.Caching;
using System.Configuration;
using System.Web.Configuration;
using RmsAuto.Store.Cms.Routing;
using System.Net;
using System.Web.UI;
using System.Text;

namespace RmsAuto.Store.Web
{

   //TODO: класс для хранения времени начала отсчета и окончания бана
    //Возможно это переделать используя чисто таймаут кеша
    public class TimedCounter
    {
        public int Count = 0;
        public DateTime d = DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["LimitCacheDuration"]));
        public DateTime BanTime = DateTime.MinValue;
    }
    
    public class RequestLimit : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.AuthorizeRequest += new EventHandler(onAuthorizeRequest);

        }

        #endregion

        //Таймаут кеша не работает если класть туда значения на события OnBeginRequest
        public void onAuthorizeRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            string filePath = context.Request.FilePath;
            string fileExtension = VirtualPathUtility.GetExtension(filePath);
            CustomErrorsSection ErrorPages = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");
            //ErrorPages.Errors.


            bool ServicePage = false;
            foreach (CustomError s in ErrorPages.Errors)
            {
               ServicePage = ServicePage ||  s.Redirect.Contains(context.Request.FilePath);
            }


            if (fileExtension.Equals(".aspx") && !ServicePage)
            {
                   
                string CacheKey = "";
                

                if (!context.Request.IsAuthenticated)
                {

                    CacheKey = context.Request.UserHostAddress;
                }
                else
                {
                    var user = context.User as CustomPrincipal;
                    var csc = new ClientSiteContext(context, user);
                    CacheKey = context.Request.UserHostAddress + csc.User.AcctgID;
                }
                
                if (context.Cache.Get(CacheKey) == null)
                {
                    context.Cache.Insert(CacheKey, new TimedCounter());
                }
                else
                {

                    var tmptc = ((TimedCounter)context.Cache[CacheKey]);
                    tmptc.Count++;

                    if (DateTime.Now > tmptc.d && DateTime.Now > tmptc.BanTime) { tmptc = new TimedCounter(); };
                    context.Cache[CacheKey] = tmptc;

                    if (tmptc.Count >= Convert.ToInt32(ConfigurationManager.AppSettings["LimitCount"]) && DateTime.Now > tmptc.BanTime)
                    {
                        tmptc.BanTime = DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["BanDuration"]));
                    }

                    if (DateTime.Now < tmptc.BanTime )
                    {

                        byte[] TextPage = null;
                        string txt;
                        if (context.Cache.Get("TextPage") == null)
                        {
                            WebClient client = new WebClient();
                            client.Encoding = context.Request.ContentEncoding;
                            TextPage = client.DownloadData(UrlManager.MakeAbsoluteUrl("/LimitCountWarning.aspx"));
                            txt = System.Text.UTF8Encoding.UTF8.GetString(TextPage);
                            context.Cache.Insert("TextPage", txt, null, DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["CacheDuration"])), new TimeSpan(0, 0, 0));
                        }
                        
                        else
                        {
                            txt = (string)context.Cache["TextPage"];
                        }

                        //if (context.Request.Url.AbsolutePath != UrlManager.MakeAbsoluteUrl("/"))
                        //{
                        //    context.Response.Redirect(UrlManager.MakeAbsoluteUrl("/"));
                        //}
                        context.Response.ContentEncoding = Encoding.UTF8;
                        context.Response.Write(txt);
                        context.Response.End();
                    }
                }
            }
        }
     }
}
