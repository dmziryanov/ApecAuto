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
    public partial class FillClientProfileExt : System.Web.UI.UserControl
    {
        public event EventHandler<EventArgs> Completed;

        public string Title
        {
            get { return (string)ViewState["__title"]; }
            set { ViewState["__title"] = value; }
        }

        public ClientCategory ClientCategory
        {
            //get { return (ClientCategory)Enum.Parse( typeof( ClientCategory ), _rblClientCategory.SelectedValue ); }
			get { return ClientCategory.Legal; } //пока что всегда ЮР. ЛИЦО
        }

        public TradingVolume TradingVolume
        {
            //get { return (TradingVolume)Enum.Parse( typeof( TradingVolume ), _rblTradingVolume.SelectedValue ); }
			get { return TradingVolume.Wholesale; } //пока что всегда ОПТ
        }

        public string Password
        {
            get { return (string)ViewState["__pwd"]; }
            private set { ViewState["__pwd"] = value; }
        }
        public string Login { get { return _editUser.Username; } }
        public string Email { get { return _txtEmail.Text; } }
        public int CountryID { get { return Convert.ToInt32( _ddlCountry.SelectedValue ); } }
        //public int RegionID { get { return Convert.ToInt32( _ddlRegion.SelectedValue ); } }
        public string Locality { get { return _txtLocality.Text; } }


		//public string ContactMiddleName { get { return _txtContactMiddleName.Text; } }
		//public string ScheduleOfice { get { return _txtScheduleOfice.Text; } }
		//public string ScheduleStock { get { return _txtScheduleStock.Text; } }

        public string RmsStoreID
        {
            get
            {
                if ( TradingVolume == TradingVolume.Wholesale )
                {
                   
                        return AcctgRefCatalog.RmsStores.Items.First().StoreId;
                    
                }
				else
				{
					throw new Exception("TradingVolume not Wholesale");
					//return _ddlRmsStores.SelectedValue;
				}
            }
        }
		

        public string ScopeType { get { return _ddlScopeType.SelectedItem.Text + "; " + _txtScopeType.Text; } }

        public string DiscountCardNumber
        {
			get { return string.Empty;/*_discountCardNumber.Value;*/ }
        }

        public string DiscountCardNumberDisplay
        {
			get { return string.Empty;/*_discountCardNumber.DisplayValue;*/ }
        }

        public string HowKnow { get { return _ddlHKT.SelectedItem.Text + "; " + _txtHKT.Text; } }

		//public string INN
		//{
		//    get
		//    {
		//        if ( ClientCategory == ClientCategory.PhysicalIP )
		//        {
		//            return _txtINNPIP.Text;
		//        }
		//        else
		//        {
		//            return _txtINNLegal.Text;
		//        }
		//    }
		//}
        //public string OGRNIP { get { return _txtOGRNIP.Text; } }
        //public string OficialAddress { get { return _txtOficialAddress.Text; } }
        //public string RealAddress { get { return _txtRealAddress.Text; } }
        //public bool NDSAggent { get { return _chNDSAggent.Checked; } }
        //public string DirectorMiddleName { get { return _txtDirectorMiddleName.Text; } }
        //public string Account { get { return _txtAccount.Text; } }
        //public string BankBIC { get { return _txtBankBIC.Text; } }
        //public string BankINN { get { return ""; } }// _txtBankINN.Text; } }
        //public string BalanceManLastName { get { return _txtBalanceManLastName.Text; } }
        //public string BalanceManFirstName { get { return _txtBalanceManFirstName.Text; } }
        //public string BalanceManMiddleName { get { return _txtBalanceManMiddleName.Text; } }
        //public string BalanceManPhone { get { return _BalanceManPhone.Value; } }
        //public string BalanceManPosition { get { return _txtBalanceManPosition.Text; } }
		//public string BalanceManEmail { get { return _txtBalanceManEmail.Text; } }



		#region === Contact information ====

		public string ContactPersonPosition { get { return _txtContactPosition.Text; } }
		public string ContactPersonName { get { return _txtContactFirstName.Text; } }
		public string ContactPersonSurname { get { return _txtContactLastName.Text; } }
		public string ContactPersonPhone { get { return _ContactPhone.Value; } }
		public string ContactPersonExtPhone { get { return _ContactExtPhone.Value; } }
		/*public string ContactPersonFax { get { return _ContactFax.Value; } }
		public string ContactPersonEmail { get { return _txtContactPersonEmail.Text; } }*/
        public string DeliveryAddress { get { return lCFShippingAddress.Text; } }
        public int RegisterAs { get { return int.Parse(SellerInfo.SelectedValue); } }
		#endregion

		#region === Company details ====
		
		/*public string CompanyName { get { return _txtLegalName.Text; } }
		public string CompanyRegistrationID { get { return _txtCompanyRegistrationID.Text; } }
		public string CompanyAddress { get { return _txtLegalAddress.Text; } }*/

		#endregion

		# region === Bank details ====

	/*	public string BankName { get { return _txtBankName.Text; } }
		public string IBAN { get { return _txtIBAN.Text; } }
		public string SWIFT { get { return _txtSWIFT.Text; } }
		public string BankAddress { get { return _txtBankAddress.Text; } }*/

		#endregion

		#region === General manager ====

	/*	public string DirectorName { get { return _txtDirectorFirstName.Text; } }
		public string DirectorSurname { get { return _txtDirectorLastName.Text; } }*/

		#endregion

		#region === Correspondent bank ====

		/*public string CorrespondentBankName { get { return _txtCorrespondentBankName.Text; } }
		public string CorrespondentIBAN { get { return _txtCorrespondentIBAN.Text; } }
		public string CorrespondentSWIFT { get { return _txtCorrespondentSWIFT.Text; } }
		public string CorrespondentBankAddress { get { return _txtCorrespondentBankAddrss.Text; } }*/

		#endregion

		//public string IPName
		//{ 
		//    get
		//    {
		//        if (_txtIPName == null || string.IsNullOrEmpty(_txtIPName.Text))
		//            return _txtContactLastName.Text + " " + _txtContactFirstName.Text /*+ " " + _txtContactMiddleName.Text*/;
		//        else
		//            return _txtIPName.Text;
		//    }
		//}

        //public string KPP { get { return _txtKPP.Text; } }

       // public string OGRN { get { return _txtOGRN.Text; } }

        //public string DirectorPosition { get { return _ddlDirectorPosition.SelectedItem.Text; } }

        protected void _cvIsAccept_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                CheckBox cb = (CheckBox)_fillProfileWizard.FindControl("_cbAccept");
                if (cb != null)
                {
                    args.IsValid = cb.Checked;
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
        
        protected void _cvDdlIsSelect_ServerValidate( object source, ServerValidateEventArgs args )
        {
            try
            {
                CustomValidator CV = (CustomValidator)source;
                DropDownList ddl = (DropDownList)_fillProfileWizard.FindControl( CV.ControlToValidate );
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

        protected void _ValidateEmail( object source, ServerValidateEventArgs args )
        {
            args.IsValid = !ClientBO.EmailInUse( args.Value );
        }

        protected void Page_Load( object sender, EventArgs e )
        {

            //lCFNDSAggentYes.Visible = NDSAggent;
            //lCFNDSAggentNo.Visible = !NDSAggent;

            dcCommonDataContext DC = new dcCommonDataContext();
            if ( !Page.IsPostBack )
            {
                CaptureImage.RefreshCapture();
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
                //UpdateRegion();
				//int cScopeType = DC.spSelAllScopeType().Count();
				//foreach ( var oScopeType in DC.spSelAllScopeType() )
				//{
				//    _ddlScopeType.Items.Add( new ListItem()
				//    {
				//        Text = oScopeType.ScopeType_Name,
				//        Value = oScopeType.ScopeType_ID.ToString(),
				//        Selected = cScopeType == 1
				//    } );
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
				//foreach ( var store in AcctgRefCatalog.RmsStores.Items.Where( s => s.IsRetail ) )
				//{
				//    _ddlRmsStores.Items.Add( new ListItem()
				//    {
				//        Text = store.FullInfo,
				//        Value = store.StoreId
				//    } );
				//}
            }
			//else
			//{
			//    spSelBankInfoByBICResult bankinfo = DC.spSelBankInfoByBIC( _txtBankBIC.Text ).FirstOrDefault();
			//    if ( bankinfo == null )
			//    {
			//        _rfvBankValidator.IsValid = false;
			//    }
			//}
            UpdateVisiblePH( _fillProfileWizard.Controls );

			//if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
			//{
			//    TextItemControl3.TextItemID = SiteContext.Current.InternalFranchName+".ContractOffer.Opt";
			//    TextItemControl10.TextItemID = SiteContext.Current.InternalFranchName + ".ContractOffer.Rozn";
			//}

            DC.Dispose();

        }

        protected void _fillProfileWizard_ActiveStepChanged( object sender, EventArgs e )
        {
            if ( _fillProfileWizard.ActiveStepIndex == _fillProfileWizard.WizardSteps.IndexOf(_contactInfoStep ) )
            {
                Password = _editUser.Password;
            }
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
                        switch ( ClientCategory )
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
                        switch ( TradingVolume )
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

		//private void UpdateRegion()
		//{
		//    using (dcCommonDataContext DC = new dcCommonDataContext())
		//    {
		//        List<spSelRegionByCountryResult> listRegion = DC.spSelRegionByCountry(Convert.ToInt32(_ddlCountry.SelectedValue)).ToList<spSelRegionByCountryResult>();
		//        int cRegion = listRegion.Count;
		//        _ddlRegion.Items.Clear();
		//        _ddlRegion.Items.Add(new ListItem()
		//        {
		//            Text = "<выберите>",
		//            Value = "0"
		//        });
		//        foreach (var oRegion in listRegion)
		//        {
		//            _ddlRegion.Items.Add(new ListItem()
		//            {
		//                Text = oRegion.RegionName,
		//                Value = oRegion.RegionID.ToString(),
		//                Selected = cRegion == 1
		//            });
		//        }
		//    }
		//}

        protected void _fillProfileWizard_FinishButtonClick( object sender, WizardNavigationEventArgs e )
        {
            if ( Page.IsValid )
            {
                OnCompleted( EventArgs.Empty );
            }
        }

        protected virtual void OnCompleted( EventArgs args )
        {
            if ( Completed != null )
                Completed( this, args );
        }

		//protected void _ddlCountry_SelectedIndexChanged( object sender, EventArgs e )
		//{
		//    UpdateRegion();
		//}

		//protected void _txtBankBIC_TextChanged(object sender, EventArgs e)
		//{
		//    using (dcCommonDataContext DC = new dcCommonDataContext())
		//    {
		//        spSelBankInfoByBICResult bankinfo = DC.spSelBankInfoByBIC(_txtBankBIC.Text).FirstOrDefault();
		//        if (bankinfo != null)
		//        {
		//            //_txtBankINN.Text = bankinfo.BankInfo_INN;
		//            _txtBankKS.Text = bankinfo.BankInfo_Acc;
		//            _txtBankName.Text = bankinfo.BankInfo_Name;
		//            _tstBankHave.Text = "OK";
		//        }
		//        else
		//        {
		//            _tstBankHave.Text = "";
		//            _txtBankKS.Text = "не заполнен";
		//            _txtBankName.Text = "банк не найден";
		//        }
		//    }
		//}
    }
}