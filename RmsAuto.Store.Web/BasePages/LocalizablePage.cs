using System.Globalization;
using System.Threading;

namespace RmsAuto.Store.Web.BasePages
{
	public class LocalizablePage : System.Web.UI.Page
	{
		protected override void InitializeCulture()
		{
			if (!Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith(SiteContext.CurrentCulture.ToLower()))
			{
				Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
			}
			base.InitializeCulture();
		}
	}
}
