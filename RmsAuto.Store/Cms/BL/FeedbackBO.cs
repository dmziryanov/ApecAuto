using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using RmsAuto.Store.Cms.Entities;
using System.Web;
using RmsAuto.Store.Cms.Mail;
using RmsAuto.Store.Cms.Mail.Messages;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Cms.BL
{
	public class FeedbackBO
	{
		public static void SendMessage( int recipientId, string profileInfo, string email, string message )
		{
			FeedbackRecipient recipient = FeedbackDac.GetFeedbackRecipient( recipientId );

			MailEngine.SendMail(
				new MailAddress( recipient.RecipientEmail ),
				!string.IsNullOrEmpty( email ) ? new MailAddress( email, profileInfo ) : null,
				new FeedbackAlert( profileInfo, message ) );
		}
	}
}
