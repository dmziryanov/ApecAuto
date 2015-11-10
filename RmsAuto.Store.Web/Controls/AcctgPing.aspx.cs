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
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Controls
{
	[Obsolete]
	public partial class AcctgPing1 : System.Web.UI.Page
	{
		protected void Page_Load( object sender, EventArgs e )
		{
		}

		PingInfo Info
		{
			get { return (PingInfo)Session[ "AcctgPing1.Info" ]; }
			set { Session[ "AcctgPing1.Info" ] = value; }
		}

		private void SendPing()
		{
			PingInfo info = new PingInfo();
			info.SendDate = DateTime.Now;

			try
			{
				//info.ReceiveValue = SystemMonitor.Ping( info.SendDate );
			}
			catch( Exception e )
			{
				info.ErrorMessage = e.Message;
			}
			finally
			{
				info.ReceiveDate = DateTime.Now;
			}
			Info = info;
		}

		protected void PingButton_Click( object sender, EventArgs e )
		{
			SendPing();
		}

		protected void Page_PreRender()
		{
			if( Info == null )
				SendPing();

			if( Info != null )
			{
				_ltrWebTime.Text = Info.SendDate.ToString( "dd.MM.yyyy HH:mm:ss" );
				_ltrRecvTime.Text = Info.ReceiveDate.ToString( "dd.MM.yyyy HH:mm:ss" );
				_ltrDelayTime.Text = string.Format( "{0:0.000}", ( Info.ReceiveDate - Info.SendDate ).TotalSeconds );
				if( Info.ReceiveValue != null )
					_ltrAcctgTime.Text = Info.ReceiveValue.Value.ToString( "dd.MM.yyyy HH:mm:ss" );
				else
					_ltrAcctgTime.Text = Info.ErrorMessage;
			}
		}

		class PingInfo
		{
			public DateTime SendDate;
			public DateTime ReceiveDate;
			public DateTime? ReceiveValue;
			public string ErrorMessage;
		}
	}

}
