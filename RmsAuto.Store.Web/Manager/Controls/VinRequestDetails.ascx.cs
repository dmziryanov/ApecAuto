using System;
using System.Collections;
using System.Collections.Specialized;
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
using System.Data.Linq;

using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Web;
using System.Collections.Generic;

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class VinRequestDetails : System.Web.UI.UserControl
	{
		public int VinRequestId
		{
            get
            {
                return Request.QueryString.Get<int>(UrlKeys.Id);
            }
		}

        protected bool CanEditAnswer
        {
            get { return !_currentVinRequest.Proceeded; }
        }
        
        VinRequest _currentVinRequest;
        
		//  Баунд-лайаут строится до пейджлоада (во время загрузки вьюстейта), так что загружаем в ините
        protected void Page_Init( object sender, EventArgs e )
		{
		       _currentVinRequest = VinRequestsDac.GetRequest(VinRequestId);
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			_backLink.NavigateUrl = ClientVinRequests.GetUrl();

            _requestDateLabel.Text = String.Format("{0:dd.MM.yyyy HH:mm:ss}", _currentVinRequest.RequestDate);
            _garageCarDetails.CarParameters = _currentVinRequest;

            //  Иф на тот случай, если редактирование в процессе
            if (_sSendBLock.Visible)
            {
                _sSendBLock.Visible = CanEditAnswer;
            }

            BindData();
		}
        
		protected void _sendAnswerButton_Click( object sender, EventArgs e )
		{
			try
			{
				var siteContext = (ManagerSiteContext)SiteContext.Current;

                Page.Validate("OnSendGroup");
                if (Page.IsValid)
                {
                    ManagerBO.SendVinRequestAnswer(
                        VinRequestId,
                        siteContext.CurrentClient.Profile.Email,
                        siteContext.CurrentClient.Profile.ClientName);

                    Response.Redirect(Request.RawUrl, true);
                }
			}
			catch( Exception ex )
			{
				_errorLabel.Text = ex.Message;
			}
		}

        protected void _listViewItemEditing(object sender, ListViewEditEventArgs e)
        {
            _listView.EditIndex = e.NewEditIndex;
            _sSendBLock.Visible = false;
        }

        protected void _listViewItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                var listItem = _listView.Items[e.ItemIndex];
                var priceText = (listItem.FindControl("_txtPricePerUnit") as TextBox).Text;
                var reqItem = new VinRequestItem
                {
                    Id = Convert.ToInt32(_listView.DataKeys[e.ItemIndex].Value),
                    Name = (listItem.FindControl("_txtName") as TextBox).Text,
                    ManagerComment = (listItem.FindControl("_txtManagerComment") as TextBox).Text,
                    Manufacturer = (listItem.FindControl("_txtManufacturer") as TextBox).Text,
                    PartNumber = (listItem.FindControl("_txtPartNumber") as TextBox).Text,
                    PartNumberOriginal = (listItem.FindControl("_txtPartNumberOriginal") as TextBox).Text,
                    DeliveryDays = (listItem.FindControl("_txtDeliveryDays") as TextBox).Text,
                    PricePerUnit = !String.IsNullOrEmpty(priceText) ?
                        Convert.ToDecimal(priceText) : (decimal?)null,
                    Quantity = Convert.ToInt16((listItem.FindControl("_txtQuantity") as TextBox).Text)
                };
                _sSendBLock.Visible = true;
                VinRequestsDac.UpdateVinRequestItem(reqItem);
                _listView.EditIndex = -1;
            }
        }

        protected void _listViewItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            _sSendBLock.Visible = true;
            _listView.EditIndex = -1;
        }

        Dictionary<int, HashSet<string>> _listViewErrors = new Dictionary<int, HashSet<string>>();
        bool _displayErrors;
        protected bool CheckError(int itemIndex, string errCode)
        {
            return _displayErrors && _listViewErrors.ContainsKey(itemIndex) && _listViewErrors[itemIndex].Contains(errCode);
        }
        protected void _listOnValidate(object sender, ServerValidateEventArgs e)
        {
            bool good = true;

            foreach (ListViewDataItem listItem in _listView.Items)
            {
                if (!_listViewErrors.ContainsKey(listItem.DisplayIndex))
                {
                    _listViewErrors.Add(listItem.DisplayIndex, new HashSet<string>());
                }

                var manufacturer = (listItem.FindControl("_litMfr") as Literal).Text;
                if (String.IsNullOrEmpty(manufacturer))
                {
                    _listViewErrors[listItem.DisplayIndex].Add("Mfr");
                    good = false;
                }

                var partNumber = (listItem.FindControl("_litPN") as Literal).Text;
                if (String.IsNullOrEmpty(partNumber))
                {
                    _listViewErrors[listItem.DisplayIndex].Add("PartNumber");
                    good = false;
                }

                var partNumberOriginal = (listItem.FindControl("_litPNOriginal") as Literal).Text;
                if (String.IsNullOrEmpty(partNumberOriginal))
                {
                    _listViewErrors[listItem.DisplayIndex].Add("PartNumberOriginal");
                    good = false;
                }
                var deliveryDays = (listItem.FindControl("_litDeliveryDays") as Literal).Text;
                if (String.IsNullOrEmpty(deliveryDays))
                {
                    _listViewErrors[listItem.DisplayIndex].Add("DeliveryDays");
                    good = false;
                }

                var priceText = (listItem.FindControl("_litPricePerUnit") as Literal).Text;
                if (String.IsNullOrEmpty(priceText))
                {
                    _listViewErrors[listItem.DisplayIndex].Add("PricePerUnit");
                    good = false;
                } 
            }

            _displayErrors = true;
            e.IsValid = good;
        }

        void BindData()
        {
            _listView.DataSource = VinRequestsDac.GetRequestItems(VinRequestId);
            _listView.DataBind();
        }
    }
}