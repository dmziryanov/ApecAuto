using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;


namespace RmsAuto.Store.Web
{
    public class ClientSiteContext : SiteContext
    {
        public ClientSiteContext(HttpContext httpContext, CustomPrincipal user)
            : base(httpContext, user)
        {

        }

        private string _ClientDataSessionKey
        {
            get { return "__ClientData__"; }
        }

        public override ClientData CurrentClient
        {
            get
            {
                ClientData cd = null;
                if (_HttpContext.Session != null)
                {
                    cd = (ClientData)_HttpContext.Session[_ClientDataSessionKey];

                    if (cd == null)
                    {
                        var profile = ClientProfile.Load(User.AcctgID);
                        cd = new ClientData()
                        {
                            Profile = profile,
                            Status = ClientBO.GetClientStatus(profile.ClientId),
                            Cart = CreateShoppingCart(User, profile)
                        };
                        _HttpContext.Session.Add(_ClientDataSessionKey, cd);
                    }
                }
                return cd;
            }
        }
        
        private ShoppingCart CreateShoppingCart(CustomPrincipal user, ClientProfile profile)
        {
			var cart = new ShoppingCart( user.UserId, user.AcctgID, profile.ClientGroup, profile.PersonalMarkup );
            cart.ContentChanged += OnCartContentChanged;
            return cart;
        }

        /// <summary>
        /// Обработчик изменения содержимого корзины. 
        /// </summary>
        private static void OnCartContentChanged(object sender, EventArgs e)
        {
            SiteContext.Current.InvalidateTotals();
            if (SiteContext.Current.CurrentClientTotals.ItemsCount == 0)
            {
                //В случае очистки содержимого сбрасывает номер заказа,
                //устанавливаемый при загрузке заказа из файла.
                HttpContext.Current.Profile["CustOrderNum"] = null;
            }
        }

        public override string UserDisplayName
        {
            get { return CurrentClient.Profile.ClientName; }
        }

    }
}
