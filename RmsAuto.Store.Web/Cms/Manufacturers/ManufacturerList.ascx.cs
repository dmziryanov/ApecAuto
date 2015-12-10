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
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Cms.Manufacturers
{
    

    public partial class ManufacturerList : System.Web.UI.UserControl
	{
		protected string GetManufacturerDetailsUrl( string urlCode )
		{
			return UrlManager.GetManufacturerDetailsUrl( urlCode );
		}
		protected void Page_PreRender( object sender, EventArgs e )
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				_dataList.DataSource = dc.DataContext.Manufacturers.Where( m => m.ShowInCatalog ).OrderBy( m => m.Name );
				_dataList.DataBind();
			}
		}
	}
}