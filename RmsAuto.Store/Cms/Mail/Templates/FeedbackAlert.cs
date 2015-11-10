using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Web;
using System.Configuration;

namespace RmsAuto.Store.Cms.Mail.Messages
{
	[MailTemplate( "RmsAuto.Store.Cms.Mail.Templates.FeedbackAlert.xslt" )]
	public class FeedbackAlert : IMailMessage
	{
		public string ClientName { get; set; }
		public string Message { get; set; }

		public FeedbackAlert()
		{
            //Возможно это можно вынести как-то в один конструктор
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

        public FeedbackAlert(string clientName, string message)
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
            ClientName = clientName;
            Message = message;
        }

        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string SiteUrl { get; set; }

        public string Phone { get; set; }
	}
}
