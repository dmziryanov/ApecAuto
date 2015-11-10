using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Web;

namespace RmsAuto.Store.Cms.Dac
{
    public static class ShopsDac
    {
        private static Func<CmsDataContext, string, Shop> _getShopByStoreId =
            CompiledQuery.Compile(
            (CmsDataContext dc, string storeId) =>
                dc.Shops.Where(s => s.StoreId == storeId && s.ShopVisible).SingleOrDefault());

        static DataLoadOptions _getShopByStoreIdDlo;

        static ShopsDac()
        {
            _getShopByStoreIdDlo = new DataLoadOptions();
            _getShopByStoreIdDlo.LoadWith<Shop>(s => s.ShopMapFile);
            _getShopByStoreIdDlo.LoadWith<Folder>(f => f.Files);
        }

        public static List<Shop> GetShops(string storeId)
        {
            List<Shop> shops;
            using (var Ctx = new DCWrappersFactory<CmsDataContext>())
            {
                if (!LightBO.IsLight())
                {
                    shops = Ctx.DataContext.Shops.Where(x => x.ShopVisible).OrderByDescending(x => x.ShopPriority).ToList();
                }
                else
                {
                    shops = Ctx.DataContext.Shops.Where(x => x.ShopVisible && x.InternalFranchName == SiteContext.Current.InternalFranchName).OrderByDescending(x => x.ShopPriority).ToList();
                }
            }

            return shops;
        }

        public static Shop GetShopByStoreId(string storeId)
        {
            using (var dc = new CmsDataContext())
            {
                dc.LoadOptions = _getShopByStoreIdDlo;
                dc.DeferredLoadingEnabled = false;

                return _getShopByStoreId(dc, storeId);
            }
        }

        public static IEnumerable<Shop> GetShopByCityID(int CityID)
        {
            List<Shop> shops;
            List<City> Cities;

            using (var Ctx = new RmsAuto.Store.Entities.dcCommonDataContext())
            {
                Cities = Ctx.Cities.Where(x => x.CityID == CityID).ToList();
            }

            using (var Ctx = new CmsDataContext())
            {
                shops = Ctx.Shops.Where(x => x.ShopVisible && x.CityID == CityID).OrderByDescending(X => X.isRMS).ThenBy(x => x.ShopPriority).ToList();
            }

            return shops;
        }

        public static IEnumerable<Shop> GetShopByRegionID(int RegionID)
        {
            List<Shop> shops;
            //List<Region> Regions;
            List<City> Cities;
            Shop[] Allshops = null;

            using (var Ctx = new dcCommonDataContext())
            {
                Cities = Ctx.Cities.Where(x => x.RegionID == RegionID).ToList();
            }
            
            
            using (var Ctx = new CmsDataContext())
            {
                shops = Ctx.Shops.Where(x => x.ShopVisible && x.RegionID == RegionID).OrderByDescending(X => X.isRMS).ThenBy(x => x.ShopPriority).ToList();
                Allshops = Ctx.Shops.ToArray();


                //Джойним с теми, которые были получены через города
                Allshops = (from sh in Allshops
                            join city in Cities on sh.CityID equals city.CityID
                            where sh.ShopVisible == true
                            select sh).ToArray();

                //Очень странно, но если вынести Это из using то получим ошибку Открыт доступ к DataContext после Dispose
                Allshops = Allshops.Union(shops).Distinct().OrderByDescending(X => X.isRMS).ThenBy(x => x.ShopPriority).ToArray();

                //TODO: Переделать формирование списка с регионами, так как еще добавяться магазины у которых город не null 
                return Allshops;
            }
        }

        public static IEnumerable<Shop> GetShopByID(int ID)
        {
            using (var Ctx = new DCWrappersFactory<CmsDataContext>())
            {
                var shops = Ctx.DataContext.Shops.Where(x => x.ShopVisible && x.ShopID == ID).ToList();

                return shops;
            }
        }
    }
}
