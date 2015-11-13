using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Manager.Controls
{
    public partial class ClientProfileViewLite : System.Web.UI.UserControl
    {
        public ClientData data
        {
            get;
            set;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            _managerList.DataSource = ManagerBO.GetManagerList(Resources.Texts.NotSelected);
            _managerList.DataBind();
          
            chbIsChecked.Checked = data.Profile.IsChecked;
		//	chbIsAutoOrder.Checked = LightBO.GetIsAutoOrder(data.Profile.UserId);

            dcCommonDataContext DC = new dcCommonDataContext();
            //_ClientName.Text = data.Profile.ClientName;
            //_ClientNameIP.Text = data.Profile.ClientName;
			txtClientName.Text = data.Profile.ClientName;
            _ClientCategory.Text = data.Profile.Category.ToTextOrName();
            _TradingVolume.Text = data.Profile.TradingVolume.ToTextOrName();
            _Email.Text = data.Profile.Email;
            var country = DC.spSelCountryByID(data.Profile.CountryID).FirstOrDefault();
            _Country.Text = country == null ? "" : country.CountryName;
            var region = DC.spSelRegionByID(data.Profile.RegionID).FirstOrDefault();
            _Locality.Text = data.Profile.Locality;
            _ContactLastName.Text = data.Profile.ContactPersonSurname;
            _ContactFirstName.Text = data.Profile.ContactPersonName;
           // _ContactMiddleName.Text = data.Profile.ContactMiddleName;
            _ContactPhone.Text = data.Profile.ContactPersonPhone;
            _ContactExtPhone.Text = data.Profile.ContactPersonExtPhone;
            _ContactFax.Text = data.Profile.ContactPersonFax;
            //_ScheduleOfice.Text = data.Profile.ScheduleOfice;
            //_ScheduleStock.Text = data.Profile.ScheduleStock;
            _ShippingAddress.Text = data.Profile.DeliveryAddress;
            _ScopeType.Text = data.Profile.ScopeType;
            _DiscountCardNumber.Text = data.Profile.DiscountCardNumber;
            _HowKnow.Text = data.Profile.HowKnow;
            _ContactPosition.Text = data.Profile.ContactPersonPosition;




            _txtDirectorLastName.Text = data.Profile.DirectorSurname;
            _txtDirectorFirstName.Text = data.Profile.DirectorName;
            _txtBankName.Text = data.Profile.BankName;
          

            _CompanyName.Text = data.Profile.CompanyName;
            _CompanyRegistrationID.Text = data.Profile.CompanyRegistrationID;
            _CompanyAddress.Text = data.Profile.CompanyAddress;

            _txtBankName.Text = data.Profile.BankName;
            _txtIBAN.Text = data.Profile.IBAN;
            _txtSWIFT.Text = data.Profile.SWIFT;
            _txtBankAddress.Text = data.Profile.BankAddress;

            _txtCorrespondentBankName.Text = data.Profile.CorrespondentBankName;
            _txtCorrespondentIBAN.Text = data.Profile.CorrespondentIBAN;
            _txtCorrespondentSWIFT.Text = data.Profile.CorrespondentSWIFT;
            _txtCorrespondentBankAddrss.Text = data.Profile.CorrespondentBankAddress;

            
            email2.Text = LightBO.GetAdditionalEmail(data.Profile.UserId);


            //Пункт выдачи заказов
            var rmsStore = data.Profile.RmsStoreId != null ? AcctgRefCatalog.RmsStores[data.Profile.RmsStoreId] : null;
            if (rmsStore != null)
            {
                _RmsStores.Text = Server.HtmlEncode(rmsStore.StoreName);
            }
            else
            {
            }
            //Менеджер
            var manager = !string.IsNullOrEmpty(data.Profile.ManagerId) ? AcctgRefCatalog.RmsEmployees[data.Profile.ManagerId] : null;
            if (manager != null)
            {
                _managerList.SelectedValue = manager.EmployeeId;
            }
            else
            {
                _managerList.SelectedValue = "0";
            }

            //Отдел менеджера
            //var department = !string.IsNullOrEmpty( data.Profile.ManagerDeptId ) ? RefCatalog.RmsDepartments[data.Profile.ManagerDeptId] : null;
            //if ( department != null )
            //{
            //    _managerDepartmentLabel.Text = Server.HtmlEncode( department.TextValue );
            //}

            //Группа персональной скидки
            _clientGroupName.SelectedValue = ((int)data.Profile.ClientGroup).ToString();
            _prepaymentPercentLabel.Text = string.Format("{0:0.00}", data.Profile.PrepaymentPercent);
            _DelayDays.Text = LightBO.GetPaymentDelayDays(data.Profile.UserId).ToString();
            _paymentLimit.Text = LightBO.GetPaymentLimit(data.Profile.UserId).ToString();
            
            UpdateVisiblePH(Page.Controls);
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

        protected void _clientGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
			//using (var ctx = new DCFactory<StoreDataContext>())
			//{
			//    var s = ctx.DataContext.Users.Where(x => x.UserID ==  data.Profile.UserId).FirstOrDefault();
			//    s.ClientGroup = int.Parse(_clientGroupName.SelectedValue);
			//    data.Profile.ClientGroup = (RmsAuto.Acctg.ClientGroup)(int.Parse(_clientGroupName.SelectedValue));
			//    s.IsChanged = true;
			//    ctx.DataContext.SubmitChanges();
			//    ctx.SetCommit();
			//}
        }

		protected void btnSave_Click(object sender, EventArgs e)
		{
			using (var dc = new DCFactory<StoreDataContext>())
			{
                //dc.DataContext.Log = new DebuggerWriter();
                var user = dc.DataContext.Users.Where(x => x.UserID == data.Profile.UserId).FirstOrDefault();
				user.ClientGroup = int.Parse(_clientGroupName.SelectedValue);
				data.Profile.ClientGroup = (RmsAuto.Acctg.ClientGroup)(int.Parse(_clientGroupName.SelectedValue));
				user.IsChanged = true;
				user.IsChecked = chbIsChecked.Checked;
				data.Profile.IsChecked = chbIsChecked.Checked;
				//if (data.Profile.Category == ClientCategory.PhysicalIP)
				//{
				data.Profile.ClientName = txtClientName.Text;//_ClientNameIP.Text;
                //}                
               // dc.DataContext.SubmitChanges();

                data.Profile.ContactPersonSurname = _ContactLastName.Text;
                data.Profile.ContactPersonName = _ContactFirstName.Text;
                // _ContactMiddleName.Text = data.Profile.ContactMiddleName;
                data.Profile.ContactPersonPhone = _ContactPhone.Text;
                data.Profile.ContactPersonExtPhone = _ContactExtPhone.Text;
                data.Profile.ContactPersonFax = _ContactFax.Text;
               // data.Profile.ScheduleOfice = _ScheduleOfice.Text;
                //data.Profile.ScheduleStock = _ScheduleStock.Text;
                data.Profile.ShippingAddress = _ShippingAddress.Text;
                data.Profile.ScopeType = _ScopeType.Text;
                data.Profile.DiscountCardNumber = _DiscountCardNumber.Text;
                data.Profile.HowKnow = _HowKnow.Text;
                data.Profile.ContactPosition = _ContactPosition.Text;

                data.Profile.DirectorLastName = _txtDirectorLastName.Text;
                data.Profile.DirectorFirstName = _txtDirectorFirstName.Text;
                

                //DiscountCardNumber = _discountCardNumber.Value,
                
                    //INN = (ClientCategory == ClientCategory.PhysicalIP ? _txtINNPIP.Text : _txtINNLegal.Text),
                data.Profile.BankAddress = _txtBankAddress.Text;
                data.Profile.CompanyName = txtClientName.Text;


				data.Profile.BankName = _txtBankName.Text;
			    data.Profile.IBAN = _txtIBAN.Text;
				data.Profile.SWIFT = _txtSWIFT.Text;

                data.Profile.DirectorName = _txtDirectorFirstName.Text;
                data.Profile.DirectorSurname = _txtDirectorLastName.Text;

                data.Profile.CorrespondentBankName = _txtCorrespondentBankName.Text;
                data.Profile.CorrespondentIBAN = _txtCorrespondentIBAN.Text;
                data.Profile.CorrespondentSWIFT = _txtCorrespondentSWIFT.Text;
                data.Profile.CorrespondentBankAddress = _txtCorrespondentBankAddrss.Text;


                //using (dcCommonDataContext DC = new dcCommonDataContext())
                //{
                //    var bank = DC.spSelBankInfoByBIC(_txtIBAN.Text).FirstOrDefault();
                //    if (bank != null)
                //    {
                //        bank.BankInfo_Acc = _BankKS.Text;
                //        bank.BankInfo_Name = _BankName.Text;
                //    }
                //    else
                //    {
                //        _BankBIC.Text = "Неверный БИК";
                //    }
                //}

                //data.Profile.BalanceManLastName = _txtBalanceManLastName.Text;
                //data.Profile.BalanceManFirstName = _tctBalanceManFirstName.Text;
                //data.Profile.BalanceManMiddleName = _txtBalanceManMiddleName.Text;
                //data.Profile.BalanceManPhone = _txtBalanceManPhone.Text;
                //data.Profile.BalanceManPosition = _txtBalanceManPosition.Text;
                //data.Profile.BalanceManEmail = _txtBalanceManEmail.Text;
                data.Profile.Email = _Email.Text;
                //data.Profile.DirectorPosition = _txtDirectorPosition.Text;
                
                dc.DataContext.spUpdUsersLight(data.Profile.AcctgId, data.Profile.ClientName, data.Profile.Email, (byte)data.Profile.TradingVolume, (byte)data.Profile.Category,
                data.Profile.CountryID, data.Profile.RegionID, data.Profile.Locality, data.Profile.ContactPersonSurname, data.Profile.ContactPersonName, "" /*data.Profile.ContactMiddleName*/,
                data.Profile.ContactPersonPhone, data.Profile.ContactPersonExtPhone, data.Profile.ScopeType, data.Profile.HowKnow, data.Profile.ManagerId, "", data.Profile.IsChecked, data.Profile.IsRestricted,
                (int)data.Profile.ClientGroup, data.Profile.PrepaymentPercent, data.Profile.PersonalMarkup, data.Profile.ContactPersonFax, data.Profile.ScheduleOfice, data.Profile.ScheduleStock, data.Profile.ShippingAddress,
                data.Profile.RmsStoreId, data.Profile.DiscountCardNumber, data.Profile.ContactPersonPosition, data.Profile.LegalName, data.Profile.INN, data.Profile.OGRNIP, data.Profile.KPP, data.Profile.OGRN,
                data.Profile.NDSAggent, data.Profile.OficialAddress, data.Profile.RealAddress, data.Profile.Account, data.Profile.BankBIC, data.Profile.BankINN, data.Profile.DirectorPosition,
                data.Profile.DirectorLastName, data.Profile.DirectorFirstName, data.Profile.DirectorMiddleName, data.Profile.BalanceManPosition,
                data.Profile.BalanceManLastName, data.Profile.BalanceManFirstName, data.Profile.BalanceManMiddleName,
                data.Profile.BalanceManPhone, data.Profile.BalanceManEmail, data.Profile.ReclamationPeriod);
				
				dc.SetCommit();
			}

			//TODO сделать возможность в фабрике управлять connection string: чтобы иметь возможность выставить свойство MultipleActiveResultSet (MARS)
			// для того чтобы сделать оба действия в транзакции см. выше и ниже

			using (var dc = new DCFactory<StoreDataContext>(false))
			{
				string query = @"exec dbo.spLightUpdAutoOrder {0}, {1}";
				dc.DataContext.ExecuteCommand(query, SiteContext.Current.CurrentClient.Profile.UserId, false /* chbIsAutoOrder.Checked */);
			}

            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"exec dbo.spLightUpdClientName {0}, {1}";
                dc.DataContext.ExecuteCommand(query, SiteContext.Current.CurrentClient.Profile.UserId, data.Profile.ClientName /* chbIsAutoOrder.Checked */);
            }
            
            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"update dbo.Users set PrepaymentPercent = {0} where UserId = {1}";
                if (dc.DataContext.ExecuteCommand(query, double.Parse(_prepaymentPercentLabel.Text), SiteContext.Current.CurrentClient.Profile.UserId) == 1)
                {
                    data.Profile.PrepaymentPercent =  decimal.Parse(_prepaymentPercentLabel.Text);
                }
            }

            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"exec dbo.spLightUpdPaymentDelay {0}, {1}";
                dc.DataContext.ExecuteCommand(query, SiteContext.Current.CurrentClient.Profile.UserId, int.Parse(_DelayDays.Text));
            }

            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"exec dbo.spLightUpdPersonalManager {0}, {1}";
                data.Profile.ManagerId = _managerList.SelectedValue;
                dc.DataContext.ExecuteCommand(query, SiteContext.Current.CurrentClient.Profile.UserId, _managerList.SelectedValue);
            }

            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                string query = @"exec dbo.spLightUpdEmail {0}, {1}";
                data.Profile.ManagerId = _managerList.SelectedValue;
                dc.DataContext.ExecuteCommand(query, SiteContext.Current.CurrentClient.Profile.UserId, email2.Text);
            }
            
            LightBO.UpdateUserLimit(SiteContext.Current.CurrentClient.Profile.UserId, decimal.Parse(_paymentLimit.Text));

            ShowMessage("Data is successfully saved");
		}

        private void ShowMessage(string message)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>");
        }
    }
}