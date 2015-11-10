using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Cms.Entities
{
	/// <summary>
	/// Локализация FeedbackRecipient
	/// </summary>
	[ScaffoldTable( true )]
	[MetadataType( typeof( FeedbackRecipientLocMetadata ) )]
	public partial class FeedbackRecipientsLoc
	{
	}

	[DisplayName("Получатели сообщений обратной связи (локализации)")]
	[Sort( "RecipientName" )]
	public partial class FeedbackRecipientLocMetadata
	{
		[DisplayName("Язык")]
		public object Localization1 { get; set; }

		[DisplayName( "Название" )]
		[Required( ErrorMessage = "не задано название" )]
		public object RecipientName { get; set; }
	}
}
