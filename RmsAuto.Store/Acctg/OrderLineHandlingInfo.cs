using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class OrderLineHandlingInfo
    {
        [XmlElement("OrderLineWeb")]
        public int WebOrderLineId { get; set; }
        
        [XmlElement("OrderLineHW")]
        public int AcctgOrderLineId { get; set; }
        
        [XmlElement("LineStatus")]
        public string OrderLineStatus { get; set; }
    }
}
