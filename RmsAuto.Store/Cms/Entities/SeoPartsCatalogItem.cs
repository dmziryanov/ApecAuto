using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using RmsAuto.Common.DataAnnotations;
using System.Threading;
using RmsAuto.Store.Web;

namespace RmsAuto.Store.Cms.Entities
{
	[ScaffoldTable( true )]
	[MetadataType( typeof( SeoPartsCatalogItemMetadata ) )]
	public partial class SeoPartsCatalogItem
	{
		public string Name
		{
			get
			{
				if (/*Thread.CurrentThread.CurrentCulture.Name*/ SiteContext.CurrentCulture != "ru-RU")
				{
					return this.SeoPartsCatalogItemsLocs.SingleOrDefault() != null ? this.SeoPartsCatalogItemsLocs.SingleOrDefault().Name : this.NameRU;
				}
				return NameRU;
			}
			set { NameRU = value; }
		}

		public string Body
		{
			get
			{
				if (Thread.CurrentThread.CurrentCulture.Name != "ru-RU")
				{
					return this.SeoPartsCatalogItemsLocs.SingleOrDefault() != null ? this.SeoPartsCatalogItemsLocs.SingleOrDefault().Body : this.BodyRU;
				}
				return BodyRU;
			}
			set { BodyRU = value; }
		}

		public string PageTitle
		{
			get
			{
				if (Thread.CurrentThread.CurrentCulture.Name != "ru-RU")
				{
					return this.SeoPartsCatalogItemsLocs.SingleOrDefault() != null ? this.SeoPartsCatalogItemsLocs.SingleOrDefault().PageTitle : this.PageTitleRU;
				}
				return PageTitleRU;
			}
			set { PageTitleRU = value; }
		}

		public string PageKeywords
		{
			get
			{
				if (Thread.CurrentThread.CurrentCulture.Name != "ru-RU")
				{
					return this.SeoPartsCatalogItemsLocs.SingleOrDefault() != null ? this.SeoPartsCatalogItemsLocs.SingleOrDefault().PageKeywords : this.PageKeywordsRU;
				}
				return PageKeywordsRU;
			}
			set { PageKeywordsRU = value; }
		}

		public string PageDescription
		{
			get
			{
				if (Thread.CurrentThread.CurrentCulture.Name != "ru-RU")
				{
					return this.SeoPartsCatalogItemsLocs.SingleOrDefault() != null ? this.SeoPartsCatalogItemsLocs.SingleOrDefault().PageDescription : this.PageDescriptionRU;
				}
				return PageDescriptionRU;
			}
			set { PageDescriptionRU = value; }
		}

		public string PageFooter
		{
			get
			{
				if (Thread.CurrentThread.CurrentCulture.Name != "ru-RU")
				{
					return this.SeoPartsCatalogItemsLocs.SingleOrDefault() != null ? this.SeoPartsCatalogItemsLocs.SingleOrDefault().PageFooter : this.PageFooterRU;
				}
				return PageFooterRU;
			}
			set { PageFooterRU = value; }
		}
	}

	[DisplayName( "Раздел \"Производители\"" )]
	[DisplayColumn( "Name" )]
	[Sort( "Name" )]
	public partial class SeoPartsCatalogItemMetadata
	{
		[ReadOnly(true)]
		[DisplayName( "Родительский раздел" )]
		public object ParentItem { get; set; }

		[ScaffoldColumn( false )]
		[DisplayName( "Подразделы" )]
		public object ChildItems { get; set; }

		[DisplayName( "Название" )]
		[Required( ErrorMessage = "не задано название раздела" )]
		public object NameRU { get; set; }

		[DisplayName( "Основной текст" )]
		[UIHint( "Custom/Html" )]
		public object BodyRU { get; set; }

		[DisplayName( "Url-код" )]
		[Required( ErrorMessage = "не задан url-код" )]
		[RegularExpression( @"^[\w\d]+|~$", ErrorMessage = "url-код может состоять только из цифр и латинских букв" )]
		public object UrlCode { get; set; }

		[DisplayName( "Видимость" )]
		public object Visible { get; set; }
		
		[DisplayName( "Страница - title" )]
		public object PageTitleRU { get; set; }

		[DisplayName( "Страница - keywords" )]
		public object PageKeywordsRU { get; set; }

		[DisplayName( "Страница - description" )]
		public object PageDescriptionRU { get; set; }

		[DisplayName( "Страница - подвал" )]
		[UIHint( "Custom/Html" )]
		public object PageFooterRU { get; set; }

		[DisplayName( "Приоритет" )]
		public object Priority { get; set; }

		[DisplayName( "Служебная страница" )]
		public object IsServicePage { get; set; }
	}
}
