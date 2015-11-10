using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Web.Controls;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;
using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    public partial class RegisterClient : RMMPage
    {
        public bool ClientAlreadyExists
        {
            get;
            private set;
        }

        public bool ProfileCreated
        {
            get;
            private set;
        }

        public bool OnineAccessOfferSubmitted
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // deas 29.03.2011 task3511
            // добавление новой формы регистрации
            if ( Convert.ToDateTime( ConfigurationManager.AppSettings["NewRegistrationData"] ) < DateTime.Now )
            {
                _fillClientProfile.Visible = false;
            }
            else
            {
                _fillClientProfileExt.Visible = false;
            }
        }

        protected void _btnRegister_Click( object sender, EventArgs e )
        {
            if ( IsValid )
            {
                // deas 29.03.2011 task3511
                // добавление новой формы регистрации
                //if ( Convert.ToDateTime( ConfigurationManager.AppSettings["NewRegistrationData"] ) < DateTime.Now )
                //{
                    var regData = _fillClientProfileExt.RegistrationData;
                    try
                    {
                        var siteContext = ((ManagerSiteContext)SiteContext.Current);
                        int userID = ClientBO.CreateClient(
                            regData,
                            siteContext.ManagerInfo.EmployeeId,
                            _fillClientProfile.IsRestricted, siteContext.InternalFranchName);
                        ProfileCreated = true;

                        if ( !string.IsNullOrEmpty(regData.Email))
                        {
                            ClientBO.OfferOnlineAccessExt(
                                userID,
                                regData.ClientName,
                                regData.Email,
                                activationCode => UrlManager.GetClientActivationUrl(activationCode, siteContext.InternalFranchName)
                                );
                            OnineAccessOfferSubmitted = true;
                        }
                        _profilePane.Visible = false;

                        //_btnContinue.PostBackUrl = ClientProfile.GetUrl();
                        _msgPane.Visible = true;
                    }
                    catch ( AcctgException ex )
                    {
                        if ( ex.ErrorCode == AcctgError.ClientAlreadyExists )
                            ClientAlreadyExists = true;
                        else
                            throw ex;
                    }
                //}
                //else
                //{
                //    var regData = _fillClientProfile.RegistrationData;

                //    try
                //    {
                //        var siteContext = ( (ManagerSiteContext)SiteContext.Current );
                //        var clientId = ClientBO.CreateClient(
                //            regData,
                //            siteContext.ManagerInfo.EmployeeId,
                //            _fillClientProfile.IsRestricted );
                //        ProfileCreated = true;

                //        if ( !string.IsNullOrEmpty( regData.Email ) )
                //        {
                //            ClientBO.OfferOnlineAccess(
                //                clientId,
                //                regData.Email,
                //                activationCode => UrlManager.MakeAbsoluteUrl( UrlManager.GetClientActivationUrl( activationCode ) )
                //                );
                //            OnineAccessOfferSubmitted = true;
                //        }
                //        _profilePane.Visible = false;

                //        siteContext.ClientSet.AddClient( clientId );
                //        siteContext.ClientSet.SetDefaultClient( clientId );
                //        //_btnContinue.PostBackUrl = ClientProfile.GetUrl();
                //        _msgPane.Visible = true;
                //    }
                //    catch ( AcctgException ex )
                //    {
                //        if ( ex.ErrorCode == AcctgError.ClientAlreadyExists )
                //            ClientAlreadyExists = true;
                //        else
                //            throw ex;
                //    }
                //}
            }
        }
    }
}
