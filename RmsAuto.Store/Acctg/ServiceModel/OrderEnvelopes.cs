using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg.Entities
{
    public class SendOrderEnvelope : Envelope
    {
        [XmlElement("ClientCode")]
        public string ClientId { get; set; }

        public OrderInfo Order { get; set; }
    }

    public class SendOrderResultsEnvelope : Envelope
    {
       public OrderHandlingInfo Order { get; set; }
    }

    public class GetChangesResultsEnvelope : Envelope
    {
        [XmlElement("line")]
        public OrderLineInfo[] OrderLines { get; set; }

        [XmlElement("ClientRecord")]
        public ClientContactInfo[] AffectedClients { get; set; }
    }

    public class PostChangeRequestEnvelope : Envelope
    {
        [XmlElement("Line")]
        public OrderLineChangeRequest ChangeRequest { get; set; }
    }

    public class AcknowledgementEnvelope : Envelope
    {
        [XmlElement("RequestReceive")]
        public string Acknowledgement { get; set; } 
    }
}
