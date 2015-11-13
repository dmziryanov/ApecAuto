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

namespace RmsAuto.Store.Web.Manager
{
    [LightPage()]
    public partial class SaleReportTable : RMMPage
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
            "<script type='text/javascript'>docOpen = function() {" + String.Format("window:open('/Manager/SaleReport.ashx?lowDate={0}&hiDate={1}&UserID={2}&OrderId={3}')", LowDate, HiDate, srt.CurrentUserId, srt.OrderId) + "};</script>");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _btnUnloadReport.Visible = false;
        }


        protected void _btnFillReport_Click(object sender, EventArgs e)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {

                var UserId = string.IsNullOrEmpty(ClientSearchList.CurrentUserID) ? -100 : int.Parse(ClientSearchList.CurrentUserID);

                if (string.IsNullOrEmpty(RegDateMin.Text) || string.IsNullOrEmpty(RegDateMax.Text))
                {
                    ShowMessage("Fill order dates");
                    return;
                }

                _btnUnloadReport.Visible = true;

                //List<SaleReportString> ordlnsdto;

                              
            //    ordlnsdto = new List<SaleReportString>();
                //Вставили пустые те, что до заполненных
                //ordlnsdto.InsertRange(0, new List<SaleReportString>(srt.CurrentPageSize * (srt.CurrentPageIndex == 0 ? 1 : srt.CurrentPageIndex) - 1));
                //ordlnsdto.InsertRange(ordlnsdto.Count, GetStrings(ctx.DataContext, LowDate, HiDate));
                //ordlnsdto.InsertRange(ordlnsdto.Count, new List<SaleReportString>(GetStringCount(ctx.DataContext, LowDate, HiDate) - ordlnsdto.Count));
                
                //Если пользователь не заполнен берем все заказы
                //if (UserId == -1)
                //{
                //    ordlnsdto = (from n in ctx.DataContext.OrderLines
                //                 join q in ctx.DataContext.Orders on n.OrderID equals q.OrderID
                //                 join m in ctx.DataContext.Users on q.ClientID equals m.AcctgID
                //                 join l in ctx.DataContext.OrderLineStatusChanges on n.AcctgOrderLineID equals l.OrderLineID
                //                 where 
                //                (from k in ctx.DataContext.OrderLineStatusChanges
                //                 where (k.OrderLineID == n.AcctgOrderLineID) && k.StatusChangeTime >= LowDate && k.StatusChangeTime <= HiDate && k.OrderLineStatus == 161
                //                 orderby k.StatusChangeTime
                //                 select k.OrderLineStatusChangeID).Take(1).Contains(l.OrderLineStatusChangeID)
                //                 select new SaleReportString { OrderId = n.OrderID.ToString(), SortAttribute = 0, AcctgOrderLineID = n.AcctgOrderLineID.Value.ToString(), PartNumber = n.PartNumber, PartName = n.PartName, Manufacturer = n.Manufacturer, ClientName = m.Clientname, ClientID = m.AcctgID, SupplierId = n.SupplierID, CurrentStatusDate = n.CurrentStatusDate.HasValue ? n.CurrentStatusDate.Value : new DateTime(1753, 1, 1), SupplyPrice = 0, UnitPrice = n.UnitPrice, Qty = n.Qty, SaleSumm = n.Total, ProfitSumm = (n.UnitPrice - 0) * n.Qty, ProfitPercent = 0 }).ToList();
                //}
                
                //else
                
                //{
                //    ordlnsdto = (from n in ctx.DataContext.OrderLines
                //                 join q in ctx.DataContext.Orders on n.OrderID equals q.OrderID
                //                 join m in ctx.DataContext.Users on q.ClientID equals m.AcctgID
                //                 join l in ctx.DataContext.OrderLineStatusChanges on n.AcctgOrderLineID equals l.OrderLineID
                //                 where
                //                (from k in ctx.DataContext.OrderLineStatusChanges
                //                 where (k.OrderLineID == n.AcctgOrderLineID) && k.StatusChangeTime >= LowDate && k.StatusChangeTime <= HiDate && k.OrderLineStatus == 161
                //                 orderby k.StatusChangeTime
                //                 select k.OrderLineStatusChangeID).Take(1).Contains(l.OrderLineStatusChangeID)
                //                 && m.UserID == UserId
                //                 select new SaleReportString { OrderId = n.OrderID.ToString(), SortAttribute = 0, AcctgOrderLineID = n.AcctgOrderLineID.Value.ToString(), PartNumber = n.PartNumber, PartName = n.PartName, Manufacturer = n.Manufacturer, ClientName = m.Clientname, ClientID = m.AcctgID, SupplierId = n.SupplierID, CurrentStatusDate = n.CurrentStatusDate.HasValue ? n.CurrentStatusDate.Value : new DateTime(1753, 1, 1), SupplyPrice = 0, UnitPrice = n.UnitPrice, Qty = n.Qty, SaleSumm = n.Total, ProfitSumm = (n.UnitPrice - 0) * n.Qty, ProfitPercent = 0 }).ToList();
                //}
                srt.CurrentUserId = UserId.ToString();
                if (srt.OrderId != OrderId.Text)
                {
                    srt.CurrentPageIndex = 0;
                    srt.OrderId = OrderId.Text;
                }
                srt.HiDate = HiDate;
                srt.LowDate = LowDate;
                srt.DataBind();
            }
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
