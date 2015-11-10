using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    [Flags]
    public enum ShoppingCartItemIssues
    {
        NoIssues = 0,
        SparePartDiscontinued = 1,
        QtyBelowRequiredMinimum = 2,
        FinalPartPriceChanged = 4,
        QtyAboveAvailableInStock = 8,
        QtyMultiplicityViolation = 16,
        FinalPartPriceNoSet = 32
    }
    // deas 25.05.2011 task4218 запрет заказа товаров по нулевой цене
}
