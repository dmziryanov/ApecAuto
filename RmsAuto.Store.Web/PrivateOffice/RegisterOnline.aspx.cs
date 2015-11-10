using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Routing;
using System.Configuration;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class RegisterOnline : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            if ( Page.User.Identity.IsAuthenticated )
            {
                _fillClientProfileExt.Visible = false;
                _messagePane.Visible = false;
            }
            else
            {
				//_fillClientProfileExt.Visible = false;
            }
        }

        //protected void FillClientProfileCompleted( object sender, EventArgs args )
        //{
        //    ClientBO.RegisterClient(
        //        new ClientRegistrationData
        //        {
        //            Client = _fillClientProfile.ClientName,
        //            ClientCategory = _fillClientProfile.ClientCategory,
        //            TradingVolume = _fillClientProfile.TradingVolume,
        //            ContactName = _fillClientProfile.ContactPerson,
        //            MainPhone = _fillClientProfile.MainPhone,
        //            AuxPhone1 = _fillClientProfile.AuxPhone1,
        //            AuxPhone2 = _fillClientProfile.AuxPhone2,
        //            Email = _fillClientProfile.Email,
        //            FieldOfAction = _fillClientProfile.FieldOfAction,
        //            DiscountCardNumber = _fillClientProfile.DiscountCardNumber,
        //            RmsStoreId = _fillClientProfile.RmsStoreId,
        //            ShippingAddress = _fillClientProfile.ShippingAddress,
        //            Login = _fillClientProfile.Username,
        //            Password = _fillClientProfile.Password
        //        },
        //        activationCode => UrlManager.MakeAbsoluteUrl( UrlManager.GetClientActivationUrl( activationCode ) ) );

        //    _fillClientProfile.Visible = false;
        //    _fillClientProfileExt.Visible = false;
        //    _messagePane.Visible = true;
        //}

        protected void FillClientProfileCompletedExt( object sender, EventArgs args )
        {
            ClientBO.RegisterClient(
                new ClientRegistrationDataExt
                {
                    Email = _fillClientProfileExt.Email,
                    Login = _fillClientProfileExt.Login,
                    Password = _fillClientProfileExt.Password,
                    ClientCategory = _fillClientProfileExt.ClientCategory,
                    TradingVolume = _fillClientProfileExt.TradingVolume,
                    CountryID = _fillClientProfileExt.CountryID,
                    Locality = _fillClientProfileExt.Locality,
					RmsStoreID = _fillClientProfileExt.RmsStoreID,
					ScopeType = _fillClientProfileExt.ScopeType,
					DiscountCardNumber = _fillClientProfileExt.DiscountCardNumber,
					HowKnow = _fillClientProfileExt.HowKnow,

					ContactPersonPosition = _fillClientProfileExt.ContactPersonPosition,
					ContactPersonName = _fillClientProfileExt.ContactPersonName,
					ContactPersonSurname = _fillClientProfileExt.ContactPersonSurname,
                    ContactPersonPhone = _fillClientProfileExt.ContactPersonPhone,
                    ContactPersonExtPhone = _fillClientProfileExt.ContactPersonExtPhone,
                  /*  ContactPersonFax = _fillClientProfileExt.ContactPersonFax,
					ContactPersonEmail = _fillClientProfileExt.ContactPersonEmail,
                    DeliveryAddress = _fillClientProfileExt.DeliveryAddress,
                    
                    CompanyName = _fillClientProfileExt.CompanyName,
					CompanyRegistrationID = _fillClientProfileExt.CompanyRegistrationID,
					CompanyAddress = _fillClientProfileExt.CompanyAddress,

					BankName = _fillClientProfileExt.BankName,
					IBAN = _fillClientProfileExt.IBAN,
					SWIFT = _fillClientProfileExt.SWIFT,
					BankAddress = _fillClientProfileExt.BankAddress,

					DirectorName = _fillClientProfileExt.DirectorName,
					DirectorSurname = _fillClientProfileExt.DirectorSurname,

					CorrespondentBankName = _fillClientProfileExt.CorrespondentBankName,
					CorrespondentIBAN = _fillClientProfileExt.CorrespondentIBAN,
					CorrespondentSWIFT = _fillClientProfileExt.CorrespondentSWIFT,
					CorrespondentBankAddress = _fillClientProfileExt.CorrespondentBankAddress*/
                }, activationCode => UrlManager.GetClientActivationUrl(activationCode, String.Compare(ConfigurationManager.AppSettings["InternalFranchName"], SiteContext.Current.InternalFranchName, true) == 0 ? null : SiteContext.Current.InternalFranchName));
            _fillClientProfileExt.Visible = false;
            _messagePane.Visible = true;
        }
    }
}
