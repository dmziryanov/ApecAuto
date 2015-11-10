using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Dac;
using System.Data;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.Manager.Controls
{
   
    public partial class SupplyReportTable : System.Web.UI.UserControl
    {
        /// <summary>
        /// В это пабликовое свойство класть то что нужно отобразить
        /// </summary>
        public IEnumerable<ReportString> searchResults { get { return (IEnumerable<ReportString>)ViewState["SupplyReportString"]; } set { ViewState["SupplyReportString"] = value; } }
        public List<ReportString> DTSorting { get { return (List<ReportString>)ViewState["DTSorting"]; } set { ViewState["DTSorting"] = value; } }
        public DateTime LowDate { get { return (DateTime)Session["LowDateSaleReport"]; } set { Session["LowDateSaleReport"] = value; } }
        public DateTime HiDate { get { return (DateTime)Session["HiDateSaleReport"]; } set { Session["HiDateSaleReport"] = value; } }
        public string OrderId { get { return (string)Session["OrderIdSaleReport"]; } set { Session["OrderIdSaleReport"] = value; } }
        public static string sortExpression;
        public static string direction;
        public string CurrentUserId { get { return (string)Session["CurrentUserId"]; } set { Session["CurrentUserId"] = value; } }


        public int CurrentPageIndex
        {
            get { return ReportGrid.PageIndex; }
        }


        public int CurrentPageSize
        {
            get { return ReportGrid.PageSize; }
        }

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

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        public new void DataBind()
        {
            using (var ctx = new DCWrappersFactory<StoreDataContext>())
            {


                var TotalStringCount = LightBO.GetStringCount(LowDate, HiDate, CurrentPageSize, CurrentPageIndex, CurrentUserId, OrderId, LightBO.SupplyOrderLineStatus);
                List<ReportString> ordlnsdto = new List<ReportString>();
                //Вставили пустые те, что до заполненных
                ordlnsdto.InsertRange(0, new ReportString[(CurrentPageSize * (CurrentPageIndex))].ToList());
                ordlnsdto.InsertRange(ordlnsdto.Count, LightBO.GetSupplyStrings(LowDate, HiDate, CurrentPageSize, CurrentPageIndex, sortExpression, direction, CurrentUserId, OrderId));
                ordlnsdto.InsertRange(ordlnsdto.Count, new ReportString[(TotalStringCount - ordlnsdto.Count)].ToList()); //Добавили недостающие пустые строчки


                LightBO.FillSupplyPrice(ordlnsdto);
                ordlnsdto.FindAll(x => x != null).ForEach(n => n.Total = n.UnitPrice * n.Qty);

                 //Итоговая строчка добаляется на последней странице
                if ((TotalStringCount + 1) / CurrentPageSize == CurrentPageIndex && ordlnsdto.Count > 0)
                {

                    ordlnsdto = LightBO.GetSupplyStrings(LowDate, HiDate, TotalStringCount, 0, sortExpression, direction, CurrentUserId, OrderId);
                    LightBO.FillSupplyPrice(ordlnsdto);
                    ReportString s = new ReportString() { StatusChangeTime = HiDate, ClientName = ReportString.TotalString, Qty = ordlnsdto.Sum(y => y.Qty), SupplyTotal = ordlnsdto.Sum(x => x.SupplyTotal) };
                    s.SortAttribute = 1;
                    ordlnsdto.Add(s);
                }

                ReportGrid.DataSource = ordlnsdto.ToArray();
                ReportGrid.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SortList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_btnSearchClient_Click(null, null);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
        }


        //protected void ClientGridView_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    if (e.SortExpression == "ClientName")
        //        if (e.SortDirection == SortDirection.Ascending)
        //            ClientGridView.DataSource = searchResults.OrderBy(x => x.ClientName);
        //        else
        //            ClientGridView.DataSource = searchResults.OrderByDescending(x => x.ClientName);

        //    //if (SortList.SelectedValue == "1")
        //    //if (SortList.SelectedValue == "2")
        //    //    searchResults = searchResults.OrderBy(x => x.CreationTime);
        //}


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
            //Эти переменные потом используются в DataBind()
            SupplyReportTable.sortExpression = sortExpression;
            SupplyReportTable.direction = direction;

            DataBind();
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

        protected void _pageSizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportGrid.PageSize = Convert.ToInt32(_pageSizeBox.SelectedValue);
            DataBind();
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

       
        protected void ClientGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGrid.PageIndex = e.NewPageIndex;
            DataBind();
        }
    }
}