using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
    public enum PaymentStatus
    {
        [Text("не оплачен")] None,
        [Text("оплачен полностью")] Full,
        [Text("оплачен частично")] Partial
    }
}
