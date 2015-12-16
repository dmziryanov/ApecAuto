using System;
using System.Linq;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Cms
{
    public partial class ShopList : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //_pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;
            this.LoadComplete += new System.EventHandler(this.ShopList_LoadComplete);


            h2.Visible = true;
            this.ShopList2.Visible = true;
            _pageTitleLiteral.Text = "Магазины";


        }

        public void InitializeComponent()
        {
            // Здесь это почему-то не работает
            // this.LoadComplete += new System.EventHandler(this.ShopList_LoadComplete);

        }

        private void ShopList_LoadComplete(object sender, EventArgs e)
        {

        }
    }
}
