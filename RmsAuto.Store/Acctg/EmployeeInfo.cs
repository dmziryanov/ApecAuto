using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    public class EmployeeInfo
    {
        public string EmployeeId { get; set; }

        [XmlElement("EmployeeName")]
        public string FullName { get; set; }

        [XmlElement("EmployeeDept")]
        public string DeptId { get; set; }

        [XmlElement("ShopId")]
        public string StoreId { get; set; }

        [XmlElement("EmployeePosition")]
        public string Position { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("icq")]
        public string Icq { get; set; }

        public string Phone { get; set; }
        public string InternalFranchName { get; set; }
    }
}
