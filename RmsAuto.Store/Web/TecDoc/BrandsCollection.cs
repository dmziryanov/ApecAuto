using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Entities;
using System.Collections.Generic;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.TecDoc
{
	/// <summary>
	/// Коллекция брэндов
	/// </summary>
	class BrandsCollection
	{
		int[] _visibleCountryIds;
		BrandItem[] _brands;
		RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer[] _manufactures;
		Dictionary<KeyValuePair<string, VehicleType>, BrandItem> _brandUrlCodeHash = new Dictionary<KeyValuePair<string, VehicleType>, BrandItem>();
		Dictionary<KeyValuePair<int, VehicleType>, BrandItem> _brandManufacturerIdHash = new Dictionary<KeyValuePair<int, VehicleType>, BrandItem>();
		Dictionary<string, RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer> _manufacturerNameHash = new Dictionary<string, RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer>( StringComparer.CurrentCultureIgnoreCase );

		public BrandItem[] GetBrands()
		{
			return _brands;
		}
		public BrandItem GetBrandByUrlCode( string urlCode, VehicleType vehicleType )
		{
			KeyValuePair<string, VehicleType> key = new KeyValuePair<string, VehicleType>( urlCode.ToLower(), vehicleType );
			return _brandUrlCodeHash.ContainsKey( key ) ? _brandUrlCodeHash[ key ] : null;
		}
		public BrandItem GetBrandByManufacturerID( int manufacturerId, VehicleType vehicleType )
		{
			KeyValuePair<int, VehicleType> key = new KeyValuePair<int, VehicleType>( manufacturerId, vehicleType );
			return _brandManufacturerIdHash.ContainsKey( key ) ? _brandManufacturerIdHash[ key ] : null;
		}

		public RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer[] GetManufacturers()
		{
			return _manufactures;
		}
		public RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer GetManufacturerByName( string name )
		{
			RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer res;
			_manufacturerNameHash.TryGetValue( name, out res );
			return res;
		}
		
		public int[] GetVisibleCountryIds()
		{
			return _visibleCountryIds;
		}

		public BrandsCollection()
		{
			RmsAuto.Store.Cms.Entities.Brand[] brands;
			int[] visibleCountryIds;
			using( var dc = new CmsDataContext() )
			{
				brands = dc.Brands.ToArray();
			}
			using( var dc = new RmsAuto.TechDoc.Entities.TecdocStoreDataContext() )
			{
				visibleCountryIds = dc.CountryVisibilities.Select( c => c.CountryID ).ToArray();
			}

			_visibleCountryIds = visibleCountryIds;

			var manufacturers = RmsAuto.TechDoc.Facade.ListManufacturers( true, visibleCountryIds ).ToDictionary( m => m.Name, StringComparer.CurrentCultureIgnoreCase );

			var list = new List<BrandItem>();
			var manList = new List<RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer>();
			foreach( var brand in brands )
			{
				RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer manufacturer;
				manufacturers.TryGetValue( brand.Name, out manufacturer );
				if( manufacturer != null && ( manufacturer.IsCarManufacturer == 1 && brand.VehicleType == RmsAuto.Store.Cms.Entities.VehicleType.Car
						|| manufacturer.IsTruckManufacturer == 1 && brand.VehicleType == RmsAuto.Store.Cms.Entities.VehicleType.Truck ) )
				{
					var item = new BrandItem( brand, manufacturer );
					list.Add( item );
					_brandUrlCodeHash.Add( new KeyValuePair<string, VehicleType>( item.Brand.UrlCode.ToLower(), item.Brand.VehicleType ), item );
					_brandManufacturerIdHash.Add( new KeyValuePair<int, VehicleType>( item.Manufacturer.ID, item.Brand.VehicleType ), item );

					if( !_manufacturerNameHash.ContainsKey( manufacturer.Brand.Name ) )
					{
						_manufacturerNameHash.Add( manufacturer.Brand.Name, manufacturer );
						manList.Add( manufacturer );
					}
				}
			}
			_brands = list.OrderBy( i => i.Brand.Name ).ToArray();
			_manufactures = manList.OrderBy( i => i.Name ).ToArray();
		}
	}
}
