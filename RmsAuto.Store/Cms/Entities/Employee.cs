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
	[MetadataType( typeof( EmployeeMetadata ) )]
	public partial class Employee
	{
	}

	[DisplayName( "Сотрудники магазинов" )]
	[Sort( "Shop.ShopName,EmpPriority,EmpName" )]
	public partial class EmployeeMetadata
	{

		[DisplayName( "ФИО" )]
		[Required( ErrorMessage = "не задано ФИО" )]
		public object EmpName { get; set; }

		[DisplayName( "Должность" )]
		[Required( ErrorMessage = "не задана должность" )]
		public object EmpPosition { get; set; }

		[DisplayName( "Email" )]
		public object EmpEmail { get; set; }

		[DisplayName( "ICQ" )]
		public object EmpICQ { get; set; }

		[DisplayName( "Телефон" )]
		public object EmpPhone { get; set; }

		[DisplayName( "Видимость" )]
		public object EmpVisible { get; set; }

		[DisplayName( "Магазин" )]
		[Required( ErrorMessage = "не задан магазин" )]
		public object Shop { get; set; }

		[DisplayName( "Приоритет" )]
		public object EmpPriority { get; set; }
	}
}
