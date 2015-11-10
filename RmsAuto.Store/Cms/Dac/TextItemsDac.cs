using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Cms.Dac
{
	public static class TextItemsDac
	{
		private static Func<CmsDataContext, string, string, TextItem> _getTextItem =
			CompiledQuery.Compile(
			( CmsDataContext dc, string id, string culture ) =>
                dc.TextItems.Where( o => o.TextItemID == id && o.Localization == culture ).SingleOrDefault() );


		public static TextItem GetTextItem( string id, string culture )
		{
            using (var dc = new DCWrappersFactory<CmsDataContext>())
			{
                TextItem rezTI = _getTextItem( dc.DataContext, id, culture );
                if (rezTI == null)
                    rezTI = _getTextItem( dc.DataContext, id, "ru-RU" );
				return rezTI;
			}
		}

	}
}
