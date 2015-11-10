using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.Import;

namespace RmsAuto.Store.Adm.Controls
{
    public partial class ImportReportView : System.Web.UI.UserControl
    {
        
         
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindReportData(ImportReport report)
        {
            if (report == null)
                throw new ArgumentNullException("report");

            _ltrFilename.Text = report.Filename;
            _ltrMode.Text = report.Mode == ImportMode.Smart ? "вставка/обновление" : "массированный";
            _ltrElapsed.Text = report.Elapsed.ToString();
            _ltrFound.Text = report.TotalCount.ToString();
            _ltrSkipped.Text = report.InvalidCount.ToString();
            _ltrRemoved.Text = report.Counters.Deleted.ToString();
            _ltrAdded.Text = report.Counters.Added.ToString();
            _ltrUpdated.Text = report.Counters.Updated.ToString();
            
            if (report.HasErrors)
            {
                _rptDbErrors.DataSource = report.Errors;
                _rptDbErrors.DataBind();
            }

            if (report.HasValidationErrors)
            {
                _rptValidationErrors.DataSource = report.ValidationErrors;
                _rptValidationErrors.DataBind();
            }
        }

        protected void _rptValidationErrors_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var rptDetails = (Repeater)e.Item.FindControl("_rptDetails");
                rptDetails.DataSource = ((ValidationError)e.Item.DataItem).Details;
                rptDetails.DataBind();
            }
        }

        protected string DisplayFailReason(ValidationFailReason reason)
        {
            switch (reason)
            {
                case ValidationFailReason.NullabilityViolation:
                    return "отсутствует значение обязательного столбца";
                case ValidationFailReason.TypeConvertionFailure:
                    return "некорректный формат данных";
                case ValidationFailReason.MaxLengthViolation:
                    return "размер данных столбца превышает максимальную длину";
                default:
                    return string.Empty;
            }
        }

        private string SuppliersList(IEnumerable<int> ids)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (int id in ids)
            {
                if (sb.Length > 0)
                    sb.Append(',');
                sb.Append(id.ToString());
            }
            return sb.ToString();
        }
    }
}