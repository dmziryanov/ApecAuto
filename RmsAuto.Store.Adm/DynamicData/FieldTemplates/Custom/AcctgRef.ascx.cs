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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates
{
    public partial class AcctgRef : AcctgRefBase
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
            if (FieldValue != null)
            {
                var refItem = AcctgRefCatalog.GetReference(_RefName)[FieldValue];
                if (refItem != null)
                {
                    var propDesc = TypeDescriptor.GetProperties(refItem)[_DisplayField];
                    if (propDesc == null)
                        throw new Exception(_DisplayField + " property no found");
                    Literal1.Text = propDesc.GetValue(refItem).ToString();
                }
                else
                    Literal1.Text = "элемент справочника не найден";
            }
        }
    }
}
