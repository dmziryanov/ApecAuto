using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using System.Text;
using RmsAuto.Store.Acctg.Entities;
using System.IO;

namespace RmsAuto.Store.Web.Controls
{
    public partial class FillFranchProfileExt : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RmsAuto.Store.Entities.dcCommonDataContext DC = new RmsAuto.Store.Entities.dcCommonDataContext();
            if (!Page.IsPostBack)
            {
				using (var dc = new dcCommonDataContext())
				{
					int cScopeType = dc.spSelAllScopeType().Count();
					foreach (var oScopeType in dc.spSelAllScopeType())
					{
						_ddlScopeType.Items.Add(new ListItem()
						{
							Text = oScopeType.ScopeType_Name,
							Value = oScopeType.ScopeType_ID.ToString(),
							Selected = cScopeType == 1
						});
					}
				}
                
                _ddlScopeType.Items.RemoveAt(_ddlScopeType.Items.Count - 1);
                _ddlScopeType.Items.RemoveAt(_ddlScopeType.Items.Count - 1);
            }
        }
        public FranchEnvelope getFranchEnvelope()
        {
            return new FranchEnvelope()
            {
                DateTimeFranchBlankCreated = DateTime.Now.ToString(),
                City = _txtLocality.Text,
                AddressJur = _jurAddress.Text,
                AddressGeo = _geoAddress.Text,
                BusinessType = _ddlScopeType.SelectedItem.Text,
                ManagerName = _cntName.Text,
                ManagerAppointment = _appoint.Text,
                Phone = _ContactPhone.Value,
                PhoneSecond = _ContactPhone_2.Value,
                Email = _txtEmail.Text,
                Site = TextBox8.Text,
                MainArticlePositions = TextBox12.Text,
                AutoparkOwn = TextBox14.Text,
                FranchHistory = TextBox15.Text,
                PublicityHistory = TextBox16.Text,
                Competitors = TextBox17.Text,
                TurnoverPerYear = TextBox18.Text,
                PartOfRegionTurnover = TextBox1.Text,
                EmplyeeNumber = TextBox19.Text,
                StoreSquare = TextBox21.Text,
                ShopSquare = TextBox22.Text,
                OfficeSquare = TextBox23.Text,
                ArticleAvailabilityNumber = TextBox24.Text,
                ArticleOrderedNumber = TextBox25.Text,
                SuppliesNumber = TextBox26.Text,
                WholesaleClients = TextBox27.Text,
                RetailClients = TextBox28.Text,
                IncomingDocs = TextBox29.Text,
                OutgoingDocs = TextBox30.Text,
                PaymentsOutPerMonth = TextBox31.Text,
                PaymentsInPerMonth = TextBox32.Text,
                AchalandageStrategy = TextBox2.Text,
                LabelPrinterModel = TextBox33.Text,
                PayDescModel = TextBox34.Text,
                KKModel = TextBox35.Text,
                BarcodeScaner = TextBox36.Text,
                RegistrationSystemName = TextBox37.Text,
                RSClientCount = TextBox38.Text,
                ExternalChannelSpeed = TextBox39.Text,
                LocalChannelSpeed = TextBox40.Text,
                ClientBankName = TextBox41.Text,
                AutomatedStoreSystem = TextBox42.Text,
                Coments = TextBox421.Text,
                ExternalViewFileName = externalPhoto.Value,
                ExternalViewFileValue = imageToString(externalPhoto.FileValue),
                InternalViewFileName = internalPhoto.Value,
                InternalViewFileValue = imageToString(internalPhoto.FileValue)
            };
        }
        string imageToString(byte [] ba)
        {
            if (ba == null || ba.Length == 0) return "";
            return Convert.ToBase64String(ba);
        }
        /*void writeXmlImageFile(string fn, string str)
        {
            FileStream fs; try { fs = new FileStream(fn, FileMode.Create); } catch { return; }
            byte[] ba = Convert.FromBase64String(str);
            fs.Write(ba, 0, ba.Length);
            fs.Close();
        }*/
    }
}