using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;

namespace RmsAuto.Store.Web.TecDoc
{
	public static class TecDocAggregator
	{

		public static BrandItem[] GetBrands()
		{
			return BrandsCache.Default.GetBrands();
		}

		public static BrandItem GetBrandByUrlCode( string urlCode, VehicleType vehicleType )
		{
			return BrandsCache.Default.GetBrandByUrlCode( urlCode, vehicleType );
		}

		public static BrandItem GetBrandByManufacturerID( int manufacturerId, VehicleType vehicleType )
		{
			return BrandsCache.Default.GetBrandByManufacturerID( manufacturerId, vehicleType );
		}

		public static RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer[] GetManufacturers()
		{
			return BrandsCache.Default.GetManufacturers();
		}

		public static RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer GetManufacturerByName( string name )
		{
			return BrandsCache.Default.GetManufacturerByName( name );
		}

		public static RmsAuto.TechDoc.Entities.TecdocBase.Model GetModelById( int modelId )
		{
			return RmsAuto.TechDoc.Facade.GetModelById( modelId, false, BrandsCache.Default.GetVisibleCountryIds() );
		}

		public static RmsAuto.TechDoc.Entities.TecdocBase.CarType GetModificationById( int modificationId )
		{
			return RmsAuto.TechDoc.Facade.GetModification( modificationId, false, BrandsCache.Default.GetVisibleCountryIds() );
		}

		public static RmsAuto.TechDoc.Entities.Helpers.SearchTreeNodeHelper GetTreeItem( int modificationId, int searchTreeNodeId )
		{
			return RmsAuto.TechDoc.Facade.GetTreeItem( modificationId, searchTreeNodeId );
		}

		public static List<RmsAuto.TechDoc.Entities.TecdocBase.CarType> GetModifications( int modelId )
		{
			return RmsAuto.TechDoc.Facade.ListModifications( modelId, false, BrandsCache.Default.GetVisibleCountryIds() );
		}

		public static List<RmsAuto.TechDoc.Entities.TecdocBase.Model> GetModels( int manufacturerId, VehicleType? vehicleType )
		{
			if( vehicleType == null )
			{
				return RmsAuto.TechDoc.Facade.ListModels( manufacturerId, false, false, BrandsCache.Default.GetVisibleCountryIds() )
					.Union( RmsAuto.TechDoc.Facade.ListModels( manufacturerId, true, false, BrandsCache.Default.GetVisibleCountryIds() ) )
					.OrderBy( m => m.Name.Tex_Text )
					.ToList();
			}
			else
			{
				return RmsAuto.TechDoc.Facade.ListModels( manufacturerId, vehicleType.Value == VehicleType.Car, false, BrandsCache.Default.GetVisibleCountryIds() );
			}
		}

	}
}
