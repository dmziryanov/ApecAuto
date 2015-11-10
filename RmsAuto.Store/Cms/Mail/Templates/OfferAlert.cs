using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Web;
using System.Configuration;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Cms.Mail.Templates
{
	[MailTemplate( "RmsAuto.Store.Cms.Mail.Templates.OfferAlert.xslt" )]
	public class OfferAlert : IMailMessage
	{
        public OfferAlert()
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

        public string OfferTitle { get; set; }
        public string OfferSubject { get; set; }
        public string OfferBody { get; set; }
        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string SiteUrl { get; set; }

        public string Phone { get; set; }
	}
}
