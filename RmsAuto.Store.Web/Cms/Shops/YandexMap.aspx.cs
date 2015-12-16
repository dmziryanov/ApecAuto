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

        protected void Page_Load(object sender, EventArgs e)
        {
            Shop[] shops = null;
            Shop[] Allshops = null;


            Allshops = ShopsDac.GetShops("").ToArray();
            ShopsLoaded = Allshops.Count();
            firstShop = Allshops.FirstOrDefault();

            CityRepeater.DataSource = getShops(ref Allshops);
            CityRepeater.DataBind();

        }
        object[] getShops(ref Shop[] sl)
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
