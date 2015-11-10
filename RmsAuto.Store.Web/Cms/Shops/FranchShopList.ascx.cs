using System;
using System.Linq;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Acctg.Entities;
using System.Collections.Generic;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Web.Cms.Shops
{
	public partial class FranchShopList : System.Web.UI.UserControl
	{
        public int ShopsLoaded;
        public Shop firstShop;
		
        protected void Page_Load( object sender, EventArgs e )
		{
            Shop [] shops = new Shop [0];


                shops = ShopsDac.GetShops("").ToArray();
                ShopsLoaded = shops.Length;
         

                _listView.DataSource = getShops(ref shops);
                _listView.DataBind();
		}

        object[] getShops(ref Shop[] sl)
        {
            
            List<object> ol = new List<object>();
            foreach (Shop sh in sl)
            {
                
                string ShopString = sh.ShopName + ", " +
                       sh.ShopAddress + (sh.ShopMetro != null && sh.ShopMetro.Length > 0 ? ", " + sh.ShopMetro : "");
                ol.Add(new
                {
                    ShopId = sh.ShopID,
                    ShopString = sh.ShopName + ", " +
                    sh.ShopAddress + (sh.ShopMetro != null && sh.ShopMetro.Length > 0 ? ", " + sh.ShopMetro : "")
                });
            }
            return ol.ToArray();
        }
     
	}
}