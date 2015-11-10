using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RmsAuto.Store.Acctg;
using RmsAuto.Common.Misc;
using RmsAuto.Common.References;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.BL;

namespace RmsAuto.Store
{
    public class OwnStore
    {
        public int SupplierID;
        public int vSupplierID; 
    }

    public class SearchOwnStore
    {
        public int SupplierID;
        public int vSupplierID;
    }

    public static class StoreRefCatalog
    {
        public static readonly string OwnStores = "OwnStores";
        public static readonly string SearchOwnStores = "SearchOwnStores";
        
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
                yield return OwnStores;
                yield return SearchOwnStores;
            }
        }
     
        private static Dictionary<string, IReference> _References
        {
            get
            {
                if (_references == null) //это потокобезопасно, так как CreateReferences заполняет  временный массив tmp, а потом мы его атомарно переклыдаваем куда нужно
                    lock (_sync)
                        if (_references == null)
                        {
                            var tmp = CreateReferences();
                            Interlocked.Exchange( ref _references, tmp);
                        }
                return _references;
            }
        }

        private static Dictionary<string, IReference> CreateReferences()
        {
            var references = new List<IReference>();

            references.Add(new Reference<int, OwnStore>(OwnStores, "собственные склады", new StoreItemProxy<OwnStore>(), (s) => s.GetItems(), r => r.SupplierID));
            references.Add(new Reference<int, SearchOwnStore>(SearchOwnStores, "собственные склады для поиска", new StoreItemProxy<SearchOwnStore>(), (s) => s.GetItems(), s => s.SupplierID));
        
            return references.ToDictionary(r => r.Name);
        }

        public static Reference<int, OwnStore> RefOwnStores
        {
            get { return (Reference<int, OwnStore>)GetReferenceInternal(OwnStores); }
        }


        public static Reference<int, SearchOwnStore> RefSearchOwnStores
        {
            get { return (Reference<int, SearchOwnStore>)GetReferenceInternal(SearchOwnStores); }
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
