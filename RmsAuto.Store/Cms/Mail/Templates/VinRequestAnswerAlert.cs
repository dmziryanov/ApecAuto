using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Routing;
using System.Xml.Serialization;
using System.Web;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Web;
using System.Configuration;

namespace RmsAuto.Store.Cms.Mail.Messages
{
	[MailTemplate( "RmsAuto.Store.Cms.Mail.Templates.VinRequestAnswerAlert.xslt" )]
    public class VinRequestAnswerAlert : IMailMessage
    {
        public int VinRequestId { get; set; }
        public string VinRequestUrl { get; set; }

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<AnswerLineItem> Items { get; set; }

        public decimal TotalPrice { get; set; }

        public VinRequestAnswerAlert()
        {
            this.Items = new List<AnswerLineItem>();
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

        public VinRequestAnswerAlert(int reqId) : this()
        {
            this.VinRequestId = reqId;
            this.VinRequestUrl = UrlManager.MakeAbsoluteUrl(UrlManager.GetVinRequestDetailsUrl(this.VinRequestId));
        }

        public string CompanyName { get; set; }

        public string CompanyUrl { get; set; }

        public string SiteUrl { get; set; }

        public string Phone { get; set; }
    }

    [Serializable]
    public class AnswerLineItem
    {
        public string Name { get; set; }
        public short Qty { get; set; }

        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public string DeliveryPeriod { get; set; }
        public decimal PricePerUnit { get; set; }
        public string PartNumberOriginal { get; set; }
        public string ManagerComment { get; set; }
        public string Description { get; set; }

        public string SearchUrl { get; set; }
    }
}
