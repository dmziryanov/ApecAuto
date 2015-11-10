using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Common.Caching;

namespace RmsAuto.Store.Web.TecDoc
{
	public class SeoTecDocTextTemplatesCache
	{
		#region Singleton

		public static SeoTecDocTextTemplatesCache Default
		{
			get { return _cache.CachedObject; }
		}
		//TODO: управление таймаутом кэша
		static SingleObjectCache<SeoTecDocTextTemplatesCache> _cache = new SingleObjectCache<SeoTecDocTextTemplatesCache>( null, 1 );

		#endregion

		#region SeoTecDocTextTemplates

		static Random _rnd = new Random();
		Dictionary<SeoTecDocTextTemplate.TextTypes, SeoTecDocTextTemplate[]> _dict;

		public SeoTecDocTextTemplatesCache()
		{
			_dict = SeoTecDocTextTemplatesDac.GetTextTemplates()
				.GroupBy( tt => tt.TextType )
				.ToDictionary( g => g.Key, g => g.ToArray() );
		}

		SeoTecDocTextTemplate[] GetTextTemplates( SeoTecDocTextTemplate.TextTypes textType )
		{
			SeoTecDocTextTemplate[] res;
			_dict.TryGetValue( textType, out res );
			return res;
		}

		SeoTecDocTextTemplate GetRandomTextTemplate( SeoTecDocTextTemplate.TextTypes textType )
		{
			var items = GetTextTemplates(textType);
			if( items != null && items.Length > 0 )
			{
				var index = _rnd.Next( items.Length );
				return items[ index ];
			}
			else
			{
				return null;
			}
		}

		string FormatRandomTextTemplate( SeoTecDocTextTemplate.TextTypes textType, string value )
		{
			return null;
			/*var textTemplate = GetRandomTextTemplate( textType );
			return textTemplate != null ? textTemplate.TextTemplate.Replace( "##", value ) : null;*/
		}

		public class SeoPageTextEnvelope
		{
			public string PageTitle { get; set; }
			public string PageFooter { get; set; }
		}

		public SeoPageTextEnvelope GetPageText( string pageUrl, string value )
		{
			var text = SeoTecDocTextTemplatesDac.GetPageText( pageUrl );
			if( text == null )
			{
				var title = GetRandomTextTemplate( SeoTecDocTextTemplate.TextTypes.PageTitle );
				var footer = GetRandomTextTemplate( SeoTecDocTextTemplate.TextTypes.PageFooter );
				if( title != null && footer != null )
				{
					text = SeoTecDocTextTemplatesDac.AddPageText( pageUrl, title.ID, footer.ID );
				}
			}

			return new SeoPageTextEnvelope
			{
				PageTitle = text != null ? text.PageTitleTemplate.TextTemplate.Replace( "##", value ) : value,
				PageFooter = text != null ? text.PageFooterTemplate.TextTemplate.Replace( "##", value ) : null
			};
		}

		#endregion
	}

}
