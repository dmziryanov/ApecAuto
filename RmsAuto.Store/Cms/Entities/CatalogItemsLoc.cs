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
	/// Локализация для CatalogItem-а
	/// </summary>
	[ScaffoldTable(true)]
	[MetadataType(typeof(CatalogItemsLocMetaData))]
	public partial class CatalogItemsLoc
	{
		//public int CatalogItemID { get; set; }
		//public string Localization { get; set; }
		//public string CatalogItemName { get; set; }
		//public string CatalogItemImageUrl { get; set; }
		//public string PageTitle { get; set; }
		//public string PageKeywords { get; set; }
		//public string PageDescription { get; set; }
		//public string PageFooter { get; set; }
		//public string PageBody { get; set; }
	}

	[DisplayName("Раздел сайта (локализации)")]
	[DisplayColumn("CatalogItemName")]
	[Sort("CatalogItemName")]
	public partial class CatalogItemsLocMetaData
	{
		[DisplayName("Раздел")]
		//[UIHint("Custom/CatalogItemForeignKey")]
		public object CatalogItemID { get; set; }
		//[DisplayName("ID")]
		//public object CatalogItemID { get; set; }

		[DisplayName("Язык")]
		public object Localization1 { get; set; }

		[DisplayName("Название")]
		[Required(ErrorMessage = "не задано название раздела")]
		public object CatalogItemName { get; set; }

		[DisplayName("Страница - title")]
		public object PageTitle { get; set; }

		[DisplayName("Страница - keywords")]
		public object PageKeywords { get; set; }

		[DisplayName("Страница - description")]
		public object PageDescription { get; set; }

		[DisplayName("Страница - текст")]
		[UIHint("Custom/Html")]
		public object PageBody { get; set; }

		[DisplayName("Страница - подвал")]
		[UIHint("Custom/Html")]
		public object PageFooter { get; set; }
	}
}
