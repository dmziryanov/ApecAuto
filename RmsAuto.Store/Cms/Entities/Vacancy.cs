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
	[MetadataType( typeof( VacancyMetadata ) )]
	public partial class Vacancy
	{

	}

	[DisplayName( "Вакансии" )]
	[Sort( "VacancyName" )]
	public partial class VacancyMetadata
	{
		[DisplayName( "Название" )]
		[Required( ErrorMessage = "не задано название вакансии" )]
		public object VacancyName { get; set; }

		[DisplayName( "Пол" )]
		[UIHint( "Enumeration", null, "EnumType", typeof( Gender ) )]
		public object VacancyGender { get; set; }

		[DisplayName( "Возраст от" )]
		[Required( ErrorMessage = "не задан возраст от" )]
		public object VacancyAgeFrom { get; set; }

		[DisplayName( "Возраст до" )]
		public object VacancyAgeTo { get; set; }

		[DisplayName( "Образование" )]
		public object VacancyEducation { get; set; }

		[UIHint( "Custom/Html" )]
		[DisplayName( "Опыт работы" )]
		public object VacancyExperience { get; set; }

		[DisplayName( "Занятость" )]
		public object VacancyEmployment { get; set; }

		[DisplayName( "Предполагаемый уровень доходов" )]
		public object VacancyIncomeLevel { get; set; }

		[UIHint( "Custom/Html" )]
		[DisplayName( "Требования" )]
		public object VacancyRequirement { get; set; }

		[UIHint( "Custom/Html" )]
		[DisplayName( "Обязанности" )]
		public object VacancyFunctions { get; set; }

		[UIHint( "Custom/Html" )]
		[DisplayName( "Дополнительно" )]
		public object VacancyNote { get; set; }

		[UIHint( "Custom/Html" )]
		[DisplayName( "Контакты" )]
		[Required( ErrorMessage = "не заданы контакты" )]
		public object VacancyContacts { get; set; }

		[DisplayName( "Видимость" )]
		public object VacancyVisible { get; set; }

		[DisplayName( "Магазин" )]
		public object Shop { get; set; }
	}


}
