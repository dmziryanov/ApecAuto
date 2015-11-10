using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class ClientContactInfo
    {
        [XmlElement("ClientCode")]
        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public int ClientType { get; set; }

        [XmlIgnore]
        public ClientCategory ClientCategory 
        {
            get { return (ClientCategory)(byte)ClientType; }
        }

        [XmlElement("ClientMainContact")]
        public string ContactPerson { get; set; }

        [XmlElement("ClientEmail")]
        public string Email { get; set; }

        [XmlElement("ClientMainPh")]
        public string MainPhone { get; set; }

        [XmlElement("ClientMobPh")]
        public string MobilePhone { get; set; }

        [XmlElement("EmployeeId")]
        public string ManagerId { get; set; }
    }
}
