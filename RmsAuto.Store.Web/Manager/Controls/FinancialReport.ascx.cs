using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;
using RmsAuto.Store.BL;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Web.Manager.Controls
{
    [Serializable()]
    public class FinReportString
    {
        public int UserID { get; set; }
        public string ClientName { get; set; }
		public string ClientNameDecor
		{
			get
			{
				if (ClientName != TotalString) return ClientName + AcctgID.WithBrackets();
				return TotalString;
			}
			set {}
		}
        public string AcctgID { get; set; }
        public string Manager { get; set; }
        public object CashPayments { get; set; }
        public object NonCashPayments { get; set; }
        public object CashReturn { get; set; }
        public object NonCashReturn { get; set; }
        public object GoodsReturn { get; set; }
        public object Supply { get; set; }
        public string SortAttribute { get; set; }
        public object bClientSaldo { get; set; }
        public object eClientSaldo { get; set; }

		public const string TotalString = "Total:";
    }

    public partial class FinancialReport : System.Web.UI.UserControl
    {
        public List<FinReportString> searchResults { get { return (List<FinReportString>)ViewState["FinReportString"]; } set { ViewState["FinReportString"] = value; } }
        public List<FinReportString> DTSorting { get { return (List<FinReportString>)ViewState["DTSorting"]; } set { ViewState["DTSorting"] = value; } }

        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set
            {
                ViewState["sortDirection"] = value;
            }
        }

        public string ManagerId;
        public int? UserId;
        public PaymentMethod pm;
        public DateTime pDateMin;
        public DateTime pDateMax;


        public void DataBind()
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                string s = @"Select a.UserID, u.ClientName, m.AcctgID, m.ClientName as Manager,  ABS(sum(ISNULL(CashPayments,0))) as CashPayments, ABS(sum(ISNULL(NonCashPayments,0))) as NonCashPayments, ABS(sum(ISNULL(CashReturn,0))) as CashReturn, ABS(sum(ISNULL(NonCashReturn,0))) as NonCashReturn, ABS(sum(ISNULL(GoodsReturn,0))) as GoodsReturn, sum(ISNULL(Supply,0)) as Supply from 
  (
              select UserID, 
              (Case WHEN PaymentType = 1 AND PaymentMethod = 1  Then PaymentSum End) as CashPayments, 
              0 as NonCashPayments, 
              0 as CashReturn, 
              0 as NonCashReturn, 
              0 as GoodsReturn, 
              0 as Supply,
              PaymentDate
              FROM [dbo].[UserPayments]
  UNION ALL
              select UserID, 
              0, 
              Case WHEN PaymentType = 1 AND PaymentMethod = 2  Then PaymentSum End, 
              0, 
              0, 
              0, 
              0,
              PaymentDate FROM [dbo].[UserPayments]
  UNION ALL
              select UserID, 
              0, 
              0, 
              Case WHEN PaymentType = 3 AND PaymentMethod = 1 Then PaymentSum ELSE 0 End, 
              0, 
              0, 
              0,
              PaymentDate FROM [dbo].[UserPayments]
  UNION ALL
              select UserID, 
              0, 
              0, 
              0, 
              Case WHEN PaymentType = 3 AND PaymentMethod = 2 Then PaymentSum ELSE 0 End, 
              0, 
              0,
              PaymentDate FROM [dbo].[UserPayments]
  UNION ALL
              select UserID, 
              0, 
              0, 
              0, 
              0, 
              Case WHEN PaymentType = 2 Then PaymentSum ELSE 0 End, 
              0,
              PaymentDate FROM [dbo].[UserPayments]
 UNION ALL
              select UserID, 
              0, 
              0, 
              0, 
              0, 
              0, 
              Case WHEN PaymentType = 0 Then PaymentSum ELSE 0 End,
              PaymentDate FROM [dbo].[UserPayments]
  ) a, dbo.Users u, dbo.Users m where u.InternalFranchName = {2} and a.UserID = u.UserID AND (u.ManagerId = m.AcctgID  OR u.ManagerId IS NULL) AND PaymentDate >= {0} AND PaymentDate <= {1}  Group by a.UserID, u.ClientName, m.ClientName, m.AcctgID";


                searchResults = dc.DataContext.ExecuteQuery<FinReportString>(s, pDateMin, pDateMax, SiteContext.Current.InternalFranchName).ToList();
            }

            searchResults.ForEach(x => x.bClientSaldo = LightBO.GetUserLightBalance(x.UserID, pDateMin));
            searchResults.ForEach(x => x.eClientSaldo = LightBO.GetUserLightBalance(x.UserID, pDateMax));

            if (ManagerId != null && ManagerId != "0")
                searchResults = searchResults.Where(x => x.AcctgID == ManagerId).ToList();

            if (UserId.HasValue && UserId.Value > 0)
                searchResults = searchResults.Where(x => x.UserID == UserId.Value).ToList();
            


            var SummaryString = new FinReportString() { ClientName = FinReportString.TotalString, eClientSaldo = searchResults.Sum(x => (decimal)x.eClientSaldo), bClientSaldo = searchResults.Sum(x => (decimal)x.bClientSaldo),  CashPayments = searchResults.Sum(x => (decimal)x.CashPayments), CashReturn = searchResults.Sum(x => (decimal)x.CashReturn), GoodsReturn = searchResults.Sum(x => (decimal)x.GoodsReturn), NonCashPayments = searchResults.Sum(x => (decimal)x.NonCashPayments), NonCashReturn = searchResults.Sum(x => (decimal)x.NonCashReturn), Supply = searchResults.Sum(x => (decimal)x.Supply), SortAttribute = "1" };
            searchResults.Add(SummaryString);

            Session["FinSearchResults"] = searchResults;

            ReportGrid.DataSource = searchResults;
            ReportGrid.DataBind();
        }

        protected void _pageSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportGrid.DataSource = searchResults; //Либо то, что отсортировали, либо если не успели лежит в сурсе по умолчанию
            ReportGrid.PageSize = Convert.ToInt32(_pageSizeBox.SelectedValue);
            ReportGrid.DataBind();
        }

        protected void ClientGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGrid.DataSource = searchResults; //Либо то, что отсортировали, либо если не успели лежит в сурсе по умолчанию
            ReportGrid.PageIndex = e.NewPageIndex;
            ReportGrid.DataBind();
        }

        protected void ClientGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string sortExpression = e.SortExpression;
                ViewState["z_sortexpresion"] = e.SortExpression;

                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    GridViewSortDirection = SortDirection.Descending;
                    SortGridView(sortExpression, "DESC");
                }
                else
                {
                    GridViewSortDirection = SortDirection.Ascending;
                    SortGridView(sortExpression, "ASC");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при сортировке: " + ex.Message);
            }
        }

        //TODO: Возможно стоит сделать общий GridView, который позволяет делать все
        private void SortGridView(string sortExpression, string direction)
        {
            //В массив заносятся по пордяку имя клиента 
            var sorting = new string[4] { "ClientNameASC", "ClientNameDESC", "AcctgIdASC", "AcctgIdDESC" };

            switch (Array.IndexOf(sorting, sortExpression + direction) + 1)
            {
                case 0: DTSorting = searchResults.OrderBy(x => x.SortAttribute).ThenBy(x => x.ClientName).ToList(); break;
                case 1: DTSorting = searchResults.OrderBy(x => x.SortAttribute).ThenByDescending(x => x.ClientName).ToList(); break;
                case 2: DTSorting = searchResults.OrderBy(x => x.SortAttribute).ThenBy(x => x.AcctgID).ToList(); break;
                case 3: DTSorting = searchResults.OrderBy(x => x.SortAttribute).ThenByDescending(x => x.AcctgID).ToList(); break;
            }

            //DTSorting = new DataView(DTSorting, "", sortExpression + " " + direction, DataViewRowState.CurrentRows).ToTable();
            ReportGrid.DataSource = DTSorting;
            ReportGrid.DataBind();
        }


        protected void ClientGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    if (GridViewSortDirection == SortDirection.Ascending)
                        e.Row.Cells[sortColumnIndex].Controls.Add(new Label() { Text = "↑", Visible = true });
                    else
                        e.Row.Cells[sortColumnIndex].Controls.Add(new Label() { Text = "↓", Visible = true });
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                }
            }
        }

        int GetSortColumnIndex()
        {
            // Iterate through the Columns collection to determine the index
            // of the column being sorted.
            //Здесь можно сделать  SupplyReport общим наименованием
            foreach (DataControlField field in ReportGrid.Columns)
            {
                if (field.SortExpression == (string)ViewState["z_sortexpresion"])
                {
                    return ReportGrid.Columns.IndexOf(field);
                }
            }

            return -1;
        }
    }
}