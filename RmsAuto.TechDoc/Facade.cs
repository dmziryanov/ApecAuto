using System.Linq;
using System.Collections.Generic;
using System.Web;
using RmsAuto.TechDoc.Cache;
using RmsAuto.TechDoc.Entities.TecdocBase;
using RmsAuto.TechDoc.Entities.Helpers;
using System.Data.Linq;
using RmsAuto.Common.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace RmsAuto.TechDoc
{
	public static class Facade
	{
		#region Manufacturers

		/// <summary>
		/// Получить полный список марок автомобилей
		/// </summary>
		/// <returns></returns>
		public static List<Manufacturer> ListManufacturers()
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				return ctx.ListManufacturers();
			}
		}


		/// <summary>
		/// Получить список марок автомобилей, отфильтрованный по стране и 
		/// опционально отфильтрованный по маркам
		/// </summary>
		/// <param name="showAll"></param>
		/// <returns></returns>
		public static List<Manufacturer> ListManufacturers( bool showAll, IEnumerable<int> countriesIds )
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				DataLoadOptions dlo = new DataLoadOptions();
				dlo.LoadWith<Manufacturer>( m => m.Brand );
				ctx.LoadOptions = dlo;
				return ctx.ListManufacturers( showAll, countriesIds );
			}
		}

		/// <summary>
		/// Получить марку автомобиля
		/// </summary>
		/// <param name="id"></param>
		/// <param name="showAll"></param>
		/// <returns></returns>
		public static Manufacturer GetManufacturer( int id, bool showAll, IEnumerable<int> countriesIds )
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				return ctx.GetManufacturer( id, showAll, countriesIds );
			}
		}

		public static Manufacturer GetManufacturerByName( string name, bool showAll, IEnumerable<int> countriesIds )
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				return ctx.GetManufacturerByName( name, showAll, countriesIds );
			}
		}

		#endregion

		#region Models

		public static List<Model> ListModels( int manufacturerID )
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				var dlo = new DataLoadOptions();
				dlo.LoadWith<Model>( m => m.Name );
				ctx.LoadOptions = dlo;

				return ctx.ListModels( manufacturerID );
			}
		}

		public static List<Model> ListModels( int manufacturerId, bool isCarModel, bool showAll, IEnumerable<int> countriesIds )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				ctx.DeferredLoadingEnabled = false;
				DataLoadOptions dlo = new DataLoadOptions();
				dlo.LoadWith<Model>( m => m.Name );
				ctx.LoadOptions = dlo;

				return ctx.ListModels( manufacturerId, isCarModel, showAll, countriesIds );
			}
		}

		public static Model GetModelById( int modelId, bool showAll, IEnumerable<int> countriesIds )
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				DataLoadOptions dlo = new DataLoadOptions();
				dlo.LoadWith<Model>( m => m.Manufacturer );
				dlo.LoadWith<Model>( m => m.Name );
				ctx.LoadOptions = dlo;
				return ctx.GetModel( modelId, showAll, countriesIds );
			}
		}

		#endregion

		#region Modifications

		public static List<CarType> ListModifications( int modelID )
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				var dlo = new DataLoadOptions();
				dlo.LoadWith<CarType>( m => m.Name );
				dlo.LoadWith<CarType>( m => m.FullName );
				//dlo.LoadWith<CarType>(m => m.Model);
				//dlo.LoadWith<Model>(m => m.Name);
				ctx.LoadOptions = dlo;

				return ctx.ListModelModifications( modelID );
			}
		}

		public static List<CarType> ListModifications( int modelId, bool showAll, IEnumerable<int> countriesIds )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				DataLoadOptions dlo = new DataLoadOptions();
				dlo.LoadWith<CarType>( m => m.Name );
				dlo.LoadWith<CarType>( m => m.EngineName );
				dlo.LoadWith<CarType>( m => m.FuelSupplyName );
				ctx.LoadOptions = dlo;

				return ctx.ListModifications( modelId, showAll, countriesIds );
			}
		}

		public static CarType GetModification( int modificationId, bool showAll, IEnumerable<int> countriesIds )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				DataLoadOptions dlo = new DataLoadOptions();
				dlo.LoadWith<CarType>( m => m.Name );
				dlo.LoadWith<CarType>( m => m.Model );
				dlo.LoadWith<Model>( m => m.Name );
				dlo.LoadWith<Model>( m => m.Manufacturer );
				ctx.LoadOptions = dlo;

				return ctx.GetModification( modificationId, showAll, countriesIds );
			}
		}

		#endregion

		#region Tree

		public static SearchTreeNodeHelper GetTreeItem( int modificationId, int searchTreeNodeId )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				return ctx.GetTreeItem( modificationId, searchTreeNodeId );
			}
		}
		public static List<SearchTreeNodeHelper> GetTree( int modificationId )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				return ctx.GetTree( modificationId );
			}
		}

		#endregion

		#region Parts

		public static List<PartsListItem> ListParts( int modificationId, int searchTreeNodeId )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				var dlo = new DataLoadOptions();
				dlo.LoadWith<Article>( a => a.Supplier );
				dlo.LoadWith<Article>( a => a.CompleteName );
				ctx.LoadOptions = dlo;
				return ctx.ListParts( modificationId, searchTreeNodeId );
			}
		}


		public static PartInfo GetPartInfo( int artId )
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				var dlo = new DataLoadOptions();
				dlo.LoadWith<Article>( a => a.Supplier );
				dlo.LoadWith<Article>( a => a.CompleteName );
				ctx.LoadOptions = dlo;

				return ctx.GetPartInfo( artId );
			}
		}

		public static IEnumerable<KeyValuePair<string, string>> GetPartAddInfo( int artId )
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				return ctx.GetPartAddInfo( artId );
			}
		}

		public static IEnumerable<CarType> GetAppliedCars( int artId )
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				var dlo = new DataLoadOptions();
				dlo.LoadWith<CarType>( m => m.Name );
				dlo.LoadWith<CarType>( m => m.BodyName );
				dlo.LoadWith<CarType>( m => m.FuelSupplyName );
				dlo.LoadWith<CarType>( m => m.Model );
				dlo.LoadWith<Model>( m => m.Name );
				dlo.LoadWith<Model>( m => m.Manufacturer );
				ctx.LoadOptions = dlo;

				return ctx.GetAppliedCars( artId );
			}
		}

		public static ImageInfo GetImage( int id )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				return ctx.GetImage( id );
			}
		}

		public static List<ImageInfo> GetImages( int artId )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				return ctx.GetImages( artId );
			}
		}

		#endregion

		#region Suppliers

		public static List<Supplier> GetSuppliers()
		{
			using( var ctx = new RmsAuto.TechDoc.Entities.TecdocBase.TecdocBaseDataContext() )
			{
				return ctx.GetSuppliers();
			}
		}

		#endregion

		#region Country

		public static Dictionary<int, Country> GetCountries()
		{
			return TDCacheHelper.GetCountries();
		}

		public static Country GetCountryById( int id )
		{
			return GetCountries()[ id ];
		}

		#endregion

		#region Lookup by name

		public static int LookupManufacturerId( string mName )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				return ctx.LookupManufacturerId( mName );
			}
		}

		public static int LookupModelId( int manufacturerId, string mName )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				return ctx.LookupModelId( manufacturerId, mName );
			}
		}

		public static int LookupModificationId( int modelId, string mName )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				return ctx.LookupModificationId( modelId, mName );
			}
		}

		#endregion

		/// <summary>
		/// Загружает доп инфу по спарпартам. Перенесен из фасада текдока, все равно используется только в поиске в связке со типом SparePart.
		/// </summary>
		/// <param name="keys">Инумерабл спарпартов</param>
		/// <returns>Словарь доп инфы по парткеям</returns>
		public static Dictionary<PartKey, AdditionalInfo> GetAdditionalInfo( IEnumerable<PartKey> keys )
		{
			//var brands = keys.Select( k => k.Manufacturer ).Distinct();
			var partNumbers = keys.Select( k => k.PartNumber ).Distinct().ToArray();

			using( var ctx = new TecdocBaseDataContext() )
			{
				var list = new List<AdditionalInfo>();
				for( var i = 0 ; i < partNumbers.Length ; i += 2000 )
				{
					
					/*var res = ctx.ArticleLookups
						 .Where( la => partNumbers.Skip( i ).Take( Math.Min( 2000, partNumbers.Length - i ) ).Contains( la.SearchNumber ) && la.ARL_KIND == '1' )
						 .GroupBy( la => new
						 {
							 Brand = la.Article.Supplier.Name,
							 PartNumber = la.Article.ArticleNumber,
							 TecDocArticulId = la.Article.ID
						 } ).Select( result => new AdditionalInfo()
						 {
							 Key = new PartKey( result.Key.Brand, result.Key.PartNumber ), //may be SearchNumber ???

							 TecDocArticulId = result.Key.TecDocArticulId,

							 PartDescription = result.First().Article.CompleteName.Text,

							 HasPics = ( from gra in ctx.GraphicsToArticleLinks
										 join gr in ctx.Graphics on gra.LGA_GRA_ID equals gr.ID
										 where gra.LGA_ART_ID == result.Key.TecDocArticulId && gr.GRA_GRD_ID != null
										 select gra.LGA_ART_ID ).Any(),
							 HasAppliedCars = ( from la_1 in ctx.ArticleLinks
												join lat_1 in ctx.ArticleLinkToCarTypeLinks on la_1.ID equals lat_1.ArticleLinkID
												where la_1.LA_ART_ID == result.Key.TecDocArticulId
												select lat_1.CarTypeID ).Any(),
							 HasDescription = ( from ac in ctx.ArticleCriterionLinks
												where ac.ACR_ART_ID == result.Key.TecDocArticulId
												select ac.ACR_CRI_ID ).Any()
						 } );*/

					var res = ctx.ArticleLookups
						 .Where( la => partNumbers.Skip( i ).Take( Math.Min( 2000, partNumbers.Length - i ) ).Contains( la.SearchNumber ) && la.ARL_KIND == '1' )
						 .Select( la => new AdditionalInfo()
						 {
							 Key = new PartKey( la.Article.Supplier.Name, la.Article.ArticleNumber ),

							 TecDocArticulId = la.Article.ID,

							 PartDescription = la.Article.CompleteName.Tex_Text,

							 HasPics = ( from gra in ctx.GraphicsToArticleLinks
										 join gr in ctx.Graphics on gra.LGA_GRA_ID equals gr.ID
										 where gra.LGA_ART_ID == la.Article.ID && gr.GRA_GRD_ID != null
										 select gra.LGA_ART_ID ).Any(),
							 HasAppliedCars = ( from la_1 in ctx.ArticleLinks
												join lat_1 in ctx.ArticleLinkToCarTypeLinks on la_1.ID equals lat_1.ArticleLinkID
												where la_1.LA_ART_ID == la.Article.ID
												select lat_1.CarTypeID ).Any(),
							 HasDescription = ( from ac in ctx.ArticleCriterionLinks
												where ac.ACR_ART_ID == la.Article.ID
												select ac.ACR_CRI_ID ).Any()
						 } );

					list.AddRange( res );
				}
				return list.GroupBy( ai => ai.Key ).ToDictionary( g => g.Key, g => g.First() );
			}
		}
	}
}
