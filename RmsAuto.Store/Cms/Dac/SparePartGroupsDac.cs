using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Cms.Dac
{
	public static class SparePartGroupsDac
	{
		private static Func<CmsDataContext, IQueryable<SparePartGroup>> _getSparePartGroups =
		CompiledQuery.Compile(
		( CmsDataContext dc ) =>
			dc.SparePartGroups.Where( g => g.Visible ) );


		public static SparePartGroup[] GetSparePartGroups()
		{
			using( var dc = new DCWrappersFactory<CmsDataContext>() )
			{
				return _getSparePartGroups( dc.DataContext ).ToArray();
			}
		}

	}
}
