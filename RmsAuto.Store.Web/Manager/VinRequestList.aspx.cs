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
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Manager
{
	public partial class VinRequestList : System.Web.UI.Page
	{
		public static string GetUrl()
		{
			return "~/Manager/VinRequestList.aspx";
		}

		protected void Page_Load( object sender, EventArgs e )
		{
            // deas 02.03.2011 task2985
            // закоментированно для применения фильтров при перелистывании страниц
            //if ( !IsPostBack )
			{
				ApplyFilters();
			}
		}

		void ApplyFilters()
		{
			var siteContext = (ManagerSiteContext)SiteContext.Current;
			string filter1;
			switch( _filterBox.SelectedValue )
			{
				case "1": filter1 = "ManagerId=\"" + siteContext.User.AcctgID + "\""; break;
				case "2": filter1 = "StoreId=\"" + AcctgRefCatalog.RmsEmployees[ siteContext.User.AcctgID ].StoreId + "\""; break;
				case "0":
				default:
					filter1 = "true"; break;
			}
			string filter2;
			switch( _statusBox.SelectedValue )
			{
				case "1": filter2 = "Proceeded==false"; break;
				case "2": filter2 = "Proceeded==true"; break;
				case "0":
				default:
					filter2 = "true"; break;
			}
			_vinRequestDataSource.Where = filter1 + " and " + filter2;
            // deas 02.03.2011 task2985
            // добавленно для применения фильтров при перелистывании страниц
            _listView.DataBind();
		}

        // deas 02.03.2011 task2985
        // закоментированно для применения фильтров при перелистывании страниц
        //protected void _filterBox_SelectedIndexChanged( object sender, EventArgs e )
        //{
        //    //ApplyFilters();
        //}
        //protected void _statusBox_SelectedIndexChanged( object sender, EventArgs e )
        //{
        //    //ApplyFilters();
        //}

		protected void _listView_ItemCommand( object sender, ListViewCommandEventArgs e )
		{
			if (e.CommandName == "Open")
			{
				var siteContext = (ManagerSiteContext)SiteContext.Current;
				int id = Convert.ToInt32(e.CommandArgument);
                using (var dc = new DCFactory<StoreDataContext>())
				{
					VinRequest request = dc.DataContext.VinRequests.Where(r => r.Id == id).Single();

					if(siteContext.ClientSet.Contains(request.ClientId))
					{
						siteContext.ClientSet.SetDefaultClient( request.ClientId );
					}
					else
					{
                        
						siteContext.ClientSet.AddClient(request.ClientId, true);
					}

					Response.Redirect( ClientVinRequestDetails.GetUrl( id ), true );
				}
			}
		}

       
	}
}
