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

namespace RmsAuto.Store.Web.Manager.Store
{
	public class FloatingSearchFormLink : HyperLink
	{
		public string OnClientSearch { get; set; }

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );
			base.NavigateUrl = "#";
		}

		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );

			if( Visible )
			{
				base.Attributes[ "onclick" ] = "FloatingSearchFormLink_ShowFloatingSearchWindow(); return false;";

				Page.ClientScript.RegisterClientScriptBlock(
					this.GetType(),
					"search",
					@"
					window.FloatingSearchForm_Callback = function( e ) { " + OnClientSearch + @" }

					function FloatingSearchFormLink_ShowFloatingSearchWindow() 
					{
						var wnd = open( '" + ResolveUrl( "~/Manager/Store/FloatingSearchForm.aspx" ) + @"', 'FloatingSearchWindow', 'scroll=no,toolbar=no,width=200,height=150', true );
						wnd.focus(); 
					}",
					true );
			}
		}


		
	}
}
