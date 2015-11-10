using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Cms.Entities
{


	[ScaffoldTable( true )]
	[MetadataType( typeof( TextItemMetadata ) )]
	public partial class TextItem
	{
		partial void OnValidate( System.Data.Linq.ChangeAction action )
		{
			if( action == System.Data.Linq.ChangeAction.Delete )
			{
				if( TextItemFixed )
				{
					throw new ValidationException( "Невозможно удалить текстовую страницу" );
				}
			}
		}
	}

	[DisplayName( "Текстовый блок" )]
    [Sort("TextItemID")]
	public partial class TextItemMetadata
	{
		[DisplayName( "Идентификатор" )]
		[Required( ErrorMessage = "не задан идентификатор" )]
		[RegularExpression(@"[\w\d\-\.]+",ErrorMessage="идентификатор может содержать буквы, цифры, знаки . и -")]
		public object TextItemID { get; set; }

		[DisplayName( "Заголовок" )]
		[Required( ErrorMessage = "не задан заголовок" )]
		public object TextItemHeader { get; set; }

		[UIHint("Custom/Html")]
		[DisplayName( "Текст" )]
		[Required( ErrorMessage = "не задан текст" )]
		public object TextItemBody { get; set; }

        [DisplayName( "Язык" )]
        public object Localization1 { get; set; }

		[ScaffoldColumn(false)]
		public object TextItemFixed { get; set; }
	}
}
