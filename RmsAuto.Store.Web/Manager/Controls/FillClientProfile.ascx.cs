using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Manager.Controls
{
    public partial class FillClientProfile : System.Web.UI.UserControl
    {
        public ClientRegistrationDataExt RegistrationData
        {
            get
            {
                return new ClientRegistrationDataExt
                {
                    //ClientCategory = (ClientCategory)Enum.Parse(typeof(ClientCategory), _rblClientCategory.SelectedValue),
                    //TradingVolume = (TradingVolume)Enum.Parse(typeof(TradingVolume), _rblTradingVolume.SelectedValue),
                    ClientName = /*ClientCategory == ClientCategory.Physical ?  FullName : */_txtCompanyName.Text,
                    FieldOfAction = _ddlFieldsOfAction.SelectedItem.Text + " " + _txtFieldOfAction.Text,
                    DiscountCardNumber = _discountCardNumber.Value,
                    MainPhone = _mainPhone.Value,
                    AuxPhone1 = _auxPhone1.Value,
                    AuxPhone2 = _auxPhone2.Value,
                    RmsStoreId = _ddlRmsStores.SelectedValue,
                    ShippingAddress = _txtShippingAddress.Text,
                    Email = _txtEmail.Text
                };
            }
        }

        public bool IsRestricted
        {
            get { return _chkRestrict.Checked; }
        }
        
        //private ClientCategory ClientCategory
        //{
        //    get { return (ClientCategory)Enum.Parse(typeof(ClientCategory), _rblClientCategory.SelectedValue); }
        //}

        private string FullName
        {
            get { return _txtLastName.Text + " " + _txtFirstName.Text + " " + _txtMiddleName.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //foreach( ListItem item in _rblClientCategory.Items )
            //{
            //    item.Attributes.Add( "onClick", String.Format( "javascript:checkClientCategory({0})", item.Value ) );
            //}

            if (!Page.IsPostBack)
            {
                BindRmsStores();
                _ddlRmsStores.SelectedValue = SiteContext
                    .GetCurrent<ManagerSiteContext>().ManagerInfo.StoreId;
            }
        }

        protected void ValidateCompanyName(object source, ServerValidateEventArgs args)
        {
            args.IsValid = /*ClientCategory == ClientCategory.Physical || */!string.IsNullOrEmpty(_txtCompanyName.Text);
        }

        protected void ValidateMainPhone(object source, ServerValidateEventArgs args)
        {
            args.IsValid = _mainPhone.HasValue;
        }

        protected void ValidateEmail(object source, ServerValidateEventArgs args)
        {
            args.IsValid = string.IsNullOrEmpty(args.Value) || !ClientBO.EmailInUse(_txtEmail.Text);
        }

        private void BindRmsStores()
        {
            _ddlRmsStores.DataSource = AcctgRefCatalog.RmsStores.Items;
            _ddlRmsStores.DataTextField = "FullInfo";
            _ddlRmsStores.DataValueField = "StoreId";
            _ddlRmsStores.DataBind();
        }
    }
}