using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RmsAuto.Store.Cms.Mail
{
	/// <summary>
	/// Почтовое сообщение
	/// </summary>
	public class MailData
	{
		/// <summary>
		/// Тема сообщения
		/// </summary>
		public string Subject { get; set; }

		/// <summary>
		/// Тело сообщения (HTML)
		/// </summary>
		public string Body { get; set; }
	}
}
