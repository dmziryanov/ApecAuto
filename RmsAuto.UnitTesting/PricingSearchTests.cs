using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.BL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace RmsAuto.UnitTesting
{
	[TestClass]
	public class PricingSearchTests
	{
		[TestMethod]
		public void Test1()
		{
			//var res1 = PricingSearch.SearchSpareParts( "1001", false );
			//var res2 = PricingSearch.SearchSpareParts( "1001", true );

			//Debug.Assert( res2.Length > res1.Length );

			var res3 = PricingSearch.SearchSparePartManufactures( "1001", false );
			var res4 = PricingSearch.SearchSparePartManufactures( "1001", true );

			Debug.Assert( res4.Length > res3.Length );

			var res5 = PricingSearch.SearchSpareParts( "1001", "SPIDAN", false );
			var res6 = PricingSearch.SearchSpareParts( "1001", "SPIDAN", true );

			Debug.Assert( res6.Length > res5.Length );

		}
	}
}
