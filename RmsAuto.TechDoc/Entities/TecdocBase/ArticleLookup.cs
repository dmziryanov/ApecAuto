using System;
using RmsAuto.TechDoc.Cache;
using RmsAuto.TechDoc.Entities.Helpers;

namespace RmsAuto.TechDoc.Entities.TecdocBase
{
    public partial class ArticleLookup
    {
        public override string ToString()
        {
            return String.Format("{0} => {1} ({2})", this.SearchNumber, this.DisplayNumber, this.Article.ToString());
        }
    }
}
