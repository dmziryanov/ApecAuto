using System;
using System.Linq;
using System.Collections.Generic;
using RmsAuto.Common.Configuration;
using System.Configuration;

namespace RmsAuto.Store.Configuration
{
    public class StocksConfiguration : ConfigurationSectionSingleton<StocksConfiguration>
    {
        public override string SectionName
        {
            get { return "rmsauto.stockSuppliers"; }
        }

        [ConfigurationProperty("pageSize", IsRequired = true)]
        public int PageSize
        {
            get
            {
                return TryGet<int>("pageSize");
            }
            set
            {
                Set("pageSize", value);
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(SuppliersCollection))]
        public SuppliersCollection Suppliers
        {
            get { return this[""] as SuppliersCollection; }
        }

        public IEnumerable<int> SupplierIds
        {
            get
            {
                return this.Suppliers.Cast<StockSupplierElement>().Select(s => s.Id);
            }
        }
    }

    public class SuppliersCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public StockSupplierElement this[int idx]
        {
            get
            {
                return (StockSupplierElement)this.BaseGet(idx);

            }
            set
            {
                if (BaseGet(idx) != null)
                {
                    BaseRemoveAt(idx);
                }

                BaseAdd(idx, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new StockSupplierElement();
        }
     
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StockSupplierElement)element).Id;
        }    
    }

    public class StockSupplierElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public int Id
        {
            get { return Convert.ToInt32(this["id"]); }
            set { this["id"] = value; }
        }

        public StockSupplierElement() { }

        public StockSupplierElement(int id)
        {
            this.Id = id;
        }
    }
}
