using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Cms
{
    public partial class YandexMap : System.Web.UI.Page
    {
        protected int ShopsLoaded;
        protected Shop firstShop;
        protected List<City> Cities = null;
        protected Region r;  
		
        protected void Page_Load( object sender, EventArgs e )
		{
            Shop [] shops = null;
            Shop [] Allshops = null;
           
        
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
              
                CityRepeater.DataSource = getShops(ref shops);
                CityRepeater.DataBind();
            }

            if (CmsContext.Current.PageParameters["Region"] == "Region")
            {
                // Ссылка "Центральный офис" использует алгоритм отображения магазинов по региону
                // "100" - 'регион' "Центральный офис"
                if ("100" == CmsContext.Current.PageParameters["ID"])
                {
                    Shop[] sl = (new CmsDataContext()).Shops.Where(x => x.isRMS == true).ToArray();
                    CityRepeater.DataSource = getShops(ref sl);
                    CityRepeater.DataBind(); return;
                }
                
             
                using (var Ctx = new dcCommonDataContext())
                {
                    Cities = Ctx.Cities.Where(x => x.RegionID.ToString() == CmsContext.Current.PageParameters["ID"]).ToList();
                    r = Ctx.Regions.Where(x => x.RegionID.ToString() == CmsContext.Current.PageParameters["ID"]).FirstOrDefault();
                }

                Cities.Add(new City() { CityID = null });
                Allshops = ShopsDac.GetShopByRegionID(Convert.ToInt32(CmsContext.Current.PageParameters["ID"])).ToArray();
                ShopsLoaded = Allshops.Count();
                firstShop = Allshops.FirstOrDefault();

                CityRepeater.DataSource = getShops(ref Allshops);
                CityRepeater.DataBind();
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
