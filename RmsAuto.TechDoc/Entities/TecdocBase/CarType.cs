using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.TechDoc.Cache;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using RmsAuto.TechDoc.Entities.Helpers;

namespace RmsAuto.TechDoc.Entities.TecdocBase
{
    public partial class CarType : ITecdocItem
    {
        public override string ToString()
        {
            return String.Format("{0} ({1} по {2})", this.Name, this.DateFrom, this.DateTo);
        }

        public string DateFrom
        {
            get
            {
                return Utils.ParseIntToDT(this.TYP_PCON_START);
            }
        }
        public string DateTo
        {
            get
            {
                return Utils.ParseIntToDT(this.TYP_PCON_END);
            }
        }

        public bool Invisible { get; set; }
    }
}
