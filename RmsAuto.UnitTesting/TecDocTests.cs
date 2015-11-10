using RmsAuto.Common.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RmsAuto.TechDoc.Entities.TecdocBase;
using System.Linq;
using System.Diagnostics;
using RmsAuto.TechDoc.Entities;
using RmsAuto.TechDoc;
using System.Xml;
using RmsAuto.TechDoc.Configuration;
using System.Configuration;
using System.Web;
using System.Data.Linq;
using RmsAuto.TechDoc.Entities.Helpers;
using RmsAuto.Common.Data;

namespace RmsAuto.UnitTesting
{
	[TestClass]
	public class TecDocTests
	{
		#region Base

		[TestMethod]
		public void ListCountriesTest()
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				var cs = ctx.ListCountries();
				Assert.IsNotNull( cs );
				Assert.IsTrue( cs.Count > 0 );
			}
		}

		[TestMethod]
		public void ListMfrsTest()
		{

			using( var ctx = new TecdocBaseDataContext() )
			{
				var mfrs = ctx.ListManufacturers( true, new int[] { 185, 246 } );
				Assert.IsNotNull( mfrs );
			}
		}

		[TestMethod]
		public void PartTypeTreeTest()
		{
			int typ_id = 15270; //   525 i (192 HP) Modification
			//int mod_id = 1449;  //   BMW 5 (E39) Model
			int st_item_id = 10001; //  Номер айтема в дереве поиска

			using( TecdocBaseDataContext ctx = new TecdocBaseDataContext() )
			{
				ctx.Log = new DebuggerWriter();
				var tree = ctx.GetTree( typ_id );

				var itemName = ctx.GetTreeItem( typ_id, st_item_id );
			}
		}

		[TestMethod]
		public void ListPartsTest()
		{
			int typID = 15270;  //   525 i (192 HP) Modification
			int catID = 10142;  //   Аккум

			int badTypID = -1;
			int badCatID = -1;

			using( TecdocBaseDataContext ctx = new TecdocBaseDataContext() )
			{
				XmlDocument doc = new XmlDocument();
				try
				{
					var arr1 = ctx.ListParts( typID, catID );
					Assert.IsTrue( arr1.Count > 0, "Вернулся пустой документ для возможных параметров" );
					var arr2 = ctx.ListParts( badTypID, badCatID );
					Assert.IsTrue( arr2.Count == 0, "Вернулся непустой документ для невозможных параметров" );
				}
				catch( Exception e )
				{
					if( e is AssertFailedException )
					{
						throw;
					}
					else
					{
						Assert.Fail( String.Format( "Не могу загрузить список ЗЧ для {0}/{1}", typID, catID ) );
					}
				}
			}
		}

		[TestMethod]
		public void GetAppliedCarsTest()
		{
			int artID = 638819;
			int badArtID = -1;

			using( TecdocBaseDataContext ctx = new TecdocBaseDataContext() )
			{
				ctx.Log = new DebuggerWriter();
				try
				{
					var res = ctx.GetAppliedCars( artID );
					Assert.IsTrue( res.Count() > 0, "Вернулся пустой документ для возможного артикула" );
                    res = ctx.GetAppliedCars( badArtID );
                    Assert.IsTrue(res.Count() == 0, "Вернулся непустой документ для невозможного артикула");
				}
				catch( Exception e )
				{
					if( e is AssertFailedException )
					{
						throw;
					}
					else
					{
						Assert.Fail( String.Format( "Не могу загрузить список машин для ЗЧ с артикулом {0}", 638819 ) );
					}
				}
			}
		}

		[TestMethod]
		public void ListModelsTest()
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				var mfr = GetManufacturer( null );
                var arr1 = ctx.ListModels(mfr.ID, true, true, new int[] { 185, 246 });
                var arr2 = ctx.ListModels(mfr.ID, true, false, new int[] { 185, 246 });
				Assert.IsTrue( arr1.Count >= arr2.Count );

				var curModels = ctx.ListModels( mfr.ID );

				
			}
		}

		[TestMethod]
		public void ListModificationsTest()
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				var mfr = GetManufacturer( null, true );
				var models = mfr.Models.ToList();
				if( mfr.Models.Count > 0 )
				{
                    var arr1 = ctx.ListModifications(models[0].ID, true, new int[] { 185, 246 });
                    var arr2 = ctx.ListModifications(models[0].ID, false, new int[] { 185, 246 });
					Assert.IsTrue( arr2.Count <= arr1.Count );
				}
			}
		}

		[TestMethod]
		public void GetPartInfoTest()
		{
			int artID = 638819;
			int badArtID = -1;

			using( var ctx = new TecdocBaseDataContext() )
			{
				ctx.Log = new DebuggerWriter();
				var val1 = ctx.GetPartInfo( artID );
				try
				{
					var val2 = ctx.GetPartInfo( badArtID );
				}
				catch( ArgumentException )
				{

				}
			}
		}

		[TestMethod]
		public void GetPartAddInfoTest()
		{
			int artID = 638819;
			int badArtID = -1;

			using( var ctx = new TecdocBaseDataContext() )
			{
				ctx.Log = new DebuggerWriter();
				XmlDocument xml1 = new XmlDocument();
				XmlDocument xml2 = new XmlDocument();
				var val1 = ctx.GetPartAddInfo( artID );
				var val2 = ctx.GetPartAddInfo( badArtID );
				Assert.IsTrue( val1.Count() >= val2.Count() );
                Assert.IsTrue( val2.Count() == 0);
			}
		}

		[TestMethod]
		public void GetImageTest()
		{

		}

		[TestMethod]
		public void GetImagesTest()
		{
			int artID = 638819;
			int badArtID = -1;

			using( var ctx = new TecdocBaseDataContext() )
			{
				ctx.Log = new DebuggerWriter();
				var val1 = ctx.GetImages( artID );
				var val2 = ctx.GetImages( badArtID );
				Assert.IsTrue( val2.Count == 0 );
			}
		}

		#endregion

		#region Search engine



		#endregion

		#region Helpers

		string MfrBrand = "BMW";
		RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer GetManufacturer( string mfrBrand )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				var toRet = ctx.Manufacturers.Where( m => m.Name == ( mfrBrand == null ? this.MfrBrand : mfrBrand ) ).FirstOrDefault();
				Assert.IsNotNull( toRet );
				return toRet;
			}
		}
		RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer GetManufacturer( string mfrBrand, bool withModels )
		{
			using( var ctx = new TecdocBaseDataContext() )
			{
				if( withModels )
				{
					var dlo = new DataLoadOptions();
					dlo.LoadWith<RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer>( m => m.Models );
					ctx.LoadOptions = dlo;
				}
				var toRet = ctx.Manufacturers.Where( m => m.Name == ( mfrBrand == null ? this.MfrBrand : mfrBrand ) ).FirstOrDefault();
				Assert.IsNotNull( toRet );
				return toRet;
			}
		}

		#endregion
	}
}
