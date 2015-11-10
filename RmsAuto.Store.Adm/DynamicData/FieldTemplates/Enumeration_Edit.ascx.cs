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

using RmsAuto.Common.Misc;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates
{
    public partial class Enumeration_EditField : System.Web.DynamicData.FieldTemplateUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownList1.ToolTip = Column.Description;

            if (DropDownList1.Items.Count == 0)
            {
                if (!Column.IsRequired)
                {
                    DropDownList1.Items.Add(new ListItem("[не выбран]", ""));
                }
                
				foreach (object value in Enum.GetValues(Column.ColumnType))
                {
                    DropDownList1.Items.Add(new ListItem(((Enum)value).ToTextOrName(), value.ToString()));
                }

                var filter = Column.Attributes.OfType<StaticFilterAttribute>().FirstOrDefault();
				if (filter != null)
				{
					DropDownList1.SelectedValue = Enum.ToObject(Column.ColumnType, filter.Value).ToString();
					DropDownList1.Enabled = false;
			    }
            }
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
			if( DropDownList1.SelectedValue != "" )
			{
			    dictionary[ Column.Name ] = Enum.Parse( Column.ColumnType, DropDownList1.SelectedValue );
			}
			else
			{
				dictionary[ Column.Name ] = null;
			}
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

			if( Mode == DataBoundControlMode.Edit && DropDownList1.Enabled && FieldValue != null )
			{
				DropDownList1.SelectedValue = FieldValue.ToString();
			}
        }

        public override Control DataControl
        {
            get
            {
                return DropDownList1;
            }
        }
    }
}
