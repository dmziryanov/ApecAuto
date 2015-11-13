using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Cms.Dac
{
    public static class SeoShortLinksDac
    {
        private static Func<CmsDataContext, int, SeoShortLink> _getSeoShortLink =
            CompiledQuery.Compile(
            ( CmsDataContext dc, int id ) =>
                dc.SeoShortLinks.Where( o => o.SeoLinksID == id ).SingleOrDefault() );


        public static SeoShortLink GetSeoShortLink( int id )
        {
            using (var dc = new DCFactory<CmsDataContext>())
            {
                return _getSeoShortLink( dc.DataContext, id );
            }
        }

        public static List<int> GetLastLink( int countItem, string culture )
        {
            var DC = new DCFactory<CmsDataContext>();
            return DC.DataContext.SeoShortLinks.Where( t => t.Localization == culture ).OrderByDescending( t => t.UpdateDate ).Take( countItem )
                .Select( t => t.SeoLinksID ).ToList<int>();
        }
    }
}
