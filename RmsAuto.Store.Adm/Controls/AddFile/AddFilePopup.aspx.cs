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

namespace RmsAuto.Store.Adm.Controls.AddFile
{
    public partial class AddFilePopup : System.Web.UI.Page
	{
		protected void Page_Load( object sender, EventArgs e )
		{
            AddFileControl1.SelectType = (AddFileControl.AddFileSelectType)Enum.Parse(typeof(AddFileControl.AddFileSelectType), Request["select"], true);

			switch( AddFileControl1.SelectType )
			{
                case AddFileControl.AddFileSelectType.File: _subTitleLiteral.Text = "Choose file"; break;
                case AddFileControl.AddFileSelectType.Image: _subTitleLiteral.Text = "Choose image"; break;
                case AddFileControl.AddFileSelectType.Folder: _subTitleLiteral.Text = "Choose folder"; break;
			}
			Response.Cache.SetCacheability( HttpCacheability.NoCache );
			ScriptsManager.RegisterJQuery( this );
		}
	}

}
