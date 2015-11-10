using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;


namespace RmsAuto.Store.Web.Manager.Controls
{
    public partial class FillClientProfileExt : System.Web.UI.UserControl
    {
        public ClientRegistrationDataExt RegistrationData
        {
            get
            {
                return new ClientRegistrationDataExt
                {
                    TradingVolume = Acctg.TradingVolume.Wholesale,
                    
                    ClientCategory =  ClientCategory.Legal,
                    Email = _txtEmail.Text,
                    ClientName = _txtLegalName.Text,
                    //ClientCategory = (ClientCategory)Enum.Parse( typeof( ClientCategory ), _rblClientCategory.SelectedValue ),
                    //TradingVolume = (TradingVolume)Enum.Parse( typeof( TradingVolume ), _rblTradingVolume.SelectedValue ),
                    CountryID = Convert.ToInt32( _ddlCountry.SelectedValue ),
                    RegionID =  1,//_ddlRegion.Text,
                    Locality = _ddlRegion.Text,

                    ContactPersonPosition = _txtContactPosition.Text,
                    ContactPersonSurname = _txtContactLastName.Text,
                    ContactPersonName = _txtContactFirstName.Text,
                   // ContactMiddleName = _txtContactMiddleName.Text,
                    ContactPersonPhone = _ContactPhone.Value,
                    ContactPersonExtPhone = _ContactExtPhone.Value,
                    ContactPersonFax = _ContactFax.Value,
                    
                    //ScheduleOfice = _txtScheduleOfice.Text,
                    //ScheduleStock = _txtScheduleStock.Text,
                    //RmsStoreID = TradingVolume == TradingVolume.Wholesale ? AcctgRefCatalog.RmsStores.Items.Where( t => t.IsWholesale ).First().StoreId : _ddlRmsStores.SelectedValue,
                    RmsStoreID = AcctgRefCatalog.RmsStores.Items.Where(t => t.IsWholesale).First().StoreId,
                    DeliveryAddress = _txtShippingAddress.Text,
                    ScopeType = _txtScopeType.Text,
                    //DiscountCardNumber = _discountCardNumber.Value,
                    HowKnow = _ddlHKT.SelectedItem.Text + "; " + _txtHKT.Text,
                    //INN = (ClientCategory == ClientCategory.PhysicalIP ? _txtINNPIP.Text : _txtINNLegal.Text),
                    BankAddress = _txtBankAddress.Text,
                    CompanyName = _txtLegalName.Text,
					CompanyRegistrationID = _txtCompanyRegistrationID.Text,
					CompanyAddress = _txtCompanyRegistrationID.Text,

					BankName = _txtBankName.Text,
					IBAN = _txtIBAN.Text,
					SWIFT = _txtSWIFT.Text,

                    DirectorName = _txtDirectorFirstName.Text,
                    DirectorSurname = _txtDirectorLastName.Text,

                    CorrespondentBankName = _txtCorrespondentBankName.Text,
					CorrespondentIBAN = _txtCorrespondentIBAN.Text,
					CorrespondentSWIFT = _txtCorrespondentSWIFT.Text,
                    CorrespondentBankAddress = _txtCorrespondentBankAddrss.Text,
                   // CooreIBAN = _txtIBAN.Text,
                  //  SWIFT = _txtSWIFT.Text,
                    BankINN = ""//_txtBankINN.Text,
               
                };
            }
        }

        public bool IsRestricted
        {
            get { return _chkRestrict.Checked; }
        }

        public string BankName { get { return _txtBankName.Text; } }
        public string IBAN { get { return _txtIBAN.Text; } }
        public string SWIFT { get { return _txtSWIFT.Text; } }
        public string BankAddress { get { return _txtBankAddress.Text; } }


        //public ClientCategory ClientCategory
        //{
        //    get { return (ClientCategory)Enum.Parse( typeof( ClientCategory ), _rblClientCategory.SelectedValue ); }
        //}

        //public TradingVolume TradingVolume
        //{
        //    get { return (TradingVolume)Enum.Parse( typeof( TradingVolume ), _rblTradingVolume.SelectedValue ); }
        //}

