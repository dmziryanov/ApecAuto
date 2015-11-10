using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Controls
{
	public partial class LiveTexScript : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (SiteContext.Current.InternalFranchName == "omsk")
			{
				string script = @"
					var liveTex = true,
							liveTexID = 56164,
							liveTex_object = true;
						(function() {
							var lt = document.createElement('script');
							lt.type ='text/javascript';
							lt.async = true;
							lt.src = 'http://cs15.livetex.ru/js/client.js';
							var sc = document.getElementsByTagName('script')[0];
							sc.parentNode.insertBefore(lt, sc);
						})();";
				if (!Page.ClientScript.IsClientScriptBlockRegistered("LiveTex"))
				{
					Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LiveTex", script, true);
				}
			}
		}
	}
}