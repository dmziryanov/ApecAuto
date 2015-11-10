using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class OrderLineChangeRequest
    {
        [XmlElement("OrderLineHW")]
        public int AcctgOrderLineId { get; set; }

        [XmlElement("UserRequest")]
        public int AcceptOrderLineChange { get; set; }
    }
}
