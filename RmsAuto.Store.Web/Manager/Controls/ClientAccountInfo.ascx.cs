using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class ClientAccountInfo : System.Web.UI.UserControl
	{
		public ClientData Data
		{
			get;
			set;
		}
		protected void Page_Load( object sender, EventArgs e )
		{
			if( Data != null && !Data.IsGuest )
			{
				_statusLabel.Text = Data.Status.ToTextOrName();
				if( Data.Status == ClientStatus.Offline )
				{
					_offline1Row.Visible = true;
					_offline2Row.Visible = true;
					_loginRow.Visible = false;
				}
				else 
				{
					_offline1Row.Visible = false;
					_offline2Row.Visible = false;
					_loginRow.Visible = true;
					User user = UserDac.GetUserByClientId( Data.Profile.ClientId );
					_loginLabel.Text = Server.HtmlEncode( user.Username );
				}
			}
		}

		protected void _createAccountButton_Click( object sender, EventArgs e )
		{
			try
			{
                ClientBO.OfferOnlineAccess(
                    Data.Profile.ClientId, 
                    _emailBox.Text,
                    activationCode => UrlManager.GetClientActivationUrl(activationCode, SiteContext.Current.InternalFranchName));
				((ManagerSiteContext)SiteContext.Current).ClientSet.RefreshClientInfo(Data.Profile.ClientId);
				ShowMessage("Invitation is successfully sent!");
			}
			catch( Exception ex )
			{
				_errorLabel.Text = ex.Message;
			}
		}

		private void ShowMessage(string message)
		{
			Page.ClientScript.RegisterStartupScript(
				this.GetType(),
				"__messageBox",
				"<script type='text/javascript'>alert('" + message + "');</script>");
		}
	}
}