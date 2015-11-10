using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Web
{
    public class ManagerSiteContext : SiteContext
    {
        public ManagerSiteContext(HttpContext httpContext, CustomPrincipal user)
            : base(httpContext, user)
        {
        }

        public override ClientData CurrentClient
        {
            get  
            {
                return ClientSet.DefaultClient ?? 
                    new ClientData
                    {
                        Profile = ClientBO.GuestProfile,
                        Cart = ShoppingCart.GetTemporary(),
                        IsGuest = true
                    }; 
            }
        }

        public bool ClientDataSectionEnabled(ClientDataSection section)
        {
            if (CurrentClient.IsGuest)
                switch (section)
                {
                    //В этих режимах выбранный пользователь не требуется
                    case ClientDataSection.AllOrders:
                    case ClientDataSection.ClientOrdelinesLoad:
                    case ClientDataSection.Orders:
                        return true;

                    default:
                        throw new InvalidOperationException("Current client not selected");
                }

            switch (section)
            {
                case ClientDataSection.Profile:
				case ClientDataSection.Reclamations:
                case ClientDataSection.AllOrders:
                case ClientDataSection.ClientOrdelinesLoad:
                    return true;
                case ClientDataSection.Garage:
                case ClientDataSection.VinRequests:
                    return ClientBO.CanManageClient(ManagerInfo, CurrentClient.Profile);
                case ClientDataSection.Cart:
                case ClientDataSection.CartImport:
                    return ClientBO.CanUseClientCart(ManagerInfo, CurrentClient.Profile);
				case ClientDataSection.Orders:
					return ClientBO.CanManageClient( ManagerInfo, CurrentClient.Profile );
				default:
                    throw new InvalidOperationException("Invalid clientData section");
            }
        }

        public override string UserDisplayName
        {
            get 
            {
                var empInfo = AcctgRefCatalog.RmsEmployees[User.AcctgID];
                if (empInfo == null)
                    throw new BLException("EmployeesRef doesn't contain record for EmployeeID: " + User.AcctgID); 
                return empInfo.FullName; 
            }
        }

		public EmployeeInfo ManagerInfo
		{
			get { return AcctgRefCatalog.RmsEmployees[ User.AcctgID ]; }
		}

        public HandyClientSet ClientSet
        {
            get
            {
                var set = (HandyClientSet)_HttpContext.Session["_handyClientSet"];
                if (set == null)
                {
                    set = HandyClientSet.Load(User.UserId);
                    _HttpContext.Session.Add("_handyClientSet", set);
                }
                return set;
            }
        }
    }      
}
