using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Cms.Routing
{
	public class PageFields
	{
		/// <summary>
		/// Заголовок страницы
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Описание (meta description)
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Ключевые слова (meta keywords)
		/// </summary>
		public string Keywords { get; set; }

		/// <summary>
		/// Подвал страницы
		/// </summary>
		public string Footer { get; set; }
	}
}
