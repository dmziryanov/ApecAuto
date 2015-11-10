using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Acctg;
using System.Web.UI.WebControls;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Entities
{
    [Serializable]
    public partial class ShoppingCartItem
    {
		/// <summary>
		/// Отображаемый минимальный срок поставки
		/// </summary>
		public int DisplayDeliveryDaysMin
		{
			get { return DeliveryDaysMin > 0 ? DeliveryDaysMin : 1; }
		}

		/// <summary>
		/// Отображаемый максимальный срок поставки
		/// </summary>
		public int DisplayDeliveryDaysMax
		{
			get { return DeliveryDaysMax > 0 ? DeliveryDaysMax : 1; }
		}

        [NonSerialized]
        private SparePartFranch _part;

        [NonSerialized]
        private ShoppingCartItemIssues _issues;
       
        public SparePartFranch SparePart
        {
            get { return _part; }
        }

		[NonSerialized]
		private bool? _isAnalogsNotSupported;

		/// <summary>
		/// Флаг указыващий что поставщик данной детали (свойство SparePart) не поддерживает аналоги (т.е. заказать по данном артикулу можно строго эту деталь)
        /// Если null всегда поддерживает аналоги (т.е. заказать по данном артикулу всегда можно только с аналогами)
		/// </summary>
		public bool? IsAnalogsNotSupported
		{
			get { return _isAnalogsNotSupported; }
		}

        internal void AssignSparePart(SparePartFranch part, RmsAuto.Acctg.ClientGroup clientGroup, decimal personalMarkup)
        {
            _issues = ShoppingCartItemIssues.NoIssues;

            if (part != null)
            {
				if( UnitPrice != part.GetFinalSalePrice( clientGroup, personalMarkup ) )
                    _issues |= ShoppingCartItemIssues.FinalPartPriceChanged;
                
                UpdateQtyIssues( part );
                //if (Qty < part.DefaultOrderQty)
                //    _issues |= ShoppingCartItemIssues.QtyBelowRequiredMinimum;
                //else if (Qty % part.DefaultOrderQty != 0)
                //    _issues |= ShoppingCartItemIssues.QtyMultiplicityViolation;

                //if (part.QtyInStock.GetValueOrDefault() > 0 && Qty > part.QtyInStock)
                //    _issues |= ShoppingCartItemIssues.QtyAboveAvailableInStock;

                // deas 25.05.2011 task4218 запрет заказа товаров по нулевой цене
                if ( UnitPrice == 0 )
                    _issues |= ShoppingCartItemIssues.FinalPartPriceNoSet;
            }
            else
                _issues |= ShoppingCartItemIssues.SparePartDiscontinued;

            _part = part;

            //Здесь не нужно использовать фабрику так как 
            using (var dc = new StoreDataContext())
			{
				try
				{
					var resList = dc.spSelSuppliersWithoutAnalogs().ToList().Select( s => s.SupplierID ).ToList<int>();
                    _isAnalogsNotSupported = resList.Contains( part.SupplierID );

                    //TODO: дописать запрос
                    IEnumerable<int> resList2 = dc.ExecuteQuery<int>("Select [SupplierID] From SuppliersWithOnlyAnalogs");
                    if (resList2.Contains(part.SupplierID)) { _isAnalogsNotSupported = null; };
				}
				catch
				{
					//просто не падаем
				}
				finally
				{
					if (dc.Connection.State == System.Data.ConnectionState.Open)
						dc.Connection.Close();
				}
			}
        }

        // deas 01.03.2011 task2401
        // для расчета ограничений по итоговым данным
        public void UpdateQtyIssues( SparePartFranch part )
        {
            if ( Qty < part.DefaultOrderQty )
                _issues |= ShoppingCartItemIssues.QtyBelowRequiredMinimum;
            else if ( Qty % part.DefaultOrderQty != 0 )
                _issues |= ShoppingCartItemIssues.QtyMultiplicityViolation;

            if ( part.QtyInStock.GetValueOrDefault() > 0 && Qty > part.QtyInStock )
                _issues |= ShoppingCartItemIssues.QtyAboveAvailableInStock;
        }

        public bool Discontinued
        {
            get { return (_issues & ShoppingCartItemIssues.SparePartDiscontinued) != 0; }
        }
       
        public bool PriceChanged
        {
            get { return (_issues & ShoppingCartItemIssues.FinalPartPriceChanged) != 0; }
        }
       
        public bool QtyBelowMinimum
        {
            get { return (_issues & ShoppingCartItemIssues.QtyBelowRequiredMinimum) != 0; }
        }

        public bool QtyMultiplicityViolation
        {
            get { return (_issues & ShoppingCartItemIssues.QtyMultiplicityViolation) != 0; }
        }
               
        public bool QtyAboveAvailableInStock
        {
            get { return (_issues & ShoppingCartItemIssues.QtyAboveAvailableInStock) != 0; }
        }

        public bool FinalPartPriceNoSet
        {
            get { return ( _issues & ShoppingCartItemIssues.FinalPartPriceNoSet ) != 0; }
        }

        public bool HasIssues
        {
            get { return _issues != ShoppingCartItemIssues.NoIssues; }
        }
       
        public Decimal ItemTotal
        {
            get { return _UnitPrice * _Qty; }
        }

    }
}
