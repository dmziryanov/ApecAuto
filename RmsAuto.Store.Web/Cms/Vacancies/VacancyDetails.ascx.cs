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
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using System.Net;

namespace RmsAuto.Store.Web.Cms.Vacancies
{
	public partial class VacancyDetails : System.Web.UI.UserControl
	{
		public int VacancyID
		{
			get { return ViewState[ "VacancyID" ] != null ? (int)ViewState[ "VacancyID" ] : 0; }
			set { ViewState[ "VacancyID" ] = value; }
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			using( CmsDataContext dc = new CmsDataContext() )
			{
				var vacancy = dc.Vacancies.Where( v => v.VacancyVisible && v.VacancyID == VacancyID ).SingleOrDefault();
				
				if( vacancy == null )
					throw new HttpException( (int)HttpStatusCode.NotFound, "Not found" );

				_repeater.DataSource = new object[] { vacancy };
				_repeater.DataBind();
			}
        }
	}
}