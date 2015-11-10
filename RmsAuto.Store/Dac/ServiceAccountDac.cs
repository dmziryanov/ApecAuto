using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Dac
{
	public static class ServiceAccountDac
	{
		private static Func<StoreDataContext, string, IQueryable<string>> _getClientIdByServiceAccount =
			CompiledQuery.Compile((StoreDataContext dc, string wcfServiceAccount) =>
				from sa in dc.ServiceAccounts where sa.WcfServiceAccount.ToLower() == wcfServiceAccount.ToLower() select sa.ClientID);

        private static Func<StoreDataContext, string, IQueryable<string>> _getRightsByClientID =
            CompiledQuery.Compile((StoreDataContext dc, string wcfServiceAccount) =>
                from sa in dc.ServiceAccounts where sa.ClientID.ToLower() == wcfServiceAccount.ToLower() select sa.Permissions);


		public static string GetClientIDByWcfServiceAccount(string wcfServiceAccount)
		{
			using (var dc = new StoreDataContext())
			{
				return _getClientIdByServiceAccount(dc, wcfServiceAccount).SingleOrDefault();
			}
		}

        public static string[] getRightsByClientID(string ClientID)
        {
            using (var dc = new StoreDataContext())
            {
                var s = _getRightsByClientID(dc, ClientID).SingleOrDefault();
                if (!string.IsNullOrEmpty(s))
                    return s.Split(';');
                else
                    return new string[0] ;
            }
        }
	}
}
