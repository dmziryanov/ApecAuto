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

using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using RmsAuto.Store.Web.Manager.BasePages;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Manager
{
	public partial class ClientCartPrint : RMMPage
	{
        public string Phone
        {
            get
            {
                return AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Phone;
            }
        }

		public static string GetUrl( string clientId, bool showPartNumbers )
		{
			return string.Format( "~/Manager/ClientCartPrint.aspx?client_id={0}&show_pn={1}",
				HttpContext.Current.Server.UrlEncode( clientId ),
				showPartNumbers ? "1" : "0" );
		}

		public string ClientId
		{
			get { return Request[ "client_id" ]; }
		}
		public bool ShowPartNumbers
		{
			get { return Request[ "show_pn" ] == "1"; }
		}

		protected ShoppingCartTotals Totals;

		protected void Page_PreRender( object sender, EventArgs e )
		{
			ManagerSiteContext ctx = (ManagerSiteContext)SiteContext.Current;
			var client = ctx.ClientSet[ ClientId ];
			_clientNameLabel.Text = client.Profile.ClientName;

			Totals = client.Cart.GetTotals();

            _cartItemsRepeater.DataSource = client.Cart.GetItems();
            _cartItemsRepeater.DataBind();
        }

        protected void _cartItemsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
               e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var cartItem = (ShoppingCartItem)e.Item.DataItem;

                string warning = null;
                if (cartItem.Discontinued)
                {
                    warning = "Good is not available.<br/>For successful validation necessary to remove good from the cart";
                }
                else
                {
                    if (cartItem.QtyBelowMinimum)
                        warning += String.Format(
                            "Minimal order quantity: {0}",
                            cartItem.SparePart.MinOrderQty);

                    if (cartItem.PriceChanged)
                        warning += "<br/>Price on this details is changed";
                }
                ((Label)e.Item.FindControl("_lblWarning")).Text = warning;
            }
        
		}
	}
}
