using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using RmsAuto.Store.Acctg;
using RmsAuto.Store.Entities;
using System.Web.UI;
using System.IO;

namespace RmsAuto.Store.Web
{
    public class VinRequestEventArgs : EventArgs
    {
        public VinRequest VinRequest { get; protected set; }

        public VinRequestEventArgs(VinRequest req)
        {
            this.VinRequest = req;
        }
    }

    public delegate void VinRequestEventHandler(object sender, VinRequestEventArgs e);
}
