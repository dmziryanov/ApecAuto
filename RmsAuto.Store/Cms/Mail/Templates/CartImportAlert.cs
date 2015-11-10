using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web;
using RmsAuto.Store.Acctg;
using System.Configuration;

namespace RmsAuto.Store.Cms.Mail.Messages
{
    [MailTemplate( "RmsAuto.Store.Cms.Mail.Templates.CartImportAlert.xslt" )]
    public class CartImportAlert : IMailMessage
    {
        public CartImportAlert()
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

        public string ClientToHeader { get; set; }

        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string SiteUrl { get; set; }

        public string Phone { get; set; }
    }
}
