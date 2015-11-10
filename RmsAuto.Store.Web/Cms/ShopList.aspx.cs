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
            if (CmsContext.Current.PageParameters["Region"] == "Region")
            {
                // Ссылка "Центральный офис" использует алгоритм отображения магазинов по региону
                // "100" - 'регион' "Центральный офис"
                if ("100" == CmsContext.Current.PageParameters["ID"])
                {
                    imgId.Visible = false;
                    newHyperLink.ResolveClientUrl(System.Configuration.ConfigurationManager.AppSettings["WebSiteUrl"] + "/About/Shops/Map/" + CmsContext.Current.PageParameters["Region"] + "/" + CmsContext.Current.PageParameters["ID"] + ".aspx");
                    newHyperLink.Text = "Магазины РМС: ";
                    newHyperLink.CssClass = "header3Style";
                }
                else using (var ctx = new dcCommonDataContext())
                {
                    string RegionName = ctx.Regions.Where(x => x.RegionID.ToString() == CmsContext.Current.PageParameters["ID"]).Select(x => x.RegionName).FirstOrDefault();
                    newHyperLink.Text = "Магазины в регионе: " + RegionName;
                    newHyperLink.CssClass = "header3Style";
                }
            } else

            if (CmsContext.Current.PageParameters["Region"] == "City")
            {
                using (var ctx = new dcCommonDataContext())
                {
                    string CityName = ctx.Cities.Where(x => x.CityID.ToString() == CmsContext.Current.PageParameters["ID"]).Select(x => x.Name).FirstOrDefault();
                    newHyperLink.Text = "Наши магазины в городе: " + CityName;
                    newHyperLink.CssClass = "header3Style";
                }
            } else
            
            
            {
                h1.Visible = false;
                h2.Visible = true;
                this.ShopList1.Visible = false;
                this.ShopList2.Visible = true;
                _pageTitleLiteral.Text = "Наши магазины";
            }
 
		}

        public void InitializeComponent()
        {
            // Здесь это почему-то не работает
            // this.LoadComplete += new System.EventHandler(this.ShopList_LoadComplete);

        }

        private void ShopList_LoadComplete(object sender, EventArgs e)
        {
            if (ShopList1.ShopsLoaded == 1)
            {
                Response.Redirect(UrlManager.GetShopDetailsUrl(ShopList1.firstShop.ShopID));
            }
        }
	}
}
