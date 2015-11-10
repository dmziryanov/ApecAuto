using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
    [Serializable()]
    public enum ClientStatus
	{
        [Text("Site account is not created")]
		Offline = 0,
        		
		[Text("Site account is created")]
		Online = 1 
	}
}
