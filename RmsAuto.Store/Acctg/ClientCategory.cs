using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Acctg
{
    public enum ClientCategory : byte
    {
		//[Text("юридическое лицо")]
		[Text("legal entity")]
		Legal = 0,
        [Text("физическое лицо")] 
		Physical = 1,
        [Text( "Индивидуальный предприниматель" )]
		PhysicalIP = 2
    }
}
