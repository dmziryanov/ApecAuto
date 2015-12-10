using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.TechDoc.Search;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using RmsAuto.TechDoc.Cache;
using RmsAuto.TechDoc.Entities.Helpers;
using RmsAuto.TechDoc.Configuration;
using System.Data.Linq;
using RmsAuto.Common.Data;

namespace RmsAuto.TechDoc.Entities.TecdocBase
{
	public partial class TecdocBaseDataContext
	{
		private readonly VisibilityFilterProvider _filterProvider = new VisibilityFilterProvider();

		public Dictionary<int, Country> ListCountries()
		{
			return this.Countries.OrderBy( co => co.Name.Tex_Text ).ToDictionary( co => co.ID );
		}

		public List<Manufacturer> ListManufacturers( bool showAll, IEnumerable<int> countriesIds )
		{
            //var manufacturers = this.Manufacturers.Where( m => m.IsCarManufacturer == true || m.IsTruckManufacturer == true ).OrderBy( m => m.Name ).ToList();
            //_filterProvider.ApplyFilter(manufacturers, VisibilityFilterType.Country);

            var manufacturers1 = (from m in this.Manufacturers
                                 where (m.IsCarManufacturer == 1 || m.IsTruckManufacturer == 1)
                                 select m).Distinct().OrderBy(m => m.Name);
			
			var manufacturers = manufacturers1.ToList();

			if( !showAll )
			{
				_filterProvider.ApplyFilter( manufacturers, VisibilityFilterType.Manufacturer );
			}

			return manufacturers;
		}
		public Manufacturer GetManufacturer( int id, bool showAll, IEnumerable<int> countriesIds )
		{
            //var manufacturers = this.Manufacturers.Where( m => m.ID == id && ( m.IsCarManufacturer == true || m.IsTruckManufacturer == true ) ).ToList();
            //_filterProvider.ApplyFilter( manufacturers, VisibilityFilterType.Country );

            
            int res;
            if (countriesIds.Count() > 0)
            {
                res = this.CountryDeliveries.Where(cd => countriesIds.Contains(cd.CountryID) && cd.ManufacturerID == id).Count();
            }
            else
            {
                res = 1;
            }
            if (res > 0)
            {
                var manufacturers = this.Manufacturers.Where(m => m.ID == id).ToList();
                if (!showAll)
                {
                    _filterProvider.ApplyFilter(manufacturers, VisibilityFilterType.Manufacturer);
                }

                return manufacturers.SingleOrDefault();
            }
            return null;
		}

		public Manufacturer GetManufacturerByName( string name, bool showAll, IEnumerable<int> countriesIds )
		{
			//var manufacturers = this.Manufacturers.Where( m => m.ID == id && ( m.IsCarManufacturer == true || m.IsTruckManufacturer == true ) ).ToList();
			//_filterProvider.ApplyFilter( manufacturers, VisibilityFilterType.Country );


			var manufacturers = this.Manufacturers.Where( m => m.Name == name )
				.Where( m => this.CountryDeliveries.Any( cd => countriesIds.Contains( cd.CountryID ) && cd.ManufacturerID == m.ID ) ).ToList();
			if( !showAll )
			{
				_filterProvider.ApplyFilter( manufacturers, VisibilityFilterType.Manufacturer );
			}


			return manufacturers.SingleOrDefault();
		}


		public int LookupManufacturerId( string mName )
		{
			return this.Manufacturers.Where( m => m.Name == mName ).Select( m => m.ID ).FirstOrDefault();
		}
		public int LookupModelId( int manufacturerId, string modelName )
		{
			return this.Models.Where( m => m.Manufacturer.ID == manufacturerId && m.Name.Tex_Text == modelName ).Select( m => m.ID ).FirstOrDefault();
		}
		public int LookupModificationId( int modelId, string mName )
		{
			return this.CarTypes.Where( ct => ct.Model.ID == modelId && ct.Name.Tex_Text == mName ).Select( ct => ct.ID ).FirstOrDefault();
		}

		/// <summary>
		/// Список моделей по производителю для сайта
		/// </summary>
		/// <param name="manufacturerID">ID производителя</param>
		/// <param name="isCarModel">Для машинок онли или нет</param>
		/// <param name="showAll">Показывать все или фильтровать по видимости</param>
		public List<Model> ListModels( int manufacturerID, bool isCarModel, bool showAll, IEnumerable<int> countriesIds )
		{
            //var ret = from m in this.Models
            //          where m.MOD_MFA_ID == manufacturerID &&
            //                m.IsCarModel == ( isCarModel ? 1 : 0 ) &&
            //                m.MOD_CDS_ID.HasValue
            //          orderby m.Name.Text
            //          select m;
            //var models = ret.ToList();
            //_filterProvider.ApplyFilter( models, VisibilityFilterType.Country );

            var ret = from m in this.Models
                       where (m.MOD_MFA_ID == manufacturerID &&
                             m.IsCarModel == (isCarModel ? 1 : 0) &&
                             m.MOD_CDS_ID.HasValue)
                      orderby m.Name.Tex_Text
                      select m;

            var models = ret.Distinct().OrderBy(m => m.Name.Tex_Text).ToList();

            if (!showAll)
			{
				_filterProvider.ApplyFilter( models, VisibilityFilterType.Model );
			}

			return models;
		}

