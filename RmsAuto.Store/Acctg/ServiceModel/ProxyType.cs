using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg.Entities
{
    class ProxyType
    {
        private Dictionary<string, ServiceMethodInfo> _methods =
            new Dictionary<string, ServiceMethodInfo>();

        public ProxyType(Type type)
        {
            var envTypes = new List<Type>();
            var methods =
                (from method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
               .Where(method => Attribute.IsDefined(method, typeof(ServiceMethodAttribute)))
                 select new
                 {
                     MethodName = method.Name,
                     ServiceAttribute = (ServiceMethodAttribute)Attribute.GetCustomAttribute(method, typeof(ServiceMethodAttribute))
                 }).ToArray();
            
            var mappings = (from m in methods select new 
                { 
                  MethodName = m.MethodName,
                  Action = m.ServiceAttribute.Action,
                  ArgsTypeIndex = AddOrGetIndexOfExistingType(envTypes, m.ServiceAttribute.ArgType),
                  ResultsTypeIndex = AddOrGetIndexOfExistingType(envTypes, m.ServiceAttribute.ReturnType)
                }).ToArray();

            var xmlRoot = new XmlRootAttribute("Command");
            var envSerializers = envTypes.ConvertAll<XmlSerializer>(t => new XmlSerializer(t, xmlRoot));
            foreach (var m in mappings)
            {
                var svcMethod = new ServiceMethodInfo()
                {
                    MethodName = m.MethodName,
                    ServiceAction = m.Action,
                    ArgsSerializer = envSerializers[m.ArgsTypeIndex],
                    ResultsSerializer = envSerializers[m.ResultsTypeIndex]
                };
                _methods.Add(svcMethod.MethodName, svcMethod);
            }
        }

        public ServiceMethodInfo GetMethod(string methodName)
        {
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentException("MethodName cannot be empty", "methodName");
            if (_methods.ContainsKey(methodName))
                return _methods[methodName];
            else
                throw new InvalidOperationException(methodName + " - doesn't exists or have no [ServiceMethod] applied"); 
        }

        private int AddOrGetIndexOfExistingType(List<Type> list, Type item)
        {
            int index = list.IndexOf(item);
            if (index == -1)
            {
                index = list.Count;
                list.Insert(index, item);
            }
            return index;
        }
    }
}
