using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.BL;
using System.Globalization;

namespace RmsAuto.Store.Adm
{
	public partial class AcctgTransferChangesLog : Security.BasePage/*System.Web.UI.Page*/
    {
        private DateTime? _lastStatusChangeTime = OrderBO.GetLastOrderLineStatusChangeTime();

        public DateTime? LastStatusChangeTime
        {
            get { return _lastStatusChangeTime; }
        }

        public DateTime NextCheckpointTime
        {
            get 
            {
				if (!string.IsNullOrEmpty(_txtCheckpointTime.Text))
				{
					return DateTime.Parse(_txtCheckpointTime.Text, new CultureInfo("ru-RU"));
					//return DateTime.ParseExact(_txtCheckpointTime.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
				}
				else
					return
						_lastStatusChangeTime ??
						System.Data.SqlTypes.SqlDateTime.MinValue.Value; 
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                _logEntriesView.PageIndex = Convert.ToInt32(Request["page"]);
        }

		//protected void _getChanges_Click(object sender, EventArgs e)
		//{
		//    OrderBO.ReceiveAcctgOrderChanges(NextCheckpointTime);
		//    _logEntriesView.DataBind();
		//}

        protected void _clearLogEntries_Click(object sender, EventArgs e)
        {
            OrderBO.ClearTransferChangesLog();
            _logEntriesView.DataBind();
        }
    }
}