		/// <summary>
		/// Получить модель
		/// </summary>
		/// <param name="showAll">Показывать все или фильтровать по видимости</param>
		public Model GetModel( int modelId, bool showAll, IEnumerable<int> countriesIds )
		{
            int res;
       /*     if (countriesIds.Count() > 0)
            {
                res = this.CountryDeliveries.Where(cd => countriesIds.Contains(cd.CountryID) && cd.ModelID == modelId).Count();
            }
            else
            {
                res = 1;
            }*/

            //if (res > 0)
            {
                var models = this.Models.Where(m => m.MOD_ID == modelId).ToList();
                if (!showAll)
                {
                    _filterProvider.ApplyFilter(models, VisibilityFilterType.Model);
                }

                return models.SingleOrDefault();
            }
            return null;
		}

		/// <summary>
		/// Возвращает все модели, по которым есть запчасти, вне зависимости от класса (грузовик/легковая)
		/// </summary>
		/// <param name="manufacturerID">Производитель</param>
		/// <returns>Список моделей</returns>
		public List<Model> ListModels( int manufacturerID )
		{
			return ( from m in this.Models
					 where m.MOD_MFA_ID == manufacturerID &&
						   m.MOD_CDS_ID.HasValue
					 select m ).OrderBy( m => m.Name.Tex_Text ).ToList();
		}

		/*/// <summary>
		/// Список всех моделей без привязки к производителям и наличию запчастей по ним
		/// </summary>
		/// <returns>Список моделей</returns>
		public List<Model> ListModels()
		{
			return ( from m in this.Models
					 where m.MOD_CDS_ID.HasValue
					 select m ).OrderBy( m => m.Name.Text ).ToList();
		}*/


		public List<CarType> ListModifications( int modelID, bool showAll, IEnumerable<int> countriesIds )
		{
            //var ret = ( from ct in this.CarTypes
            //            where ct.TYP_MOD_ID == modelID
            //            select ct ).OrderBy( ct => ct.Name.Text ).ToList();

            //_filterProvider.ApplyFilter( ret, VisibilityFilterType.Country );

            var ret = (from ct in this.CarTypes
                       where ct.TYP_MOD_ID == modelID select ct).OrderBy(ct => ct.Name.Tex_Text).ToList();

			if( !showAll )
			{
				_filterProvider.ApplyFilter( ret, VisibilityFilterType.Modification );
			}

			return ret.ToList();
		}

        public CarType GetModification( int modificationID, bool showAll, IEnumerable<int> countriesIds)
		{
            //var ret = ( from ct in this.CarTypes
            //            where ct.ID == modificationID
            //            select ct ).ToList();

            //_filterProvider.ApplyFilter( ret, VisibilityFilterType.Country );
            int res;

   /*         if (countriesIds.Count() > 0)
            {
                res = this.CountryDeliveries.Where(cd => countriesIds.Contains(cd.CountryID) && cd.TypeID == modificationID).Count();
            }
            else
            {
                res = 1;
            }
            if (res > 0)*/
            {
                var ret = this.CarTypes.Where(ct => ct.ID == modificationID).ToList();
                if (!showAll)
                {
                    _filterProvider.ApplyFilter(ret, VisibilityFilterType.Modification);
                }

                return ret.SingleOrDefault();
            }
            return null;
		}

		/// <summary>
		/// Возвращает модификации для заданной модели для администрирования
		/// </summary>
		/// <param name="modelID">Id модели</param>
		/// <returns>Список модификаций</returns>
		public List<CarType> ListModelModifications( int modelID )
		{
			return ( from ct in this.CarTypes
					 where ct.TYP_MOD_ID == modelID
					 select ct ).OrderBy( ct => ct.Name.Tex_Text ).ToList();
		}

		public string LookupModificationName( int modificationId )
		{
			return ( from ct in this.CarTypes
					 where ct.ID == modificationId
					 select ct.Name.Tex_Text ).FirstOrDefault();
		}

		//TODO: зачем modelID ???
		public List<SearchTreeNodeHelper> GetTree( /*int modelID,*/ int modificationID )
		{
			List<SearchTreeNodeHelper> list = ( from lt in this.ArticleLinkToCarTypeLinks
												join lgs in this.GenericArticleToStrLinks on lt.GenericArticleID equals lgs.GenericArticleID
												where lgs.SearchTreeNode.STR_TYPE == 1 &&
													  lt.CarType.ID == modificationID /*&&
													  lt.CarType.TYP_MOD_ID == modelID*/
												select new SearchTreeNodeHelper( lgs.SearchTreeNode, lt.CarType ) ).Distinct().ToList();

			foreach( SearchTreeNodeHelper item in list )
			{
				item.Children = list.Where( i => i.ParentSearchTreeNodeID == item.SearchTreeNodeID ).OrderBy( i => i.Text ).ToList();
			}

			return list.Where( i => i.ParentSearchTreeNodeID == null ).OrderBy( i => i.Text ).ToList();
		}

