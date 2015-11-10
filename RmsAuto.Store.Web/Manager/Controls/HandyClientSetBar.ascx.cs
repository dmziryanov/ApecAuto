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
using RmsAuto.Store.Web;
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Web.Manager.Controls
{
    public partial class HandyClientSetBar : System.Web.UI.UserControl
    {
        public enum CartDisplay
        {
            Full, Empty, Disabled
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		protected void Page_PreRender( object sender, EventArgs e )
		{
			BindClientList(); 
		}
        protected void _rptHandyClients_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var clientId = (string)e.CommandArgument;

            switch (e.CommandName)
            {
                case "Manage":
					( (ManagerSiteContext)SiteContext.Current ).ClientSet.SetDefaultClient( clientId );
					Response.Redirect( ClientOrders.GetUrl(), true );
                    break;
                case "Remove":
                    var redirect = SiteContext.GetCurrent<ManagerSiteContext>()
                        .ClientSet.IsClientDefault(clientId);
                    SiteContext.GetCurrent<ManagerSiteContext>().ClientSet.RemoveClient(clientId);
                    if (redirect)
                        Response.Redirect( Default.GetUrl(), true );
                    break;
                case "Cart":
                    ((ManagerSiteContext)SiteContext.Current).ClientSet.SetDefaultClient(clientId);
                    Response.Redirect(ClientCart.GetUrl(), true);
                    break;
            }
        }

		protected CartDisplay GetCartDisplay(ClientData clientData)
		{
            if (ClientBO.CanUseClientCart(
                SiteContext.GetCurrent<ManagerSiteContext>().ManagerInfo,
                clientData.Profile))
            {
                return clientData.Cart.GetTotals().PartsCount == 0 ? CartDisplay.Empty : CartDisplay.Full;
            }
            return CartDisplay.Disabled;
		}

		protected bool IsClientOnline( ClientData clientData )
		{
			return clientData.Status == RmsAuto.Store.Entities.ClientStatus.Online;
		}
        
		private void BindClientList()
        {
			_rptHandyClients.DataSource = ( (ManagerSiteContext)SiteContext.Current ).ClientSet.Clients.OrderBy( c => c.Profile.ClientName );
            _rptHandyClients.DataBind();
        }

    }
}