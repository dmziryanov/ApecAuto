using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using System.Globalization;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class ClientPaymentsView : System.Web.UI.UserControl
	{
		public string CurrentUserID     { get { return (hfUserID == null ? "" : (string)hfUserID.Value); } set { hfUserID.Value = value; } }
        public string CurrentClientName { get { return (string.IsNullOrEmpty(hfClientName.Value) ? Resources.Texts.CustomerIsNotSelected : (string)hfClientName.Value); } set { hfClientName.Value = value; } }
        private int curPaymentsCount  { get { return int.Parse(hfCount.Value); } set { hfCount.Value = value.ToString(); } }
        private int tmpcurPaymentsCount;
        
        public string CurrentClientBalance
		{ 
			get { return (hfClientBalance == null ? "-" : (string)hfClientBalance.Value); }
			set { hfClientBalance.Value = value; }
		}

		#region === Load data ===
		public int GetPaymentsCount(int userID, DateTime dateFrom, DateTime dateTo, string VaryParam)
		{
			tmpcurPaymentsCount = LightBO.GetUserLightPaymentsCount(userID, dateFrom, dateTo.AddHours(23).AddMinutes(59).AddSeconds(59));
            if (hfCount != null)
            {
                curPaymentsCount = tmpcurPaymentsCount;
            }
            return tmpcurPaymentsCount;
            
		}

		public List<UserLightPayment> GetPayments(int userID, DateTime dateFrom, DateTime dateTo, int startIndex, int pageSize, string VaryParam)
		{
			var l = LightBO.GetUserLightPayments(userID, dateFrom, dateTo.AddHours(23).AddMinutes(59).AddSeconds(59), startIndex, pageSize);
           // curPaymentsCount = l.Count();
            return l;
		}

		#endregion

		protected void Page_Init(object sender, EventArgs e)
		{
            if (string.IsNullOrEmpty(_DateFrom.Text)) { _DateFrom.Text = DateTime.Now.Date.ToShortDateString(); }
            if (string.IsNullOrEmpty(_DateTo.Text)) { _DateTo.Text = DateTime.Now.Date.ToShortDateString(); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            if (CurrentUserID != "-1")
            {
                ClientColumn.Visible = false;
            }
            else
                ClientColumn.Visible = true;

            if (!IsPostBack)
            {
                _rblPaymentMethod.BindEnumeration(typeof(PaymentMethod), PaymentMethod.Cash);
            }

            _objectDataSource.DataBind();
		}

		protected void _listSearchResults_SelectedIndexChanged(object sender, EventArgs e)
		{
			//При смене пользователя перезаполняем ключевые поля
			CurrentUserID = _listSearchResults.SelectedValue;
			CurrentClientName = _listSearchResults.SelectedItem.Text;// +_listSearchResults.SelectedValue.AddBrackets();

            if (CurrentUserID != "-1")
            {
                ClientColumn.Visible = false;

				CurrentClientName += UserDac.GetAcctgIDByUserID(int.Parse(CurrentUserID)).WithBrackets();
            }
            else
                ClientColumn.Visible = true;

			FillData();
		}

		protected void _btnSearchClient_Click(object sender, EventArgs e)
		{
			var searchResults = ClientSearch.Search(
					_txtClientName.Text.Trim(),
					"",
					ClientSearchMatching.Fuzzy).ToList();

            searchResults.Insert(0, new BriefClientInfo() { UserID = -1, ClientName = "All" });

			_listSearchResults.DataSource = searchResults;
			_listSearchResults.DataBind();
		}

		protected void btnInputPayment_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(CurrentUserID))
			{
				ShowMessage("User is not selected");
				return;
			}
			
			decimal payment = 0;
			NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };

			if (decimal.TryParse(txtInputPayment.Text.Replace(',','.'), NumberStyles.Number, nfi, out payment))
			{
				LightPaymentType pType = (LightPaymentType)byte.Parse(ddlPaymentTypes.SelectedValue);
				if ((pType == LightPaymentType.Payment && payment > 0) || 
					(pType == LightPaymentType.GoodsReturn && payment > 0) ||
					(pType == LightPaymentType.PayBack && payment < 0))
				{
					payment = payment * (-1.00m);
				}

                var paymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), _rblPaymentMethod.SelectedValue);

				UserLightPayment p = new UserLightPayment()
				{
					PaymentDate = DateTime.Now,
					PaymentSum = payment,
					PaymentType = (LightPaymentType)byte.Parse(ddlPaymentTypes.SelectedValue),
                    PaymentMethod = paymentMethod,
					UserID = int.Parse(CurrentUserID)
				};


                using (var dc = new DCFactory<StoreDataContext>())
                {
                    if (LightBO.AddUserLightPayment(p, dc) > 0)
                    {
                        txtInputPayment.Text = string.Empty;
                        ShowMessage("Payment is accepted!");
                    }
                    else
                        ShowMessage("Payment is failed!");
                }
				
                hfVaryParam.Value = ((new Random()).Next()).ToString();
				FillData();
			}
			else
			{
				ShowMessage("Payment's value has a wrong format!");
			}
		}

		protected void btnSearchPayments_Click(object sender, EventArgs e)
		{
			hfVaryParam.Value = ((new Random()).Next()).ToString();
            PaymentsBootstrap.NavigateUrl = "../PaymentsBootStrap.ashx?UserID=" + CurrentUserID + "&DateFrom=" + _DateFrom.Text + "&DateTo=" + _DateTo.Text;
            curPaymentsCount = tmpcurPaymentsCount;
            
			//_objectDataSource.DataBind();
		}

		protected void _objectDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			//if (e.ReturnValue.GetType() == typeof(int))
			//{
			//    int count = Convert.ToInt32(e.ReturnValue);
			//    //_sortBlock.Visible = count != 0;
			//    //_dataPager.Visible = count != 0;
			//    //_footerBlock.Visible = count != 0;
			//}
		}

		protected void _listView_DataBinding(object sender, EventArgs e)
		{
		}

		protected void _listView_DataBound(object sender, EventArgs e)
		{
		}

		protected void _pageSizeBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			_dataPager.SetPageProperties(0, Convert.ToInt32(_pageSizeBox.SelectedValue), true);
		}

		private void FillData()
		{
			//заполняем "сальдо" пользователя
			if (!string.IsNullOrEmpty(_DateTo.Text) || !string.IsNullOrEmpty(CurrentUserID))
			{
				CurrentClientBalance = LightBO.GetUserLightBalance(int.Parse(CurrentUserID), DateTime.Parse(_DateTo.Text).AddHours(23).AddMinutes(59).AddSeconds(59)).ToString("### ### ##0.00");
				Page.DataBind();
			}
		}

		private void ShowMessage(string message)
		{
			Page.ClientScript.RegisterStartupScript(
				this.GetType(),
				"__messageBox",
				"<script type='text/javascript'>alert(\"" + message + "\");</script>");
		}
	}
}