using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Common.Linq;

namespace RmsAuto.Store.Entities
{
    //[Serializable]
    //public partial class SparePart
    //{
    //    /// <summary>
    //    /// Отображаемый минимальный срок поставки
    //    /// </summary>
    //    public int DisplayDeliveryDaysMin
    //    {
    //        get { return DeliveryDaysMin > 0 ? DeliveryDaysMin : 1; }
    //    }

    //    /// <summary>
    //    /// Отображаемый максимальный срок поставки
    //    /// </summary>
    //    public int DisplayDeliveryDaysMax
    //    {
    //        get { return DeliveryDaysMax > 0 ? DeliveryDaysMax : 1; }
    //    }

    //    /// <summary>
    //    /// Цена поставщика * Скидка поставщика + Константа
    //    /// </summary>
    //    public decimal SupplierPriceWithMarkup
    //    {
    //        get { return InitialPrice * CorrectionFactor + _PriceConstantTerm.GetValueOrDefault(); }
    //    }

    //    public int DefaultOrderQty
    //    {
    //        get { return MinOrderQty ?? 1; }
    //    }

    //    public decimal GetFinalSalePrice(RmsAuto.Acctg.ClientGroup clientGroup, decimal personalMarkup)
    //    {
    //        var clientGroupPrice = InitialPrice * CorrectionFactor * CorrectionFactor39 * GetCustomFactor( clientGroup ) + _PriceConstantTerm.GetValueOrDefault();
    //        var finalPrice = clientGroupPrice * ( 100 + personalMarkup ) / 100;
    //        return Math.Round(finalPrice,2); //Math.Round(x, 2) - since coefs have 4-digits accuracy
    //    }

    //    public decimal GetCustomFactor(RmsAuto.Acctg.ClientGroup clientGroup)
    //    {
    //        switch( clientGroup )
    //        {
    //            case RmsAuto.Acctg.ClientGroup.Group1: return CustomFactor1;
    //            case RmsAuto.Acctg.ClientGroup.Group2: return CustomFactor2;
    //            case RmsAuto.Acctg.ClientGroup.Group3: return CustomFactor3;
    //            case RmsAuto.Acctg.ClientGroup.Group4: return CustomFactor4;
    //            case RmsAuto.Acctg.ClientGroup.Group5: return CustomFactor5;
    //            case RmsAuto.Acctg.ClientGroup.Group6: return CustomFactor6;
    //            case RmsAuto.Acctg.ClientGroup.Group7: return CustomFactor7;
    //            case RmsAuto.Acctg.ClientGroup.Group8: return CustomFactor8;
    //            case RmsAuto.Acctg.ClientGroup.Group9: return CustomFactor9;
    //            case RmsAuto.Acctg.ClientGroup.Group10: return CustomFactor10;
    //            case RmsAuto.Acctg.ClientGroup.Group11: return CustomFactor11;
    //            case RmsAuto.Acctg.ClientGroup.Group12: return CustomFactor12;
    //            case RmsAuto.Acctg.ClientGroup.Group13: return CustomFactor13;
    //            case RmsAuto.Acctg.ClientGroup.Group14: return CustomFactor14;
    //            case RmsAuto.Acctg.ClientGroup.Group15: return CustomFactor15;
    //            case RmsAuto.Acctg.ClientGroup.Group16: return CustomFactor16;
    //            case RmsAuto.Acctg.ClientGroup.Group17: return CustomFactor17;
    //            case RmsAuto.Acctg.ClientGroup.Group18: return CustomFactor18;
    //            case RmsAuto.Acctg.ClientGroup.Group19: return CustomFactor19;
    //            case RmsAuto.Acctg.ClientGroup.Group20: return CustomFactor20;
    //            case RmsAuto.Acctg.ClientGroup.Group21: return CustomFactor21;
    //            case RmsAuto.Acctg.ClientGroup.Group22: return CustomFactor22;
    //            case RmsAuto.Acctg.ClientGroup.Group23: return CustomFactor23;
    //            case RmsAuto.Acctg.ClientGroup.Group24: return CustomFactor24;
    //            case RmsAuto.Acctg.ClientGroup.Group25: return CustomFactor25;
    //            default: throw new IndexOutOfRangeException();
    //        }
    //    }

    //    public RmsAuto.Common.Data.PartKey PartKey
    //    {
    //        get
    //        {
    //            return new RmsAuto.Common.Data.PartKey( this.Manufacturer, this.PartNumber );
    //        }
    //    }

    //}
}
