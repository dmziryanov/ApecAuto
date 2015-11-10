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
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Configuration;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Misc;
using System.Collections.Specialized;
using RmsAuto.Common.Web.UrlState;
using System.Net;

namespace RmsAuto.Store.Web.Controls
{
	public partial class OrderLinesRequiresReaction : System.Web.UI.UserControl
	{
		#region Load OrderLines

		public static OrderLine[] GetOrderLines( int sort )
		{
			return OrderTracking.GetOrderLinesRequiresReaction( SiteContext.Current.CurrentClient.Profile.ClientId, (OrderTracking.OrderLineSortFields)sort );
		}

		#endregion

		protected string GetThisUrl()
		{
			return ( (IPageUrlState)Page ).UrlStateContainer.GetPageUrl();
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			if( !IsPostBack )
			{
				//сортировка
				foreach( OrderTracking.OrderLineSortFields sortField in Enum.GetValues( typeof( OrderTracking.OrderLineSortFields ) ) )
				{
					_sortBox.Items.Add( new ListItem( sortField.GetDescription(), ( (int)sortField ).ToString() ) );
				}
				_sortBox.SelectedValue = Convert.ToString( (int)OrderTracking.OrderLineSortFields.OrderIDDesc );
				
				//восстановить состояние
				if( !string.IsNullOrEmpty( Request[ "view" ] ) )
				{
					var view = (PrivateOffice.OrderList.Views)int.Parse( Request[ "view" ] );
					if (view == PrivateOffice.OrderList.Views.RequiresReactionView)
					{
						if (!string.IsNullOrEmpty(Request["sort"]))
							_sortBox.SelectedValue = Request["sort"];
					}
				}
			}
		}

		protected void OnClientChangeReaction( object sender, ChangeReactionEventArgs e )
		{
			if( SiteContext.Current.User.Role != SecurityRole.Client )
				throw new HttpException( (int)HttpStatusCode.Forbidden, "Access denied" );
			OrderBO.ApplyStatusChangeClientReaction( e.OrderLineId, e.Reaction, DateTime.Now );
			_listView.DataBind();
		}

		protected void _sortBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			_listView.DataBind();
		}

		protected void _listView_DataBinding( object sender, EventArgs e )
		{
			//сохранить информацию о состоянии
			IPageUrlState page = (IPageUrlState)Page;
			page.UrlStateContainer[ "sort" ] = _sortBox.SelectedValue;

            //OrderLine[] orderLines = GetOrderLines(0);
            //if (orderLines != null)
            //    foreach (var orderLine in orderLines)
            //    {
            //        if (orderLine.CurrentStatus == OrderLineStatusUtil.StatusByte("PartNumberTransition") && orderLine.ParentOrderLine != null)
            //        {
            //            PartNumberTransitionHintPlaceHolder.Visible = true;
            //            break;
            //        }
            //    }
		}

		protected void _objectDataSource_Selected( object sender, ObjectDataSourceStatusEventArgs e )
		{
			if( e.ReturnValue.GetType().IsArray )
			{
				int count = ((Array)e.ReturnValue).Length;
				_sortBlock.Visible = count != 0;
			}
		}
	}
}