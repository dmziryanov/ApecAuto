using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Cms.Dac
{
	public static class SeoTecDocTextTemplatesDac
	{
		private static Func<CmsDataContext, Table<SeoTecDocTextTemplate>> _getSeoTecDocTextTemplates =
			CompiledQuery.Compile(
			( CmsDataContext dc ) =>
				dc.SeoTecDocTextTemplates );

		private static Func<CmsDataContext, string, SeoTecDocText> _getSeoTecDocText =
			CompiledQuery.Compile(
			( CmsDataContext dc, string pageUrl ) =>
				dc.SeoTecDocTexts.Where( t => t.PageUrl == pageUrl ).SingleOrDefault() );

		private static DataLoadOptions _getSeoTecDocTextDlo;

		static SeoTecDocTextTemplatesDac()
		{
			_getSeoTecDocTextDlo = new DataLoadOptions();
			_getSeoTecDocTextDlo.LoadWith<SeoTecDocText>( t => t.PageTitleTemplate );
			_getSeoTecDocTextDlo.LoadWith<SeoTecDocText>( t => t.PageFooterTemplate );
		}

		public static SeoTecDocTextTemplate[] GetTextTemplates()
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				return _getSeoTecDocTextTemplates( dc.DataContext ).ToArray();
			}
		}

		public static SeoTecDocText GetPageText( string pageUrl )
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				dc.DataContext.LoadOptions = _getSeoTecDocTextDlo;
				dc.DataContext.DeferredLoadingEnabled = false;
				return _getSeoTecDocText( dc.DataContext, pageUrl );
			}
		}

		public static SeoTecDocText AddPageText( string pageUrl, int pageTitleTemplateID, int pageFooterTemplateID )
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				SeoTecDocText text = new SeoTecDocText();
				text.PageUrl = pageUrl;
				text.PageTitleID = pageTitleTemplateID;
				text.PageFooterID = pageFooterTemplateID;
				dc.DataContext.SeoTecDocTexts.InsertOnSubmit( text );
				dc.DataContext.SubmitChanges();

				return GetPageText( pageUrl );
			}
		}
	}
}
