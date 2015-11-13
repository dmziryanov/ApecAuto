using System;
using RmsAuto.Store.Import;

namespace RmsAuto.Store.MaintSvcs
{
	enum ImportEntity
	{
		Prices = 0,
		PriceFactors,
		Crosses,
		CrossesBrands,
		CrossesGroups,
		CrossesLinks
	}

	public static class ImportEntityUtil
	{
		internal static string GetFormatName(this ImportEntity e)
		{
			switch(e)
			{
				case ImportEntity.Prices: return ImportFacade.PricesFormatName;
				case ImportEntity.PriceFactors: return ImportFacade.PriceFactorsFormatName;
				case ImportEntity.Crosses: return ImportFacade.CrossesFormatName;
				case ImportEntity.CrossesBrands: return ImportFacade.CrossesBrandsFormatName;
				case ImportEntity.CrossesGroups: return ImportFacade.CrossesGroupsFormatName;
				case ImportEntity.CrossesLinks: return ImportFacade.CrossesLinksFormatName;
				default:
					throw new Exception( "Unknown import entity type: " + e.ToString() );
			}
			throw new ArgumentException();
		}
	}
}
