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
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Web.Cms.Shops
{
	public partial class ShopEmployeeList : System.Web.UI.UserControl
	{
		public int ShopID
		{
			get { return ViewState[ "ShopId" ] != null ? (int)ViewState[ "ShopId" ] : 0; }
			set { ViewState[ "ShopId" ] = value; }
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			_linqDataSource.WhereParameters.Add(
				new StaticParameter( "ShopID", Convert.ToInt32( ShopID ) ) );
		}
	}
}