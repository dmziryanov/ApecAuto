using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg.Entities
{
    public class SellerInfoEnvelope : Envelope
    {
        [XmlElement("Record")]
        public SellerInfo SellerInfo { get; set; }
    }
}
