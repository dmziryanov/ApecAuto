using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
    public static class CommonDac
    {
        public static Dictionary<int, int> GetPermutations()
        {
            Dictionary<int, int> res = new Dictionary<int, int>();
            using (var dc = new StoreDataContext())
            {
                try
                {
                    res = dc.Permutation1Cs.Select(x => x).ToDictionary(x => x.OldSupplierId, x => x.NewSupplierId);
                }
                catch (Exception ex)
                {
					Logger.WriteError( "Произошла ошибка при загрузке словаря перестановок SupplierID", EventLogerID.UnknownError, EventLogerCategory.UnknownCategory, ex );
                }
                finally
                {
                    if (dc.Connection.State == System.Data.ConnectionState.Open)
                        dc.Connection.Close();
                }
            }
            return res;
        }
    }
}
