using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.SeoPartsCatalogItems
{
	public partial class Edit : Security.BasePage/*System.Web.UI.Page*/
	{
		protected MetaTable table;

		int? ParentID
		{
			get { return string.IsNullOrEmpty( Request[ "ParentID" ] ) ? null : (int?)Convert.ToInt32( Request[ "ParentID" ] ); }
		}

		protected void Page_Init( object sender, EventArgs e )
		{
			DynamicDataManager1.RegisterControl( DetailsView1 );
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			table = DetailsDataSource.GetTable();
			Title = table.DisplayName;
		}

		protected void DetailsView1_ItemCommand( object sender, DetailsViewCommandEventArgs e )
		{
			if( e.CommandName == DataControlCommands.CancelCommandName )
			{
				Response.Redirect( table.ListActionPath + ( ParentID != null ? "?ParentID=" + ParentID : "" ) );
			}
		}

		protected void DetailsView1_ItemUpdated( object sender, DetailsViewUpdatedEventArgs e )
		{
			if( e.Exception == null || e.ExceptionHandled )
			{
				Response.Redirect( table.ListActionPath + ( ParentID != null ? "?ParentID=" + ParentID : "" ) );
			}
		}

		protected void DetailsView1_ItemInserting( object sender, DetailsViewInsertEventArgs e )
		{
			e.Values[ "ParentID" ] = ParentID;
		}

	}
}
