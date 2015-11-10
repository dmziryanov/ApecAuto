using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.BL;
using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    [LightPage()]
    public partial class FinancialReport : RMMPage
    {
		public DateTime HiDate
		{
			get
			{
				DateTime dt = DateTime.MaxValue;
                DateTime.TryParse(RegDateMax.Text, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out dt);
				return dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
			}
		}
		
        public DateTime LowDate
		{
			get
			{
				DateTime dt = DateTime.MinValue;
                DateTime.TryParse(RegDateMin.Text, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out dt);
				return dt;
			}
		}
		protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _managerList.DataSource = ManagerBO.GetManagerList(Resources.Texts.NotSelected);
                _managerList.DataBind();
            }
            _btnUnloadReport.Visible = false;
        }

        protected void _btnFillReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RegDateMin.Text) || string.IsNullOrEmpty(RegDateMax.Text))
            {
                ShowMessage("Fill date boxes");
                return;
            }

            _btnUnloadReport.Visible = true;

			if (!string.IsNullOrEmpty(ClientSearchList.CurrentUserID))
			{
				srt.UserId = int.Parse(ClientSearchList.CurrentUserID);
			}
            srt.ManagerId = _managerList.SelectedValue;
			srt.pDateMin = LowDate;
			srt.pDateMax = HiDate;
            srt.DataBind();
        }

        private void ShowMessage(string message)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>");
        }
    }
}
