using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Web.Controls;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.BL;
using System.Web.Security;
using RmsAuto.Store.Acctg;    

namespace RmsAuto.Store.Web
{
    public partial class Cart1 : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected ClientData CurrentClient
        {
            get { return SiteContext.Current.CurrentClient; }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            _guestPriceHint_TextItemControl.Visible =
                SiteContext.Current.CurrentClient.IsGuest &&
                this.ShoppingCartItems1.CartItemsCount > 0;

            // deas 02.03.2011 task3033
            // добавленна проверка на существование пункта выдачи заказов указанного в профиле клиента
            _btnCheckout.Visible =
            !CurrentClient.IsGuest &&
            !CurrentClient.Profile.IsRestricted &&
            (CurrentClient.Profile.IsChecked ||
            (CurrentClient.Profile.TradingVolume == TradingVolume.Retail && CurrentClient.Profile.Category == ClientCategory.Physical)) && 
            (this.ShoppingCartItems1.CartItemsCount > 0) &&
            !this.ShoppingCartItems1.ContainsBadItems &&
            ( AcctgRefCatalog.RmsStores[SiteContext.Current.CurrentClient.Profile.RmsStoreId] != null );
            
			_printLink.NavigateUrl = UrlManager.GetCartPrintUrl();
        }

        protected void _btnCheckout_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
				this.ShoppingCartItems1.Recalc();
				_cartVersionValidator.PinVersion();
				if( SiteContext.Current.CurrentClient.Cart.GetTotals().ItemsCount != 0 )
				{
					Response.Redirect( UrlManager.GetCheckoutUrl() );
				}
            }
        }
        protected void _btnLogon_Click(object sender, EventArgs e)
        {
            FormsAuthentication.RedirectToLoginPage();
        }

		public string ErrorClientStore
		{
			get
			{
				if (LightBO.IsLight())
				{
					//для лайта дополнительно выводим его телефон в сообщении
					return string.Format(Resources.Exceptions.Cart_ErrorClientStoreLight, AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Phone);
				}
				else
				{
					return Resources.Exceptions.Cart_ErrorClientStore;
				}
			}
		}
		public string ClientNotChecked
		{
			get
			{
				if (LightBO.IsLight())
				{
					//для лайта дополнительно выводим его телефон в сообщении
					return string.Format(Resources.Texts.ClientNotCheckedLight, AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Phone);
				}
				else
				{
					return Resources.Texts.ClientNotChecked;
				}
			}
		}
    }
}
