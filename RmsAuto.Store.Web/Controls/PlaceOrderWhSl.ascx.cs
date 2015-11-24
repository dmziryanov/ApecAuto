using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Web;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Web.Controls
{
    public partial class PlaceOrderWhSl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if( !Page.IsPostBack )
            {
                _shoppingCartItems.DataSource = SiteContext.Current.CurrentClient.Cart.GetItems().Where( t => t.AddToOrder == true );
                _shoppingCartItems.DataBind();
                _txtCustOrderNum.Text = Convert.ToString(Context.Profile["CustOrderNum"]);
            }
        }

		protected void Page_PreRender( object sender, EventArgs e )
		{
			//доверенность показывать только юр.лицам
			_powerOfAttorneyBlock.Visible = SiteContext.Current.CurrentClient.Profile.IsLegalWholesale;
		}

        protected void _placeOrderWizard_NextButtonClick( object sender, WizardNavigationEventArgs e )
        {
            try
            {
                var store = AcctgRefCatalog.RmsStores[SiteContext.Current.CurrentClient.Profile.RmsStoreId];
                if ( store == null )
                    throw new Exception( string.Format( "Client profile error: unknown RmsStoreId='{0}'", SiteContext.Current.CurrentClient.Profile.RmsStoreId ) );

				string custOrderNum = _txtCustOrderNum.Text;
				var orderNo = SiteContext.Current.CurrentClient.Cart.PlaceOrder(
                    PaymentMethod.Clearing,
                    "Pickup from store",
                    _txtOrderNotes.InnerText,
                    !string.IsNullOrEmpty( _txtCustOrderNum.Text ) ? _txtCustOrderNum.Text : null,
                    SiteContext.Current.CurrentClient.Profile.ManagerId,
                    store.StoreNumber );

                //Заккомментили т.к. если тут выпадаем в ошибку, то заказ ушел, а пользователь увидит Error и отправит заказ еще раз
				//var order = OrderBO.LoadOrderData( SiteContext.Current.CurrentClient.Profile.ClientId, orderNo );

                _placeOrderSuccessPanel.Visible = true;
                //_placeOrderStatusLabel.Text = "Заказ успешно размещен";
                _placeOrderNumberLabel.Text = orderNo.Select(x => x.ToString()).Aggregate((x, y) =>
                {
                    return x + ";" + y;
                });//OrderTracking.GetOrderDisplayNumber( order );

                //клиентский номер заказа
                if ( !string.IsNullOrEmpty( custOrderNum /*order.CustOrderNum*/ ) )
                {
                    _custOrderNumPlaceHolder.Visible = true;
                    _custOrderNumLabel.Text = Server.HtmlEncode( custOrderNum /*order.CustOrderNum*/ );
                    //_paymentOrderPrintLink.NavigateUrl = UrlManager.GetPaymentOrderPrintUrlOpt(orderNo);
                }
                else
                {
                    _custOrderNumPlaceHolder.Visible = false;
                }

                //ссылка на печать заказа
                _orderPrintLink.NavigateUrl = "";// UrlManager.GetOrderPrintUrl( orderNo /*order.OrderID*/ );
                //_paymentOrderPrintLink.NavigateUrl = UrlManager.GetPaymentOrderPrintUrl(orderNo);
                //_paymentOrderPrintLinkOpt.NavigateUrl = "~/cms/invoice.ashx?type=payment&order=" + orderNo; /*UrlManager.GetPaymentOrderPrintUrlOpt(orderNo)*/;

                if (LightBO.IsLight())
                {
                    switch (SiteContext.Current.CurrentClient.Profile.Category)
                    {
                        //case ClientCategory.PhysicalIP:
                        //    _paymentOrderPrintLink.Visible = true;
                        //    divider.Visible = true;
                        //    _paymentOrderPrintLinkOpt.Visible = true;
                        //    _paymentOrderPrintLinkOpt.CssClass = "GrayTextStyle";
                        //    break;

                        //case ClientCategory.Legal:
                        //    _paymentOrderPrintLink.Visible = false;
                        //    _paymentOrderPrintLinkOpt.Visible = true;
                        //    break;

                        //case ClientCategory.Physical:
                        //    _paymentOrderPrintLink.Visible = true;
                        //    _paymentOrderPrintLinkOpt.Visible = false;
                        //    break;

                    }
                }
                else
                {
                    //Для основного сайта не отображаем розничную оплату , пусть печатают в 1С счет на оплату
                    //_paymentOrderPrintLink.Visible = true;
                }


                _backLink.NavigateUrl = "~/Default.aspx";
                //_backLink.Text = "Главная страница";

            }
            catch ( BLException ex )
            {
                _placeOrderSuccessPanel.Visible = false;
				_errorLabel.Visible = true;
                _errorLabel.Text += ": " + ex.Message;
            }

        }
                       
        protected void _placeOrderWizard_CancelButtonClick(object sender, EventArgs e)
        {
            
        }

        protected void _shoppingCartItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var cartItem = (ShoppingCartItem)e.Item.DataItem;
                if (cartItem.VinCheckupDataID.HasValue)
                {
                    ((Literal)e.Item.FindControl("_ltrVinData")).Text = "проверить применяемость на: " +
                        ClientCarsDac.GetGarageCar(cartItem.VinCheckupDataID.Value).GetFullName();
                }
            }
        }
    }
}