using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
    public enum SecurityRole : byte
    {
        [Text("клиент")] Client = 0,
        [Text("оператор")] Manager = 1
    }
}