        protected void _cvDdlIsSelect_ServerValidate( object source, ServerValidateEventArgs args )
        {
            try
            {
                CustomValidator CV = (CustomValidator)source;
                DropDownList ddl = (DropDownList)FindControl( CV.ControlToValidate );
                if ( ddl != null )
                {
                    args.IsValid = ddl.SelectedValue != "0";
                }
                else
                {
                    args.IsValid = false;
                }
            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void Page_Load( object sender, EventArgs e )
        {
            dcCommonDataContext DC = new dcCommonDataContext();
            if ( !Page.IsPostBack )
            {
                int cCountry = DC.spSelAllCountry().Count();
                foreach ( var oCountry in DC.spSelAllCountry() )
                {
                    _ddlCountry.Items.Add( new ListItem()
                    {
                        Text = oCountry.CountryName,
                        Value = oCountry.CountryID.ToString(),
                        Selected = cCountry == 1
                    } );
                }
                UpdateRegion();
                //int cScopeType = DC.spSelAllScopeType().Count();
                //foreach ( var oScopeType in DC.spSelAllScopeType() )
                //{
                //    _ddlScopeType.Items.Add(new ListItem()
                //    {
                //        Text = oScopeType.ScopeType_Name,
                //        Value = oScopeType.ScopeType_ID.ToString(),
                //        Selected = cScopeType == 1
                //    });
                //}
                //int cHKT = DC.spSelAllHowKnowType().Count();
                //foreach ( var oHKT in DC.spSelAllHowKnowType() )
                //{
                //    _ddlHKT.Items.Add( new ListItem()
                //    {
                //        Text = oHKT.HowKnowType_Name,
                //        Value = oHKT.HowKnowType_ID.ToString(),
                //        Selected = cHKT == 1
                //    } );
                //}
                //ManagerSiteContext MSC = (ManagerSiteContext)SiteContext.Current;
                //foreach ( var store in AcctgRefCatalog.RmsStores.Items.Where( s => s.IsRetail ) )
                //{
                //    _ddlRmsStores.Items.Add(new ListItem()
                //    {
                //        Text = store.FullInfo,
                //        Value = store.StoreId,
                //        Selected = MSC.ManagerInfo.StoreId == store.StoreId
                //    });
                //}
            }
            else
            {
                //spSelBankInfoByBICResult bankinfo = DC.spSelBankInfoByBIC( _txtBankBIC.Text ).FirstOrDefault();
                //if ( bankinfo == null )
                {
                  
                }
            }
            UpdateVisiblePH( Controls );
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
                        //switch ( ClientCategory )
                        //{
                        //    case ClientCategory.Legal:
                        //        if ( ph.ID.Contains( "Legal" ) )
                        //        {
                        //            ph.Visible = true;
                        //        }
                        //        else
                        //        {
                        //            ph.Visible = false;
                        //        }
                        //        break;
                        //    case ClientCategory.PhysicalIP:
                        //        if ( ph.ID.Contains( "IP" ) )
                        //        {
                        //            ph.Visible = true;
                        //        }
                        //        else
                        //        {
                        //            ph.Visible = false;
                        //        }
                        //        break;
                        //    case ClientCategory.Physical:
                        //        if ( ph.ID.Contains( "Phy" ) )
                        //        {
                        //            ph.Visible = true;
                        //        }
                        //        else
                        //        {
                        //            ph.Visible = false;
                        //        }
                        //        break;
                        //}
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
                        //switch ( TradingVolume )
                        //{
                        //    case TradingVolume.Retail:
                        //        if ( ph.ID.Contains( "Rozn" ) )
                        //        {
                        //            ph.Visible = true;
                        //        }
                        //        else
                        //        {
                        //            ph.Visible = false;
                        //        }
                        //        break;
                        //    case TradingVolume.Wholesale:
                        //        if ( ph.ID.Contains( "Opt" ) )
                        //        {
                        //            ph.Visible = true;
                        //        }
                        //        else
                        //        {
                        //            ph.Visible = false;
                        //        }
                        //        break;
                        //}
                    }
                }
            }
        }

        private void UpdateRegion()
        {
            dcCommonDataContext DC = new dcCommonDataContext();
            List<spSelRegionByCountryResult> listRegion = DC.spSelRegionByCountry( Convert.ToInt32( _ddlCountry.SelectedValue ) ).ToList<spSelRegionByCountryResult>();
            int cRegion = listRegion.Count;
          
        }

        protected void _ddlCountry_SelectedIndexChanged( object sender, EventArgs e )
        {
            UpdateRegion();
        }

        protected void _txtBankBIC_TextChanged( object sender, EventArgs e )
        {
            dcCommonDataContext DC = new dcCommonDataContext();
            //spSelBankInfoByBICResult bankinfo = DC.spSelBankInfoByBIC( _txtBankBIC.Text ).FirstOrDefault();
            //if ( bankinfo != null )
            {
                //_txtBankINN.Text = bankinfo.BankInfo_INN;
              //  _txtBankKS.Text = bankinfo.BankInfo_Acc;
               // _txtBankName.Text = bankinfo.BankInfo_Name;
               // _tstBankHave.Text = "OK";
            }
            //else
            {
                // _tstBankHave.Text = "";
                //_txtBankKS.Text = "не заполнен";
                //_txtBankName.Text = "банк не найден";
            }
        }
    }
}