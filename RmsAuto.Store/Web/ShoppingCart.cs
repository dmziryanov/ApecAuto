using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using RmsAuto.Common.Data;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    [Serializable()]
    public sealed class ShoppingCart
	{
		public static ShoppingCart GetTemporary()
		{
			return new ShoppingCart( ClientBO.GuestProfile.ClientGroup, ClientBO.GuestProfile.PersonalMarkup );
		}

		/* данный атрибут необходим чтобы сериализация сессии в БД проходила корректно */
		[field:NonSerialized()]
		public event EventHandler<EventArgs> ContentChanged;
		[field:NonSerialized()]
		public event EventHandler<ArticleAddedEventArgs> ArticleAdded;


        /* Так было, убрали, так как корзины сериализуется, и возникается проблема с делегатом
        public event EventHandler<EventArgs> ContentChanged;
        public event EventHandler<ArticleAddedEventArgs> ArticleAdded;

        //Нужно сделать так, чтобы сериализовалось и хранилось  
        /*[NonSerialized()]
        public EventHandler<EventArgs> ContentChanged;
        public EventHandler<ArticleAddedEventArgs> ArticleAdded;*/


		private int _ownerId;
		private string _clientId;
        private RmsAuto.Acctg.ClientGroup _clientGroup;
		private decimal _personalMarkup;
		private IShoppingCartStorage _storage;

        private ShoppingCart(RmsAuto.Acctg.ClientGroup clientGroup, decimal personalMarkup)
		{
			_clientGroup = clientGroup;
			_personalMarkup = personalMarkup;
			_storage = new TemporaryShoppingCartStorage();
		}

        public ShoppingCart(int ownerId, string clientId, RmsAuto.Acctg.ClientGroup clientGroup, decimal personalMarkup)
		{
			if( string.IsNullOrEmpty( clientId ) )
				throw new ArgumentException( "Client id cannot be empty", "clientId" );

			_ownerId = ownerId;
			_clientId = clientId;
			_clientGroup = clientGroup;
			_personalMarkup = personalMarkup;
			_storage = new PersistentShoppingCartStorage( _ownerId, _clientId );
		}

        public RmsAuto.Acctg.ClientGroup ClientGroup
		{
			get { return _clientGroup; }
		}

		public void Add( SparePartPriceKey key, int qty, bool addToOrder )
		{
            // deas 27.04.2011 task3929 добавление в корзине флага отправить в заказ
			if( key == null )
				throw new ArgumentNullException( "key" );
			if( qty <= 0 )
				throw new ArgumentException( "Quantity must be greater than zero", "qty" );

			var part = SparePartsDac.Load( key );
			if( part == null )
                throw new BLException( "К сожалению, данная позиция более не доступна. Попробуйте повторить поисковый запрос снова", true );

            // deas 28.02.2011 task2401
            // добавлено сравнение по ReferenceID
			var items = _storage.LoadItems().ToList();
			var existing = items.SingleOrDefault( i =>
				i.PartNumber == key.PN &&
				i.Manufacturer == key.Mfr &&
				i.SupplierID == key.SupplierId && 
                i.ReferenceID == "");

			if( existing != null )
			{
				existing.Qty += qty;
				//existing.ReferenceID = "";
				existing.UnitPrice = part.GetFinalSalePrice( _clientGroup, _personalMarkup );
			}
			else
			{
				items.Add(
					new ShoppingCartItem
					{
                        AddToOrder = addToOrder,
						Manufacturer = part.Manufacturer,
						PartNumber = part.PartNumber,
						SupplierID = part.SupplierID,
						DeliveryDaysMin = part.DeliveryDaysMin,
						DeliveryDaysMax = part.DeliveryDaysMax,
						PartName = part.PartName,
						PartDescription = part.PartDescription,
						Qty = qty,
						ReferenceID = "",
						UnitPrice = part.GetFinalSalePrice( _clientGroup, _personalMarkup )
					});
			}
			using (var dc = new DCFactory<StoreDataContext>())
            {
                Save(dc.DataContext, items);
			    OnArticleAdded( new ArticleAddedEventArgs( key, qty ) );
            }
		}

		public void AddRange( IEnumerable<ShoppingCartAddItem> items )
		{
			if( items == null )
				throw new ArgumentNullException( "items" );

			using( var dc = new DCFactory<StoreDataContext>())
			{
				var existingItems = LoadItems(dc.DataContext);

				// timeout 100 seconds.
				dc.DataContext.CommandTimeout = 100000;
				dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");

				//foreach (var item in items)
				//{
				//    var part = SparePartsDac.Load(dc, item.Key);
				//    if (part == null)
				//    {
				//        item.PartNotFound = true;
				//        continue;
				//    }

				//    decimal unitPrice = part.GetFinalSalePrice( _clientGroup, _personalMarkup );

				//    if (existingItems.ContainsKey(item.Key))
				//    {
				//        var existing = existingItems[item.Key];
				//        existing.Qty += item.Quantity;
				//        existing.ReferenceID = item.ReferenceID;
				//        existing.UnitPrice = unitPrice;
				//    }
				//    else
				//    {
				//        existingItems.Add(item.Key,
				//            new ShoppingCartItem
				//            {
				//                Manufacturer = part.Manufacturer,
				//                PartNumber = part.PartNumber,
				//                SupplierID = part.SupplierID,
				//                DeliveryDaysMin = part.DeliveryDaysMin,
				//                DeliveryDaysMax = part.DeliveryDaysMax,
				//                PartName = part.PartName,
				//                PartDescription = part.PartDescription,
				//                Qty = item.Quantity,
				//                ReferenceID = item.ReferenceID,
				//                StrictlyThisNumber = item.StrictlyThisNumber,
				//                UnitPrice = unitPrice
				//            });
				//    }
				//}
				//var l = new List<SparePartPriceKey>();
               
				foreach(var item in items)
				{
                    SparePartFranch part = null;

					try
					{
                        // deas 11.05.2011 task3741 Изменено на более универсальный поиск товара
                        if (SiteContext.Current.InternalFranchName == "rmsauto")
                        {
                            part = SparePartsDac.Load(dc.DataContext, new SparePartPriceKey(item.Key.Mfr, item.Key.PN, item.Key.SupplierId));
                        }
                        else
                        {
                            part = SparePartsDacFranch.Load(dc.DataContext, new SparePartPriceKey(item.Key.Mfr, item.Key.PN, item.Key.SupplierId));
                        }
                        //part = sp.Where(
                        //itm =>
                        //itm.Manufacturer ==  && itm.PartNumber ==  &&
                        //itm.SupplierID ==  ).First();

						if(part == null)
						{
							item.PartNotFound = true;
							continue;
						}
					}
					catch( ArgumentNullException )
					{
						item.PartNotFound = true;
						continue;
					}
					
                    catch( InvalidOperationException )
					{
						item.PartNotFound = true;
						continue;
					}

                    // task4957 меняем значение ключа на правильное(как в бд)
                    item.Key = new ShoppingCartKey(part.Manufacturer, part.PartNumber, part.SupplierID, item.ReferenceID);
                    
                    if ( existingItems.ContainsKey( new ShoppingCartKey( part.Manufacturer, part.PartNumber, part.SupplierID, item.ReferenceID ) ) )
					{
                        var existing = existingItems[ item.Key ];
						existing.Qty += item.Quantity;
						existing.ReferenceID = item.ReferenceID;
						existing.UnitPrice = part.GetFinalSalePrice( _clientGroup, _personalMarkup );
					}
					else
					{
                        // deas 28.04.2011 task3929 добавление в корзине флага отправить в заказ
                        existingItems.Add(item.Key,
										  new ShoppingCartItem
										  {
                                              AddToOrder = true,
											  Manufacturer = part.Manufacturer,
											  PartNumber = part.PartNumber,
											  SupplierID = part.SupplierID,
											  DeliveryDaysMin = part.DeliveryDaysMin,
											  DeliveryDaysMax = part.DeliveryDaysMax,
											  PartName = part.PartName,
											  PartDescription = part.PartDescription,
											  Qty = item.Quantity,
											  ReferenceID = item.ReferenceID,
											  StrictlyThisNumber = item.StrictlyThisNumber,
											  UnitPrice = part.GetFinalSalePrice( _clientGroup, _personalMarkup ),
											  //если данное поле не null, то в заказ должна пойти цена и этого поля, а не UnitPrice
											  UnitPriceYesterday = item.priceClientYesterday 
										  } );
					}
				}

				Save( dc.DataContext, existingItems.Values );
			}
		}

		public void Update( IEnumerable<ShoppingCartItem> updates)
		{
			if( updates == null )
				throw new ArgumentNullException( "updates" );

			using( var context = new DCFactory<StoreDataContext>() )
			{
                // deas 28.02.2011 task2401
                // изменен словарь поиска для обновления ReferenceID
				//var items = LoadItems( context );
                var items = _storage.LoadItems( context.DataContext ).ToDictionary<ShoppingCartItem, int>( i => i.ItemID );

				// timeout 100 seconds.
				context.DataContext.CommandTimeout = 100000;


				var spareParts = SparePartsDac.LoadMassive(context.DataContext,
					updates.Select( u => new SparePartPriceKey( u.Manufacturer, u.PartNumber, u.SupplierID ) ).Distinct()
					).ToDictionary( p => new SparePartPriceKey( p.Manufacturer, p.PartNumber, p.SupplierID ) );

				foreach( var upd in updates )
				{
                    // deas 28.02.2011 task2401
                    // добавлен дополнительный ключ для цен
					var keyForSpare = new SparePartPriceKey( upd.Manufacturer, upd.PartNumber, upd.SupplierID );
                    //var key = new ShoppingCartKey( upd.Manufacturer, upd.PartNumber, upd.SupplierID, upd.ReferenceID );
					if( items.ContainsKey( upd.ItemID ) )
					{
                        var item = items[upd.ItemID];

                        // deas 28.04.2011 task3929 добавление в корзине флага отправить в заказ
                        item.AddToOrder = upd.AddToOrder;
						item.Qty = upd.Qty;
						item.ReferenceID = upd.ReferenceID;
						item.StrictlyThisNumber = upd.StrictlyThisNumber;
						item.VinCheckupDataID = upd.VinCheckupDataID;
						item.ItemNotes = upd.ItemNotes;

						SparePartFranch part;
                        if ( spareParts.TryGetValue( keyForSpare, out part ) )
							item.UnitPrice = part.GetFinalSalePrice( _clientGroup, _personalMarkup );
					}
				}

                // deas 28.02.2011 task2401
                // поиск и удаление повторов
                // раньше проверялось при добавлении в корзину
                foreach ( int dKey in items.Keys )
                {
                    ShoppingCartItem sci = items[dKey];
                    if ( sci.Qty > 0 )
                    {
                        var duplicateItem = from i in items.Values
                                            where i.Manufacturer == sci.Manufacturer
                                                && i.PartNumber == sci.PartNumber
                                                && i.SupplierID == sci.SupplierID
                                                && i.ReferenceID == sci.ReferenceID
                                                && i.ItemID != sci.ItemID
                                            select i;
                        foreach ( ShoppingCartItem delI in duplicateItem )
                        {
                            sci.Qty += delI.Qty;
                            delI.Qty = 0;
                        }
                    }
                }
				//var sparePartKeys = updates.Where(
				//    u => items.ContainsKey( new SparePartPriceKey( upd.Manufacturer, upd.PartNumber, upd.SupplierID ) )
				//    );

				//var sp = SparePartsDac.LoadMassive( context, l );
				//foreach( var el in sp )
				//{
				//    var key = new SparePartPriceKey( el.Manufacturer, el.PartNumber, el.SupplierID );
				//    var item = items[ key ];
				//    var upd = updates.Where( itm => itm.Manufacturer == el.Manufacturer && itm.PartNumber == el.PartNumber && itm.SupplierID == el.SupplierID ).FirstOrDefault();
				//    item.Qty = upd.Qty;
				//    item.ReferenceID = upd.ReferenceID;
				//    item.StrictlyThisNumber = upd.StrictlyThisNumber;
				//    item.VinCheckupDataID = upd.VinCheckupDataID;
				//    item.ItemNotes = upd.ItemNotes;
				//    item.UnitPrice = el.GetFinalSalePrice( _clientGroup, _personalMarkup );
				//}

				Save( context.DataContext, items.Values );
			}
		}

		public int PlaceOrder(
			PaymentMethod paymentMethod,
			string shippingAddress,
			string orderNotes,
			string custOrderNum,
			string employeeId,
			string storeNumber )
		{
			if( string.IsNullOrEmpty( _storage.ClientId ) )
				throw new InvalidOperationException( "Anonymous shopping cart cannot be used placing an order" );

            var orderNum = OrderBO.CreateOrder(
                _storage.OwnerId,
                _storage.ClientId,
                storeNumber,
                employeeId,
                GetItems().Where( t => t.AddToOrder == true ),
                paymentMethod,
                shippingAddress,
                orderNotes,
                custOrderNum );

			OnContentChanged( EventArgs.Empty );

			return orderNum;
		}
		/* 
		[Obsolete]      
		public decimal Total
		{
			get { return _storage.LoadItems().Sum(i => i.UnitPrice * i.Qty); }
		}

		[Obsolete]
		public int PartsCount
		{
			get { return _storage.LoadItems().Sum(i => i.Qty); }
		}

		[Obsolete]
		public int ItemsCount
		{
			get { return _storage.LoadItems().Count(); }
		}
		*/

		public ShoppingCartTotals GetTotals()
		{
			var items = _storage.LoadItems();
			return new ShoppingCartTotals
			{
				Total = items.Sum( i => i.UnitPrice * i.Qty ),
				PartsCount = items.Sum( i => i.Qty ),
				ItemsCount = items.Count()
			};
		}

        public ShoppingCartTotals GetAddToOrderTotals()
        {
            var items = _storage.LoadItems().Where( t => t.AddToOrder == true );
            return new ShoppingCartTotals
            {
                Total = items.Sum( i => i.UnitPrice * i.Qty ),
                PartsCount = items.Sum( i => i.Qty ),
                ItemsCount = items.Count()
            };
        }

		public ShoppingCartTotals GetTotals( StoreDataContext context )
		{
			var items = _storage.LoadItems( context );
			return new ShoppingCartTotals
			{
				Total = items.Sum( i => i.UnitPrice * i.Qty ),
				PartsCount = items.Sum( i => i.Qty ),
				ItemsCount = items.Count()
			};
		}

		public string GetVersion()
		{
			return GetVersionInternal( _storage.LoadItems() );
		}

		public IEnumerable<ShoppingCartItem> GetItems()
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				dc.DataContext.ExecuteCommand( "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED" );
				// timeout 100 seconds.
				dc.DataContext.CommandTimeout = 100000;
				//return _storage.LoadItems(dc)
				//    .Each(item =>
				//   item.AssignSparePart(
				//   SparePartsDac.Load(dc, new SparePartPriceKey(item.Manufacturer, item.PartNumber, item.SupplierID)),
				//   _clientGroup,
				//   _personalMarkup));
				List<ShoppingCartItem> st = _storage.LoadItems( dc.DataContext ).ToList();
				var sp = SparePartsDac.LoadMassive( dc.DataContext, st );
				foreach( var item in st )
				{
					item.AssignSparePart( sp.Where(
						itm =>
						itm.Manufacturer == item.Manufacturer && itm.PartNumber == item.PartNumber &&
						itm.SupplierID == item.SupplierID ).FirstOrDefault(), _clientGroup, _personalMarkup );
				}
				return st;
			}
		}

		public void Merge( ShoppingCart other )
		{
			if( other == null )
				throw new ArgumentNullException( "other" );

            using (var context = new DCFactory<StoreDataContext>())
			{
				if( other.GetTotals( context.DataContext ).ItemsCount == 0 )
					return;
				var items = LoadItems( context.DataContext );

				foreach( var otherItem in other._storage.LoadItems( context.DataContext ) )
				{
                    var key = new ShoppingCartKey(
						otherItem.Manufacturer,
						otherItem.PartNumber,
						otherItem.SupplierID,
                        otherItem.ReferenceID );
					if( items.ContainsKey( key ) )
					{
						items[ key ].Qty += otherItem.Qty;
						//items[ key ].ReferenceID = otherItem.ReferenceID;
					}
					else
					{
                        // deas 28.04.2011 task3929 добавление в корзине флага отправить в заказ
                        items.Add( key,
							 new ShoppingCartItem
							 {
                                 AddToOrder = otherItem.AddToOrder,
								 Manufacturer = otherItem.Manufacturer,
								 PartNumber = otherItem.PartNumber,
								 SupplierID = otherItem.SupplierID,
								 DeliveryDaysMin = otherItem.DeliveryDaysMin,
								 DeliveryDaysMax = otherItem.DeliveryDaysMax,
								 PartName = otherItem.PartName,
								 PartDescription = otherItem.PartDescription,
								 Qty = otherItem.Qty,
								 ReferenceID = otherItem.ReferenceID,
								 UnitPrice = otherItem.UnitPrice
							 } );
					}
				}
				Save( context.DataContext, items.Values );
				other.ClearItems( context.DataContext );
			}
		}

        private Dictionary<ShoppingCartKey, ShoppingCartItem> LoadItems( StoreDataContext context )
		{
            // deas 28.02.2011 task2401
            // Изменен ключ корзины для учета ReferenceID
			return _storage
				.LoadItems( context )
                .ToDictionary<ShoppingCartItem, ShoppingCartKey>(
                i => new ShoppingCartKey( i.Manufacturer, i.PartNumber, i.SupplierID, i.ReferenceID ) );
		}

		private void ClearItems( StoreDataContext context )
		{
			_storage.ClearItems( context );
			OnContentChanged( EventArgs.Empty );
		}

		private void Save( StoreDataContext context, IEnumerable<ShoppingCartItem> items )
		{
			_storage.SaveItems( context, items );
			OnContentChanged( EventArgs.Empty );
		}

		private string GetVersionInternal( IEnumerable<ShoppingCartItem> items )
		{
			var version = new StringBuilder();
			foreach( var item in items.Where( i => i.ItemVersion != null ).OrderBy( i => i.ItemID ) )
			{
				foreach( var b in item.ItemVersion.ToArray().SkipWhile( b => b == 0 ) )
					version.Append( b );
			}
			return version.ToString();
		}

		private void OnContentChanged( EventArgs e )
		{
			if( ContentChanged != null )
				ContentChanged( this, e );
		}

		private void OnArticleAdded( ArticleAddedEventArgs e )
		{
			if( ArticleAdded != null )
				ArticleAdded( this, e );
		}
	}
}
