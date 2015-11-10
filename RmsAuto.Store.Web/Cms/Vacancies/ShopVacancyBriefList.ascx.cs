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

namespace RmsAuto.Store.Web.Cms.Vacancies
{
	public partial class ShopVacancyBriefList : System.Web.UI.UserControl
	{

        public int Count;

        public int ShopID
		{
            get { return ViewState["ShopID"] != null ? (int)ViewState["ShopID"] : 0; }
            set { ViewState["ShopID"] = value; }
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            using (CmsDataContext dc = new CmsDataContext())
            {
                var vac = dc.Vacancies.Where(v => v.VacancyVisible && v.ShopID == ShopID).OrderBy(v => v.VacancyName);
                Count = vac.Count();
                _itemRepeater.DataSource = vac;
                _itemRepeater.DataBind();
            }
        }
		
        protected void Page_PreRender( object sender, EventArgs e )
		{
   
		}
	}
}