using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;
using System.Configuration;

namespace RmsAuto.Store.Web
{
    public partial class Checkout : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
			//TODO: убрать временную блокировку заказа для розничных клиентов
			if((ConfigurationManager.AppSettings[ "DisableRetailOrdering" ] ?? "" ).ToLower() == "true" )
			{
				bool isRetail = SiteContext.Current.CurrentClient.Profile.TradingVolume == TradingVolume.Retail && !SiteContext.Current.CurrentClient.Profile.IsChecked;
				_checkoutPlaceHolder.Visible = !isRetail;
				_retailRefusalPlaceHolder.Visible = isRetail;
			}
			else
			{
				_checkoutPlaceHolder.Visible = true;
				_retailRefusalPlaceHolder.Visible = false;
			}
        }

        protected void Page_Load( object sender, EventArgs e )
        {
            // deas 14.04.2011 task3809
            // добавлен новый мастер оформления заказа для оптовых клиентов
            if ( !Page.IsPostBack )
            {
                if ( SiteContext.Current.CurrentClient.Profile.TradingVolume == TradingVolume.Wholesale )
                {
                    _placeOrderWhSl.Visible = true;
                }
                else
                {
                    _placeOrder.Visible = true;
                }
            }
        }
    }
}
