using System.Web.UI.WebControls;

namespace RmsAuto.Store.Web.Controls
{
    public partial class PhotoFileLoader : System.Web.UI.UserControl
    {
        public byte[] FileValue
        {
            get { return fileUploadId.HasFile ? fileUploadId.FileBytes : null; }
        }
        public string FullFileName
        {
            get { return fileUploadId.HasFile ? Server.HtmlEncode(fileUploadId.PostedFile.FileName) : string.Empty; }
        }
        public bool CheckFileIsSelected
        {
            get { return fileSelection.Enabled; }
            set { fileSelection.Visible = value; }
        }
        public bool CheckFileSize
        {
            get { return fileSize.Enabled; }
            set { fileSize.Visible = value; }
        }
        public bool CheckFileExtension
        {
            get { return fileExtension.Enabled; }
            set { fileExtension.Visible = value; }
        }
        public string Value
        {
            get { return fileUploadId.HasFile ? Server.HtmlEncode(fileUploadId.FileName) : string.Empty; }
        }
        protected void fileSelectionValidator(object source, ServerValidateEventArgs args)
        {
            args.IsValid = fileUploadId.HasFile;
        }
        protected void fileSizeValidator(object source, ServerValidateEventArgs args)
        {
            args.IsValid = fileUploadId.HasFile ? fileUploadId.PostedFile.ContentLength < 2100000 : true;
        }
        protected void fileExtensionValidator(object source, ServerValidateEventArgs args)
        {
            if (!fileUploadId.HasFile) { args.IsValid = true; return; }
            string extension = System.IO.Path.GetExtension(Server.HtmlEncode(fileUploadId.FileName));
            args.IsValid = (extension == ".png" || extension == ".jpg" || extension == ".tif");
        }
    }
}