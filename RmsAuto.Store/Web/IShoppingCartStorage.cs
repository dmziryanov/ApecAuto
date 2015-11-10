using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    interface IShoppingCartStorage
    {
        int OwnerId { get; }
        string ClientId { get; }
        IEnumerable<ShoppingCartItem> LoadItems();
        IEnumerable<ShoppingCartItem> LoadItems(StoreDataContext context);
        void SaveItems(IEnumerable<ShoppingCartItem> items);
        void SaveItems(StoreDataContext context, IEnumerable<ShoppingCartItem> items);
        void ClearItems();
        void ClearItems(StoreDataContext context);
    }
}
