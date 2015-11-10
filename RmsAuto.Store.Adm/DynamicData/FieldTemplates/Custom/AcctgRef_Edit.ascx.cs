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
using System.ComponentModel.DataAnnotations;

using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates
{
    public partial class AcctgRef_EditField : AcctgRefBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownList1.ToolTip = Column.Description;

            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(DynamicValidator1);

            if (DropDownList1.Items.Count == 0)
            {
                if (!Column.IsRequired)
                {
                    DropDownList1.Items.Add(new ListItem("[не выбран]", ""));
                }
                PopulateDropDownList();
            }
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            dictionary[Column.Name] = ConvertEditedValue(DropDownList1.SelectedValue);
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            if (Mode == DataBoundControlMode.Edit && FieldValue != null)
            {
                ListItem item = DropDownList1.Items.FindByValue(this.FieldValue.ToString());
                if (item != null)
                {
                    DropDownList1.SelectedValue = FieldValue.ToString();
                }
            }
        }

        public override Control DataControl
        {
            get
            {
                return DropDownList1;
            }
        }

        private void PopulateDropDownList()
        {
			foreach( var item in AcctgRefCatalog.GetReference(_RefName).Items )
			{
				DropDownList1.Items.Add( new ListItem( Convert.ToString( DataBinder.Eval( item, _DisplayField ) ),
					Convert.ToString( DataBinder.Eval( item, _KeyField ) ) ) );

			}
        }
    }
}
