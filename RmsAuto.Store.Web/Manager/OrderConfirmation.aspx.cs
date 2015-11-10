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
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Manager
{
	public partial class OrderConfirmation : ClientBoundPage
	{
		public static string GetUrl()
		{
			return "~/Manager/OrderConfirmation.aspx";
		}

        public override ClientDataSection DataSection
        {
            get { return ClientDataSection.Cart; }
        }

        protected string OrderNumber
        {
            get
            {
				if( string.IsNullOrEmpty( Request[ "orderNo" ] ) )
					throw new BLException( "Отсутствует номер заказа" );
				var orderNo = int.Parse( Request[ "orderNo" ] );
                return OrderTracking.GetOrderDisplayNumber( OrderBO.LoadOrderData( ClientData.Profile.ClientId, orderNo ) );
            }
        }

		protected void Page_Load( object sender, EventArgs e )
		{
            _btnContinue.PostBackUrl = Default.GetUrl();
	    }
	}
}
