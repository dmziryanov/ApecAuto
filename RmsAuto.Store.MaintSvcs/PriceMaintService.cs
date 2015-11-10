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
    partial class PriceMaintService : ServiceBase
    {
        private ImportManager _manager;

        public PriceMaintService(string Name)
        {
            InitializeComponent();
            this.ServiceName = Name;
        }

        protected override void OnStart(string[] args)
        {
            _manager = new ImportManager(
            Configuration.PricePickupPath,
            Configuration.PriceFactorPickupPath,
            Configuration.CrossPickupPath,
            Configuration.CrossBrandsPickupPath,
            Configuration.CrossGroupsPickupPath,
            Configuration.CrossLinksPickupPath,
            Configuration.ImportPollingSeconds,
            Configuration.TraceMode, this.ServiceName);
            _manager.Start();
        }

        protected override void OnStop()
        {
            _manager.Stop();
        }
    }
}
