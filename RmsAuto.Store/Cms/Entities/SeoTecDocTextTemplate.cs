using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web;
using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Cms.Entities
{
    [ScaffoldTable(true)]
	[MetadataType( typeof( SeoTecDocTextTemplateMetadata ) )]
	public partial class SeoTecDocTextTemplate
    {
		public enum TextTypes
		{
			[Text( "страница - title" )]
			PageTitle,

			[Text( "страница - footer" )]
			PageFooter
		}
    }

	[DisplayName( "TecDoc - шаблоны title и footer" )]
	public partial class SeoTecDocTextTemplateMetadata
    {
		[DisplayName( "Тип" )]
		[Required( ErrorMessage = "не задан тип" )]
		[UIHint( "Enumeration", null, "EnumType", typeof( SeoTecDocTextTemplate.TextTypes ) )]
		public object TextType { get; set; }

		[DisplayName("Шаблон текста")]
        [Required(ErrorMessage= "не задан шаблон текста")]
        public object TextTemplate { get; set; }
	}
}
