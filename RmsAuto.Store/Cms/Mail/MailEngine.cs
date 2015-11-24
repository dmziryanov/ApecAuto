using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Xml.Xsl;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace RmsAuto.Store.Cms.Mail
{

	public static class MailEngine
	{
		public static void SendMail( MailAddress recipient, MailAddress replyTo, IMailMessage message )
		{
			SendMailInternal( new MailAddress[] { recipient }, null, replyTo, message );
		}

        public static void SendMail(MailAddress recipient, IMailMessage message)
        {
            SendMailInternal(new MailAddress[] { recipient }, null, null, message);
        }

        /// <summary>
        /// Отправка электронного письма с вложением
        /// </summary>
        /// <param name="recipient">адрес получателя</param>
        /// <param name="attachments">вложения</param>
        /// <param name="message">письмо</param>
        public static void SendMailWithAttachments( MailAddress recipient, Attachment[] attachments, IMailMessage message )
        {
            SendMailInternal( new MailAddress[] { recipient }, null, null, attachments, message );
        }

        /// <summary>   
		/// метод для замены метода SendMailWithBcc
		/// </summary>
		public static void SendMailWithBccAndAttachments(MailAddress recipient, MailAddress bcc, Attachment[] attachments, IMailMessage message)
		{
			SendMailInternal(new MailAddress[] { recipient }, bcc, null, attachments, message);
		}

		[Obsolete]
		public static void SendMailWithBcc(MailAddress recipient, MailAddress bcc, IMailMessage message)
        {
            SendMailInternal(new MailAddress[] { recipient }, bcc, null, message);
        }

        private static void SendMailInternal(MailAddress[] recipient, MailAddress bcc, MailAddress replyTo, IMailMessage message)
		{
			MailData data = ConvertMailMessage( message );

			MailMessage msg = new MailMessage();
			#region  !!! UNCOMMENT FOR RELEASE !!!
			//Сделано для того чтобы с тестового сайта не шла рассылка существующим клиентам при
			//каких-либо манипуляциях с их заказами (т.к. тестовая база является копией боевой, то в ней лежат инфа о реальных заказах)
			foreach (MailAddress addr in recipient)
			    msg.To.Add(addr);

			if (bcc != null)
			    msg.Bcc.Add(bcc);

			msg.ReplyTo = replyTo;
			#endregion
			
			#region !!! COMMENT FOR RELEASE !!!
			//Принудительна отсылка всех уведовлений на мой ящик, чтобы тестировать рассылки
			
			//msg.To.Add(new MailAddress("Murkin@rmsauto.ru"));
            //msg.To.Add(new MailAddress("ziryanov@rmsauto.ru"));
			
			#endregion

			msg.Subject = data.Subject;
			msg.Body = data.Body;
			msg.IsBodyHtml = true;
            msg.From = new MailAddress("spareautocom@yandex.ru");

			var attachAttributes = Attribute.GetCustomAttributes( message.GetType(), typeof( MailAttachmentAttribute ) );
			foreach( MailAttachmentAttribute item in attachAttributes )
			{
				msg.Attachments.Add( new Attachment( new MemoryStream( item.Data ), item.Name ) );
			}

            var cl =new SmtpClient();
            cl.Credentials = new NetworkCredential("spareautocom@yandex.ru", "db76as");
            cl.Port = 25;
            cl.EnableSsl=true;
            cl.Host = "smtp.yandex.ru";
            //cl.ClientCertificates
            //cl.
            cl.Send( msg );
		}

		/// <summary>
		/// перегрузка имеющегося метода для отправки уведомлений с аттачами
		/// </summary>
		private static void SendMailInternal(MailAddress[] recipient, MailAddress bcc, MailAddress replyTo, Attachment[] attachments, IMailMessage message)
		{
			// Тут тоже самое что в методе SendMailInternal, но с вложениями
			MailData data = ConvertMailMessage(message);

			MailMessage msg = new MailMessage();
			#region  !!! UNCOMMENT FOR RELEASE !!!
			//Сделано для того чтобы с тестового сайта не шла рассылка существующим клиентам при
			//каких-либо манипуляциях с их заказами (т.к. тестовая база является копией боевой, то в ней лежат инфа о реальных заказах)
			foreach (MailAddress addr in recipient)
				msg.To.Add(addr);

			if (bcc != null)
				msg.Bcc.Add(bcc);

			msg.ReplyTo = replyTo;
			#endregion

			#region !!! COMMENT FOR RELEASE !!!
			////Принудительна отсылка всех уведовлений на мой ящик, чтобы тестировать рассылки
			//msg.To.Add(new MailAddress("Kirillov@rmsauto.ru"));
			//msg.To.Add(new MailAddress("Murkin@rmsauto.ru"));
			//msg.To.Add(new MailAddress("Manyanin@rmsauto.ru"));
			#endregion

			msg.Subject = data.Subject;
			msg.Body = data.Body;
			msg.IsBodyHtml = true;

			//var attachAttributes = Attribute.GetCustomAttributes(message.GetType(), typeof(MailAttachmentAttribute));
			//foreach (MailAttachmentAttribute item in attachAttributes)
			//{
			//    msg.Attachments.Add(new Attachment(new MemoryStream(item.Data), item.Name));
			//}

			foreach (var item in attachments)
			{
				msg.Attachments.Add(item);
			}

			new SmtpClient().Send(msg);
		}

	
		private static MailData ConvertMailMessage( IMailMessage message )
		{
			Type messageType = message.GetType();
			MailTemplateAttribute mailTemplateAttribute = (MailTemplateAttribute)Attribute.GetCustomAttribute( messageType, typeof( MailTemplateAttribute ) );

			using( MemoryStream resStream = new MemoryStream() )
			{
				using( MemoryStream srcStream = new MemoryStream() )
				{
					XmlSerializer xs1 = new XmlSerializer( messageType );
					xs1.Serialize( srcStream, message );

					srcStream.Seek( 0, SeekOrigin.Begin );
					using( XmlTextReader rd = new XmlTextReader( srcStream ) )
					{
						mailTemplateAttribute.XslTransform.Transform( rd, null, resStream );
					}
				}

				resStream.Seek( 0, SeekOrigin.Begin );

				XmlDocument doc = new XmlDocument();
				doc.Load( resStream );

				MailData res = new MailData();
				res.Body = doc.DocumentElement.OuterXml;
				XmlNode node = doc.SelectSingleNode( "/html/head/title" );
				if( node == null || string.IsNullOrEmpty( node.InnerText ) )
					throw new Exception( "message title is empty" );
				res.Subject = node.InnerText;

				return res;
			}
		}

	}






}
