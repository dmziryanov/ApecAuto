using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Common.Web.UrlState;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;
using System.Net;
using RmsAuto.Store.Cms.Routing;
using System.Text;
using System.Data;

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class ClientOrderLines : System.Web.UI.UserControl
	{
		public string CurrentClientId { get { return (hfClientId == null ? "" : (string)hfClientId.Value); } set { hfClientId.Value = value; } }
		public string CurrentClientName { get { return (string.IsNullOrEmpty(hfClientName.Value) ? "Customer is not selected" : (string)hfClientName.Value); } set { hfClientName.Value = value; } }

		//public string CurrentClientId { get; set; }

		//public string CurrentClientName { get; set; }


		//TODO:Это пока не нужно

		// public static OrderTracking.OrderLineTotals CurrentOrderLineTotals
		// {
		//     get { return (OrderTracking.OrderLineTotals)HttpContext.Current.Items["RmsAuto.Store.Web.ControlsOrderLinesWholesale.CurrentOrderLineTotals"]; }
		//     set { HttpContext.Current.Items["RmsAuto.Store.Web.ControlsOrderLinesWholesale.CurrentOrderLineTotals"] = value; }
		//}
		//public static OrderLine[] CurrentOrderLines
		//{
		//     get { return (OrderLine[])HttpContext.Current.Items["RmsAuto.Store.Web.ControlsOrderLinesWholesale.CurrentOrderLines"]; }
		//     set { HttpContext.Current.Items["RmsAuto.Store.Web.ControlsOrderLinesWholesale.CurrentOrderLines"] = value; }
		//}
		protected void Page_Load(object sender, EventArgs e)
		{
			//  CurrentClientId = _listSearchResults.SelectedValue;
			_objectDataSource.DataBind();
		}

		protected void _btnSearchClient_Click(object sender, EventArgs e)
		{
			var searchResults = ClientSearch.Search(
					_txtClientName.Text.Trim(),
					"",
					ClientSearchMatching.Fuzzy);

			_listSearchResults.DataSource = searchResults;
			_listSearchResults.DataBind();
		}

		public int GetOrderLinesCount(string ClientId, string VaryParam)
		{
            using (var DC = new DCFactory<StoreDataContext>(IsolationLevel.ReadCommitted, false, SiteContext.Current.InternalFranchName, true))
            {
                var ordlns = from n in DC.DataContext.OrderLines
                             join
                                 m in DC.DataContext.Orders on n.OrderID equals m.OrderID
                             where m.ClientID == ClientId && n.CurrentStatus == 161
                             select n;

                return ordlns.ToArray().Count();
            }
		}

		public OrderLine[] GetOrderLines(int size, int startIndex, string ClientID, string VaryParam)
		{
			OrderLine[] res;
            using (var DC = new DCFactory<StoreDataContext>(IsolationLevel.ReadCommitted, false, SiteContext.Current.InternalFranchName, true))
            {
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<OrderLine>(l => l.OrderLineStatusChanges);
                options.LoadWith<OrderLine>(l => l.Order);
                DC.DataContext.LoadOptions = options;
                var ordlns = from n in DC.DataContext.OrderLines
                             join
                                 m in DC.DataContext.Orders on n.OrderID equals m.OrderID
                             where m.ClientID == ClientID && n.CurrentStatus == 161
                             select n;

                res = ordlns.OrderBy(x => x.OrderID).Skip(startIndex).Take(size).ToArray();
            }

			return res;
		}

		protected void _listSearchResults_SelectedIndexChanged(object sender, EventArgs e)
		{
			CurrentClientId = _listSearchResults.SelectedValue;
            CurrentClientName = _listSearchResults.SelectedItem.Text + _listSearchResults.SelectedValue.WithBrackets();

			//снова скрываем кнопку "отгрузить" (до тех пор пока не будет нажата кнопка "накладная")

			//Page.Response.Redirect(Page.Request.Url.ToString(), true);
			//Page.ClientScript.RegisterStartupScript(this.GetType(), "newwnd", "location.reload();", true);
		}

		protected void _listView_DataBinding(object sender, EventArgs e)
		{
			//сохранить информацию о состоянии
			IPageUrlState page = (IPageUrlState)Page;
			//page.UrlStateContainer["sort"] = _sortBox.SelectedValue;
			//page.UrlStateContainer["start"] = _dataPager.StartRowIndex.ToString();
			//page.UrlStateContainer["size"] = _dataPager.PageSize.ToString();

			//page.UrlStateContainer["orderid"] = _objectDataSource.SelectParameters["orderID"].DefaultValue;
			//page.UrlStateContainer["orderdate"] = _objectDataSource.SelectParameters["orderDate"].DefaultValue;
			//page.UrlStateContainer["manufacturer"] = _objectDataSource.SelectParameters["manufacturer"].DefaultValue;
			//page.UrlStateContainer["partnumber"] = _objectDataSource.SelectParameters["partNumber"].DefaultValue;
			//page.UrlStateContainer["partname"] = _objectDataSource.SelectParameters["partName"].DefaultValue;
			//page.UrlStateContainer["estsupplydate"] = _objectDataSource.SelectParameters["estSupplyDate"].DefaultValue;
			//page.UrlStateContainer["status"] = _objectDataSource.SelectParameters["status"].DefaultValue;
		}

		protected void _listView_DataBound(object sender, EventArgs e)
		{
			//  if (CurrentOrderLineTotals != null)
			{
				// _totalLabel.Text = string.Format("{0:### ### ##0.00}", CurrentOrderLineTotals.TotalSum);
			}
			// if (CurrentOrderLines != null)
			{
				//  _pageTotalLabel.Text = string.Format("{0:### ### ##0.00}", CurrentOrderLines.AsQueryable().Where(OrderBO.TotalStatusExpression).Sum(l => l.Total));
				//foreach (var orderLine in CurrentOrderLines)
				//{
				//    if (orderLine.CurrentStatus == OrderLineStatusUtil.StatusByte("PartNumberTransition") && orderLine.ParentOrderLine != null)
				//    {
				//        PartNumberTransitionHintPlaceHolder.Visible = true;
				//        break;
				//    }
				//}
			}
		}

		protected void _objectDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			if (e.ReturnValue.GetType() == typeof(int))
			{
				int count = Convert.ToInt32(e.ReturnValue);
				//_sortBlock.Visible = count != 0;
				//_dataPager.Visible = count != 0;
				//_footerBlock.Visible = count != 0;
			}
		}

		protected void _pageSizeBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			_dataPager.SetPageProperties(0, Convert.ToInt32(_pageSizeBox.SelectedValue), true);
		}

		protected void lbPrint_Click(object sender, EventArgs e)
		{
            //this.Session["PrintOrderLineIDs"] = hfOrderlinesIDS.Value.TrimEnd(';'); // Убираем последнюю точку с запятой
            //Session["ClientID"] = CurrentClientId;
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "showbtn", "$('#lbShip').css('visibility', 'visible');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "newwnd", "window.open('OrderLinesPrint.aspx', '_target1');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "newwnd2", "window.open('OrderLinesPrint.aspx?ToFile=false', '_target2');", true);
            
		}

        protected void lbCommercialInvoice_Click(object sender, EventArgs e)
        {
            this.Session["PrintOrderLineIDs"] = hfOrderlinesIDS.Value;
            Session["ClientID"] = CurrentClientId;
            Response.Redirect(String.Format("~/cms/invoice.ashx?type=commercial&country={0}&descr={1}", HttpUtility.UrlEncode(txtCountryOfOrigin1.Text), HttpUtility.UrlEncode(txtDescrOfGoods1.Text)));
        }

        protected void lbPackingInvoice_Click(object sender, EventArgs e)
        {
            this.Session["PrintOrderLineIDs"] = hfOrderlinesIDS.Value;
            Session["ClientID"] = CurrentClientId;
            Response.Redirect(String.Format("~/cms/invoice.ashx?type=packing&country={0}&descr={1}&pkgs={2}&weight={3}", HttpUtility.UrlEncode(txtCountryOfOrigin2.Text), HttpUtility.UrlEncode(txtDescrOfGoods2.Text), HttpUtility.UrlEncode(txtTotalPkgs.Text), HttpUtility.UrlEncode(txtTotalWeight.Text)));
        }


		protected void lbShip_Click(object sender, EventArgs e)
		{
            //int[] ids = hfOrderlinesIDS.Value.TrimEnd(';').Split(';').Select(s => int.Parse(s)).ToArray();
            

            //using (var dc = new DCFactory<StoreDataContext>())
            //{
            //    var printLines = from ol in dc.DataContext.OrderLines
            //                     where ids.Contains(ol.OrderLineID)
            //                     select ol;
            //    printLines.Each(x => x.CurrentStatus = 162);
            //    var summ = printLines.ToArray().Sum(pl => pl.Total);
            //    dc.DataContext.SubmitChanges();
            //    //Вносим платеж
            //    var userID = dc.DataContext.Users.Where(u => u.AcctgID.Equals(CurrentClientId)).Select(u => u.UserID).Single();
            //    LightBO.AddUserLightPayment(new UserLightPayment()
            //    {
            //        PaymentDate = DateTime.Now,
            //        PaymentSum = summ,
            //        PaymentType = LightPaymentType.Shipping,
            //        UserID = userID
            //    });
            //}

            //hfVaryParam.Value = ((new Random()).Next()).ToString();
		}
	}
}