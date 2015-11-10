using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using RmsAuto.Store.Data;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Web;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace RmsAuto.Store.Entities
{



    [Serializable()]
    public partial class OrderLineDTO
    { 

    }


    [Serializable()]
    public partial class OrderLine
    {
        private SparePartFranch _part = null;

        public SparePartFranch Part
        {
            get
            {
                if (_part == null)
                {
                    if (SiteContext.Current.InternalFranchName == "rmsauto")
                    { _part = SparePartsDac.Load(new SparePartPriceKey(_Manufacturer, _PartNumber, _SupplierID)); }
                    else
                    { _part = SparePartsDacFranch.Load(new SparePartPriceKey(_Manufacturer, _PartNumber, _SupplierID)); }
                }
                return _part;
            }
        }

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

        public decimal Total
        {
            get { return _UnitPrice * _Qty; }
        }

        public OrderLineStatusChange LastStatusChange
        {
            get
            {
                return _OrderLineStatusChanges.OrderBy(olsc => olsc.StatusChangeTime).LastOrDefault();
            }
        }

        public Boolean IsParentForPartNumberTransition(Int32 orderLineID, OrderLine childOrderLine)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                //List<SuperTypeTurn> list = (from db in entity.SuperTypeTurn.Include("TypeTurn").Include("TypeTurn.Turn")
                //                            from typeTurn in db.TypeTurn
                //                            from turn in typeTurn.Turn
                //                            orderby db.SortOrder, typeTurn.SortOrder, turn.SortOrder
                //                            select db).ToList();

                //var el = from o in dc.OrderLines
                //         where o.OrderLineID == orderLineID
                //         select o;
                //IQueryable<EntitySet<OrderLine>> childOrderLines = from o in dc.OrderLines
                //                      select o.ChildOrderLines;

                // Нашли все дочерние
                //var childOrderLines = from o in dc.OrderLines
                //                      where o.ParentOrderLine.OrderLineID == orderLineID
                //                      select o;

                // deas 23.05.2011 task4130 Ускорение работы со статусами
                //var stPartNumberTransition = OrderLineStatusUtil.StatusByte(dc, "PartNumberTransition");
                var stPartNumberTransition = OrderLineStatusUtil.StatusByte("PartNumberTransition");

                //var el = from o in childOrderLines
                //         where o.CurrentStatus == stPartNumberTransition
                //         select o;
                if (childOrderLine.CurrentStatus == stPartNumberTransition)
                    return true;
                else
                    return false;
            }
        }

        ///// <summary>
        ///// Т.к. при передаче статуса строки из 1С информация об этом поле может терятся
        ///// выносим хранение этого поля в отдельную таблицу dbo.OrderLinesProcessed и при
        ///// необходимости извлекаем его оттуда
        ///// </summary>
        //public byte Processed
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (!AcctgOrderLineID.HasValue)
        //                return 0;
        //            else
        //            {
        //                using (var dc = new StoreDataContext())
        //                {
        //                    var el = dc.OrderLinesProcesseds.Where( p => p.AcctgOrderLineID == _AcctgOrderLineID.Value ).SingleOrDefault();

        //                    return el.Processed;
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            //никогда не падаем ибо это все-таки строка заказа
        //            return 0;
        //        }
        //    }
        //}

        public bool CanChangeLiteStatus(int StatusID)
        {
            if ((OrderLineStatusUtil.LowBoundary < StatusID && OrderLineStatusUtil.HiBoundary > StatusID && OrderLineStatusUtil._allStatuses.Where(x => x.OrderLineStatusID == StatusID).Select(x => !x.IsFinal).FirstOrDefault()) ||
            (CurrentStatus == OrderLineStatusUtil.FinalRMSState))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChangeLiteStatus(byte StatusID)
        {
            //Такая вот логика изменения статусов. Пытаемся захардкодить граф переходов руками :)
            if (CanChangeLiteStatus(StatusID))
            {
                this.CurrentStatus = StatusID;
                this._CurrentStatusDate = DateTime.Now;
            }
        }


    }
}
