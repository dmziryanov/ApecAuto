using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    [Serializable()]
    public class BriefClientInfo
    {
		public int UserID { get; set; }

        [XmlElement("ClientCode")] 
        public string ClientID { get; set; }

        public string ClientName { get; set; }

        [XmlElement("ClientMainPh")] 
        public string MainPhone { get; set; }

        public byte TradingVolume { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsAutoOrder { get; set; }

        public bool IsChecked { get; set; }

        public string Manager { get; set; }

        public string ManagerId { get; set; }
    }
}
