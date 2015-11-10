using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates
{
    public partial class DecimalCorrection_EditField 
        : System.Web.DynamicData.FieldTemplateUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.ToolTip = Column.Description;
            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(DynamicValidator1);
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            string text = null;
            if (!string.IsNullOrEmpty(TextBox1.Text))
            {
                var percent = int.Parse(TextBox1.Text);
                if (Math.Abs(percent) > 100)
                    throw new OverflowException("Percent overflow");
                text = (1.0m + (decimal)percent/100).ToString("#0.##");
            }
            dictionary[Column.Name] = ConvertEditedValue(text);
        }

        public override Control DataControl
        {
            get
            {
                return TextBox1;
            }
        }

        protected string PercentEditString
        {
            get
            {
                if (FieldValue != null)
                {
                    var d = (decimal)FieldValue;
                    return ((d - 1) * 100).ToString("##0");
                }
                else
                    return string.Empty;
            }
        }
    }
}
