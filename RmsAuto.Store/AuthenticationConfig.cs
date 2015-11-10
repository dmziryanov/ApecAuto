using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;

namespace RmsAuto.Store
{
    static class AuthenticationConfig
    {
        private static FormsAuthenticationConfiguration _forms;
        private static bool _initialized;
        private static Exception _initException;
        private static object _syncObj = new object();

        private static void Initialize()
        {
            if (!_initialized)
                lock (_syncObj)
                    if (!_initialized)
                    {
                        try
                        {
                            _forms = ((AuthenticationSection)
                                WebConfigurationManager.GetSection("system.web/authentication")).Forms;
                        }
                        catch (Exception ex)
                        {
                            _initException = ex;
                        }
                        _initialized = true;
                    }
            if (_initException != null)
                throw _initException; 
        }

        public static FormsAuthenticationConfiguration Forms
        {
            get
            {
                Initialize();
                return _forms;
            }
        }
    }
}
