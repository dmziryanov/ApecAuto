using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.References;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.BL
{
    public class StoreItemProxy<TItem> : IItemsProxy<TItem>
    {
        #region IItemsProxy<TItem> Members

        public IEnumerable<TItem> GetItems()
        {
            if ((typeof(TItem).FullName) == typeof(OwnStore).FullName)
            {
                return OwnStores as IEnumerable<TItem>;
            }

            if ((typeof(TItem).FullName) == typeof(SearchOwnStore).FullName)
            {
                return OwnStoresForSearchResults as IEnumerable<TItem>;
            }

            throw new Exception("Не найден метод поддерживающий данный тип");
        }

        #endregion

        /// <summary>
        /// Список SupplierID собственных складов наличия
        /// </summary>
        public static IEnumerable<OwnStore> OwnStores
        {
            get
            {
                using (var dc = new dcCommonDataContext())
                {
                    foreach(int i in dc.ExecuteQuery<int>(@"SELECT SupplierID FROM dbo.OwnStoreSuppliers"))
                    {
                        yield return new OwnStore() { SupplierID = i, vSupplierID = i };
                    }
                
                    if (dc.Connection.State == System.Data.ConnectionState.Open)
                        dc.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Список SupplierID собственных складов наличия для результатов поиска
        /// </summary>
        public static IEnumerable<SearchOwnStore> OwnStoresForSearchResults
        {
            get
            {
                using (var dc = new dcCommonDataContext())
                {
                    foreach(int i in dc.ExecuteQuery<int>(@"SELECT SupplierID FROM dbo.OwnStoreSuppliers WHERE IsTopSearchResults = 1").ToList())
                    {
                        yield return new SearchOwnStore() { SupplierID = i, vSupplierID = i };
                    }

                    if (dc.Connection.State == System.Data.ConnectionState.Open)
						dc.Connection.Close();
                }
            }
        }
    }
}
