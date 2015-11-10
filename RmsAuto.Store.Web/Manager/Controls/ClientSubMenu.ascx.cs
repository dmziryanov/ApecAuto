using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using RmsAuto.Store.BL;
using RmsAuto.Store.Web;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class ClientSubMenu : System.Web.UI.UserControl
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _menuItems.DataSource = CreateItems();
                _menuItems.DataBind();
            }
        }
        
        private IEnumerable<ClientSubMenuItem> CreateItems()
        {
            var context = (ManagerSiteContext)SiteContext.Current;
            var hostPage = (ClientBoundPage)Page;

            yield return new ClientSubMenuItem
            {
                DataSection = ClientDataSection.Profile,
                DisplayText = "Client’s profile",
                SectionUrl = ClientProfile.GetUrl(),
                Enabled = context.ClientDataSectionEnabled(ClientDataSection.Profile),
                Selected = hostPage.DataSection == ClientDataSection.Profile
            };

            //Пока это не надо
            //yield return new ClientSubMenuItem
            //{
            //    DataSection = ClientDataSection.Garage,
            //    DisplayText = "Автомобили",
            //    SectionUrl = ClientGarage.GetUrl(),
            //    Enabled = context.ClientDataSectionEnabled(ClientDataSection.Garage),
            //    Selected = hostPage.DataSection == ClientDataSection.Garage
            //};

            //yield return new ClientSubMenuItem
            //{
            //    DataSection = ClientDataSection.VinRequests,
            //    DisplayText = "Запросы по VIN",
            //    SectionUrl = ClientVinRequests.GetUrl(),
            //    Enabled = context.ClientDataSectionEnabled(ClientDataSection.VinRequests),
            //    Selected = hostPage.DataSection == ClientDataSection.VinRequests
            //};

            yield return new ClientSubMenuItem
            {
                DataSection = ClientDataSection.Cart,
                DisplayText = "Cart",
                SectionUrl = ClientCart.GetUrl(),
                Enabled = context.ClientDataSectionEnabled(ClientDataSection.Cart),
                Selected = hostPage.DataSection == ClientDataSection.Cart
            };

            yield return new ClientSubMenuItem
            {
                DataSection = ClientDataSection.Orders,
                DisplayText = "Invoices and orders",
                SectionUrl = ClientOrders.GetUrl(),
                Enabled = context.ClientDataSectionEnabled(ClientDataSection.Orders),
                Selected = hostPage.DataSection == ClientDataSection.Orders
            };

            yield return new ClientSubMenuItem
            {
                DataSection = ClientDataSection.CartImport,
                DisplayText = "Upload from file",
                SectionUrl = ClientCartImport.GetUrl(),
                Enabled = context.ClientDataSectionEnabled(ClientDataSection.CartImport),
                Selected = hostPage.DataSection == ClientDataSection.CartImport
            };

            if (!AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
            {

                yield return new ClientSubMenuItem
                {
                    DataSection = ClientDataSection.Reclamations,
                    DisplayText = "Reclamations",
                    SectionUrl = ClientReclamations.GetUrl(),
                    Enabled = context.ClientDataSectionEnabled(ClientDataSection.Reclamations),
                    Selected = hostPage.DataSection == ClientDataSection.Reclamations
                };
            }

            if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
            {
                yield return new ClientSubMenuItem
                {
                    DataSection = ClientDataSection.ClientOrdelinesLoad,
                    DisplayText = "Dispatch",
                    SectionUrl = ClientOrdelinesLoad.GetUrl(),
                    Enabled = context.ClientDataSectionEnabled(ClientDataSection.Reclamations),
                    Selected = hostPage.DataSection == ClientDataSection.Reclamations
                };
            }
        }
	}

    public class ClientSubMenuItem
    {
        public ClientDataSection DataSection { get; set; }

        public string DisplayText { get; set; }

        public string SectionUrl { get; set; }

        public bool Selected { get; set; }

        public bool Enabled { get; set; }
    }
}