using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg.Entities
{
    class ServiceMethodInfo
    {
        public string MethodName { get; set; }
        public string ServiceAction { get; set; }
        public XmlSerializer ArgsSerializer { get; set; }
        public XmlSerializer ResultsSerializer { get; set; } 
    }
}
