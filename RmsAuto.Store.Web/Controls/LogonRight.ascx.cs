using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;

using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Controls
{
    public partial class LogonRight : LogonBaseControl
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            ProceedLogon(txtLogin.Value, txtPassword.Value, false /*chkPersist.Checked*/);
        }

        public override string ErrorMessage
        {
            get { return errMessage.Text; }
            set { errMessage.Text = value; }
        }

		//protected string CurrentClientName
		//{
		//	get 
		//	{
		//		try
		//		{
		//			var ctx = SiteContext.Current;
		//			if( ctx.User.Role == RmsAuto.Store.Entities.SecurityRole.Client )
		//			{
		//				return ctx.UserDisplayName;
		//			}
		//			else
		//			{
		//				return ctx.UserDisplayName + " :: " + ctx.CurrentClient.Profile.ClientName;
		//			}
		//		}
		//		catch
		//		{
		//			//Если возникли проблемы с подгрузкой профиля из Хансы, то хотя бы логин покажем :)
		//			return SiteContext.Current.User.Identity.Name;
		//		}
		//	}
		//}

		protected string CurrentClientID
		{
			get
			{
				try
				{
					return "№ Клиента " + SiteContext.Current.CurrentClient.Profile.ClientId;
				}
				catch
				{
					return "№ Клиента " + SiteContext.Current.User.AcctgID;
				}
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			DataBind();
            base.OnPreRender(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
			
			try
			{
				if (SiteContext.Current.InternalFranchName == "rmsauto")
				{
			
				}
			}
			catch { }
			
            //_btnViewCabinetLink.NavigateUrl = UrlManager.GetPrivateOfficeUrl();
        }

    }
}