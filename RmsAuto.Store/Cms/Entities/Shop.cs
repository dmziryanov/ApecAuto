using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Cms.Entities
{
	[ScaffoldTable( true )]
	[MetadataType(typeof(ShopMetadata))]
	public partial class Shop
	{
        partial void OnValidate(System.Data.Linq.ChangeAction action)
        {
            //city-region validation logic
            if (CityID != null)
                  RegionID = null; //Если задан город, то регион сбрасываем, чтобы не было ситуации Москва в Астраханской области.
        }


	}

	[DisplayName( "Магазины" )]
	[Sort( "ShopName" )]
	public partial class ShopMetadata
	{
		[DisplayName( "Название" )]
		[Required( ErrorMessage = "не задано название магазина" )]
		public object ShopName { get; set; }

		[UIHint( "Custom/Html" )]
		[DisplayName( "Адрес" )]
		[Required( ErrorMessage = "не задан адрес магазина" )]
		public object ShopAddress { get; set; }

        [DisplayName("Идентификтор")]
        [Required(ErrorMessage = "не задан идентификатор магазина")]
        public object StoreId { get; set; }

        //[UIHint( "Custom/Html" )]
		[DisplayName( "Метро" )]
		public object ShopMetro { get; set; }

        //[UIHint( "Custom/Html" )]
		[DisplayName( "Время работы" )]
		[Required( ErrorMessage = "не задано время работы магазина" )]
		public object ShopWorkTime { get; set; }

        //[UIHint( "Custom/Html" )]
		[DisplayName( "Телефоны" )]
		[Required( ErrorMessage = "не заданы контактные телефоны магазина" )]
		public object ShopPhones { get; set; }

		[UIHint("Custom/Html")]
		[DisplayName( "Комментарий" )]
		public object ShopNote { get; set; }

		[DisplayName( "Видимость" )]
		public object ShopVisible { get; set; }

		[DisplayName( "Сотрудники" )]
		public object Employees { get; set; }

		[DisplayName( "Вакансии" )]
		public object Vacancies { get; set; }

		[UIHint( "Custom/FileForeignKey", null, "IsImage", true )]
		[DisplayName( "Схема проезда" )]
		public object ShopMapFile { get; set; }

		[UIHint("Custom/FolderForeignKey")]
		[DisplayName( "Фотогалерея" )]
		public object ShopGalleryFolder { get; set; }

		[DisplayName( "Приоритет" )]
		public object ShopPriority { get; set; }

      
        [UIHint("Custom/AcctgRef", null, "BindingOptions", "Cities;CityID;Name")]
        [DisplayName("Город")]
        public object CityID { get; set; }

        [UIHint("Custom/AcctgRef", null, "BindingOptions", "Region;RegionID;RegionName")]
        [DisplayName("Регион")]
        public object RegionID { get; set; }

        [DisplayName("Сайт магазина")]
        public object SiteUrl { get; set; }

        [DisplayName("Наименование франча")]
        public object FranchName { get; set; }

        [DisplayName("Долгота на Яндекс-картах")]
        [Required(ErrorMessage = "не задана долгота")]
        public object XCoord { get; set; }

        [DisplayName("Широта на Яндекс-картах")]
        [Required(ErrorMessage = "не задана широта")]
        public object YCoord { get; set; }

        [DisplayName("Комментарий карты")]
        public object MapComment { get; set; }

        [DisplayName("магазин РМС")]
        public object isRMS { get; set; }

      

	}
}
