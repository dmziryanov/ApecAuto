using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Configuration;
using System.Configuration;

namespace RmsAuto.TechDoc.Configuration
{
    public class TecdocConfiguration : ConfigurationSectionSingleton<TecdocConfiguration>
    {
        public short NoLanguage { get { return byte.MaxValue; } }

        [ConfigurationProperty("languageId", IsRequired = true)]
        public int LanguageId
        {
            get
            {
                return base.TryGet<int>("languageId");
            }
            set
            {
                base.Set("languageId", value);
            }
        }

        [ConfigurationProperty("noLanguageId", IsRequired = true)]
        public int NoLanguageId
        {
            get
            {
                return base.TryGet<int>("noLanguageId");
            }
            set
            {
                base.Set("noLanguageId", value);
            }
        }

        [ConfigurationProperty("originalType", IsRequired = true)]
        public char OriginalType
        {
            get
            {
                return base.TryGet<char>("originalType");
            }
            set
            {
                base.Set("originalType", value);
            }
        }

        public override string SectionName
        {
            get { return "tecdoc.conf"; }
        }
    }
}
