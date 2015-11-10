using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

using RmsAuto.Common.Misc;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    class TemporaryShoppingCartStorage : IShoppingCartStorage
    {
        private const string _CookieName = ".SHOPPING_CART";

        private ShoppingCartItem[] _innerItems;
        private LosFormatter _formatter = new LosFormatter();

        public TemporaryShoppingCartStorage()
        {
            
        }

        private void Initialize()
        {
            if (_innerItems == null)
            {
                _innerItems = HttpContext.Current.Session[_CookieName] != null 
                    && !string.IsNullOrEmpty( HttpContext.Current.Session[_CookieName].ToString() ) ?
                    DeserializeItems( HttpContext.Current.Session[_CookieName].ToString() ) :
                    new ShoppingCartItem[0];
                // deas 25.02.2011 изменено с cookies на сессию, т.к. cookies ограничен по размеру
                //var cookie = HttpContext.Current.Request.Cookies[_CookieName];
                //_innerItems = cookie != null && !string.IsNullOrEmpty(cookie.Value) ?
                //    DeserializeItems(cookie.Value) : 
                //    new ShoppingCartItem[0];
            }
        }
        
        #region IShoppingCartStorage Members

        public int OwnerId { get { return 0; } }

        public string ClientId { get { return string.Empty; } }

        public IEnumerable<ShoppingCartItem> LoadItems(StoreDataContext context)
        {
            return LoadItems();
        }

        public IEnumerable<ShoppingCartItem> LoadItems()
        {
            Initialize();
            return _innerItems;            
        }

        public void SaveItems(StoreDataContext context, IEnumerable<ShoppingCartItem> items)
        {
            SaveItems(items);
        }

        public void SaveItems(IEnumerable<ShoppingCartItem> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            _innerItems = items.Where(i => i.Qty > 0).ToArray();
            // deas 28.02.2011 task2401
            // для расстановки уникальных кодов в корзине не зарегистрированных пользователей.
            if ( _innerItems.Count() > 0 )
            {
                int maxCount = _innerItems.Max( t => t.ItemID );
                foreach ( var oneI in _innerItems.Where( t => t.ItemID == 0 ) )
                {
                    maxCount++;
                    oneI.ItemID = maxCount;
                }
            }
            HttpContext.Current.Session[_CookieName] = SerializeItems(_innerItems);
            // deas 25.02.2011 изменено с cookies на сессию, т.к. cookies ограничен по размеру
            //HttpContext.Current.Response.Cookies.Set(
            //    new HttpCookie(_CookieName)
            //    {
            //        Expires = DateTime.MaxValue,
            //        Value = SerializeItems(_innerItems)
            //    });
        }

        public void ClearItems(StoreDataContext context)
        {
            ClearItems();
        }

        public void ClearItems()
        {
            _innerItems = new ShoppingCartItem[0];
            HttpContext.Current.Session[_CookieName] = null;
            // deas 25.02.2011 изменено с cookies на сессию, т.к. cookies ограничен по размеру
            //HttpContext.Current.Response.Cookies.Set(
            //    new HttpCookie(_CookieName)
            //    {
            //        Expires = DateTime.MaxValue,
            //        Value = SerializeItems(_innerItems)
            //    });
        }

        #endregion

        private ShoppingCartItem[] DeserializeItems(string value)
        {
            return (ShoppingCartItem[])_formatter.Deserialize(value);
        }

        private string SerializeItems(ShoppingCartItem[] items)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                _formatter.Serialize(writer, items);
            }
            return sb.ToString();
        }
    }
}
