using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Web.Manager.Controls
{
    public partial class ClientProfileView : System.Web.UI.UserControl
    {
		public ClientData data
		{
			get;
			set;
		}
        protected void Page_PreRender( object sender, EventArgs e )
        {
            _checkedTrueLabel.Visible = data.Profile.IsChecked;
            _checkedFalseLabel.Visible = !data.Profile.IsChecked;

            _restrictedTrueLabel.Visible = data.Profile.IsRestricted;
            _restrictedFalseLabel.Visible = !data.Profile.IsRestricted;

            dcCommonDataContext DC = new dcCommonDataContext();
            _ClientName.Text = data.Profile.ClientName;
            _ClientCategory.Text = data.Profile.Category.ToTextOrName();
            _TradingVolume.Text = data.Profile.TradingVolume.ToTextOrName();
            _Email.Text = data.Profile.Email;
            var country = DC.spSelCountryByID( data.Profile.CountryID ).FirstOrDefault();
            _Country.Text = country == null ? "" : country.CountryName;
            var region = DC.spSelRegionByID( data.Profile.RegionID ).FirstOrDefault();
            _Region.Text = region == null ? "" : region.RegionName;
            _Locality.Text = data.Profile.Locality;
            _ContactLastName.Text = data.Profile.ContactLastName;
            _ContactFirstName.Text = data.Profile.ContactFirstName;
            _ContactMiddleName.Text = data.Profile.ContactMiddleName;
            _ContactPhone.Text = data.Profile.ContactPhone;
            _ContactExtPhone.Text = data.Profile.ContactExtPhone;
            _ContactFax.Text = data.Profile.ContactFax;
            _ScheduleOfice.Text = data.Profile.ScheduleOfice;
            _ScheduleStock.Text = data.Profile.ScheduleStock;
            _ShippingAddress.Text = data.Profile.ShippingAddress;
            _ScopeType.Text = data.Profile.ScopeType;
            _DiscountCardNumber.Text = data.Profile.DiscountCardNumber;
            _HowKnow.Text = data.Profile.HowKnow;
            _ContactPosition.Text = data.Profile.ContactPosition;
            _INN_Legal.Text = data.Profile.INN;
            _INN_IP.Text = data.Profile.INN;
            _OGRNIP.Text = data.Profile.OGRNIP;
            _OficialAddress.Text = data.Profile.OficialAddress;
            _RealAddress.Text = data.Profile.RealAddress;
            _NDSAggent.Text = data.Profile.NDSAggent ? "Является плательщиком НДС" : "Не является плательщиком НДС";
            _DirectorLastName.Text = data.Profile.DirectorLastName;
            _DirectorFirstName.Text = data.Profile.DirectorFirstName;
            _DirectorMiddleName.Text = data.Profile.DirectorMiddleName;
            _Account.Text = data.Profile.Account;
            _BankBIC.Text = data.Profile.BankBIC;
            var bank = DC.spSelBankInfoByBIC( data.Profile.BankBIC ).FirstOrDefault();
            if ( bank != null )
            {
                _BankKS.Text = bank.BankInfo_Acc;
                _BankName.Text = bank.BankInfo_Name;
            }
            _BalanceManLastName.Text = data.Profile.BalanceManLastName;
            _BalanceManFirstName.Text = data.Profile.BalanceManFirstName;
            _BalanceManMiddleName.Text = data.Profile.BalanceManMiddleName;
            _BalanceManPhone.Text = data.Profile.BalanceManPhone;
            _BalanceManPosition.Text = data.Profile.BalanceManPosition;
            _BalanceManEmail.Text = data.Profile.BalanceManEmail;
            _LegalName.Text = data.Profile.LegalName;
            _KPP.Text = data.Profile.KPP;
            _OGRN.Text = data.Profile.OGRN;
            _DirectorPosition.Text = data.Profile.DirectorPosition;


            //Пункт выдачи заказов
            var rmsStore = data.Profile.RmsStoreId != null ? AcctgRefCatalog.RmsStores[data.Profile.RmsStoreId] : null;
            if ( rmsStore != null )
            {
                _RmsStores.Text = Server.HtmlEncode( rmsStore.StoreName );
            }
            else
            {
            }
            //Менеджер
            var manager = !string.IsNullOrEmpty( data.Profile.ManagerId ) ? AcctgRefCatalog.RmsEmployees[data.Profile.ManagerId] : null;
            if ( manager != null )
            {
                _managerLabel.Text = manager.FullName;
            }

            //Отдел менеджера
			//var department = !string.IsNullOrEmpty( data.Profile.ManagerDeptId ) ? RefCatalog.RmsDepartments[data.Profile.ManagerDeptId] : null;
			//if ( department != null )
			//{
			//    _managerDepartmentLabel.Text = Server.HtmlEncode( department.TextValue );
			//}

            //Группа персональной скидки
            //_clientGroupName. = data.Profile.ClientGroupName;
            _prepaymentPercentLabel.Text = string.Format( "{0:0.00}%", data.Profile.PrepaymentPercent );
             UpdateVisiblePH( Page.Controls );
        }

        private void UpdateVisiblePH( ControlCollection CC )
        {
            foreach ( Control oCtrl in CC )
            {
                UpdateVisiblePH( oCtrl.Controls );
                if ( oCtrl is PlaceHolder )
                {
                    PlaceHolder ph = (PlaceHolder)oCtrl;
                    if ( ph.ID.StartsWith( "vreg_" ) )
                    {
                        switch ( data.Profile.Category )
                        {
                            case ClientCategory.Legal:
                                if ( ph.ID.Contains( "Legal" ) )
                                {
                                    ph.Visible = true;
                                }
                                else
                                {
                                    ph.Visible = false;
                                }
                                break;
                            case ClientCategory.PhysicalIP:
                                if ( ph.ID.Contains( "IP" ) )
                                {
                                    ph.Visible = true;
                                }
                                else
                                {
                                    ph.Visible = false;
                                }
                                break;
                            case ClientCategory.Physical:
                                if ( ph.ID.Contains( "Phy" ) )
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
            foreach ( Control oCtrl in CC )
            {
                UpdateVisiblePH( oCtrl.Controls );
                if ( oCtrl is PlaceHolder )
                {
                    PlaceHolder ph = (PlaceHolder)oCtrl;
                    if ( ph.ID.StartsWith( "vregRO_" ) )
                    {
                        switch ( data.Profile.TradingVolume )
                        {
                            case TradingVolume.Retail:
                                if ( ph.ID.Contains( "Rozn" ) )
                                {
                                    ph.Visible = true;
                                }
                                else
                                {
                                    ph.Visible = false;
                                }
                                break;
                            case TradingVolume.Wholesale:
                                if ( ph.ID.Contains( "Opt" ) )
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