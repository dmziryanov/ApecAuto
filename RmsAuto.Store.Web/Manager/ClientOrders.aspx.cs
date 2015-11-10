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
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;
using System.Net;
using RmsAuto.Common.Web.UrlState;

namespace RmsAuto.Store.Web.Manager
{
	public partial class ClientOrders : ClientBoundPage, IPageUrlState
	{
		public static string GetUrl()
		{
			return "~/Manager/ClientOrders.aspx";
		}

        public override ClientDataSection DataSection
        {
            get { return ClientDataSection.Orders; }
        }

      
        
		protected void Page_Load( object sender, EventArgs e )
		{
			if( !( (ManagerSiteContext)SiteContext.Current ).ClientDataSectionEnabled( ClientDataSection.Orders ) )
				throw new HttpException( (int)HttpStatusCode.Forbidden, "Access Denied" );

			if( !IsPostBack )
			{
				if( !string.IsNullOrEmpty( Request[ "view" ] ) )
				{
					RmsAuto.Store.Web.PrivateOffice.OrderList.Views view = (RmsAuto.Store.Web.PrivateOffice.OrderList.Views)int.Parse( Request[ "view" ] );
					_multiView.ActiveViewIndex = (int)view;
				}
				else
				{
					//проверить наличие записей требующих подтверждения
					var reqReactionCount = OrderTracking.GetOrderLinesRequiresReactionCount( SiteContext.Current.CurrentClient.Profile.ClientId );
					if( reqReactionCount == 0 )
						_multiView.ActiveViewIndex = (int)RmsAuto.Store.Web.PrivateOffice.OrderList.Views.ActiveOrdersView;
				}

                if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite && SiteContext.Current.User.Role == SecurityRole.Manager)
                {
                    OrderList1.Visible = false;
                    OrderList2.Visible = false;
                    OrderList3.Visible = true;
                    OrderList4.Visible = true;
                }
                else
                {
                    OrderList3.Visible = false;
                    OrderList4.Visible = false;

                    OrderList1.Visible = true;
                    OrderList2.Visible = true;
                }
			}

			UrlStateContainer[ "view" ] = Convert.ToString( _multiView.ActiveViewIndex );
		}

		protected void _multiView_ActiveViewChanged( object sender, EventArgs e )
		{
			UrlStateContainer[ "view" ] = Convert.ToString( _multiView.ActiveViewIndex );
		}


		#region IPageUrlState Members

		public UrlStateContainer UrlStateContainer
		{
			get { return _urlStateContainer; }
		}
		UrlStateContainer _urlStateContainer;

		protected override void OnPreInit( EventArgs e )
		{
			_urlStateContainer = new UrlStateContainer( Request.Url );

			base.OnPreInit( e );
		}

		protected override void Render( HtmlTextWriter writer )
		{
			Form.Action = UrlStateContainer.GetBasePageUrl();

			base.Render( writer );
		}

		#endregion
	}
}
