using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Manager
{
    public partial class ClientCartImport : ClientBoundPage
    {
        public static string GetUrl()
        {
            return "~/Manager/ClientCartImport.aspx";
        }

        public override ClientDataSection DataSection
        {
            get { return ClientDataSection.CartImport; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!ClientData.Profile.IsChecked)
            //    throw new HttpException( (int)HttpStatusCode.Forbidden, "Импорт заказов для клиентов без флага \"проверен\" запрещено");
            //if (ClientData.Profile.IsLegalWholesale)
            //    throw new HttpException( (int)HttpStatusCode.Forbidden, "Оформление заказов для клиентов типа \"юр.лицо-опт\" запрещено" );

            if (!Page.IsPostBack)
                ResetPlaceOrderVisibility();

            ClientData.Cart.ContentChanged += new EventHandler<EventArgs>(Cart_ContentChanged);

            //_printWithPartNumbers.NavigateUrl = ClientCartPrint.GetUrl(ClientData.Profile.ClientId, true);
            //_printWithoutPartNumbers.NavigateUrl = ClientCartPrint.GetUrl(ClientData.Profile.ClientId, false);
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            ClientData.Cart.ContentChanged -= new EventHandler<EventArgs>(Cart_ContentChanged);
        }

        void Cart_ContentChanged(object sender, EventArgs e)
        {
            //ResetPlaceOrderVisibility();
        }

        private void ResetPlaceOrderVisibility()
        {
            //_placeOrderPanel.Visible = ShoppingCartItems1.CartItemsCount > 0 && !ShoppingCartItems1.ContainsBadItems;
        }
    }
}
