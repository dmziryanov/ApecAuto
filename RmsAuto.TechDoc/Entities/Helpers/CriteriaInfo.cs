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
    public class CriteriaInfo
    {
        public List<KeyValuePair<string, string>> Details { get; set; }

        public CriteriaInfo()
        {
            this.Details = new List<KeyValuePair<string, string>>();
        }
    }
}
