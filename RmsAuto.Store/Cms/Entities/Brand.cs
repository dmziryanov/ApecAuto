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
	[MetadataType( typeof( BrandMetadata ) )]
    public partial class Brand
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
                    if (dc.Brands.SingleOrDefault(
						m => m.Name == Name && m.VehicleType == VehicleType ) != null )
                        throw new ValidationException("марка с названием '" +
                            Name +
                            "' уже существует");
                }
            }
        }
    }

    [DisplayName("Марки автомобилей")]
    [DisplayColumn("Name")]
	public partial class BrandMetadata
    {
		[DisplayName( "Тип" )]
		[Required( ErrorMessage = "не задан тип" )]
		[UIHint( "Enumeration", null, "EnumType", typeof( VehicleType ) )]
		public object VehicleType { get; set; }

		[DisplayName("Название")]
        [Required(ErrorMessage= "не задано название марки")]
        public object Name { get; set; }

        [DisplayName("Основной текст")]
		[UIHint( "Custom/Html" )]
		public object Info { get; set; }

        [DisplayName("Ссылка на AutoXP")]
        [RegularExpression(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?",
            ErrorMessage = "неверный формат веб адреса")]
        public object AutoXPUrl { get; set; }

		[UIHint("Custom/FileForeignKey")]
		[DisplayName( "Логотип" )]
		public object LogoFile { get; set; }

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
