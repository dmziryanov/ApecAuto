using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Store.Configuration;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Entities
{
    public partial class Order
    {
        //private OrderStatus? _status;
        
        public string ShippingAddress { get; set; }

        public string ClientName { get; set; }

        public decimal ClientSaldo { get; set; }

        public decimal ActiveOrdersSum { get; set; }

        public decimal ActiveOrdersSumToDate { get; set; }

        public decimal ClientSaldoToDate { get; set; }

        public decimal PrepaymentPercent { get; set; }

        private int? _PaymentLimit;

        public int? PaymentLimit { get { return _PaymentLimit < 0 ? int.MaxValue : _PaymentLimit; } set { _PaymentLimit = value; } }

        //public decimal Total
        //{
        //    get
        //    {
        //        return _OrderLines.AsQueryable().Where( OrderBO.TotalStatusExpression ).Sum( l => l.Total );
        //    }
        //}

        //public OrderStatus Status
        //{
        //    get
        //    {
        //        if ( !_status.HasValue )
        //        {
        //            if ( _OrderLines.Count == 0 )
        //                _status = OrderStatus.New;
        //            else
        //            {
        //                var stNew = OrderLineStatusUtil.StatusByte( "New" );
        //                bool allNew = _OrderLines.All( l => l.CurrentStatus == stNew );

        //                bool allFinal = _OrderLines.AsQueryable().All( OrderBO.FinalStatusExpression );

        //                if ( allNew )
        //                    _status = OrderStatus.New;
        //                else if ( allFinal )
        //                    _status = OrderStatus.Completed;
        //                else
        //                    _status = OrderStatus.Processing;
        //            }
        //        }
        //        return _status.Value;
        //    }
        //}

        //public DateTime? CompletedDate
        //{
        //    get
        //    {
        //        if( Status == OrderStatus.Completed )
        //        {
        //            return OrderLines.Max( l => l.CurrentStatusDate );
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
    }
}
