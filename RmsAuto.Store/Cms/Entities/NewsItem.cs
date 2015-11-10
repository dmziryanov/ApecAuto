using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Cms.Entities
{
	[ScaffoldTable( true )]
	[MetadataType( typeof( NewsItemMetadata ) )]
	public partial class NewsItem
	{
	}

	[DisplayName( "Новости" )]
	[Sort( "NewsItemDate descending" )]
	public partial class NewsItemMetadata
	{
		[DisplayName( "Дата новости" )]
		[Required( ErrorMessage = "не задана дата новости" )]
		public object NewsItemDate { get; set; }

		[DisplayName( "Заголовок" )]
		[Required( ErrorMessage = "не задан заголовок" )]
		public object NewsItemHeader { get; set; }

		[UIHint( "Custom/Html" )]
		[DisplayName( "Текст новости" )]
		[Required( ErrorMessage = "не задан текст новости" )]
		public object NewsItemText { get; set; }

		[DisplayName( "Аннотация" )]
		public object NewsItemAnnotation { get; set; }

		[DisplayName( "Видимость" )]
		[Required( ErrorMessage = "не задана видимость" )]
		public object NewsItemVisible { get; set; }

		[DisplayName("Акция")]
		//[Required(ErrorMessage = "не задана принадлежность к акции")]
		public object IsDiscount { get; set; }

		[UIHint( "Custom/FileForeignKey" )]
		[DisplayName( "Иконка (90×60)" )]
		public object IconFile { get; set; }

		[DisplayName("Язык")]
		public object Localization1 { get; set; }
	}
}
