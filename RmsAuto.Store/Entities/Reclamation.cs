using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RmsAuto.Store.Entities
{
    [Serializable()]
    public partial class Reclamation
    {
    }

    [Serializable()]
    public partial class ReclamationStatus
    {
		public string Name
		{
			get
			{
				var curLocale = Thread.CurrentThread.CurrentCulture.Name;
				if (curLocale != "ru-RU")
				{
					var locale = this.ReclamationStatusesLocs.Where(loc => loc.Localization == curLocale).SingleOrDefault();
					if (locale != null && !string.IsNullOrEmpty(locale.Name)) return locale.Name;
				}
				return NameRU;
			}
			set { NameRU = value; }
		}
    }
}
