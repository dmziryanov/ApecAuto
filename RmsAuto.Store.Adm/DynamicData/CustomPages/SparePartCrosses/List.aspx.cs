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
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.SparePartCrosses
{
	public partial class List : Security.BasePage/*System.Web.UI.Page*/
	{
		protected void Page_Load( object sender, EventArgs e )
		{

		}

		protected void _deleteButton_Click( object sender, EventArgs e )
		{
			using( var dc = new DCFactory<StoreDataContext>() )
			{
				dc.DataContext.CommandTimeout = 600;
				dc.DataContext.ExecuteCommand( "truncate table dbo.SparePartCrosses" );

				Response.Redirect( Request.RawUrl, true );
			}
		}

		protected void _deleteBrandsButton_Click( object sender, EventArgs e )
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				dc.DataContext.CommandTimeout = 600;
				dc.DataContext.ExecuteCommand( "truncate table dbo.SparePartCrossesBrands" );

				Response.Redirect( Request.RawUrl, true );
			}
		}

		protected void _deleteGroupsButton_Click( object sender, EventArgs e )
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				dc.DataContext.CommandTimeout = 600;
				dc.DataContext.ExecuteCommand( "truncate table dbo.SparePartCrossesGroups" );

				Response.Redirect( Request.RawUrl, true );
			}
		}

		protected void _deleteLinksButton_Click( object sender, EventArgs e )
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				dc.DataContext.CommandTimeout = 600;
				dc.DataContext.ExecuteCommand( "truncate table dbo.SparePartCrossesLinks" );

				Response.Redirect( Request.RawUrl, true );
			}
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				_crossCountLabel.Text = dc.DataContext.SparePartCrosses.Any() ? "Загружены" : "нет";
				_crossBrandsLabel.Text = dc.DataContext.SparePartCrossesBrands.Any() ? "Загружены" : "нет";
				_crossGroupsLabel.Text = dc.DataContext.SparePartCrossesGroups.Any() ? "Загружены" : "нет";
				_crossLinksLabel.Text = dc.DataContext.SparePartCrossesLinks.Any() ? "Загружены" : "нет";
			}
		}
	
	}
}
