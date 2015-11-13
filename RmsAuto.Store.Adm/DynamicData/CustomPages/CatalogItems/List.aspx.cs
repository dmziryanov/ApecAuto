using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;
using RmsAuto.Common.Web;
using RmsAuto.Store.Cms.Entities;
using System.Collections.Generic;
using System.Threading;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.CatalogItems
{
	public partial class List : Security.BasePage/*System.Web.UI.Page*/
	{
		protected MetaTable table;

		protected void Page_Init(object sender, EventArgs e)
		{
			table = MetaModel.GetModel( typeof( CmsDataContext ) ).GetTable( typeof( CatalogItem ) );
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			Title = table.DisplayName;
			InsertHyperLink.NavigateUrl = table.GetActionPath( PageAction.Insert );
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			using( var dc = new DCFactory<CmsDataContext>() )
			{
				var groups = dc.DataContext.CatalogItems.GroupBy( c => c.ParentItemID ).ToDictionary( g => g.Key ?? 0 );

				if( groups.Count != 0 )
				{
					var list = new List<object>();
					var stack = new Stack<KeyValuePair<CatalogItem,int>>();

					foreach( var item in groups[ 0 ].OrderByDescending( c=>c.CatalogItemPriority ) )
						stack.Push( new KeyValuePair<CatalogItem, int>( item, 0 ) );

					while( stack.Count != 0 )
					{
						var node = stack.Pop();
						list.Add( new { CatalogItem = node.Key, Level = node.Value } );

						if( groups.ContainsKey( node.Key.CatalogItemID ) )
						{
							foreach( var item in groups[ node.Key.CatalogItemID ].OrderByDescending( c => c.CatalogItemPriority ) )
								stack.Push( new KeyValuePair<CatalogItem, int>( item, node.Value + 1 ) );
						}
					}

					_repeater.DataSource = list;
					_repeater.DataBind();
				}

			}
		}

		protected void _repeater_ItemCommand( object source, RepeaterCommandEventArgs e )
		{
			try
			{
				if( e.CommandName == "Delete" )
				{
                    using (var dc = new DCFactory<CmsDataContext>())
					{
						int id = Convert.ToInt32( e.CommandArgument );
						CatalogItem item = dc.DataContext.CatalogItems.Where( c => c.CatalogItemID == id ).SingleOrDefault();
						if( item != null )
							dc.DataContext.CatalogItems.DeleteOnSubmit( item );
						dc.DataContext.SubmitChanges();
						Response.Redirect( table.ListActionPath, true );
					}
				}
			}
			catch( ThreadAbortException )
			{
				throw;
			}
			catch( Exception ex )
			{
				_errorLabel.Text = ex.Message;
			}
		}


	}
}
