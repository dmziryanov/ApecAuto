using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.TechDoc.Cache;
using System.Xml.Serialization;
using RmsAuto.TechDoc.Entities.TecdocBase;
using System.Xml.Schema;
using System.Xml;

namespace RmsAuto.TechDoc.Entities.Helpers
{
    public class PartsListItem : ITecdocItem
    {
        public Article Article { get; set; }

        public bool HasDescription { get; set; }
        public bool HasPics { get; set; }
        public bool HasAppliedCars { get; set; }

        public PartsListItem()
        {

        }

        public PartsListItem(Article a)
        {
            this.Article = a;
        }


        public int ID
        {
            get { return this.Article.ID; }
            set { }
        }
    }
}
