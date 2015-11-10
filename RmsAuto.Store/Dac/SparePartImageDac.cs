using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Entities.Helpers;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
	public static class SparePartImageDac
	{
		
		public static List<SparePartImage> GetSPImageInfo(string sparePartImageID)
		{
			string[] parts = sparePartImageID.Split(',');

            //Фото брака хранятся пока что ТОЛЬКО в нашей БД, соответственно брать их откуда-то еще смысла нет
			//Если данная ситуация изменится то видимо придется делать выборку и у нас и у франчей и делать UNION
			using (var dc = new /*DCWrappersFactory<*/StoreDataContext/*>*/())
			{
				return dc/*.DataContext*/.SparePartImages.Where( 
					i => i.Manufacturer == parts[0] &&
					i.PartNumber == parts[1] &&
					i.SupplierID == Convert.ToInt32(parts[2])).OrderBy(s => s.ImageNumber).ToList();
			}
		}

		public static SparePartImage GetSparePartImage(string spid)
		{
			string[] parts = spid.Split(',');

            using (var dc = new /*DCWrappersFactory<*/StoreDataContext/*>*/())
			{
				return dc/*.DataContext*/.SparePartImages.Where(
					i => i.Manufacturer == parts[0] &&
					i.PartNumber == parts[1] &&
					i.SupplierID == Convert.ToInt32(parts[2]) &&
					i.ImageNumber == Convert.ToInt32(parts[3])).FirstOrDefault();
			}
		}
	}
}
