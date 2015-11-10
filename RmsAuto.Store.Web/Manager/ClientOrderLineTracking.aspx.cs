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
//using RmsAuto.Store.Cms;

namespace RmsAuto.Store.Web.Manager
{
	public partial class ClientOrderLineTracking : ClientBoundPage, IPageUrlState
	{
		public override ClientDataSection DataSection
		{
			get { return ClientDataSection.Orders; }
		}

		public static string GetUrl( int orderLineID, string backUrl )
		{
			return string.Format(
				"~/Manager/ClientOrderLineTracking.aspx?ID={0}&back_url={1}",
				orderLineID,
				HttpUtility.UrlEncode( backUrl ) );
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			OrderLineTracking1.OrderLineId = Convert.ToInt32( Request[ "ID" ] );

			_backLink.NavigateUrl = Request[ "back_url" ];
			_backLink.Visible = !string.IsNullOrEmpty( _backLink.NavigateUrl );

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
