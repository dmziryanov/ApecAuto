using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
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
    public partial class DecimalCorrection : System.Web.DynamicData.FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get
            {
                return Literal1;
            }
        }
        
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            string text = string.Empty;
            if (FieldValue != null)
            {
                var d = (decimal)FieldValue;
                text = string.Format("{0:#0.##%}", d - 1); 
            }
            Literal1.Text = text;
        }
    }
}
