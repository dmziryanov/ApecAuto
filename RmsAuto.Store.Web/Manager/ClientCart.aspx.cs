using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;
using System.Net;

namespace RmsAuto.Store.Web.Manager
{
	
    [Serializable()]
    public partial class ClientCart : ClientBoundPage
	{
		public static string GetUrl()
		{
			return "~/Manager/ClientCart.aspx";
		}

        public override ClientDataSection DataSection
        {
            get { return ClientDataSection.Cart; }
        }
        
		protected void Page_Load( object sender, EventArgs e )
		{
			if( !ClientBO.CanUseClientCart( ( (ManagerSiteContext)SiteContext.Current).ManagerInfo, ClientData.Profile))
			{
				throw new HttpException( (int)HttpStatusCode.Forbidden, "Нет доступа" );
			}

			if (!Page.IsPostBack)
                ResetPlaceOrderVisibility();

            ClientData.Cart.ContentChanged += new EventHandler<EventArgs>(Cart_ContentChanged);

            _printWithPartNumbers.NavigateUrl = ClientCartPrint.GetUrl(ClientData.Profile.ClientId, true);
            _printWithoutPartNumbers.NavigateUrl = ClientCartPrint.GetUrl(ClientData.Profile.ClientId, false);
		}

        protected void Page_Unload(object sender, EventArgs e)
        {
            ClientData.Cart.ContentChanged -= new EventHandler<EventArgs>(Cart_ContentChanged);
        }

        void Cart_ContentChanged(object sender, EventArgs e)
        {
            ResetPlaceOrderVisibility();
        }

        protected void _btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    var storeNumber =
                    AcctgRefCatalog.RmsStores[SiteContext.Current.CurrentClient.Profile.RmsStoreId]
                    .StoreNumber;

                    var orderNo = ClientData.Cart.PlaceOrder(
                        _orderOptions.PaymentMethod,
                        _orderOptions.ShippingAddress,
                        null,
						null,
                        SiteContext.Current.User.AcctgID,
                        storeNumber);
                    Response.Redirect(OrderConfirmation.GetUrl() + "?orderNo=" + orderNo);
                }
                catch (BLException ex)
                {
                    _errorLabel.Text = "Ошибка размещения заказа: " + ex.Message;
                }
            }
        }

        private void ResetPlaceOrderVisibility()
        {
            _placeOrderPanel.Visible = ShoppingCartItems1.CartItemsCount > 0 && !ShoppingCartItems1.ContainsBadItems;
        }
	}
}
