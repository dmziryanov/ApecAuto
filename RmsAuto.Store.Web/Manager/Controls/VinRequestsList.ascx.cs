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

using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class VinRequestsList : System.Web.UI.UserControl
	{
		protected string GetDetailsUrl( int id )
		{
			return ClientVinRequestDetails.GetUrl( id );
		}
		protected void Page_Load( object sender, EventArgs e )
		{

		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			_requestsRepeater.DataSource = VinRequestsDac.GetRequests( SiteContext.Current.CurrentClient.Profile.ClientId );
			_requestsRepeater.DataBind();
		}
	}
}