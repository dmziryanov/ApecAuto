using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Reflection;

namespace RmsAuto.Store.MaintSvcs
{
    static class Program
    {

        public static string PriceMaintServiceName;
        public static string NotificationServiceName;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            var pPath = Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf(@"\"));
            String[] lines = System.IO.File.ReadAllLines(pPath + @"\Params.txt");
            ServicesToRun = new ServiceBase[] 
			{ 
                new PriceMaintService(lines[0]),
                //new AcctgSyncService(),
				new NotificationService(lines[1]),
				//new RecNotificationService()
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
