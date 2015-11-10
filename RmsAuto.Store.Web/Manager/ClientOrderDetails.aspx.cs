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
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web.UrlState;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Manager
{
	public partial class ClientOrderDetails : ClientBoundPage, IPageUrlState
	{
		public override ClientDataSection DataSection
		{
			get { return ClientDataSection.Orders; }
		}

		public static string GetUrl( int orderId, string back_url )
		{
			return GetUrl( new[] { orderId }, back_url );
		}
		
        public static string GetUrl( int[] orderIds, string back_url )
		{
			return string.Format(
				"~/Manager/ClientOrderDetails.aspx?ID={0}&back_url={1}",
				HttpUtility.UrlEncode( string.Join( ",", orderIds.Select( id => id.ToString() ).ToArray() ) ),
				HttpUtility.UrlEncode( back_url ) );
		}

        protected void Page_InitComplete(object sender, EventArgs e)
        {
         
        }

		protected void Page_PreLoad( object sender, EventArgs e )
		{
			if( !IsPostBack )
			{
				
                var ids = Request[ "ID" ].Split( ',' ).Select(s => int.Parse( s )).ToArray();
				if( ids.Length == 0 )
					throw new ArgumentNullException( "ID" );

                if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
                {
                    _orderLinesWholesaleLite.Visible = true;
                    _orderLinesWholesale.Visible = false;    
                }
                else
                {
                    _orderLinesWholesaleLite.Visible = false;
                    _orderLinesWholesale.Visible = true;    
                }

                _orderLinesWholesale.OrderIDs = ids;
                _orderLinesWholesaleLite.OrderIDs = ids;
                
                
				_backLink.NavigateUrl = Request[ "back_url" ];
				_backLink.Visible = !string.IsNullOrEmpty( Request[ "back_url" ] );
			}

            _orderLinesWholesaleLite.SaveStatusChanges();
			UrlStateContainer.AddBaseParameterFromRequest( "ID" );
			UrlStateContainer.AddBaseParameterFromRequest( "back_url" );
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
