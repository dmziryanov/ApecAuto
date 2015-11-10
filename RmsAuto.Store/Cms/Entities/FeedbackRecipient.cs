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
	[MetadataType( typeof( FeedbackRecipientMetadata ) )]
	public partial class FeedbackRecipient
	{
	}

	[DisplayName( "Получатели сообщений обратной связи" )]
	[Sort( "RecipientName" )]
	public partial class FeedbackRecipientMetadata
	{
		[DisplayName( "Название" )]
		[Required( ErrorMessage = "не задано название" )]
		public object RecipientName { get; set; }

		[DisplayName( "Email" )]
		[Required( ErrorMessage = "не задан email" )]
		public object RecipientEmail { get; set; }

		[DisplayName( "Видимость" )]
		public object RecipientVisible { get; set; }
	}
}
