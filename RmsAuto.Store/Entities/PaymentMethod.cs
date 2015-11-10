using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Common.Misc;
using RmsAuto.Store.Web.Resource;

namespace RmsAuto.Store.Entities
{
    public enum PaymentMethod : byte
    {
        [Text("Cash")]
        [LocalizedDescription("PaymentMethod_Cash", typeof(EnumResource))]
        Cash = 1,

        [Text("Clearing")]
        [LocalizedDescription("PaymentMethod_Clearing", typeof(EnumResource))]
        Clearing = 2
		//[Text("личный кабинет QIWI")] Qiwi = 3,        
		//[Text("кредитная карта")] Card = 4
    }
}
