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
	[MetadataType( typeof( SparePartGroupMetadata ) )]
	public partial class SparePartGroup
	{
	}

	[DisplayName( "Группы запчастей (подсветка)" )]
	[Sort( "SparePartGroupID" )]
	public partial class SparePartGroupMetadata
	{
		[DisplayName( "Идентификатор" )]
		[Required( ErrorMessage = "не задан идентификатор" )]
		public object SparePartGroupID { get; set; }

		[DisplayName( "Название группы" )]
		[Required( ErrorMessage = "не задано название" )]
		public object GroupName { get; set; }

		[DisplayName( "Цвет фона (RGB)" )]
		[Required( ErrorMessage = "не задан цвет фона" )]
		[RegularExpression( "^[A-Fa-f0-9]{6}$", ErrorMessage = "Некорректный формат. должно быть RRGGBB в шестнадатеричном представлении" )]
		public object BackgroundColor { get; set; }

		[DisplayName( "Видимость" )]
		public object Visible { get; set; }
	}
}
