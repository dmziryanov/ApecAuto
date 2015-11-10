using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Cms.Entities
{
    [ScaffoldTable(true)]
	[MetadataType( typeof( ManufacturerMetadata ) )]
    public partial class Manufacturer
    {
        partial void OnNameChanged()
        {
            if (!string.IsNullOrEmpty(_Name))
                _Name = _Name.ToUpper();
        }

        partial void OnValidate(ChangeAction action)
        {
            if (action == ChangeAction.Insert)
            {
                using (CmsDataContext dc = new CmsDataContext())
                {
                    if (dc.Manufacturers.SingleOrDefault(
                        m => m.Name == Name) != null)
                        throw new ValidationException("производитель с названием '" +
                            Name +
                            "' уже существует");
                }
            }
        }
    }

    [DisplayName("Производители")]
    [DisplayColumn("Name")]
	public partial class ManufacturerMetadata
    {
        [DisplayName("Название")]
        [Required(ErrorMessage= "не задано название производителя")]
        public object Name { get; set; }

        [DisplayName("Основной текст")]
		[UIHint( "Custom/Html" )]
		public object Info { get; set; }

        [DisplayName("Веб-сайт")]
        [RegularExpression(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?",
            ErrorMessage = "неверный формат веб адреса")]
        public object WebSiteUrl { get; set; }

		[UIHint("Custom/FileForeignKey")]
		[DisplayName( "Логотип" )]
		public object LogoFile { get; set; }

		[UIHint( "Custom/FolderForeignKey" )]
		[DisplayName( "Прикреплённые файлы" )]
		public object Folder { get; set; }

		[DisplayName( "Отображать в каталоге" )]
		public object ShowInCatalog { get; set; }

		[DisplayName( "Url-код" )]
		[Required( ErrorMessage = "не задан url-код" )]
		[RegularExpression( @"^[\w\d]+$", ErrorMessage = "url-код может состоять только из цифр и латинских букв" )]
		public object UrlCode { get; set; }

		[DisplayName( "Страница - title" )]
		public object PageTitle { get; set; }

		[DisplayName( "Страница - keywords" )]
		public object PageKeywords { get; set; }

		[DisplayName( "Страница - description" )]
		public object PageDescription { get; set; }
		
		[DisplayName( "Страница - подвал" )]
		[UIHint( "Custom/Html" )]
		public object PageFooter { get; set; }

	}
}
