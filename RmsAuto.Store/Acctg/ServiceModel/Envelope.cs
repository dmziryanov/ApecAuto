using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg.Entities
{
    public class Envelope
    {
        public static Envelope Empty()
        {
            return new Envelope();
        }

        [XmlAttribute("Name")]
        public string Action { get; set; }
    }
}
