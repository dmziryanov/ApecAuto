using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Common.Misc;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Adm
{
	public partial class AcctgTransferErrors : Security.BasePage/*System.Web.UI.Page*/
    {
        public string TransferStartTime
        {
            get { return (string)ViewState["__tst"]; }
            set { ViewState["__tst"] = value;  }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TransferStartTime = TransferChangesLogEntry
                    .Load(Convert.ToInt32(Request["id"]))
                    .TransferStartTime
                    .To_ddMMyyyy_HHmmss_String();
            }
        }
    }
}
