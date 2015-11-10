using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Web;
using System.Configuration;

namespace RmsAuto.Store.Cms.Mail.Messages
{
    [Obsolete]
	[MailTemplate( "RmsAuto.Store.Cms.Mail.Templates.ClientActivationAlert.xslt" )]
	public class ClientActivationAlert : IMailMessage
	{
        public ClientActivationAlert()
        {
            string InternalFranchName = null;
            if (SiteContext.Current != null)
                { InternalFranchName = SiteContext.Current.InternalFranchName; }
            else
                { InternalFranchName = ConfigurationSettings.AppSettings["InternalFranchName"]; }

			CompanyName = AcctgRefCatalog.RmsFranches[InternalFranchName].Name;
            CompanyUrl = AcctgRefCatalog.RmsFranches[InternalFranchName].Url;
            Phone = AcctgRefCatalog.RmsFranches[InternalFranchName].Phone;
            SiteUrl = AcctgRefCatalog.RmsFranches[InternalFranchName].Url;
        }

        public int DaysToLive { get; set; }

        public string ActivationUrl { get; set; }

        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string SiteUrl { get; set; }

        public string Phone { get; set; }
	}
}
