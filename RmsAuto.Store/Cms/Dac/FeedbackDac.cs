using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Cms.Dac
{
	public static class FeedbackDac
	{
		private static Func<CmsDataContext, IOrderedQueryable<FeedbackRecipient>> _getFeedbackRecipients =
			CompiledQuery.Compile(
			( CmsDataContext dc ) =>
				dc.FeedbackRecipients.Where( r => r.RecipientVisible ).OrderBy( r => r.RecipientName ) );

		private static Func<CmsDataContext, int, FeedbackRecipient> _getFeedbackRecipient =
			CompiledQuery.Compile(
			( CmsDataContext dc, int id ) =>
				dc.FeedbackRecipients.Where( r => r.RecipientID == id && r.RecipientVisible ).SingleOrDefault() );

		public static FeedbackRecipient[] GetFeedbackRecipients(string locale)
		{
			using( var dc = new DCWrappersFactory<CmsDataContext>() )
			{
				if (locale != "ru-RU")
				{
					//TODO получается как-то громоздко, но времени в обрез, а справочник этот небольшой, поэтому оставляем так (главное что работает)
					var recipients = from rpts in dc.DataContext.FeedbackRecipients
									 join
										locs in dc.DataContext.FeedbackRecipientsLocs on rpts.RecipientID equals locs.RecipientID
									 where locs.Localization == locale
									 select new
									 {
										 RecipientID = rpts.RecipientID,
										 RecipientEmail = rpts.RecipientEmail,
										 RecipientVisible = rpts.RecipientVisible,
										 RecipientName = locs.RecipientName
									 };
					var recipients_materialized = recipients.ToArray();
					return recipients_materialized.Select(r => new FeedbackRecipient
					{
						RecipientID = r.RecipientID,
						RecipientEmail = r.RecipientEmail,
						RecipientVisible = r.RecipientVisible,
						RecipientName = r.RecipientName
					}).ToArray();
				}
				else
				{
					return _getFeedbackRecipients(dc.DataContext).ToArray();
				}
			}
		}

		public static FeedbackRecipient GetFeedbackRecipient(int id) 
		{
			using(var dc = new  DCWrappersFactory<CmsDataContext>())
			{
 				return _getFeedbackRecipient(dc.DataContext, id);
			}
		}
	}
}
