using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Web;

namespace RmsAuto.Store.Cms.Mail.Messages
{
	[MailTemplate("RmsAuto.Store.Cms.Mail.Templates.ClientActivationAlertRet.xslt")]
	public class ClientActivationAlertRet : IMailMessage
	{

        public ClientActivationAlertRet()
        {
			CompanyName = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Name;
            CompanyUrl = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Url;
            Phone = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Phone;
            SiteUrl = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].Url;
        }
        
        public int DaysToLive { get; set; }

		public string ActivationUrl { get; set; }

        public string RetailUrl { get; set; }

        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string SiteUrl { get; set; }

        public string Phone { get; set; }
	}
}
