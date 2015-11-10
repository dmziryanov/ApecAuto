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
using System.Xml.Xsl;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Entities;

namespace RmsAuto.Store.Web.TecDoc.Controls
{
	public partial class TecDocModels : System.Web.UI.UserControl
	{
		public bool IsCarModel { get; set; }
		public int ManufacturerId { get; set; }

		protected string GetModelUrl( int modelID )
		{
			return UrlManager.GetTecDocModelDetailsUrl( modelID );
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			_repeater.DataSource = TecDocAggregator.GetModels( ManufacturerId, IsCarModel ? VehicleType.Car : VehicleType.Truck );
			_repeater.DataBind();
		}
	}
}