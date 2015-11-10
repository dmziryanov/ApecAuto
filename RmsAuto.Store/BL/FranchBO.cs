using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.BL
{
	public static class FranchBO
	{
		public static List<SupplierInfo> GetSuppliers()
		{
			List<SupplierInfo> result = null;
			using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				string query = "SELECT SupplierID, DeliveryMinDays, DeliveryMaxDays FROM vSuppliers";

				result = dc.DataContext.ExecuteQuery<SupplierInfo>( query ).ToList();
			}

			return result;
		}
	}

	public class SupplierInfo
	{
		public int SupplierID { get; set; }
		public int DeliveryMinDays { get; set; }
		public int DeliveryMaxDays { get; set; }

	}
}