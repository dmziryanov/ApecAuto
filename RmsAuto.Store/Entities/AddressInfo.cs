using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    public class AddressInfo
    {
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }

        public string ToXml()
        {
            StringBuilder xml = new StringBuilder("<Address>");

            AppendXml(xml, "PostalCode", PostalCode);
            AppendXml(xml, "City", City);
            AppendXml(xml, "Region", Region); 
            AppendXml(xml, "Address", Address);

            xml.Append("</Address>");

            return xml.ToString();
        }

        private void AppendXml(StringBuilder builder, string name, string value)
        {
            builder.AppendFormat("<{0}>{1}</{0}>", name, value);
        }
    }
}
