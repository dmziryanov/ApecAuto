using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
    public enum ShippingMethod : byte
    {
        [Text("Store Pickup")]
        StorePickup = 1,
        [Text("Courier Delivery")]
        CourierDelivery = 2
    }
}
