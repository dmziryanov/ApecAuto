using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Acctg.Entities
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    class ServiceMethodAttribute : Attribute
    {
        private string _action;
        private Type _argType;
        private Type _returnType;

        public ServiceMethodAttribute(string action, Type argType, Type returnType)
        {
            if (string.IsNullOrEmpty(action))
                throw new ArgumentException("ServiceMethod action cannot be empty", "action");
            if (argType == null)
                throw new ArgumentNullException("argType");
            if (returnType == null)
                throw new ArgumentNullException("returnType");
            _action = action;
            _argType = argType;
            _returnType = returnType;
        }

        public string Action
        {
            get { return _action; }
        }

        public Type ArgType
        {
            get { return _argType; }
        }

        public Type ReturnType
        {
            get { return _returnType; }
        }
    }
}