		public SearchTreeNodeHelper GetTreeItem( int modificationID, int searchTreeNodeID )
		{
			List<SearchTreeNodeHelper> list = ( from lt in this.ArticleLinkToCarTypeLinks
												join lgs in this.GenericArticleToStrLinks on lt.GenericArticleID equals lgs.GenericArticleID
												where lgs.SearchTreeNode.STR_TYPE == 1 &&
													  lt.CarType.ID == modificationID
													  && lgs.SearchTreeNode.ID == searchTreeNodeID
												select new SearchTreeNodeHelper( lgs.SearchTreeNode, lt.CarType ) ).Distinct().ToList();

			return list.SingleOrDefault();
		}

		public List<PartsListItem> ListParts( int modificationId, int searchTreeNodeId )
		{
			var res = from lat in this.ArticleLinkToCarTypeLinks
					  join lgs in this.GenericArticleToStrLinks on lat.GenericArticleID equals lgs.GenericArticleID
					  join la in this.ArticleLinks on lat.ArticleLinkID equals la.ID
					  where lgs.StrID == searchTreeNodeId && lat.CarTypeID == modificationId
					  select new PartsListItem( la.Article )
						{
							//SupplierBrand = la.Article.Supplier.Name,
							HasPics = ( from gra in this.GraphicsToArticleLinks
                                        join gr in this.Graphics on gra.LGA_GRA_ID equals gr.ID
                                        where gra.LGA_ART_ID == la.LA_ART_ID && gr.GRA_GRD_ID != null
										  select gra.LGA_ART_ID ).Any(),
							HasAppliedCars = ( from la_1 in this.ArticleLinks
												 join lat_1 in this.ArticleLinkToCarTypeLinks on la_1.ID equals lat_1.ArticleLinkID
												 where la_1.LA_ART_ID == la.LA_ART_ID
												 select lat_1.CarTypeID ).Any(),
							HasDescription = ( from ac in this.ArticleCriterionLinks
											  where ac.ACR_ART_ID == la.LA_ART_ID
											  select ac.ACR_CRI_ID ).Any()
						};
            /*OrderBy( p => p.Article.Supplier.Name )*/
			return res.ToList().Distinct().OrderBy( p => p.Article.ArticleNumber ).ToList();
		}

		public IEnumerable<CarType> GetAppliedCars( int artID )
		{
			return this.ArticleLinks
                       .Where( al => al.LA_ART_ID == artID )
                       .SelectMany( al => al.ArticleLinkToCarTypeLinks )
					   .Select( a => a.CarType ).Distinct().ToList();
		}

		public PartInfo GetPartInfo( int artID )
		{
			var res = this.Articles.Where( a => a.ID == artID )
						  .Select( a => new Helpers.PartInfo( a ) ).FirstOrDefault();

			if( res == null )
			{
				throw new ArgumentException( "artID" );
			}

			res.OriginalNumbers = ( from al in this.ArticleLookups
									where al.ARL_KIND == TecdocConfiguration.Current.OriginalType && al.ARL_ART_ID == res.Article.ID
									select al.DisplayNumber ).ToList();
            return res;
		}

		public IEnumerable<KeyValuePair<string, string>> GetPartAddInfo( int artID )
		{
			var res = this.ArticleCriterionLinks
						  .Where( acl => acl.ACR_ART_ID == artID )
						  .Select( acl => new KeyValuePair<string, string>( acl.ArticleCriterion.FullName.Tex_Text,
																		    acl.ValueIfNumeric == null ? acl.ValueIfString.Tex_Text : acl.ValueIfNumeric ) ).ToList();
            return res;
		}

		public ImageInfo GetImage( int id )
		{
			var noLang = TecdocConfiguration.Current.NoLanguage;
			var curLang = TecdocConfiguration.Current.LanguageId;

			var graphics = this.Graphics.Where( g => g.GRA_GRD_ID == id && ( g.GRA_LNG_ID == noLang || g.GRA_LNG_ID == curLang ) ).FirstOrDefault();
			if( graphics != null )
			{
				return new ImageInfo( id, graphics.GraphicsData.GRD_GRAPHIC, graphics.DocumentType.DOC_EXTENSION, 0 );
			}

			return null;
		}

		public List<ImageInfo> GetImages( int articleID )
		{
			var res = this.GraphicsToArticleLinks.Where( gal => gal.LGA_ART_ID == articleID && gal.Graphics.GRA_GRD_ID != null )
						  .Select( gal => new ImageInfo( gal.Graphics.GRA_GRD_ID.Value,
													   null, //gal.Graphics.GraphicsData.GRD_GRAPHIC,
													   gal.Graphics.DocumentType.DOC_EXTENSION,
													   gal.LGA_SORT ) );
			return res.ToList();
		}

		#region Helpers

		internal List<Manufacturer> ListManufacturers()
		{
			return this.Manufacturers.OrderBy( m => m.Name ).ToList();
		}


		internal List<Supplier> GetSuppliers()
		{
			return ( from s in this.Suppliers
					 orderby s.Name
					 select s ).ToList();
		}

		#endregion

	}
}
