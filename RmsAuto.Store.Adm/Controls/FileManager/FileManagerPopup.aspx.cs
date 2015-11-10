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
using RmsAuto.Store.Adm.scripts;

namespace RmsAuto.Store.Adm.Controls.FileManager
{
	public partial class FileManagerPopup : System.Web.UI.Page
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			FileManagerControl1.SelectType = (FileManagerControl.FileManagerSelectType)Enum.Parse( typeof( FileManagerControl.FileManagerSelectType ), Request[ "select" ], true );

			switch( FileManagerControl1.SelectType )
			{
                case FileManagerControl.FileManagerSelectType.File: _subTitleLiteral.Text = "Choose file"; break;
                case FileManagerControl.FileManagerSelectType.Image: _subTitleLiteral.Text = "Choose image"; break;
				case FileManagerControl.FileManagerSelectType.Folder: _subTitleLiteral.Text = "Choose folder"; break;
			}
			Response.Cache.SetCacheability( HttpCacheability.NoCache );
			ScriptsManager.RegisterJQuery( this );
		}
	}

}
