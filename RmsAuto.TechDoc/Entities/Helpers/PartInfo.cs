using System;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using RmsAuto.TechDoc.Cache;
using RmsAuto.TechDoc.Entities.TecdocBase;

namespace RmsAuto.TechDoc.Entities.Helpers
{
    /// <summary>
    /// Хелпер для отображалки
    /// </summary>
    public class PartInfo
    {
        public Article Article { get; protected set; }

        public List<string> OriginalNumbers { get; set; }

        public PartInfo(Article a)
        {
            this.OriginalNumbers = new List<string>();
            this.Article = a;
        }
    }
}
