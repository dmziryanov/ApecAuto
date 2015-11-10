using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web;
using System.Configuration;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Cms.Mail.Messages
{
    [MailTemplate( "RmsAuto.Store.Cms.Mail.Templates.CartImportOK.xslt" )]
    public class CartImportOK : IMailMessage
    {

        public CartImportOK()
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
