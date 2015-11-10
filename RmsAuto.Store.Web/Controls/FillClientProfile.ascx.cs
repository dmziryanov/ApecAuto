using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Common.Web.UI;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Controls
{
    public partial class FillClientProfile : UserControl 
    {
        public event EventHandler<EventArgs> Completed;

        public string Title
        {
            get { return (string)ViewState["__title"]; }
            set { ViewState["__title"] = value; }
        }

        public ClientCategory ClientCategory
        {
            get { return (ClientCategory)Enum.Parse(typeof(ClientCategory), _rblClientCategory.SelectedValue); }
        }

        public TradingVolume TradingVolume
        {
            get { return (TradingVolume)Enum.Parse(typeof(TradingVolume), _rblTradingVolume.SelectedValue); }
        }

        public string ClientName
        {
            get 
            {
                return ClientCategory == ClientCategory.Physical ?  FullName : _txtCompanyName.Text;
            }
        }

        public string FieldOfAction
        {
            get { return _ddlFieldsOfAction.SelectedItem.Text + " " + _txtFieldOfAction.Text; }
        }

        public string DiscountCardNumber
        {
            get { return _discountCardNumber.Value; }
        }

        public string DiscountCardNumberDisplay
        {
            get { return _discountCardNumber.DisplayValue; }
        }

        public string ContactPerson
        {
            get { return ClientCategory == ClientCategory.Legal ? FullName : string.Empty; }
        }

        public string MainPhone
        {
            get { return _mainPhone.Value; }
        }

        public string AuxPhone1
        {
            get { return _auxPhone1.Value; }
        }

        public string AuxPhone2
        {
            get { return _auxPhone2.Value; }
        }
        
        public string RmsStoreId
        {
            get { return _ddlRmsStores.SelectedValue; }
        }

        public string ShippingAddress
        {
            get { return _txtShippingAddress.Text; }
        }
        
        public string Email
        {
            get { return _txtEmail.Text; }
        }

        public string Username
        {
            get { return _editUser.Username; }
        }

        public string Password
        {
            get { return (string)ViewState["__pwd"]; }
            private set { ViewState["__pwd"] = value; }
        }
        
        protected string FullName
        {
            get { return _txtLastName.Text + " " + _txtFirstName.Text + " " + _txtMiddleName.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (ListItem item in _rblClientCategory.Items)
            {
                item.Attributes.Add("onClick", String.Format("javascript:checkClientCategory({0})", item.Value));
            }
                          
            if (!Page.IsPostBack)
            {
                BindRmsStores();
                CaptureImage.RefreshCapture();
            }
        }

        protected void _phoneCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = _mainPhone.HasValue;
        }

        protected void _companyNameCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ClientCategory == ClientCategory.Physical || !string.IsNullOrEmpty(_txtCompanyName.Text);
        }

		protected void _categoryVolumeValidator_ServerValidate( object source, ServerValidateEventArgs args )
        {
			args.IsValid = !( ClientCategory == ClientCategory.Legal && TradingVolume == TradingVolume.Retail );
        }

        protected void _rblTradingVolume_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRmsStores();
        }

        private void BindRmsStores()
        {
            _ddlRmsStores.Items.Clear();
            _ddlRmsStores.DataSource = AcctgRefCatalog.RmsStores.Items.Where(
                s =>
                    (TradingVolume == TradingVolume.Wholesale && s.IsWholesale) ||
                    (TradingVolume == TradingVolume.Retail && s.IsRetail));
            _ddlRmsStores.DataTextField = "FullInfo";
            _ddlRmsStores.DataValueField = "StoreId";
            _ddlRmsStores.DataBind();
            //// Если розница
            //if (_rblTradingVolume.SelectedIndex == 0)
            //{ 
            _ddlRmsStores.AppendDataBoundItems = true;
            _ddlRmsStores.Items.Insert(0, new ListItem("", ""));
            _ddlRmsStores.SelectedValue = "";
            _rmsStoreValidator.InitialValue = "";
            //}
        }

        protected void _ValidateEmail(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !ClientBO.EmailInUse(args.Value);
        }

        protected void _fillProfileWizard_ActiveStepChanged(object sender, EventArgs e)
        {
            if (_fillProfileWizard.ActiveStepIndex == _fillProfileWizard.WizardSteps.IndexOf(
                _reviewClientInfo))
            {
                Password = _editUser.Password;
            }
            else if (_fillProfileWizard.ActiveStepIndex == _fillProfileWizard.WizardSteps.IndexOf(
                _contactInfoStep))
            {
				if( ClientCategory == ClientCategory.Legal )
				{
					_shippingAddressRow1.Visible = false;
					_shippingAddressRow2.Visible = false;
				}
				else
				{
					_shippingAddressRow1.Visible = true;
					_shippingAddressRow2.Visible = true;
				}
            }
        }
                                        
        protected void _fillProfileWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (Page.IsValid)
            {
                OnCompleted(EventArgs.Empty);
            }
        }

        protected virtual void OnCompleted(EventArgs args)
        {
            if (Completed != null)
                Completed(this, args);
        }
    }
}