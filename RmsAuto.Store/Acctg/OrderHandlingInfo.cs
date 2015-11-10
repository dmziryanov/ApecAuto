using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class OrderHandlingInfo
    {
        [XmlElement("line")]
        public OrderLineHandlingInfo[] LineHandlings { get; set; }
    }
}
