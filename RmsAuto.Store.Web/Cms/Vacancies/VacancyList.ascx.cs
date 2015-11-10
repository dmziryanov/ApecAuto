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
using RmsAuto.Store.Cms.Entities;

namespace RmsAuto.Store.Web.Cms.Vacancies
{
	public partial class VacancyList : System.Web.UI.UserControl
	{
		public int? VacancyID
		{
			get { return (int?)ViewState[ "VacancyID" ]; }
			set { ViewState[ "VacancyID" ] = value; }
		}
		protected void Page_PreRender( object sender, EventArgs e )
		{
			using( CmsDataContext dc = new CmsDataContext() )
			{
				if( VacancyID == null )
				{
					_listView.DataSource = dc.Vacancies
						.Where( v => v.VacancyVisible )
						.OrderBy( v => v.VacancyName );
				}
				else
				{
					_listView.DataSource = dc.Vacancies
						.Where( v => v.VacancyVisible && v.VacancyID == VacancyID );
				}
				_listView.DataBind();
			}
		}
	}
}