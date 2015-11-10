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
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class CartPrint : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		//public static string GetUrl( bool showPartNumbers )
		//{
		//    return string.Format( "~/Manager/ClientCartPrint.aspx?client_id={0}&show_pn={1}",
		//        HttpContext.Current.Server.UrlEncode( clientId ),
		//        showPartNumbers ? "1" : "0" );
		//}

		public bool ShowPartNumbers
		{
            get { return Page.User.Identity.IsAuthenticated; }
		}

		public string Phone
		{
			get
			{
				return AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Phone;
			}
		}

		protected ShoppingCart Cart;

		protected void Page_PreRender( object sender, EventArgs e )
		{
			Response.Cache.SetCacheability( HttpCacheability.NoCache );

			var client = SiteContext.Current.CurrentClient;
			_clientNameLabel.Text = !client.IsGuest ? client.Profile.ClientName : "";
			Cart = client.Cart;
            _totalSumLiteral.Text = Cart.GetTotals().Total.ToString();
            _totalCountLiteral.Text = Cart.GetTotals().ItemsCount.ToString();

            _cartItemsRepeater.DataSource = Cart.GetItems();
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
