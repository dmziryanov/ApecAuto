extern alias DataStreams1;
using System;
using System.IO;
using System.Linq;
using System.Web;
using DataStreams1::DataStreams.Common;
using DataStreams1::DataStreams.Xls;
using DataStreams1::DataStreams.Xlsx;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    public partial class UploadSpareParts : RMMPage
    {
        public string GetTemplateUrl()
        {
            string templateFileName = "SparePartTemplate_eng.csv";
            int fileID = FilesDac.GetFileIDByName(templateFileName);
            return UrlManager.GetFileUrl(fileID);
        }
		
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void AddError(UploadStatuses.ErrorReason reason, string arg)
        {
            lSummaryInfo.Text = lSummaryInfo.Text + TextError(reason, arg) + Environment.NewLine;
        }

        private string TextError(UploadStatuses.ErrorReason reason, string args)
        {
            //TODO implement this method
            return string.Empty;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = fuMain.PostedFile;
            if (string.IsNullOrEmpty(fuMain.FileName))
            {
                AddError(UploadStatuses.ErrorReason.FileNotSpecified, "");
                ShowMessage("File is not choosen");
                return;
            }
            if (!fuMain.HasFile)
            {
                AddError(UploadStatuses.ErrorReason.FileNotFoundOrEmpty, file.FileName);
                return;
            }

            bool xlsx;

            switch (Path.GetExtension(file.FileName))
            {
                case ".csv": xlsx = false; break;
                case ".zip": xlsx = true; break;
                default:
                    {
                        AddError(UploadStatuses.ErrorReason.InvalidFileFormat, "");
                        return;
                    }
            }

            SpreadsheetReader reader = null;
            try
            {
                var s = Server.MapPath(@"App_data\prices\" + SiteContext.Current.InternalFranchName + @"\");
                System.IO.Directory.CreateDirectory(s);
                file.SaveAs(s + file.FileName);
                using (var fs = File.Create(s + Path.ChangeExtension(file.FileName, ".imp")))
                {
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                AddError(UploadStatuses.ErrorReason.ReaderFailed, file.FileName);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
    }
}