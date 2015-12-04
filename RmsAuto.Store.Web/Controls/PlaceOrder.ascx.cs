using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Web;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Web;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Cms.Entities;

namespace RmsAuto.Store.Web.Controls
{
    public partial class PlaceOrder : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			_contractTermsMessageLabel.Visible = false;
			if( !Page.IsPostBack )
            {
                _rblPaymentMethod.BindEnumeration(typeof(PaymentMethod), PaymentMethod.Cash);
                _shoppingCartItems.DataSource = SiteContext.Current.CurrentClient.Cart.GetItems().Where( t => t.AddToOrder == true );
                _shoppingCartItems.DataBind();
                _txtCustOrderNum.Text = Convert.ToString(Context.Profile["CustOrderNum"]);
            }
        }

		protected void Page_PreRender( object sender, EventArgs e )
		{
			//принимать условия договора необходимо только для розничных клиентов
			_contractTermsPlaceHolder.Visible = SiteContext.Current.CurrentClient.Profile.TradingVolume == TradingVolume.Retail;

			_contractTermsFrame.Attributes[ "src" ] = UrlManager.GetContractTermsFrameUrl();


            TextItem retailContractTermsLinkItem;
            
            if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
            {
                retailContractTermsLinkItem = TextItemsDac.GetTextItem(SiteContext.Current.InternalFranchName + ".RetailContractTerms.Link", SiteContext/*.Current*/.CurrentCulture);
            }
            else
            {
                retailContractTermsLinkItem = TextItemsDac.GetTextItem("RetailContractTerms.Link", SiteContext/*.Current*/.CurrentCulture);
            }

			if( retailContractTermsLinkItem != null )
				_contractTermsLinkLiteral.Text = retailContractTermsLinkItem.TextItemBody;
			else
				_contractTermsLinkLiteral.Visible = false;

			//доверенность показывать только юр.лицам
			_powerOfAttorneyBlock.Visible = SiteContext.Current.CurrentClient.Profile.IsLegalWholesale;
		}


        protected void _placeOrderWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
			//проверить условие согласия с договором
			if( SiteContext.Current.CurrentClient.Profile.TradingVolume == TradingVolume.Retail && !_contractTermsAgreementBox.Checked )
			{
				e.Cancel = true;
				_contractTermsMessageLabel.Visible = true;

				Page.ClientScript.RegisterStartupScript(
					this.GetType(),
					"scroll_down",
					@"$(function(){ 
						$('input[name$=FinishImageButton]').focus();
					});",
					true );
			}
			else
			{
				try
				{
					var store = AcctgRefCatalog.RmsStores[ SiteContext.Current.CurrentClient.Profile.RmsStoreId ];
					if( store == null )
						throw new Exception( string.Format( "Client profile error: unknown RmsStoreId='{0}'", SiteContext.Current.CurrentClient.Profile.RmsStoreId ) );

					var paymentMethod = (PaymentMethod)Enum.Parse( typeof( PaymentMethod ), _rblPaymentMethod.SelectedValue );

					string custOrderNum = _txtCustOrderNum.Text;
					var orderNo = SiteContext.Current.CurrentClient.Cart.PlaceOrder(
						paymentMethod,
						_shippingOptions.ShippingAddress,
						_txtOrderNotes.InnerText,
						!string.IsNullOrEmpty( _txtCustOrderNum.Text ) ? _txtCustOrderNum.Text : null,
						SiteContext.Current.CurrentClient.Profile.ManagerId,
						store.StoreNumber );

					//Заккомментили т.к. если тут выпадаем в ошибку, то заказ ушел, а пользователь увидит Error и отправит заказ еще раз
					//var order = OrderBO.LoadOrderData( SiteContext.Current.CurrentClient.Profile.ClientId, orderNo );

					_placeOrderSuccessPanel.Visible = true;
					//_placeOrderStatusLabel.Text = "Заказ успешно размещен";
					_placeOrderNumberLabel.Text = orderNo.Select(x => x.ToString()).Aggregate((x,y) =>
					{
					    return x + ";" + y;
					});

					//клиентский номер заказа
					if( !string.IsNullOrEmpty( custOrderNum/*order.CustOrderNum*/ ) )
					{
						_custOrderNumPlaceHolder.Visible = true;
						_custOrderNumLabel.Text = Server.HtmlEncode( custOrderNum/*order.CustOrderNum*/ );
					}
					else
					{
						_custOrderNumPlaceHolder.Visible = false;
					}

					//ссылка на печать заказа
				    _orderPrintLink.NavigateUrl = "";//UrlManager.GetOrderPrintUrl( orderNo/*order.OrderID*/ );

					//ссылка на печать платёжки
					if(paymentMethod == PaymentMethod.Clearing && SiteContext.Current.CurrentClient.Profile.PrepaymentPercent != 0)
					{
                        //_paymentOrderPrintLink.NavigateUrl = UrlManager.GetPaymentOrderPrintUrl(orderNo);
                        //_paymentOrderPrintLinkOpt.NavigateUrl = UrlManager.GetPaymentOrderPrintUrlOpt(orderNo);


                        if (LightBO.IsLight())
                        {
                            switch (SiteContext.Current.CurrentClient.Profile.Category)
                            {
                                case ClientCategory.PhysicalIP:
                                    _paymentOrderPrintLink.Visible = true;
                                    divider.Visible = true;
                                    _paymentOrderPrintLinkOpt.Visible = true;
                                    _paymentOrderPrintLinkOpt.CssClass = "GrayTextStyle";
                                    break;

                                case ClientCategory.Legal:
                                    _paymentOrderPrintLink.Visible = false;
                                    _paymentOrderPrintLinkOpt.Visible = true;
                                    break;

                                case ClientCategory.Physical:
                                    _paymentOrderPrintLink.Visible = true;
                                    _paymentOrderPrintLinkOpt.Visible = false;

                                    break;
                            }
                        }
                        else
                        {
                            _paymentOrderPrintLink.Visible = true;
                        }
					}
					else
					{
						//_paymentOrderPrintLink.Visible = false;

					}

					_backLink.NavigateUrl = "~/Default.aspx";
					//_backLink.Text = "Главная страница";

				}
				catch( BLException ex )
				{
					_placeOrderSuccessPanel.Visible = false;
					_errorLabel.Visible = true;
					_errorLabel.Text += ": " + ex.Message;
				}
			}
        }
        
        protected void _placeOrderWizard_ActiveStepChanged(object sender, EventArgs e)
        {
            if (_placeOrderWizard.ActiveStepIndex ==
                _placeOrderWizard.WizardSteps.IndexOf(_orderReviewStep))
            {
                var shippingAddress = _shippingOptions.ShippingAddress;
                _ltrShippingInfo.Text = !string.IsNullOrEmpty(shippingAddress) ? shippingAddress : "Store pickup";
                _ltrPaymentMethod.Text = _rblPaymentMethod.SelectedItem.Text;
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