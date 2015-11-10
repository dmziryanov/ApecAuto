using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;
using RmsAuto.Common.Web;
using RmsAuto.Store.Cms.Entities;
using System.Collections.Generic;
using RmsAuto.Store.Cms.BL;
using System.Configuration;
using System.Text;
using System.Collections;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.SeoPartsCatalogItems
{
	public partial class List : Security.BasePage/*System.Web.UI.Page*/
	{
		protected MetaTable table;

		int? ParentID
		{
			get { return string.IsNullOrEmpty( Request[ "ParentID" ] ) ? null : (int?)Convert.ToInt32( Request[ "ParentID" ] ); }
		}

		public string GetListUrl( int parentId )
		{
			return table.GetActionPath( PageAction.List ) + "?ParentID=" + parentId;
		}

		public string GetEditUrl( object dataitem )
		{
			return table.GetActionPath( PageAction.Edit, dataitem ) + "&ParentID=" + ParentID;
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			DynamicDataManager1.RegisterControl(GridView1, true /*setSelectionFromUrl*/);
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			table = GridDataSource.GetTable();
			Title = table.DisplayName;
			
			InsertHyperLink.NavigateUrl = table.GetActionPath( PageAction.Insert ) +"?ParentID="+ParentID;

			// Disable various options if the table is readonly
			if( table.IsReadOnly )
			{
				GridView1.Columns[ 0 ].Visible = false;
				InsertHyperLink.Visible = false;
			}

			GridDataSource.AutoGenerateWhereClause = false;
			if( ParentID==null )
			{
				GridDataSource.Where = "ParentID==null";
				InsertHyperLink.Visible = false;
			}
			else
			{
				GridDataSource.Where = "ParentID==" + ParentID;
				InsertHyperLink.Visible = true;
			}

			if( !IsPostBack )
			{
				//using (var dc = new DCWrappersFactory<CmsDataContext>())
				//{
					var catalogItems = SeoPartsCatalogDac.GetVirtualPathItemsById( ParentID );

					var crumbItems = new List<object>();
					crumbItems.Add( new { Name = "Главная", Url = table.ListActionPath } );
					foreach( var c in catalogItems )
						crumbItems.Add( new { Name = c.Name, Url = GetListUrl( c.ID ) } );

					_breadCrumbsRepeater.DataSource = crumbItems;
					_breadCrumbsRepeater.DataBind();

					var currentItem = catalogItems.LastOrDefault();
					if( currentItem != null && catalogItems.All( c => c.Visible ) )
					{
						_previewLink.Visible = true;
						StringBuilder sb = new StringBuilder();
						sb.Append( ConfigurationManager.AppSettings[ "WebSiteUrl" ] );
						sb.Append( "/Parts" );
						foreach( var urlCode in SeoPartsCatalogBO.GetVirtualPathByItems( catalogItems ) )
						{
							sb.Append( "/" );
							sb.Append( urlCode );
						}
						sb.Append( ".aspx" );
						_previewLink.NavigateUrl = sb.ToString();
					}
					else
					{
						_previewLink.Visible = false;
					}
					
				//}
			}

            if (string.IsNullOrEmpty(GridView1.SortExpression))
            {
                GridView1.Sort("Priority", SortDirection.Ascending);
            }
        }
	}
}
