using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.TechDoc.Cache;

namespace RmsAuto.TechDoc.Entities.TecdocBase
{
    public partial class Country : ITecdocItem
    {
        public int ID
        {
            get { return this.COU_ID; }
            set { }
        }
    }
}
