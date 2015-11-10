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
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Dac;
using System.ComponentModel;

namespace RmsAuto.Store.Web.Cms.TextItems
{
	public partial class TextItemControl : System.Web.UI.UserControl
	{
		public string TextItemID
		{
			get { return (string)ViewState[ "TextItemID" ]; }
			set { ViewState[ "TextItemID" ] = value; }
		}

		public string DefaultHeader
		{
			get { return (string)ViewState[ "DefaultHeader" ]; }
			set { ViewState[ "DefaultHeader" ] = value; }
		}

		public string DefaultBody
		{
			get { return (string)ViewState[ "DefaultBody" ]; }
			set { ViewState[ "DefaultBody" ] = value; }
		}

		[DefaultValue( true )]
		public bool ShowHeader
		{
			get { return ViewState[ "ShowHeader" ]!=null ? (bool)ViewState[ "ShowHeader" ] : true; }
			set { ViewState[ "ShowHeader" ] = value; }
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
            var textItem = TextItemsDac.GetTextItem( TextItemID, SiteContext/*.Current*/.CurrentCulture );

			var header = textItem != null ? textItem.TextItemHeader : DefaultHeader;
			var body = textItem != null ? textItem.TextItemBody : DefaultBody;

			if( ShowHeader && !string.IsNullOrEmpty( header ) )
			{
				_headerPlaceHolder.Visible = true;
				_headerLabel.Text = Server.HtmlEncode( header );
			}
			else
			{
				_headerPlaceHolder.Visible = false;
			}
			_bodyLabel.Text = body;
		}
	}
}