using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;
using RmsAuto.Store.Web.Manager.Controls;
using RmsAuto.Store.Dac;
using RmsAuto.Store.BL;
using RmsAuto.Store.Web.Manager.BasePages;
using System.Data;

namespace RmsAuto.Store.Web.Manager
{
    [LightPage()]
    public partial class SupplyReportT : RMMPage
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            _btnUnloadReport.OnClientClick = "docOpen();";
            Page.ClientScript.RegisterStartupScript(
            this.GetType(),
            "__docOpen",
            "<script type='text/javascript'>docOpen = function() {" + String.Format("window:open('/Manager/SupplyReport.ashx?lowDate={0}&hiDate={1}&UserID={2}&OrderId={3}')", LowDate, HiDate, srt.CurrentUserId, srt.OrderId) + "};</script>");
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            _btnUnloadReport.Visible = false;
        }

        protected void _btnFillReport_Click(object sender, EventArgs e)
        {
            
                var UserId = string.IsNullOrEmpty(ClientSearchList.CurrentUserID) ? -100 : int.Parse(ClientSearchList.CurrentUserID);


                if (string.IsNullOrEmpty(RegDateMin.Text) || string.IsNullOrEmpty(RegDateMax.Text))
                {
                    ShowMessage("Fill date boxes");
                    return;
                }
                
                _btnUnloadReport.Visible = true;
                srt.CurrentUserId = UserId.ToString();
                srt.OrderId = OrderId.Text;
                srt.HiDate = HiDate;
                srt.LowDate = LowDate;
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
