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
	partial class RecNotificationService : ServiceBase
	{
		private RecNotificationManager _manager =
			new RecNotificationManager( Configuration.RecNotificationPollingSeconds );

		public RecNotificationService()
		{
			InitializeComponent();
		}

		protected override void OnStart( string[] args )
		{
			_manager.Start();
		}

		protected override void OnStop()
		{
			_manager.Stop();
		}
	}
}
