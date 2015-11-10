using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Acctg;
using System.Configuration;
using RmsAuto.Store.Web;

namespace RmsAuto.Store.Cms.Mail.Messages
{
	[MailTemplate( "RmsAuto.Store.Cms.Mail.Templates.OrderLineTrackingAlert.xslt" )]
	//[MailAttachment(  "down_arc.gif", "RmsAuto.Store.Cms.Mail.Images.down_arc.gif" )]
	//[MailAttachment(  "question.gif", "RmsAuto.Store.Cms.Mail.Images.question.gif" )]
	public class OrderLineTrackingAlert : IMailMessage
	{
		public class OrderLine
		{
			public string OrderDisplayNumber { get; set; }
			public string CustOrderNum { get; set; }
			public string OrderDate { get; set; }

			public string Manufacturer { get; set; }
			public string PartNumber { get; set; }
			public string PartName { get; set; }
			public string Qty { get; set; }
            
            // Код позиции
            public string ReferenceID { get; set; }
			public string UnitPrice { get; set; }
			public string Total { get; set; }
			public string EstSupplyDate { get; set; }

			public string CurrentStatusName { get; set; }
			public string CurrentStatusDescription { get; set; }
			public string CurrentStatusDate { get; set; }
            
			public OrderLine ParentOrderLine { get; set; }
		}

        public OrderLineTrackingAlert()
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

		/// <summary>
		/// Конструктор для Light-версии франчайзи
		/// (чтобы подпись к письмам различалась для разных лайтов)
		/// </summary>
		/// <param name="internalFranchName">внутреннее имя франча</param>
		public OrderLineTrackingAlert(string internalFranchName)
		{
			CompanyName = AcctgRefCatalog.RmsFranches[internalFranchName].Name;
			CompanyUrl = AcctgRefCatalog.RmsFranches[internalFranchName].Url;
			Phone = AcctgRefCatalog.RmsFranches[internalFranchName].Phone;
			SiteUrl = AcctgRefCatalog.RmsFranches[internalFranchName].Url;
		}

		public OrderLine[] OrderLines { get; set; }

        // Текстовое поле перед подписью
        public string BlockMailFooter { get; set; }

		public string EstSupplyDateHint { get; set; }

        public string NumChange { get; set; }
        
        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string SiteUrl { get; set; }

        public string Phone { get; set; }
	}
}
