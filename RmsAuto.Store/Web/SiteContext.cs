using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using System.Web.UI;
using System.IO;
using System.Configuration;

namespace RmsAuto.Store.Web
{
    public abstract class SiteContext
    {
        private const string _ItemsDictKey = "__siteContext";

        class AnonymousSiteContext : SiteContext
        {
            private ClientData _guestData = 
                new ClientData
                {
                    Profile = ClientBO.GuestProfile,
                    Cart = ShoppingCart.GetTemporary(),
                    IsGuest = true
                };
            
            public AnonymousSiteContext(HttpContext context, CustomPrincipal user)
                : base(context, null)
            {
            }

            public override ClientData CurrentClient
            {
                get { return _guestData; }
            }

            public override string UserDisplayName
            {
                get { return string.Empty; }
            }
        }

        public static SiteContext Current
        {
            get {
                if (HttpContext.Current != null)
                    { return (SiteContext)HttpContext.Current.Items[_ItemsDictKey]; }
                else
                    { return null; }
            }
        }


        public static T GetCurrent<T>() where T : SiteContext
        {
            return (T)Current; 
        }

        private CustomPrincipal _user;
        private HttpContext _httpContext;
        private ShoppingCartTotals _currentClientTotals;

        public static void Attach(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");
            
            if (httpContext.Request.IsAuthenticated)
            {
                var user = httpContext.User as CustomPrincipal;
                if (user.Role == SecurityRole.Client)
                {
                    var csc = new ClientSiteContext(httpContext, user);
                    httpContext.Items.Add(_ItemsDictKey, csc);

					if(!csc.CurrentClient.Profile.IsRestricted)
					{
						csc.CurrentClient.Cart.Merge(ShoppingCart.GetTemporary());
					}

				}
                else
                    httpContext.Items.Add(_ItemsDictKey, new ManagerSiteContext(httpContext, user));

              
            }
            else
                httpContext.Items[_ItemsDictKey] = new AnonymousSiteContext(httpContext, null);
        }

        public static void AttachFranchName(HttpContext httpContext)
        {
            //Данный механизм обеспечивает поиск InternalFranchName по урлу хоста из справочника
            //Что позволяет не использовать куку и механизм переключения
            var strUrl = httpContext.Request.Url.Host;
            _internalFranchName = AcctgRefCatalog.RmsFranches.Items.Where(x => x.Url.Contains(strUrl)).First().InternalFranchName;
             
            //Возможно это понадобиться в дальнейшем, будет настройка в справочнике лайтов, брать InternalFranchName из Config  или из куки
            // httpContext.Request.Cookies["InternalFranchName"] == null ? ConfigurationManager.AppSettings["InternalFranchName"] : httpContext.Request.Cookies["InternalFranchName"].Value;
        }

        protected SiteContext(HttpContext httpContext, CustomPrincipal user)
        {
            _httpContext = httpContext;
            _user = user;
        }

        public abstract ClientData CurrentClient
        {
            get;
        }

        public abstract string UserDisplayName
        {
            get;
        }

        public virtual ShoppingCartTotals CurrentClientTotals
        {
            get
            {
                if (_currentClientTotals == null)
                    _currentClientTotals = CurrentClient.Cart.GetTotals();
                return _currentClientTotals;
            }
        }

        public bool IsAnonymous
        {
            get { return _user == null; }
        }

        //used for BEFORE attach SiteContext (in catalog routing system)
        public static string _internalFranchName;

        public static bool isFranchNameAttached() 
        {
            return !string.IsNullOrEmpty(_internalFranchName);
        }
		/// <summary>
		/// Внутреннее имя франчайзи (по сути его регион)
		/// </summary>
        public string InternalFranchName { 
            get {
                    try
                    {
                        //TODO: Реализовать проверку на анонимно IsAnonymous и если авторизован, то название франча брать из данных пользователя
                        if (SiteContext.Current.IsAnonymous)
                        {
                            if (HttpContext.Current.Session["InternalFranchName"] != null)
                                return (string)HttpContext.Current.Session["InternalFranchName"];
                            else
                                return isFranchNameAttached() ? _internalFranchName : ConfigurationManager.AppSettings["InternalFranchName"];
                        }
                        else
                        {
                            return SiteContext.Current.User.InternalFranchName;
                        }
                    }
                    catch
                    {
                        return ConfigurationManager.AppSettings["InternalFranchName"];
                    }
                } 
            set { } 
        }

        public CustomPrincipal User
        {
            get { return _user; }
        }

        protected HttpContext _HttpContext
        {
            get { return _httpContext; }
        }

        internal void InvalidateTotals()
        {
            _currentClientTotals = null;
        }

        const string _currencyCookieName = "__SiteContextCurrencyCode";

        private string _currencyCode;

        public string CurrencyCode
        {
            get 
            {
                if (_currencyCode == null)
                    _currencyCode = GetCurrencyCode();
                return !string.IsNullOrEmpty(_currencyCode) ? 
                _currencyCode :
                CurrencyRate.RurRate.CurrencyCode;
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("CurrencyCode cannot be empty", "value");
                _currencyCode = value;
                SetCurrencyCode(value);
            }
		}

		#region === старый вариант ===
		///// <summary>
		///// Текущая локаль
		///// </summary>
		//public string CurrentCulture
		//{
		//    get
		//    {
		//        if (_HttpContext.Request.Cookies["CurrentCulture"] == null)
		//        {
		//            HttpCookie coockie = new HttpCookie("CurrentCulture");
		//            coockie.Value = "ru-RU"; //по умолчанию русская локаль
		//            coockie.Expires = DateTime.Now.AddMonths(1);
		//            _HttpContext.Response.Cookies.Add(coockie);
		//        }
		//        return _HttpContext.Request.Cookies["CurrentCulture"].Value;
		//    }
		//    set
		//    {
		//        //удаляем "старый" cookie
		//        _HttpContext.Request.Cookies["CurrentCulture"].Expires = DateTime.Now.AddYears(-1);
		//        //создаем "новый" cookie
		//        HttpCookie coockie = new HttpCookie("CurrentCulture");
		//        coockie.Value = value;
		//        coockie.Expires = DateTime.Now.AddMonths(1);
		//        _HttpContext.Response.Cookies.Add(coockie);
		//    }
		//}
		#endregion

		/// <summary>
		/// Текущая локаль
		/// </summary>
		public static string CurrentCulture
		{
			get
			{
                string currentCulture = ConfigurationManager.AppSettings["CurrentCulture"];
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["UseLocalization"]))
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["CurrentCulture"];
                    if (cookie != null) currentCulture = cookie.Value;
                }

                return currentCulture;
			}
			set
			{
                HttpCookie cookie = HttpContext.Current.Request.Cookies["CurrentCulture"];
                if (cookie == null) cookie = new HttpCookie("CurrentCulture");
				cookie.Value = value;
				cookie.Expires = DateTime.Now.AddMonths(1);
				HttpContext.Current.Response.Cookies.Set(cookie);
			}
		}

		protected virtual string GetCurrencyCode()
        {
            var cookie = _HttpContext.Request.Cookies[_currencyCookieName];
            return (cookie != null) ? cookie.Value : null;
        }

        protected virtual void SetCurrencyCode(string currencyCode)
        {
            _HttpContext.Response.Cookies[_currencyCookieName].Value = currencyCode;
        }
    }
}
