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
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.TecDoc.Controls
{
	public partial class TecDocModifications : System.Web.UI.UserControl
	{
		public int ModelId { get; set; }

		protected string GetModificationUrl( int modificationId )
		{
			return UrlManager.GetTecDocModificationDetailsUrl( modificationId );
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			_repeater.DataSource = TecDocAggregator.GetModifications( ModelId );
			_repeater.DataBind();
		}
	}
}