using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Acctg
{
    public enum TradingVolume : byte
    {
		//[Text("розница")]
		[Text("retail")]
		Retail = 0,
		//[Text("опт")]
		[Text("wholesale")]
		Wholesale = 1
    }
}
