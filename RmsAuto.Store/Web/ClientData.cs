using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Store.Acctg;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    [Serializable()]
    public class ClientData
    {
        public ClientProfile Profile { get; set; }
		public ClientStatus Status { get; internal set; }
        public ShoppingCart Cart { get; internal set; }
        public bool IsGuest { get; internal set; }
	}
}
