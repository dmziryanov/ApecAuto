using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Controls
{
    public abstract class LogonBaseControl : System.Web.UI.UserControl
    {
        public abstract string ErrorMessage { get; set; }

        public virtual void ProceedLogon(string login, string password, bool shouldRemember)
        {
            string errCode;
			User user = LogonService.Logon(login, password, shouldRemember, out errCode);
			if (user != null)
			{
				if (user.Role == SecurityRole.Manager)
				{
					Response.Redirect(RmsAuto.Store.Web.Manager.Default.GetUrl(), true);
				}
				else
				{
					if (Request.QueryString["ReturnUrl"] != null)
					{
						var url = new Uri(
							Request.Url.GetLeftPart(UriPartial.Authority) +
							FormsAuthentication.GetRedirectUrl(login, shouldRemember));

						var queryArgs = HttpUtility.ParseQueryString(url.Query);
						FillExtraQueryArgs(queryArgs);

						string redirectUrl = url.AbsolutePath;
						if (queryArgs.Count > 0)
							redirectUrl += "?" + queryArgs.ToWwwQueryString();

						Response.Redirect(redirectUrl, true);
					}
					else
					{
						Response.Redirect(UrlManager.GetOrdersUrl(), true);
					}
				}
			}
			else
			{
				string msg = string.Empty;
				switch (errCode)
				{
					case "LogonUserIsNotActivated": msg = Resources.ValidatorsMessages.LogonUserIsNotActivated;
						break;
					case "LogonWrongLoginOrPassword": msg = Resources.ValidatorsMessages.LogonWrongLoginOrPassword;
						break;
					default: msg = errCode;
						break;
				}
				ErrorMessage = msg;
				if (!Page.ClientScript.IsClientScriptBlockRegistered("LoginOpen"))
					Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoginOpen", "$(function(){$('div.modal').show();});", true);

			}
        }

        protected void FillExtraQueryArgs(NameValueCollection queryArgs)
        {
            foreach (string key in Request.QueryString.AllKeys)
                if (key != "ReturnUrl")
                    queryArgs.Add(key, Request.QueryString[key]);
        }

        public virtual void OnLogOff(object sender, EventArgs e)
        {
            LogonService.Logoff();
        }
    }
}
