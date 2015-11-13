using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web
{
    [Serializable()]
    class PersistentShoppingCartStorage : IShoppingCartStorage
    {
        private int _ownerId;
        private string _clientId;

        public PersistentShoppingCartStorage(int ownerId, string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("Client' Id cannot be empty");
            _ownerId = ownerId;
            _clientId = clientId;
        }

        #region IShoppingCartStorage Members

        public int OwnerId { get { return _ownerId; } }

        public string ClientId { get { return _clientId; } }

        private static Func<StoreDataContext, int, string, IQueryable<ShoppingCartItem>>
                 _selectItemsQuery =
                 CompiledQuery.Compile(
                 (StoreDataContext context, int owner, string client) =>
                    from item in context.ShoppingCartItems
                    where item.OwnerID == owner && item.ClientID == client
                    select item);


        public IEnumerable<ShoppingCartItem> LoadItems()
        {
            using (var context = new DCFactory<StoreDataContext>())
            {
                return LoadItems(context.DataContext);
            }
        }

        public IEnumerable<ShoppingCartItem> LoadItems(StoreDataContext context)
        {
            return _selectItemsQuery(context, _ownerId, _clientId).ToArray();
        }

        public void SaveItems( StoreDataContext context, IEnumerable<ShoppingCartItem> items )
        {
            //using (var context = new StoreDataContext())
            //{
            foreach ( var item in items )
            {
                // deas 28.02.2011 task2401
                // изменено сравнение на ItemID для хранения нескольких строк с разными ReferenceID
                var existing =
                    context.ShoppingCartItems.SingleOrDefault(
                    i => i.ItemID == item.ItemID );
                //i.OwnerID == _ownerId &&
                //i.ClientID == _clientId &&
                //i.Manufacturer == item.Manufacturer &&
                //i.PartNumber == item.PartNumber &&
                //i.SupplierID == item.SupplierID );
                if ( existing != null )
                {
                    if ( item.Qty == 0 )
                        context.ShoppingCartItems.DeleteOnSubmit( existing );
                    else
                    {
                        existing.UnitPrice = item.UnitPrice;
                        existing.Qty = item.Qty;
                        existing.ReferenceID = item.ReferenceID;
                        existing.VinCheckupDataID = item.VinCheckupDataID;
                        existing.StrictlyThisNumber = item.StrictlyThisNumber;
                        existing.ItemNotes = item.ItemNotes;
                    }
                }
                else
                {
                    // deas 28.04.2011 task3929 добавление в корзине флага отправить в заказ
                    context.ShoppingCartItems.InsertOnSubmit(
                        new ShoppingCartItem
                        {
                            ClientID = _clientId,
                            OwnerID = _ownerId,
                            Manufacturer = item.Manufacturer,
                            PartNumber = item.PartNumber,
                            SupplierID = item.SupplierID,
                            DeliveryDaysMin = item.DeliveryDaysMin,
                            DeliveryDaysMax = item.DeliveryDaysMax,
                            PartName = item.PartName,
                            PartDescription = item.PartDescription,
                            StrictlyThisNumber = item.StrictlyThisNumber,
                            Qty = item.Qty,
                            ReferenceID = item.ReferenceID,
                            UnitPrice = item.UnitPrice,
                            AddToOrder = item.AddToOrder,
							UnitPriceYesterday = item.UnitPriceYesterday
                        }
                        );
                }
            }
            context.SubmitChanges();
            //}
        }

        public void SaveItems(IEnumerable<ShoppingCartItem> items)
        {
            using (var context = new DCFactory<StoreDataContext>())
            {
                SaveItems(context.DataContext, items);
            }
        }

        public void ClearItems(StoreDataContext context)
        {
            context
                .ShoppingCartItems
                .DeleteAllOnSubmit(
                context
                .ShoppingCartItems
                .Where(i => 
                    i.OwnerID == _ownerId &&
                    i.ClientID == _clientId));
            context.SubmitChanges();
        }

        public void ClearItems()
        {
            using (var context = new DCFactory<StoreDataContext>())
            {
                ClearItems(context.DataContext);
            }
        }
        
        #endregion
    }
}
