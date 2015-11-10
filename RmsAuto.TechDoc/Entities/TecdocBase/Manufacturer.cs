using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using RmsAuto.TechDoc.Entities.Helpers;

namespace RmsAuto.TechDoc.Entities.TecdocBase
{
    public partial class Manufacturer : ITecdocItem
    {
        public bool Invisible { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.ID);
        }

    }
}
