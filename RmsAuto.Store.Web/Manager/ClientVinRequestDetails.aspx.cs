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
using System.Net;

namespace RmsAuto.Store.Web.Manager
{
	public partial class ClientVinRequestDetails : ClientBoundPage
	{
		public static string GetUrl( int vinRequestId )
		{
			return string.Format( "~/Manager/ClientVinRequestDetails.aspx?id={0}", vinRequestId );
		}

        public override ClientDataSection DataSection
        {
            get { return ClientDataSection.VinRequests; }
        }

		protected override void OnPreInit(EventArgs e)
		{
			throw new Exception("Page not found.");
			//base.OnPreInit(e);
		}
		protected void Page_Load( object sender, EventArgs e )
		{
		}
	}
}
