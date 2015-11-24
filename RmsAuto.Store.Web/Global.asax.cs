using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.Web.Security;
using Laximo.Guayaquil.Data;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web.LaximoCatalogs;
using System.Web;

namespace RmsAuto.Store.Web
{
    public class Global : System.Web.HttpApplication
	{
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