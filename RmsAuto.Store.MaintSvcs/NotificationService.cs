using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace RmsAuto.Store.MaintSvcs
{
	partial class NotificationService : ServiceBase
	{
        private NotificationManager _manager;
			

        public NotificationService(string Name)
		{
            InitializeComponent();
            this.ServiceName = Name;
		}

		protected override void OnStart( string[] args )
		{
            _manager = new NotificationManager(Configuration.NotificationPollingSeconds, this.ServiceName);
            _manager.Start();
		}

		protected override void OnStop()
		{
			_manager.Stop();
		}
	}
}
