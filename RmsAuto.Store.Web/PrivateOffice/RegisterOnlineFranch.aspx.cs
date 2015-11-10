using System;
using System.Web.UI;
using System.Configuration;
using RmsAuto.Store.Cms.Mail;
using System.Net.Mail;
using RmsAuto.Store.Cms.Mail.Messages;
using RmsAuto.Store.Acctg.Entities;
using System.Text;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class RegisterOnlineFranch : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(ConfigurationManager.AppSettings["NewRegistrationData"]) >= DateTime.Now)
            _fillFranchProfileExt.Visible = false;
        }

        public static string ConvertEncoding(string value, Encoding src, Encoding trg)
        {
            Decoder dec = src.GetDecoder();
            byte[] ba = trg.GetBytes(value);
            int len = dec.GetCharCount(ba, 0, ba.Length);
            char[] ca = new char[len];
            dec.GetChars(ba, 0, ba.Length, ca, 0);
            return new string(ca);
        }
        protected void FillFranchProfileCompletedExt(object sender, EventArgs args)
        {
            ServiceProxy.Default.SendFranchBlankRequest(_fillFranchProfileExt.getFranchEnvelope());
            _fillFranchProfileExt.Visible =  completeButton.Visible = false;
            _messagePane.Visible = true;
        }
    }
}
