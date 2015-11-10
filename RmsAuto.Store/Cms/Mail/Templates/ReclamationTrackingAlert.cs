using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Web;
using RmsAuto.Store.Acctg;
using System.Configuration;

namespace RmsAuto.Store.Cms.Mail.Templates
{
	[MailTemplate( "RmsAuto.Store.Cms.Mail.Templates.ReclamationTrackingAlert.xslt" )]
	public class ReclamationTrackingAlert : IMailMessage
	{
		public class Reclamation
		{
			public string ReclamationNumber { get; set; }
			public string ReclamationType { get; set; }
			public string ReclamationDate { get; set; }

			public string Manufacturer { get; set; }
			public string PartNumber { get; set; }
			public string PartName { get; set; }
			public string Qty { get; set; }

			public string CurrentStatus { get; set; }
			public string CurrentStatusDate { get; set; }
		}

        public ReclamationTrackingAlert()
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

		public Reclamation[] Reclamations { get; set; }

		/// <summary>
		/// Текстовое поле перед подписью
		/// </summary>
		public string BlockMailFooter { get; set; }

        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string SiteUrl { get; set; }

        public string Phone { get; set; }
	}
}
