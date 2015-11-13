using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Entities;
using System.Data.Linq;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Cms.Mail.Templates;
using RmsAuto.Store.Cms.Dac;
using System.Net.Mail;
using RmsAuto.Store.Cms.Mail;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.BL
{
	public static class ReclamationBO
	{
		public interface ISendReclamationTrackingAlertsLog
		{
			void LogSuccessfulAlert( string clientId );
			//void LogManagerListRequestFailed();
			void LogEmptyEmail( string clientId );
			void LogInvalidEmail( string clientId, string email );
			void LogError( string clientId, Exception ex );
		}

		/// <summary>
		/// Отправка оповещений об изменении статусов рекламаций
		/// </summary>
		/// <param name="log">лог отправки</param>
		public static void SendReclamationTrackingAlerts(ISendReclamationTrackingAlertsLog log)
		{
			using (var dc = new DCFactory<StoreDataContext>())
			{
				var clients = (from u in dc.DataContext.Users.Where( user => user.Role == SecurityRole.Client )
							   join alert in dc.DataContext.ReclamationAlertInfos on u.AcctgID equals alert.ClientID
							   select u.AcctgID)
								.Distinct()
								.ToArray();

				foreach (var clientID in clients)
				{
					try
					{
						SendReclamationTrackingAlerts( clientID, log );
					}
					catch (Exception ex)
					{
						log.LogError( clientID, ex );
					}
				}
			}
		}

		private static void SendReclamationTrackingAlerts( string clientID, ISendReclamationTrackingAlertsLog log )
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				DataLoadOptions options = new DataLoadOptions();
				options.LoadWith<Reclamation>( r => r.ReclamationStatus );
				dc.DataContext.LoadOptions = options;

				var profile = ClientProfile.Load( clientID );
				var reclamationAlertInfo = dc.DataContext.ReclamationAlertInfos.Single( a => a.ClientID == clientID );

				//на всякий случай, для новых пользователей исключить отсылку "старых" оповещений, когда он был офлайн клиентом.
				if (reclamationAlertInfo == null)
				{
					dc.DataContext.ReclamationAlertInfos.InsertOnSubmit( new ReclamationAlertInfo { ClientID = clientID, ReclamationLastAlertDate = DateTime.Now } );
					dc.DataContext.SubmitChanges();
				}

				var reclamations = (from r in dc.DataContext.Reclamations
									where r.ClientID == clientID
										&& r.CurrentStatusDate > reclamationAlertInfo.ReclamationLastAlertDate
									orderby r.CurrentStatusDate
									select r).ToArray();

				if (reclamations.Length != 0)
				{
					//составляем оповещение
					ReclamationTrackingAlert alert = new ReclamationTrackingAlert();
                    
					var blockMailFooter = TextItemsDac.GetTextItem( "ReclamationTrackingAlert.BlockMailFooter", "ru-RU" );
					alert.BlockMailFooter = blockMailFooter != null ? blockMailFooter.TextItemBody.ToString() : "";

					alert.Reclamations = reclamations.Select( r =>
						new ReclamationTrackingAlert.Reclamation
						{
							CurrentStatus = r.ReclamationStatus.Name,
							CurrentStatusDate = r.CurrentStatusDate.HasValue ? r.CurrentStatusDate.Value.ToString( "dd.MM.yyyy" ) : string.Empty,
							Manufacturer = r.Manufacturer,
							PartName = r.PartName,
							PartNumber = r.PartNumber,
							Qty = r.Qty.ToString(),
							ReclamationDate = r.ReclamationDate.ToString( "dd.MM.yyyy" ),
							ReclamationNumber = r.sReclamationNumber,
							ReclamationType = r.ReclamationType == RmsAuto.Store.Web.ReclamationTracking.ReclamationType.Reclamation ? "Запрос на возврат" : "Запрос на отказ от получения"
						} ).ToArray();

					MailAddress clientEmail = GetReclamationTrackingEmail( profile.Email,
						profile.IsLegalWholesale ? profile.ContactPerson : profile.ClientName, log, clientID );

					//отправляем оповещение
					MailEngine.SendMail( clientEmail, alert );
					
					//если email не задан или некорректный, то дату последнего оповещения всё равно сдвигаем,
					//чтобы не заспамить клиента, когда в его профиле будет корректный email.
					reclamationAlertInfo.ReclamationLastAlertDate = reclamations.Max( r => r.CurrentStatusDate ).Value;

					dc.DataContext.SubmitChanges();

					log.LogSuccessfulAlert( clientID );
				}
			}
		}

		private static MailAddress GetReclamationTrackingEmail( string address, string displayName, ISendReclamationTrackingAlertsLog log, string refId )
		{
			MailAddress email = null;
			try
			{
				email = new MailAddress( address, displayName );
			}
			//по идее ни того, ни другого случая возникать не должно
			catch (ArgumentException)
			{
				log.LogEmptyEmail( refId );
			}
			catch (FormatException)
			{
				log.LogInvalidEmail( refId, address );
			}
			return email;
		}
	}
}
