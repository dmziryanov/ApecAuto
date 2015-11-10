using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
	public static class DiscountSparePartDac
	{
		public static string[] GetDiscountSparePartBrands()
		{
			using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				return dc.DataContext.DiscountSpareParts.OrderBy(d => d.Manufacturer)
					.Select(d => d.Manufacturer)
					.Distinct().ToArray();
			}
		}

		public static DiscountSparePart[] GetDiscountSparePart(string brand, int startIndex, int size)
		{
            using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				return dc.DataContext.DiscountSpareParts
					.Where(d => d.Manufacturer == brand)
					.OrderByDescending(d => d.SupplierID)
					.ThenBy(d => d.InitialPrice)
					.Skip(startIndex).Take(size).ToArray();
			}
		}

		public static int GetDiscountSparePartCount(string brand)
		{
            using (var dc = new DCWrappersFactory<StoreDataContext>())
			{
				return dc.DataContext.DiscountSpareParts.Where(d => d.Manufacturer == brand).Count();
			}
		}
	}
}
