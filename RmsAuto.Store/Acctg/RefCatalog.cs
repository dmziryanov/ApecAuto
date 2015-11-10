using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RmsAuto.Common.Misc;
using RmsAuto.Common.References;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Acctg
{

    public interface IRefCatalog
    {
        IEnumerable<string> RefNames {get; set;}
    }
    
    //TODO: сделать свойство this, и возвращаеть коллекции
    public class AcctgRefCatalog
    {
        public static readonly string Stores = "Stores";
		public static readonly string ShopStores = "ShopStores";
        public static readonly string Employees = "Employees";
        public static readonly string Rates = "Rates";
        public static readonly string Regions = "Region";
        public static readonly string Cities = "Cities";
        public static readonly string Franches = "Franches";
		
        
        //public static readonly string Suppliers = "Suppliers";
        //public static readonly string Areas = "ShippingAreas";
        //public static readonly string Depts = "Depts";
        
        private static Dictionary<string, IReference> _references;
        
        private static readonly object _sync = new object();
                        
        public static IReference GetReference(string refName)
        {
            return GetReferenceInternal(refName);
        }
              
        public static IEnumerable<string> RefNames
        {
            get
            {
                yield return Stores;
				yield return ShopStores;
                yield return Employees;
                yield return Rates;
                yield return Regions;
                yield return Cities;
                yield return Franches;
                //yield return Areas;
                //yield return Depts;
            }
        }

        public static Reference<string, CurrencyRate> CurrencyRates
        {
            get { return (Reference<string, CurrencyRate>)GetReferenceInternal(Rates); }
        }


        public static Reference<string, StoreInfo> RmsStores
        {
            get { return (Reference<string, StoreInfo>)GetReferenceInternal(Stores); }
        }

		public static Reference<string, ShopStoreInfo> RmsShopStores
		{
			get { return (Reference<string, ShopStoreInfo>)GetReferenceInternal(ShopStores); }
		}

        public static Reference<string, spSelFranchesFromRmsResult> RmsFranches
        {
            get { return (Reference<string, spSelFranchesFromRmsResult>)GetReferenceInternal(Franches); }
        }

		
		public static Reference<string, EmployeeInfo> RmsEmployees
        {
            get { return (Reference<string, EmployeeInfo>)GetReferenceInternal(Employees); }
        }
                        
        //private static Dictionary<string, IReference> _References
        //{
        //    get
        //    {
        //        if (_references == null) //Это непотокобезопасно, так как CreateReferences может не успеть заполнить лист после инициализации
        //            lock (_sync)
        //                if (_references == null)
        //                {
        //                      _references = CreateReferences();
        //                }
        //        return _references;
        //    }
        //}

        private static Dictionary<string, IReference> _References
        {
            get
            {
                if (_references == null) //А это потокобезопасно, так как CreateReferences заполняет  временный массив, а потом мы его атомарно переклыдаваем куда нужно
                    lock (_sync)
                        if (_references == null)
                        {
                            var tmp = CreateReferences();
                            Interlocked.Exchange(ref _references, tmp);
                        }
                return _references;
            }
        }

        private static Dictionary<string, IReference> CreateReferences()
        {
            var references = new List<IReference>();

            references.Add(new Reference<string, CurrencyRate>( Rates, "курсы валют", new AcctgItemProxy<CurrencyRate>(), (s) => s.GetItems(), r => r.CurrencyCode));
            references.Add(new Reference<string, StoreInfo>(Stores, "пункты выдачи заказов", new AcctgItemProxy<StoreInfo>(), (s) => s.GetItems(), s => s.StoreId));
			references.Add(new Reference<string, ShopStoreInfo>(ShopStores, "собственные склады наличия магазинов", new AcctgItemProxy<ShopStoreInfo>(), (s) => s.GetItems(), s => s.RmsStoreID));
            references.Add(new Reference<string, EmployeeInfo>(Employees, "сотрудники", new AcctgItemProxy<EmployeeInfo>(), (s) => s.GetItems(), e => e.EmployeeId));
            references.Add(new Reference<int, Region>(Regions, "Регионы", new AcctgItemProxy<Region>(), (s) => s.GetItems(), q => q.RegionID));
            references.Add(new Reference<int, City>(Cities, "Города", new AcctgItemProxy<City>(), (s) => s.GetItems(), g => g.CityID.Value));
            references.Add(new Reference<string, spSelFranchesFromRmsResult>(Franches, "Франчайзи", new AcctgItemProxy<spSelFranchesFromRmsResult>(), (s) => s.GetItems(), s => s.InternalFranchName));
            //references.Add(new Reference<string, ReferenceItem>(Areas, "районы доставки", (s) => s.GetReference("Regions"), r => r.Key));
            //references.Add(new Reference<string, ReferenceItem>(Depts, "отделы", (s) => s.GetReference("Departments"), r => r.Key));
                        
            return references.ToDictionary(r => r.Name);
        }

        private static IReference GetReferenceInternal(string refName)
        {
            if (string.IsNullOrEmpty(refName))
                throw new ArgumentException("Reference' name cannot be empty", "refName");
            if (!_References.ContainsKey(refName))
                throw new Exception("Reference : '" + refName + "' doesn't exist");
            return _References[refName];
        }
    }
}
