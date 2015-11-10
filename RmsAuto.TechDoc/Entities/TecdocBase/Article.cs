using System;
using RmsAuto.TechDoc.Cache;
using RmsAuto.TechDoc.Entities.Helpers;

namespace RmsAuto.TechDoc.Entities.TecdocBase
{
    public partial class Article : ITecdocItem
    {

        public override string ToString()
        {
            return this.Name != null
                   ? String.Format("{1} ({0}) {2}", this.Name.Tex_Text, this.ArticleNumber, this.Supplier.ToString())
                   : String.Format("{0}, {1}", this.ArticleNumber, this.Supplier.ToString());
        }
    }
}
