using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Controls
{
    public partial class ClientProfile : System.Web.UI.UserControl
    {
        ClientData data = SiteContext.Current.CurrentClient;

        protected void Page_Load(object sender, EventArgs e)
        {
            dcCommonDataContext DC = new dcCommonDataContext();

            _ClientName.Text = data.Profile.ClientName;
            _ClientCategory.Text = data.Profile.Category.ToTextOrName();
            _TradingVolume.Text = data.Profile.TradingVolume.ToTextOrName();
            _Email.Text = data.Profile.Email;
            var country = DC.spSelCountryByID(data.Profile.CountryID).FirstOrDefault();
            _Country.Text = country == null ? "" : country.CountryName;
            //var region = DC.spSelRegionByID( data.Profile.RegionID ).FirstOrDefault();
            //_Region.Text = region == null ? "" : region.RegionName;
            _Locality.Text = data.Profile.Locality;
            _ScopeType.Text = data.Profile.ScopeType;
            //_DiscountCardNumber.Text = data.Profile.DiscountCardNumber;
            _HowKnow.Text = data.Profile.HowKnow;

            _ContactPersonPosition.Text = data.Profile.ContactPersonPosition;
            _ContactPersonSurname.Text = data.Profile.ContactPersonSurname;
            _ContactPersonName.Text = data.Profile.ContactPersonName;
            _ContactPersonPhone.Text = data.Profile.ContactPersonPhone;
            _ContactPersonExtPhone.Text = data.Profile.ContactPersonExtPhone;
            _ContactPersonFax.Text = data.Profile.ContactPersonFax;
            _ContactPersonEmail.Text = data.Profile.ContactPersonEmail;
            _DeliveryAddress.Text = data.Profile.DeliveryAddress;

            _CompanyName.Text = data.Profile.CompanyName;
            _CompanyRegistrationID.Text = data.Profile.CompanyRegistrationID;
            _CompanyAddress.Text = data.Profile.CompanyAddress;

            _BankName.Text = data.Profile.BankName;
            _IBAN.Text = data.Profile.IBAN;
            _SWIFT.Text = data.Profile.SWIFT;
            _BankAddress.Text = data.Profile.BankAddress;

            _DirectorName.Text = data.Profile.DirectorName;
            _DirectorSurname.Text = data.Profile.DirectorSurname;

            _CorrespondingBankName.Text = data.Profile.CorrespondentBankName;
            _CorrespondingIBAN.Text = data.Profile.CorrespondentIBAN;
            _CorrespondingSWIFT.Text = data.Profile.CorrespondentSWIFT;
            _CorrespondingBankAddress.Text = data.Profile.CorrespondentBankAddress;

            ////Пункт выдачи заказов
            //var rmsStore = data.Profile.RmsStoreId != null ? AcctgRefCatalog.RmsStores[data.Profile.RmsStoreId] : null;
            //if ( rmsStore != null )
            //{
            //    _RmsStores.Text = Server.HtmlEncode( rmsStore.StoreName );
            //}
            //else
            //{
            //}

            ////Группа персональной скидки
            //_clientGroupName.Text = data.Profile.ClientGroupName;

            //Пока один тип клиентов нет необходимости разбивать все на плейс-холдеры и извращаться с видимостью
            //UpdateVisiblePH(Page.Controls); 
        }

        private void UpdateVisiblePH(ControlCollection CC)
        {
            foreach (Control oCtrl in CC)
            {
                UpdateVisiblePH(oCtrl.Controls);
                if (oCtrl is PlaceHolder)
                {
                    PlaceHolder ph = (PlaceHolder)oCtrl;
                    if (ph.ID.StartsWith("vreg_"))
                    {
                        switch (data.Profile.Category)
                        {
                            case ClientCategory.Legal:
                                if (ph.ID.Contains("Legal"))
                                {
                                    ph.Visible = true;
                                }
                                else
                                {
                                    ph.Visible = false;
                                }
                                break;
                            case ClientCategory.PhysicalIP:
                                if (ph.ID.Contains("IP"))
                                {
                                    ph.Visible = true;
                                }
                                else
                                {
                                    ph.Visible = false;
                                }
                                break;
                            case ClientCategory.Physical:
                                if (ph.ID.Contains("Phy"))
                                {
                                    ph.Visible = true;
                                }
                                else
                                {
                                    ph.Visible = false;
                                }
                                break;
                        }
                    }
                }
            }
            foreach (Control oCtrl in CC)
            {
                UpdateVisiblePH(oCtrl.Controls);
                if (oCtrl is PlaceHolder)
                {
                    PlaceHolder ph = (PlaceHolder)oCtrl;
                    if (ph.ID.StartsWith("vregRO_"))
                    {
                        switch (data.Profile.TradingVolume)
                        {
                            case TradingVolume.Retail:
                                if (ph.ID.Contains("Rozn"))
                                {
                                    ph.Visible = true;
                                }
                                else
                                {
                                    ph.Visible = false;
                                }
                                break;
                            case TradingVolume.Wholesale:
                                if (ph.ID.Contains("Opt"))
                                {
                                    ph.Visible = true;
                                }
                                else
                                {
                                    ph.Visible = false;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}