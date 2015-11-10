using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using RmsAuto.Store.Cms.Mail;
using System.Net.Mail;
using System.Configuration;
using RmsAuto.Store.Cms.Mail.Templates;

namespace RmsAuto.Store.Web.PrivateOffice
{
	public partial class Offers : System.Web.UI.Page
	{
		#region ==== Constants ====
		
		/// <summary>
		/// Максимальный размер файла = 10 мегабайт
		/// </summary>
		private const int MaxSize = 1024 * 1024 * 10;

		/// <summary>
		/// Разрешенные расширения файлов для загрузки
		/// </summary>
		private string[] AllowExtentions = new string[] { ".zip", ".rar" };

		#endregion

		#region ==== Events ====

		protected void Page_Load( object sender, EventArgs e )
		{
		}

		protected void _ibSend_Click( object sender, ImageClickEventArgs e )
		{
			if (Page.IsValid)
			{
				try
				{
					//получаем список адресатов из конфига
					string[] strmails = ((string)ConfigurationManager.AppSettings["OffersMail"]).Split( ';' );
					List<MailAddress> mails = new List<MailAddress>();

					foreach (var str in strmails)
					{
						mails.Add( new MailAddress( str, "Предложение поставщика" ) );
					}

					//заполняем шаблон письма
					OfferAlert alert = new OfferAlert()
					{
						OfferTitle = _txtName.Text,
						OfferSubject = _txtSubject.Text,
						OfferBody = _txtDescription.Text
					};

					//отправляем сообщения
					if (_fileUploader.HasFile)
					{
						MemoryStream ms = new MemoryStream( _fileUploader.FileBytes );
						Attachment attach = new Attachment( ms, _fileUploader.FileName );
						foreach (var address in mails)
						{
							MailEngine.SendMailWithAttachments( address, new Attachment[] { attach }, alert );
						}
					}
					else
					{
						foreach (var address in mails)
						{
							MailEngine.SendMail( address, alert );
						}
					}
				}
				catch (Exception ex)
				{
					Logger.WriteException( ex );
					_errorLabel.Text = "Произошла ошибка, попробуйте повторить свой запрос возднее.";
					return;
				}

				_sendOkPanel.Visible = true;
				_offerPanel.Visible = false;
			}
		}

		#endregion

		#region ==== Validators ====
		protected void ValidateFile( object source, ServerValidateEventArgs args )
		{
			if (_fileUploader.HasFile)
			{
				if (AllowExtentions.Contains( Path.GetExtension( _fileUploader.FileName ) )
					&& _fileUploader.FileBytes.Length <= MaxSize)
				{
					args.IsValid = true;
					return;
				}
			}
			args.IsValid = false;
		}
		#endregion
	}
}
