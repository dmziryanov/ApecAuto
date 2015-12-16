using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.Web.Security;
using Laximo.Guayaquil.Data;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web.LaximoCatalogs;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Castle.Core.Resource;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Inspinia_MVC5;


namespace RmsAuto.Store.Web
{
    public class Global : System.Web.HttpApplication, IContainerAccessor
	{
          private static IWindsorContainer _container;
        public IWindsorContainer Container
        {
            get { return _container; }
        }

        public Global()
        {

        }

		#region === Пароль и путь к сертификату для каталогов Laximo ===
		private static string CertPath
        {
            get
            {
                string certPath = ConfigurationManager.AppSettings["CertPath"];
				if (string.IsNullOrEmpty(certPath))
				{
					throw new ArgumentNullException("certPath");
				}
                if (!Path.IsPathRooted(certPath))
                {
                    certPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, certPath);
                }
				if (!File.Exists(certPath))
				{
					throw new ArgumentException(string.Format("File '{0}' not found", certPath));
				}
                return certPath;
            }
        }

        private static string CertPwd
        {
            get
            {
                string certPwd = ConfigurationManager.AppSettings["CertPwd"];
				if (string.IsNullOrEmpty(certPwd))
				{
					throw new ArgumentNullException("certPwd");
				}
                return certPwd;
            }
        }
        #endregion

        protected void Application_Start(object sender, EventArgs e)
        {
            var filename = "assembly://Rmsauto.Store.Web/windsor.config.xml";
            IResource resource = new AssemblyResource(filename);
            _container = new WindsorContainer(new XmlInterpreter(resource));
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //add all of the controllers to the container
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeof(IController).IsAssignableFrom(type))
                {
                    _container.Register(Component.For(type).LifestylePerWebRequest());
                }
            }

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory());

            
            UrlManager.RegisterRoutes(RouteTable.Routes);
            new GlobalDBSettings.GlobalSettings().Init();

			#region === Каталогов Laximo на   пока не будет ===
			//try
			//{
			//    StringBuilder sb = new StringBuilder();
			//    sb.Append("Подгружаем каталоги Laximo." + Environment.NewLine);
			//    sb.Append("Путь к сертификату: " + CertPath + Environment.NewLine);
			//    sb.Append("Пароль к сертификату: " + CertPwd + Environment.NewLine);
			//    Logger.WriteInformation(sb.ToString(), EventLogerID.AdditionalLogic, EventLogerCategory.UnknownCategory);
			//}
			//catch { }
			//try
			//{
			//    Application["CatalogProvider"] = new CatalogProvider(CertPath, CertPwd, new CatalogCache());
			//}
			//catch (Exception ex)
			//{
			//    StringBuilder sb = new StringBuilder();
			//    sb.Append("При загрузке каталогов Laximo, произошло исключение!" + Environment.NewLine);
			//    sb.Append("----------------------------------------------------" + Environment.NewLine);
			//    sb.Append("exception message: " + Environment.NewLine);
			//    sb.Append(ex.Message);
			//    sb.Append("----------------------------------------------------" + Environment.NewLine);
			//    sb.Append("exception stacktrace: " + Environment.NewLine);
			//    sb.Append(ex.StackTrace);
			//    Logger.WriteError(sb.ToString(), EventLogerID.UnknownError, EventLogerCategory.UnknownCategory);
			//}
			#endregion
		}

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
/*            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.#1#)?favicon.ico(/.*)?" });

            routes.Add(new Route("{controller}/{action}/{id}", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { action = "Index", id = "" }),
            });*/


            routes.Add(new Route("MessageController/{action}/{id}", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { action = "SellerMessage", id = "" }),
            });


            routes.Add(new Route("Message.aspx", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { controller = "Message", action = "SellerMessage", id = "" }),
            });

            routes.Add(new Route("SendMessage.aspx", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { controller = "Message", action = "Send", id = "" }),
            });

            routes.Add(new Route("CloseMessage.aspx", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { controller = "Message", action = "CloseMessage", id = "" }),
            });
        }

        Regex regex = new Regex("^[A-Za-z0-9-]*(\\.[A-Za-z]{2,})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!Request.Url.Host.StartsWith("www.") && regex.IsMatch(Request.Url.Host) && !Request.Url.IsLoopback)
            {
                var url = new UriBuilder(Request.Url);
                url.Host = "www." + Request.Url.Host;
                Response.StatusCode = 301;
                Response.AddHeader("Location", url.ToString());
                Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Logger.WriteException( Context.Error );
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
			CatalogProvider catalogProvider = (CatalogProvider)Application["CatalogProvider"];
			if (catalogProvider != null)
				catalogProvider.Dispose();
        }

        protected void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs args)
        {
            RmsAuto.Store.Web.LogonService.FormsAuthentication_OnAuthenticate(sender, args);
        }
    }
}