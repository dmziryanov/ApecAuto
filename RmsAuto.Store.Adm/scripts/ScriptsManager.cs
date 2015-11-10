using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace RmsAuto.Store.Adm.scripts
{
	public static class ScriptsManager
	{
		public static void RegisterJQuery( Page page )
		{
			if( !page.ClientScript.IsClientScriptIncludeRegistered( "jquery" ) )
			{
				page.ClientScript.RegisterClientScriptInclude(
					"jquery", page.ResolveUrl( "~/scripts/jquery-1.3.2.min.js" ) );
			}
		}
		/*public static void RegisterTinyMce( Page page )
		{
			if( !page.ClientScript.IsClientScriptIncludeRegistered( "tinymce" ) )
			{
				page.ClientScript.RegisterClientScriptInclude(
					"tinymce", page.ResolveUrl( "~/scripts/tinymce/jscripts/tiny_mce.js" ) );
			}
		}*/
		public static void RegisterMsoFramework( Page page )
		{
			if( !page.ClientScript.IsClientScriptIncludeRegistered( "mso" ) )
			{
				page.ClientScript.RegisterClientScriptInclude(
					"mso", page.ResolveUrl( "~/scripts/mso.js" ) );
			}
		}
		public static void RegisterRmsTinyMce( Page page )
		{
			if( !page.ClientScript.IsClientScriptIncludeRegistered( "rms_tinymce" ) )
			{
				page.ClientScript.RegisterClientScriptInclude(
					"rms_tinymce", page.ResolveUrl( "~/scripts/tinymce/rms_tinymce.js" ) );
			}
		}


	}
}
