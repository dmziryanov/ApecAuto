using System;
using RmsAuto.TechDoc.Cache;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using RmsAuto.TechDoc.Entities.Helpers;

namespace RmsAuto.TechDoc.Entities.TecdocBase
{
    public partial class Brand : ITecdocItem
    {
        public override string ToString()
        {
            return this.Name;
        }

        int ITecdocItem.ID
        {
            get { return this.ID; }
            set { }
        }
    }
}
