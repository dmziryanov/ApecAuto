using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Controls
{
	public partial class LogonLogoff : LogonBaseControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public override string ErrorMessage
		{
			get
			{
				return string.Empty;
			}
			set
			{
				
			}
		}
	}
}