using System;
using RmsAuto.TechDoc.Cache;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using RmsAuto.TechDoc.Entities.Helpers;

namespace RmsAuto.TechDoc.Entities.TecdocBase
{
    public partial class Model : ITecdocItem
    {

        public override string ToString()
        {
            return String.Format("{0} ({1} по {2})", this.Name.Tex_Text, this.DateFrom, this.DateTo);
        }

        public string DateFrom
        {
            get
            {
                return Utils.ParseIntToDT(this.MOD_PCON_START);
            }
        }

        public string DateTo
        {
            get
            {
                return Utils.ParseIntToDT(this.MOD_PCON_END);
            }
        }

        public bool Invisible { get; set; }

        public int ID
        {
            get { return MOD_ID; }
            set { MOD_ID = value; }
        }
    }
}
