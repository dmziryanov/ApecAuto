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
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Configuration;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;
using System.Collections.Specialized;
using RmsAuto.Common.Web.UrlState;
using System.Net;
using System.Globalization;
using System.Collections.Generic;

namespace RmsAuto.Store.Web.Controls
{
	public partial class Reclamation : System.Web.UI.UserControl
	{
		public string Mode { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				//режим отображения
				if ( !string.IsNullOrEmpty( Mode ) && Mode.Equals( "list" ) )
				{
					_mvReclamations.ActiveViewIndex = 2;
				}
			}
		}
	}
}