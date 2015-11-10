using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities.Helpers
{
	public static class SearchHelper
	{
		public static Dictionary<SparePartKeyExt, AdditionalInfoExt> GetAdditionalInfoExt(IEnumerable<SparePartKeyExt> keys)
		{
			using (var dc = new StoreDataContext())
			{
				var res = new Dictionary<SparePartKeyExt, AdditionalInfoExt>();

				var infos = keys.Select(k => new AdditionalInfoExt()
												{
													Key = new SparePartKeyExt(k.Manufacturer, k.PartNumber, k.SupplierID),
													HasDefectPics = ( from spi in dc.SparePartImages
																	  where spi.PartNumber == k.PartNumber &&
																			spi.Manufacturer == k.Manufacturer &&
																			spi.SupplierID == k.SupplierID
																	  select spi).Any()
												});
				// оставляем только те объекты, у которых HasDefectPics = true, т.к. пока в объекте нет других вещей 
				// ("описание", "применяемость на авто" и т.д. по аналогии с текдоком) в нем есть смысл только если
				// на данную деталь существуют фото брака
				infos = infos.Where(i => i.HasDefectPics); 
				res = infos.ToDictionary(i => i.Key, i => i);
				return res;
			}
		}
	}
}
