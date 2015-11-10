using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Acctg
{
    public enum OrderLineChangeReaction : byte
    {
        Accept = 1,
        Decline = 2
    }
}
