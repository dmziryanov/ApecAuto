using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.PrivateOffice
{
    /// <summary>
    /// Страница для запроса детального баланса клиентом
    /// </summary>
    public partial class DetailBalance : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        public DateTime StartDate
        {
            get
            {
                DateTime dt = DateTime.MaxValue;
                DateTime.TryParse(dpStart.Text, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out dt);
                return dt;
            }
        }

        public DateTime EndDate
        {
            get
            {
                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(dpEnd.Text, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out dt);
                return dt;
            }
        }

        protected void Page_Load( object sender, EventArgs e )
        {
            mvMain.ActiveViewIndex = 0;
            _errorMessage.Visible = false;
			_errorNonDelivery.Visible = false;
        }

        protected void btnSend_Click( object sender, EventArgs e )
        {
            //DateTime dateStart;
            //DateTime dateEnd;
            //try
            //{
            //    string tDate = dpStart.Text;
            //    dateStart = new DateTime( Convert.ToInt32( tDate.Substring( 6, 4 ) ),
            //        Convert.ToInt32( tDate.Substring( 3, 2 ) ),
            //        Convert.ToInt32( tDate.Substring( 0, 2 ) ) );
            //}
            //catch
            //{
            //    // данная валидация есть на клиенте, так что сюда, по идее, не должны попадать
            //    _errorMessage.Text = Resources.ValidatorsMessages.IncorrectStartPeriod;//"Дата начала периода заполнена некорректно.";
            //    _errorMessage.Visible = true;
            //    return;
            //}
            //try
            //{
            //    string tDate = dpEnd.Text;
            //    dateEnd = new DateTime( Convert.ToInt32( tDate.Substring( 6, 4 ) ),
            //        Convert.ToInt32( tDate.Substring( 3, 2 ) ),
            //        Convert.ToInt32( tDate.Substring( 0, 2 ) ) );
            //}
            //catch
            //{
            //    // данная валидация есть на клиенте, так что сюда, по идее, не должны попадать
            //    _errorMessage.Text = Resources.ValidatorsMessages.IncorrectFinishPeriod;//"Дата окончания периода заполнена некорректно.";
            //    _errorMessage.Visible = true;
            //    return;
            //}
            if (StartDate.Date > EndDate.Date)
            {
				_errorMessage.Text = Resources.ValidatorsMessages.StartDateExceedFinishDate;//"Дата начала периода превышает дату окончания периода.";
				_errorMessage.Visible = true;
                return;
            }
            if (ClientProfile.SendClientBalance(SiteContext.Current.CurrentClient.Profile.ClientId, StartDate, EndDate) == Acknowledgement.OK)
            {
                mvMain.ActiveViewIndex = 1;
            }
            else
            {
                mvMain.ActiveViewIndex = 2;
            }
        }

		protected void ibSendNonDeliveryRequest_Click(object sender, EventArgs e)
		{
			string clientId = SiteContext.Current.CurrentClient.Profile.ClientId;
			int orderId = Convert.ToInt32(txtOrderNum.Text.Trim());
			string partNumber = txtPartNumber.Text.Trim();
			Order order = null;
			try
			{
				order = OrderBO.LoadOrderData(clientId, orderId);
			}
			catch
			{
				_errorNonDelivery.Text = Resources.Requests.FailedUploadData;//"Не удалось загрузить данные об указанном заказе, возможно номер заказа не существует!";
				_errorNonDelivery.Visible = true;
				return;
			}
			if (order == null)
			{
				_errorNonDelivery.Text = Resources.Requests.FailedUploadData;//"Не удалось загрузить данные об указанном заказе, возможно номер заказа не существует!";
				_errorNonDelivery.Visible = true;
				return;
			}
			else
			{
				Dictionary<int, string> dataOfOrderLines = order.OrderLines
					.Where(l => l.PartNumber.ToLower().Equals(partNumber.ToLower()))
					.Where(l => l.CurrentStatus == OrderLineStatusUtil.StatusByte("OrderedFromSupplier") || l.CurrentStatus == OrderLineStatusUtil.StatusByte("ShipmentDelay"))
					.Where(l => l.EstSupplyDate.Value.Date < DateTime.Now.Date)
					.Select(l => new { l.AcctgOrderLineID.Value, l.PartNumber } )
					.ToDictionary(l => l.Value, l => l.PartNumber );
				string dataForXml = string.Empty;
				foreach (var item in dataOfOrderLines)
				{
					dataForXml += item.Key.ToString() + "," + item.Value + ";";
				}
				if (string.IsNullOrEmpty(dataForXml))
				{
					_errorNonDelivery.Text = Resources.Requests.FailedFindLines;//"Не найдено ни одной строки в указанном заказе, удовлетворяющей условию непоставки!";
					_errorNonDelivery.Visible = true;
					return;
				}
				else
				{
					try
					{
						ServiceProxy.Default.SendNonDeliveryRequest(clientId, orderId.ToString(), dataForXml.Substring(0, dataForXml.Length - 1));
						mvNonDeliveryRequest.ActiveViewIndex = 1;
					}
					catch (Exception ex)
					{
						mvNonDeliveryRequest.ActiveViewIndex = 2;
						Logger.WriteException(ex);
					}
				}				
			}
		}

		protected void lnkReturn_Click(object sender, EventArgs e)
		{
			mvRequests.ActiveViewIndex = 1;
			mvNonDeliveryRequest.ActiveViewIndex = 0;
			txtPartNumber.Text = txtOrderNum.Text = string.Empty;
		}
    }
}
