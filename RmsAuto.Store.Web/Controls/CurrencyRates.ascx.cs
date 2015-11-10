using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Controls
{
	public partial class CurrencyRates : System.Web.UI.UserControl
	{
		protected void Page_PreRender( object sender, EventArgs e )
		{
			//Отобразить курсы валют
			CurrencyRate usd;
			CurrencyRate eur;
			try
			{
				usd = AcctgRefCatalog.CurrencyRates[ "USD" ];
			}
			catch
			{
				usd = null;
			}
			try
			{
				eur = AcctgRefCatalog.CurrencyRates[ "EUR" ];
			}
			catch
			{
				eur = null;
			}

			_currencyRatesPanel.Visible = usd != null || eur != null;
			if( usd != null )
			{
				_usdRateLabel.Text = string.Format( NumberFormatInfo.InvariantInfo, "{0:0.00}", usd.Rate );
				_usdRow.Visible = true;
			}
			else
			{
				_usdRow.Visible = false;
			}
			if( eur != null )
			{
				_eurRateLabel.Text = string.Format( NumberFormatInfo.InvariantInfo, "{0:0.00}", eur.Rate );
				_eurRow.Visible = true;
			}
			else
			{
				_eurRow.Visible = false;
			}
		}
	}
}