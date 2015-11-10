using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RmsAuto.Store.BL;

using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web.Manager.BasePages
{
	public class LightPage : Attribute { }

	public class RMMPage : LocalizablePage
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!LightBO.IsLight())
			{
				//Все "лайтовые" страницы должны помечаться атрибутом LightPage иначе будет выброшено исключение
				var attrs = this.GetType().GetCustomAttributes(typeof(LightPage), true);
				if (attrs.Length == 1)
					throw new Exception("Access denied.");
			}
		}
	}
}
