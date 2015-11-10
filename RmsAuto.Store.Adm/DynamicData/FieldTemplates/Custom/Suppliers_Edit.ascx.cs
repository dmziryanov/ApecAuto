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
using RmsAuto.TechDoc.Entities.TecdocBase;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates.Custom
{
	public partial class Suppliers_EditField : System.Web.DynamicData.FieldTemplateUserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			DropDownList1.ToolTip = Column.Description;

			if (DropDownList1.Items.Count == 0)
			{
				DropDownList1.Items.Add(new ListItem("[не выбран]", ""));
				foreach (Supplier sup in TechDoc.Facade.GetSuppliers())
				{
					DropDownList1.Items.Add(new ListItem(sup.Name, sup.Name));
				}

			}
		}

		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);
			if (Mode == DataBoundControlMode.Edit)
			{
				ListItem item = DropDownList1.Items.FindByValue(FieldValue.ToString());
				if (item != null)
					DropDownList1.SelectedValue = FieldValue.ToString();
			}
		}

		protected override void ExtractValues(IOrderedDictionary dictionary)
		{
			dictionary[Column.Name] = ConvertEditedValue(DropDownList1.Text);
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
