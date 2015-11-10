using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Cms.Dac
{
	public static class CatalogItemsDac
	{
        //private static Func<CmsDataContext, Table<CatalogItem>> _getCatalogItems =
        //    CompiledQuery.Compile(
        //    ( CmsDataContext dc ) =>
        //        dc.CatalogItems );

        static List<CatalogItem> _allItem;

		public static CatalogItem[] GetCatalogItems(string rootName, string FranchName)
		{
            using (var dc = new DCWrappersFactory<CmsDataContext>(FranchName))
			{
                _allItem = dc.DataContext.CatalogItems.ToList<CatalogItem>();
                List<CatalogItem> res = new List<CatalogItem>();
                CatalogItem rootItem = _allItem.Where( t => t.CatalogItemCode == rootName).Single();
                res.Add(rootItem);
                GetChildren(dc.DataContext, ref res, rootItem.CatalogItemID);
                return res.ToArray();
				//return _getCatalogItems( dc ).ToArray();
			}
		}

        static void GetChildren( CmsDataContext dc, ref List<CatalogItem> res, int parentID )
        {
            foreach ( CatalogItem CI in _allItem.Where( t => t.ParentItemID == parentID ) )
            {
                res.Add( CI );
                GetChildren( dc, ref res, CI.CatalogItemID );
            }
        }

		/// <summary>
		/// Возвращает локали под CatalogItem
		/// </summary>
		/// <param name="catalogItemID">ID</param>
		/// <returns></returns>
		//public static CatalogItemLoc[] GetCatalogItemsLoc(int catalogItemID)
		//{
		//    using (var dc = new CmsDataContext())
		//    {
		//        string query = @"select * from Cms.CatalogItemsLocs where CatalogItemID = {0}";
		//        IEnumerable<CatalogItemLoc> result = dc.ExecuteQuery<CatalogItemLoc>(query, catalogItemID);
		//        return result.ToArray();
		//    }
		//}

		public static CatalogItemsLoc[] GetCatalogItemsAllLoc(string InternalFranchName)
		{
            using (var dc = new DCWrappersFactory<CmsDataContext>(InternalFranchName))
			{
				string query = @"select * from Cms.CatalogItemsLocs";
				IEnumerable<CatalogItemsLoc> result = dc.DataContext.ExecuteQuery<CatalogItemsLoc>(query);
				return result.ToArray();
			}
		}
	}
}
