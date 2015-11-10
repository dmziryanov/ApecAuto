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
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Controls
{
	public partial class OrderLineList : System.Web.UI.UserControl
	{
		#region === Load OrderLines ===

		public static OrderTracking.OrderLineTotals CurrentOrderLineTotals
		{
			get { return (OrderTracking.OrderLineTotals)HttpContext.Current.Items["RmsAuto.Store.Web.ControlsOrderLineList.CurrentOrderLineTotals"]; }
			set { HttpContext.Current.Items["RmsAuto.Store.Web.ControlsOrderLineList.CurrentOrderLineTotals"] = value; }
		}

		public static OrderLine[] CurrentOrderLines
		{
			get { return (OrderLine[])HttpContext.Current.Items["RmsAuto.Store.Web.ControlsOrderLineList.CurrentOrderLines"]; }
			set { HttpContext.Current.Items["RmsAuto.Store.Web.ControlsOrderLineList.CurrentOrderLines"] = value; }
		}

		public OrderLineStatuses[] CurrentOrderLineStatuses
		{
			get { return (OrderLineStatuses[])HttpContext.Current.Session["RmsAuto.Store.Web.ControlsOrderLineList.CurrentOrderLineStatuses"]; }
            set { HttpContext.Current.Session["RmsAuto.Store.Web.ControlsOrderLineList.CurrentOrderLineStatuses"] = value; }
		}

		public static int GetOrderLinesCount(
			DateTime? orderDateStart, DateTime? orderDateEnd,
			Decimal? partPriceStart, Decimal? partPriceEnd,
			string partName,
			string orderIDs,
			string custOrderNums,
			string partNumbers,
			string referenceIDs,
			string manufacturers,
			string preset,
			string statusIDs,
			int sort, int startIndex, int size)
		{
			if (CurrentOrderLineTotals == null)
			{
				var filter = new OrderTracking.OrderLineAnalysisFilter();
				filter.OrderDateStart = orderDateStart;
				filter.OrderDateEnd = orderDateEnd;
				filter.PartPriceStart = partPriceStart;
				filter.PartPriceEnd = partPriceEnd;
				filter.PartName = partName;
				if (!string.IsNullOrEmpty(orderIDs))
				{
					filter.OrderIDs = orderIDs.Split(',').Select(s => int.Parse(s)).ToList<int>();
				}
				if (!string.IsNullOrEmpty(custOrderNums))
				{
					filter.CustOrderNums = custOrderNums.Split(',').ToList();
				}
				if (!string.IsNullOrEmpty(partNumbers))
				{
					filter.PartNumbers = partNumbers.Split(',').ToList();
				}
				if (!string.IsNullOrEmpty(referenceIDs))
				{
					filter.ReferenceIDs = referenceIDs.Split(',').ToList();
				}
				if (!string.IsNullOrEmpty(manufacturers))
				{
					filter.Manufacturers = manufacturers.Split(',').ToList();
				}

				if (preset == "allworked")
				{
					filter.ComplexStatusFilter = OrderTracking.ComplexStatusFilter.NotCompleted;
				}
				else if (preset == "allclosed")
				{
					filter.ComplexStatusFilter = OrderTracking.ComplexStatusFilter.Completed;
				}
				else if (!string.IsNullOrEmpty(statusIDs))
				{
					filter.StatusIDs = statusIDs.Split(',').Select(s => int.Parse(s)).ToList<int>();
				}

				CurrentOrderLineTotals = OrderTracking.GetOrderLinesCount(SiteContext.Current.CurrentClient.Profile.ClientId, filter);
			}
			return CurrentOrderLineTotals.TotalCount;
		}

		public static OrderLine[] GetOrderLines(
			DateTime? orderDateStart, DateTime? orderDateEnd,
			Decimal? partPriceStart, Decimal? partPriceEnd,
			string partName,
			string orderIDs,
			string custOrderNums,
			string partNumbers,
			string referenceIDs,
			string manufacturers,
			string preset,
			string statusIDs,
			int sort, int startIndex, int size)
		{
			if (CurrentOrderLines == null)
			{
				var filter = new OrderTracking.OrderLineAnalysisFilter();
				filter.OrderDateStart = orderDateStart;
				filter.OrderDateEnd = orderDateEnd;
				filter.PartPriceStart = partPriceStart;
				filter.PartPriceEnd = partPriceEnd;
				filter.PartName = partName;
				if (!string.IsNullOrEmpty(orderIDs))
				{
					filter.OrderIDs = orderIDs.Split(',').Select(s => int.Parse(s)).ToList<int>();
				}
				if (!string.IsNullOrEmpty(custOrderNums))
				{
					filter.CustOrderNums = custOrderNums.Split(',').ToList();
				}
				if (!string.IsNullOrEmpty(partNumbers))
				{
					filter.PartNumbers = partNumbers.Split(',').ToList();
				}
				if (!string.IsNullOrEmpty(referenceIDs))
				{
					filter.ReferenceIDs = referenceIDs.Split(',').ToList();
				}
				if (!string.IsNullOrEmpty(manufacturers))
				{
					filter.Manufacturers = manufacturers.Split(',').ToList();
				}
				if (!string.IsNullOrEmpty(statusIDs))
				{
					filter.StatusIDs = statusIDs.Split(',').Select(s => int.Parse(s)).ToList<int>();
				}

				if (preset == "allworked")
				{
					filter.ComplexStatusFilter = OrderTracking.ComplexStatusFilter.NotCompleted;
				}
				else if (preset == "allclosed")
				{
					filter.ComplexStatusFilter = OrderTracking.ComplexStatusFilter.Completed;
				}
				else if (!string.IsNullOrEmpty(statusIDs))
				{
					filter.StatusIDs = statusIDs.Split(',').Select(s => int.Parse(s)).ToList<int>();
				}

				CurrentOrderLines = OrderTracking.GetOrderLines(SiteContext.Current.CurrentClient.Profile.ClientId, filter, (OrderTracking.OrderLineSortFields)sort, startIndex, size);
			}
			return CurrentOrderLines;
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				//сортировка
				foreach (OrderTracking.OrderLineSortFields sortField in Enum.GetValues(typeof(OrderTracking.OrderLineSortFields)))
				{
					_sortBox.Items.Add(new ListItem(sortField.GetDescription(), ((int)sortField).ToString()));
				}

				_sortBox.SelectedValue = Convert.ToString((int)OrderTracking.OrderLineSortFields.OrderIDDesc);

				//фильтр по статусам
				try
				{
					InitStatusesFilter(null);
				}
				catch (Exception ex)
				{
					Logger.WriteException(ex);
				}				

				//восстановить состояние
				//if (string.IsNullOrEmpty(Request["view"])
				//    || (PrivateOffice.OrderList.Views)int.Parse(Request["view"]) == PrivateOffice.OrderList.Views.AnalysisOrders)
				//{
				//    if (!string.IsNullOrEmpty(Request["sort"]))
				//        _sortBox.SelectedValue = Request["sort"];
				//    if (!string.IsNullOrEmpty(Request["start"]) && !string.IsNullOrEmpty(Request["size"]))
				//    {
				//        _dataPager.SetPageProperties(int.Parse(Request["start"]), int.Parse(Request["size"]), false);
				//        _pageSizeBox.SelectedValue = Request["size"];
				//    }
				//    _filterStartDate.Text = Request["orderDateStart"];
				//    _filterEndDate.Text = Request["orderDateEnd"];
				//    _filterStartPrice.Text = Request["partPriceStart"];
				//    _filterEndPrice.Text = Request["partPriceEnd"];
				//    _filterPartName.Text = Request["partName"];
				//    _filterOrderIDs.Text = Request["orderIDs"];
				//    _filterCustOrderNums.Text = Request["custOrderNums"];
				//    _filterPartNumbers.Text = Request["partNumbers"];
				//    _filterReferenceIDs.Text = Request["referenceIDs"];
				//    _filterManufacturers.Text = Request["manufacturers"];
				//    if (!string.IsNullOrEmpty(Request["statusIDs"]))
				//    {
				//        //_filterStatus.SelectedValue = Request["statusIDs"];
				//    }
					
				//}
				ApplyFilters(false);
			}
		}

		#region === Validators ===
		protected void ValidateOrderIDs(object source, ServerValidateEventArgs args)
		{
			string value = NormalizeStringWithSeparator(args.Value);
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					value.Split(',').Select(s => int.Parse(s)).ToList<int>();
				}
				catch
				{
					args.IsValid = false;
					return;
				}
			}

			args.IsValid = true;
		}

		protected void ValidateDate(object source, ServerValidateEventArgs args)
		{
			string value = args.Value;
			CustomValidator validator =(CustomValidator)source;
			
			switch (validator.ControlToValidate)
			{
				case "_filterStartDate":
					//проверяем начальную дату
					try
					{
                        DateTime.Parse(_filterStartDate.Text.Trim(), CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None);
					}
					catch
					{
                        validator.Text = Resources.ValidatorsMessages.FormatError_InitialDate;
						args.IsValid = false;
						return;
					}
					break;
				case "_filterEndDate":
					//проверяем конечную дату
					DateTime endDate = DateTime.Now;
					try
					{
                        endDate = DateTime.Parse(_filterEndDate.Text.Trim(), CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None);
					}
					catch
					{
                        validator.Text = Resources.ValidatorsMessages.FormatError_FinishDate;
						args.IsValid = false;
						return;
					}
					if (!string.IsNullOrEmpty(_filterStartDate.Text))
					{
						//проверяем начальную дату
						DateTime startDate = DateTime.Now;
						try
						{
                            startDate = DateTime.Parse(_filterStartDate.Text.Trim(), CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None);
						}
						catch
						{
                            validator.Text = Resources.ValidatorsMessages.FormatError_InitialDate;
							args.IsValid = false;
							return;
						}
						//сравниваем даты
						if (endDate < startDate)
						{
                            validator.Text = Resources.ValidatorsMessages.StartDateExceedFinishDate;
							args.IsValid = false;
							return;
						}
					}
					break;
			}
			args.IsValid = true;
		}

		protected void ValidatePrice(object source, ServerValidateEventArgs args)
		{
			string value = args.Value;
			CustomValidator validator = (CustomValidator)source;
			switch (validator.ControlToValidate)
			{
				case "_filterStartPrice":
					//проверяем начальную цену
					try
					{
						Decimal.Parse(value.Trim(), NumberStyles.AllowDecimalPoint);
					}
					catch (Exception)
					{
                        validator.Text = Resources.ValidatorsMessages.FormatError_StartPrice;
						args.IsValid = false;
						return;
					}
					break;
				case "_filterEndPrice":
					//проверяем конечную цену
					Decimal endPrice = 0;
					try
					{
						endPrice = Decimal.Parse(value.Trim(), NumberStyles.AllowDecimalPoint);
					}
					catch (Exception)
					{
                        validator.Text = Resources.ValidatorsMessages.FormatError_FinalPrice;
						args.IsValid = false;
						return;
					}
					if (!string.IsNullOrEmpty(_filterStartPrice.Text))
					{
						//проверяем начальную цену
						Decimal startPrice = 0;
						try
						{
							startPrice = Decimal.Parse(_filterStartPrice.Text.Trim(), NumberStyles.AllowDecimalPoint);
						}
						catch (Exception)
						{
							validator.Text = Resources.ValidatorsMessages.FormatError_StartPrice;
							args.IsValid = false;
							return;
						}
						//сравниваем цены
						if (endPrice < startPrice)
						{
                            validator.Text = Resources.ValidatorsMessages.StartPriceExceedFinishPrice;
							args.IsValid = false;
							return;
						}
					}
					break;
			}
			args.IsValid = true;
		}
		#endregion

		protected void ddlStatusesPresets_SelectedIndexChanged(object sender, EventArgs e)
		{
			InitStatusesFilter(ddlStatusesPresets.SelectedValue);
		}

		protected void _searchButton_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
                ApplyFilters(true);
			}
		}

		protected void _clearFilterButton_Click(object sender, EventArgs e)
		{
			_filterStartDate.Text = string.Empty;
			_filterEndDate.Text = string.Empty;
			_filterStartPrice.Text = string.Empty;
			_filterEndPrice.Text = string.Empty;
			_filterPartName.Text = string.Empty;
			_filterOrderIDs.Text = string.Empty;
			_filterCustOrderNums.Text = string.Empty;
			_filterPartNumbers.Text = string.Empty;
			_filterReferenceIDs.Text = string.Empty;
			_filterManufacturers.Text = string.Empty;
			ddlStatusesPresets.SelectedValue = "all";
			InitStatusesFilter("all");
			ApplyFilters(true);

            // Task. #4947 Доработка фильтра в представлении "Анализ заказов"
            // снятие всех галочек в блоке "статусов"
            _filterStatuses.Items[0].Selected = false;
            foreach (var item in _filterStatuses.Items)
            {
                ((ListItem)item).Selected = true; // Поменял false на true (было сделано нелогично, сбросить фильтр подразумевает сброс к дефолтным настройкам, а по дефолту все статусы выделены)
            }
            _filterStatuses.Enabled = true;
		}

		private void ApplyFilters(bool bind)
		{
			_objectDataSource.SelectParameters["orderDateStart"].DefaultValue = _filterStartDate.Text.Trim();
			_objectDataSource.SelectParameters["orderDateEnd"].DefaultValue = _filterEndDate.Text.Trim();
			_objectDataSource.SelectParameters["partPriceStart"].DefaultValue = _filterStartPrice.Text.Trim();
			_objectDataSource.SelectParameters["partPriceEnd"].DefaultValue = _filterEndPrice.Text.Trim();
			_objectDataSource.SelectParameters["partName"].DefaultValue = _filterPartName.Text.Trim();
			_objectDataSource.SelectParameters["orderIDs"].DefaultValue = NormalizeStringWithSeparator(_filterOrderIDs.Text.Trim());
			_objectDataSource.SelectParameters["custOrderNums"].DefaultValue = NormalizeStringWithSeparator(_filterCustOrderNums.Text.Trim());
			_objectDataSource.SelectParameters["partNumbers"].DefaultValue = NormalizeStringWithSeparator(_filterPartNumbers.Text.Trim());
			_objectDataSource.SelectParameters["referenceIDs"].DefaultValue = NormalizeStringWithSeparator(_filterReferenceIDs.Text.Trim());
			_objectDataSource.SelectParameters["manufacturers"].DefaultValue = NormalizeStringWithSeparator(_filterManufacturers.Text.Trim());
			_objectDataSource.SelectParameters["preset"].DefaultValue = ddlStatusesPresets.SelectedValue;
			
			string statuses = string.Empty;
			foreach (ListItem item in _filterStatuses.Items)
			{
				if (item.Selected)
				{
					statuses += item.Value + ",";
				}
			}
			if (statuses.Length > 0)
			{
				statuses = statuses.Substring(0, statuses.LastIndexOf(','));
			}
			_objectDataSource.SelectParameters["statusIDs"].DefaultValue = statuses;

			if (bind)
			{
				_dataPager.SetPageProperties(0, _dataPager.PageSize, true);
			}
		}

		protected void _sortBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			_dataPager.SetPageProperties(0, _dataPager.PageSize, true);
		}

		protected void _pageSizeBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			_dataPager.SetPageProperties(0, Convert.ToInt32(_pageSizeBox.SelectedValue), true);
		}

		protected void _listView_DataBinding(object sender, EventArgs e)
		{
			//сохранить информацию о состоянии
			//IPageUrlState page = (IPageUrlState)Page;
			//page.UrlStateContainer["sort"] = _sortBox.SelectedValue;
			//page.UrlStateContainer["start"] = _dataPager.StartRowIndex.ToString();
			//page.UrlStateContainer["size"] = _dataPager.PageSize.ToString();

			//page.UrlStateContainer["orderdatestart"] = _objectDataSource.SelectParameters["orderDateStart"].DefaultValue;
			//page.UrlStateContainer["orderdateend"] = _objectDataSource.SelectParameters["orderDateEnd"].DefaultValue;
			//page.UrlStateContainer["partpricestart"] = _objectDataSource.SelectParameters["partPriceStart"].DefaultValue;
			//page.UrlStateContainer["partpriceend"] = _objectDataSource.SelectParameters["partPriceEnd"].DefaultValue;
			//page.UrlStateContainer["partname"] = _objectDataSource.SelectParameters["partName"].DefaultValue;
			//page.UrlStateContainer["orderids"] = _objectDataSource.SelectParameters["orderIDs"].DefaultValue;
			//page.UrlStateContainer["custOrderNums"] = _objectDataSource.SelectParameters["custOrderNums"].DefaultValue;
			//page.UrlStateContainer["partnumbers"] = _objectDataSource.SelectParameters["partNumbers"].DefaultValue;
			//page.UrlStateContainer["referenceids"] = _objectDataSource.SelectParameters["referenceIDs"].DefaultValue;
			//page.UrlStateContainer["manufacturers"] = _objectDataSource.SelectParameters["manufacturers"].DefaultValue;
			//page.UrlStateContainer["statusids"] = _objectDataSource.SelectParameters["statusIDs"].DefaultValue;
		}

		protected void _listView_DataBound(object sender, EventArgs e)
		{
			if (CurrentOrderLineTotals != null)
			{
				_totalLabel.Text = string.Format("{0:### ### ##0.00}", CurrentOrderLineTotals.TotalSum);
			}
			
            if (CurrentOrderLines != null)
			{
				_pageTotalLabel.Text = string.Format("{0:### ### ##0.00}", CurrentOrderLines.AsQueryable().Where(OrderBO.TotalStatusExpression).Sum(l => l.Total));
			}
		}

		protected void _objectDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			if (e.ReturnValue != null && e.ReturnValue.GetType() == typeof(int))
			{
				int count = Convert.ToInt32(e.ReturnValue);
				_totalsBlock.Visible = count != 0;
				_dataPager.Visible = count != 0;
				_pagerSettingsBlock.Visible = count != 0;
			}
		}

		#region ==== Helpers ====
		
		private void InitStatusesFilter(string preset)
		{
            if (_filterStatuses.Items.Count <= 0)
			{
                using (var dc = new DCWrappersFactory<StoreDataContext>())
				{
					CurrentOrderLineStatuses = dc.DataContext.OrderLineStatuses.Where(t => t.ClientShow).OrderBy(t => t.ClientShowOrder).ToArray();
					foreach (var status in CurrentOrderLineStatuses)
					{
						var item = new ListItem(OrderLineStatusUtil.DisplayName(status.OrderLineStatusID), status.OrderLineStatusID.ToString());
						item.Selected = true;
						_filterStatuses.Items.Add(item);
					}
				}
			}
			else
			{
				switch (preset)
				{
					case "all":
						_filterStatuses.Items[0].Selected = false;
						foreach (var item in _filterStatuses.Items)
						{
							((ListItem)item).Selected = true;
						}
						_filterStatuses.Enabled = true;
						break;
					case "allworked":
						for (int i = 0; i < CurrentOrderLineStatuses.Length; i++)
						{
							// Статус "Поставка невозможна" (RefusedBySupplier) должен попадать в пресет "все рабочие", т.к. в том случае когда клиент или менеджер
							// не нажал на "ПЕРЕЗАКАЗАТЬ" этот статус по сути является рабочим.
							_filterStatuses.Items[i].Selected = (!CurrentOrderLineStatuses[i].IsFinal || CurrentOrderLineStatuses[i].NameRMS == "RefusedBySupplier");
						}
						_filterStatuses.Enabled = false;
						break;
					case "allclosed":
						for (int i = 0; i < CurrentOrderLineStatuses.Length; i++)
						{
							_filterStatuses.Items[i].Selected = CurrentOrderLineStatuses[i].IsFinal;
						}
						_filterStatuses.Enabled = false;
						break;
				}
			}
		}

        private string NormalizeStringWithSeparator(string source)
		{
			string[] strings = source.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			string result = string.Empty;
			foreach (string str in strings)
			{
				result += str.Trim() + ',';
			}
			if (!string.IsNullOrEmpty(result))
			{
				result = result.Substring(0, result.LastIndexOf(','));
			}
			return result;
		}
		#endregion
	}
}