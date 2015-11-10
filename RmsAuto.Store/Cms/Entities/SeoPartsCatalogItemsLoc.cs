using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Cms.Entities
{
	/// <summary>
	/// Локализация для SeoPartsCatalogItem-а
	/// </summary>
	[ScaffoldTable(true)]
	[MetadataType(typeof(SeoPartsCatalogItemsLocMetaData))]
	public partial class SeoPartsCatalogItemsLoc
	{
	}

	[DisplayName("Раздел \"Производители\" (локализации)")]
	[DisplayColumn("Name")]
	[Sort("Name")]
	public partial class SeoPartsCatalogItemsLocMetaData
	{
		[DisplayName("Язык")]
		public object Localization1 { get; set; }

		[DisplayName("Название")]
		[Required(ErrorMessage = "не задано название раздела")]
		public object Name { get; set; }

		[DisplayName("Страница - title")]
		public object PageTitle { get; set; }

		[DisplayName("Страница - keywords")]
		public object PageKeywords { get; set; }

		[DisplayName("Страница - description")]
		public object PageDescription { get; set; }

		[DisplayName("Страница - текст")]
		[UIHint("Custom/Html")]
		public object Body { get; set; }

		[DisplayName("Страница - подвал")]
		[UIHint("Custom/Html")]
		public object PageFooter { get; set; }
	}
}
