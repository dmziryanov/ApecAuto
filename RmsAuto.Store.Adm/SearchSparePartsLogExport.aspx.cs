using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using RmsAuto.Store.Adm.scripts;
using System.Text;

namespace RmsAuto.Store.Adm
{
	public partial class SearchSparePartsLogExport : Security.BasePage/*System.Web.UI.Page*/
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			ScriptsManager.RegisterJQuery(this);
			if( !IsPostBack )
			{
				DateTime date2 = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
				_date1Box.Text = date2.AddMonths( -1 ).ToString("dd.MM.yyyy");
				_date2Box.Text = date2.ToString( "dd.MM.yyyy" );
			}
		}

		
	}
}
