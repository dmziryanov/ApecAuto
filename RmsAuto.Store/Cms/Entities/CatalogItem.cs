using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Cms.Entities
{
    [ScaffoldTable(true)]
    [MetadataType(typeof(CatalogItemMetadata))]
    public partial class CatalogItem
    {
        public static bool ItemIsInserting;
        public static byte GetBannerCount(CmsDataContext dc, Int32 catalogItemID)
        {
            var el =
                    from o in dc.CatalogItems
                    where o.CatalogItemID == catalogItemID
                    select o.BannerCount;

            return (byte)el.First();
        }

        public static byte GetBannerCount(Int32 catalogItemID)
        {
            using (var dc = new CmsDataContext())
            {
                return GetBannerCount(dc, catalogItemID);
            }
        }
        partial void OnCatalogItemMenuTypeChanged()
        {
            foreach (CatalogItem ci in this.ChildItems)
            {
                ci.CatalogItemMenuType = this.CatalogItemMenuType;
            }
        }
        //partial void OnCatalogItemCodeChanging(string oldValue, string newValue)
        //{
        //    if (ItemIsInserting && UrlExists(newValue) ||
        //    (oldValue != null && newValue != null) && (oldValue != newValue) && UrlExists(newValue))
        //        throw new Exception("UrlCode с именем \"" + newValue + "\" уже существует");
        //}

        bool UrlExists(string value)
        {
            return value == null ? false :
            (new CmsDataContext()).CatalogItems.Any(c => c.CatalogItemCode == value);
        }

		public string CatalogItemName
		{
			get
			{
				if (LocalizationData != null && _curLocale != "ru-RU")
				{
					CatalogItemsLoc cil = LocalizationData.ContainsKey(_curLocale) ? LocalizationData[_curLocale] : null;
					return cil != null ? cil.CatalogItemName : CatalogItemNameRU;
				}
				return CatalogItemNameRU;
			}
			set { this.CatalogItemNameRU = value; }
		}
		public string CatalogItemImageUrl
		{
			get
			{
				if (LocalizationData != null && _curLocale != "ru-RU")
				{
					CatalogItemsLoc cil = LocalizationData.ContainsKey(_curLocale) ? LocalizationData[_curLocale] : null;
					return cil != null ? cil.CatalogItemImageUrl : CatalogItemImageUrlRU;
				}
				return CatalogItemImageUrlRU;
			}
			set { this.CatalogItemImageUrlRU = value; }
		}
		public string PageTitle
		{
			get
			{
				if (LocalizationData != null && _curLocale != "ru-RU")
				{
					CatalogItemsLoc cil = LocalizationData.ContainsKey(_curLocale) ? LocalizationData[_curLocale] : null;
					return cil != null ? cil.PageTitle : PageTitleRU;
				}
				return PageTitleRU;
			}
			set { this.PageTitleRU = value; }
		}
		public string PageKeywords
		{
			get
			{
				if (LocalizationData != null && _curLocale != "ru-RU")
				{
					CatalogItemsLoc cil = LocalizationData.ContainsKey(_curLocale) ? LocalizationData[_curLocale] : null;
					return cil != null ? cil.PageKeywords : PageKeywordsRU;
				}
				return PageKeywordsRU;
			}
			set { this.PageKeywordsRU = value; }
		}
		public string PageDescription
		{
			get
			{
				if (LocalizationData != null && _curLocale != "ru-RU")
				{
					CatalogItemsLoc cil = LocalizationData.ContainsKey(_curLocale) ? LocalizationData[_curLocale] : null;
					return cil != null ? cil.PageDescription : PageDescriptionRU;
				}
				return PageDescriptionRU;
			}
			set { this.PageDescriptionRU = value; }
		}
		public string PageFooter
		{
			get
			{
				if (LocalizationData != null && _curLocale != "ru-RU")
				{
					CatalogItemsLoc cil = LocalizationData.ContainsKey(_curLocale) ? LocalizationData[_curLocale] : null;
					return cil != null ? cil.PageFooter : PageFooterRU;
				}
				return PageFooterRU;
			}
			set { this.PageFooterRU = value; }
		}
		public string PageBody
		{
			get
			{
				if (LocalizationData != null && _curLocale != "ru-RU")
				{
					CatalogItemsLoc cil = LocalizationData.ContainsKey(_curLocale) ? LocalizationData[_curLocale] : null;
					return cil != null ? cil.PageBody : PageBodyRU;
				}
				return PageBodyRU;
			}
			set { this.PageBodyRU = value; }
		}

		public Dictionary<string, CatalogItemsLoc> LocalizationData { get; set; }

		/// <summary>
		/// Строковое представление текущей локали
		/// </summary>
		private string _curLocale { get { return Thread.CurrentThread.CurrentCulture.Name; } }

		//решили не использовать т.к. нет у нас тут еще SiteContext.Current.InternalFranchName
		partial void OnLoaded()
		{
			//_LocalizationData = new Dictionary<string, CatalogItemLoc>();
			//if (SiteContext.Current != null && /* По идее этого кейса быть не должно, однако видимо он бывает */
			//    SiteContext.Current.InternalFranchName == "rmsauto")
			//{
			//    var locs = CatalogItemsDac.GetCatalogItemsLoc(this.CatalogItemID);
			//    foreach (var loc in locs)
			//    {
			//        _LocalizationData.Add(loc.Localization, loc);
			//    }
			//}
		}
    }

    [DisplayName("Раздел сайта")]
    [DisplayColumn("CatalogItemName")]
    [Sort("CatalogItemPriority")]
    public partial class CatalogItemMetadata
    {
        [DisplayName("ID")]
        public object CatalogItemID { get; set; }

        [DisplayName("Количество баннеров")]
        public object BannerCount { get; set; }

        // Прямое редактироание таблицы связи между Разделами сайта и Баннерами можно спрятать
        [ScaffoldColumn(false)]
        public object BannersForCatalogItems { get; set; }

        [DisplayName("Родительский раздел")]
        [UIHint("Custom/CatalogItemForeignKey")]
        public object ParentItem { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Подразделы")]
        public object ChildItems { get; set; }

        [DisplayName("Приоритет")]
        [Required(ErrorMessage = "не задан приоритет")]
        public object CatalogItemPriority { get; set; }

        [DisplayName("Название")]
        [Required(ErrorMessage = "не задано название раздела")]
        public object CatalogItemNameRU { get; set; }

        [DisplayName("Обработчик")]
        [Required(ErrorMessage = "не задан обработчик")]
        public object CatalogItemPath { get; set; }

        [DisplayName("Параметры")]
        public object CatalogItemQueryString { get; set; }

        [DisplayName("Видимость")]
        public object CatalogItemVisible { get; set; }

        [DisplayName("Открывать в новом окне")]
        public object CatalogItemOpenNewWindow { get; set; }

        [DisplayName("Картинка")]
        public object CatalogItemImageUrlRU { get; set; }

        [DisplayName("Меню")]
        [UIHint("Enumeration", null, "EnumType", typeof(CatalogItemMenuType))]
        public object CatalogItemMenuType { get; set; }

        [DisplayName("Url-код")]
        [Required(ErrorMessage = "не задан url-код")]
        [RegularExpression(@"^~{1,1}|[\w\d]+$", ErrorMessage = "url-код может состоять только из цифр и латинских букв")]
        public object CatalogItemCode { get; set; }

        [DisplayName("Страница - title")]
        public object PageTitleRU { get; set; }

        [DisplayName("Страница - keywords")]
        public object PageKeywordsRU { get; set; }

        [DisplayName("Страница - description")]
        public object PageDescriptionRU { get; set; }

        [DisplayName("Страница - текст")]
        [UIHint("Custom/Html")]
        public object PageBodyRU { get; set; }

        [DisplayName("Страница - подвал")]
        [UIHint("Custom/Html")]
        public object PageFooterRU { get; set; }

        [DisplayName("Служебная страница")]
        public object IsServicePage { get; set; }

		[DisplayName("CssClass")]
		public object CssClass { get; set; }
    }
}
