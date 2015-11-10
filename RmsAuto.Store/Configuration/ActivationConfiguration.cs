using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using RmsAuto.Common.Configuration;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Configuration
{
    public class ActivationConfiguration : ConfigurationSectionSingleton<ActivationConfiguration>
    {
        public override string SectionName
        {
            get { return "rmsauto.activation"; }
        }
        
        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(ActivationPurposeElementCollection))]
        public ActivationPurposeElementCollection PurposeSettings
        {
            get { return this[""] as ActivationPurposeElementCollection; }
        }
    }

    public class ActivationPurposeElement : ConfigurationElement
    {
        public ActivationPurposeElement() { }

        [ConfigurationProperty("purpose", IsRequired = true)]
        public UserMaintEntryPurpose Purpose
        {
            get { return (UserMaintEntryPurpose)this["purpose"]; }
            set { this["purpose"] = value; }
        }
        
        [ConfigurationProperty("daysToLive", IsRequired = true)]
        public int DaysToLive
        {
            get { return Convert.ToInt32(this["daysToLive"]); }
            set { this["daysToLive"] = value; }
        }
    }

    public class ActivationPurposeElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public ActivationPurposeElement this[UserMaintEntryPurpose purpose]
        {
            get
            {
                return (ActivationPurposeElement)this.BaseGet(purpose);
            }
            set
            {
                if (this.BaseGet(purpose) != null)
                {
                    BaseRemove(purpose);
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ActivationPurposeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ActivationPurposeElement)element).Purpose;
        }
    }
}
