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
using RmsAuto.TechDoc;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.TecDoc.Controls
{
	public partial class TecDocParts : System.Web.UI.UserControl
	{
		public int ModificationId { get; set; }
		public int SearchTreeNodeId { get; set; }

		public string GetSearchResultsUrl( string mfr, string pn )
		{
			return UrlManager.GetSearchSparePartsUrl( mfr, pn, true );
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			var parts = Facade.ListParts( ModificationId, SearchTreeNodeId );

			_repeater.DataSource =
				from p in parts
				group p by p.Article.Supplier into g
				orderby g.Key.Name
				select g;
			_repeater.DataBind();
		}
	}
}