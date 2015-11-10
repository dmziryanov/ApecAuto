using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.MaintSvcs
{
    static class Configuration
    {
        public static string PricePickupPath
        {
            get { return ConfigurationManager.AppSettings["pricePickupPath"]; }
        }

        public static string PriceFactorPickupPath
        {
            get { return ConfigurationManager.AppSettings["priceFactorPickupPath"]; }
        }

        public static string CrossPickupPath
        {
            get { return ConfigurationManager.AppSettings["crossPickupPath"]; }
        }

		public static string CrossBrandsPickupPath
		{
			get { return ConfigurationManager.AppSettings[ "crossBrandsPickupPath" ]; }
		}

		public static string CrossGroupsPickupPath
		{
			get { return ConfigurationManager.AppSettings[ "crossGroupsPickupPath" ]; }
		}
		public static string CrossLinksPickupPath
		{
			get { return ConfigurationManager.AppSettings[ "crossLinksPickupPath" ]; }
		}

        public static int ImportPollingSeconds
        {
            get { return int.Parse(ConfigurationManager.AppSettings["importPollingSeconds"]); }
        }

        public static int StatusPollingSeconds
        {
            get { return int.Parse(ConfigurationManager.AppSettings["statusPollingSeconds"]); }
        }

		public static int NotificationPollingSeconds
		{
			get { return int.Parse( ConfigurationManager.AppSettings[ "notificationPollingSeconds" ] ); }
		}

		public static int RecNotificationPollingSeconds
		{
			get { return int.Parse( ConfigurationManager.AppSettings["recNotificationPollingSeconds"] ); }
		}

        public static bool LogDetails
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["logDetails"]); }
        }

        public static string LogEncoding
        {
            get { return ConfigurationManager.AppSettings["logEncoding"] ?? "utf-8"; }
        }

        public static bool TraceMode
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["traceMode"]); }
        }
    }
}
