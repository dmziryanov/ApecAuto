using System;
using System.Linq;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Acctg.Entities;
using System.Collections.Generic;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Cms.Shops
{
	public partial class ShopList : System.Web.UI.UserControl
	{
        public int ShopsLoaded;
        public Shop firstShop;
		
        protected void Page_Load( object sender, EventArgs e )
		{
            Shop [] shops = new Shop [0];
            Shop[] Allshops = new Shop[0];
            List<City> Cities = null;

            if (CmsContext.Current.PageParameters["Region"] == "City")
            {
                using (var Ctx = new dcCommonDataContext())
                {
                    Cities = Ctx.Cities.Where(x => x.CityID.ToString() == CmsContext.Current.PageParameters["ID"]).ToList();
                }

                shops = ShopsDac.GetShopByCityID(Convert.ToInt32(CmsContext.Current.PageParameters["ID"])).ToArray();
                shops = shops.Where(s => s.isRMS != true).ToArray();
                ShopsLoaded = shops.Length;
                firstShop = shops.FirstOrDefault();

                _listView.DataSource = getShops(ref shops);
                _listView.DataBind();
            }

            if (CmsContext.Current.PageParameters["Region"] == "Region")
            {
                // Ссылка "Центральный офис" использует алгоритм отображения магазинов по региону
                // "100" - 'регион' "Центральный офис"
                if ("100" == CmsContext.Current.PageParameters["ID"])
                {
                    Shop[] sl = (new CmsDataContext()).Shops.Where(x => x.isRMS == true).ToArray();
                    _listView.DataSource = getShops(ref sl);
                    _listView.DataBind(); return;
                }
                Region r;  
                using (var Ctx = new dcCommonDataContext())
                {
                    Cities = Ctx.Cities.Where(x => x.RegionID.ToString() == CmsContext.Current.PageParameters["ID"]).ToList();
                    r = Ctx.Regions.Where(x => x.RegionID.ToString() == CmsContext.Current.PageParameters["ID"]).FirstOrDefault();
                }

                Cities.Add(new City() { CityID = null });
                Allshops = ShopsDac.GetShopByRegionID(Convert.ToInt32(CmsContext.Current.PageParameters["ID"])).ToArray();
                ShopsLoaded = Allshops.Count();


                firstShop = Allshops.FirstOrDefault();

                _listView.DataSource = getShops(ref Allshops);
                _listView.DataBind();
            }
		}
        
        object [] getShops(ref Shop[] sl)
        {
            var Ctx = new dcCommonDataContext();
            List<object> ol = new List<object>();
            foreach (Shop sh in sl)
            {
                string CityName = Ctx.Cities.FirstOrDefault(c => c.CityID == sh.CityID).Name, RegionName = "";
                if (sh.RegionID != null) RegionName = Ctx.Regions.FirstOrDefault(c => c.RegionID == sh.RegionID).RegionName;
                string ShopString = CityName + (!string.IsNullOrEmpty(CityName) ? ", " : RegionName + ", ") + sh.ShopName + ", " +
                       sh.ShopAddress + (sh.ShopMetro != null && sh.ShopMetro.Length > 0 ? ", " + sh.ShopMetro : "");
                ol.Add(new
                {
                    ShopId = sh.ShopID,
                    ShopString = CityName + (!string.IsNullOrEmpty(CityName) ? ", " : RegionName + ", ") + sh.ShopName + ", " +
                    sh.ShopAddress + (sh.ShopMetro != null && sh.ShopMetro.Length > 0 ? ", " + sh.ShopMetro : "")
                });
            }
            return ol.ToArray();
        }
	}
}