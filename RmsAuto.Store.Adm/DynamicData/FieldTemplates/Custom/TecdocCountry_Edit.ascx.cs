using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using RmsAuto.TechDoc.Entities;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates
{
	public partial class TecdocCountry_EditField : FieldTemplateUserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			DropDownList1.ToolTip = Column.Description;

			SetUpValidator(RequiredFieldValidator1);
			SetUpValidator(RegularExpressionValidator1);
			SetUpValidator(DynamicValidator1);
			if (DropDownList1.Items.Count == 0)
			{
				DropDownList1.Items.Add(new ListItem("[не выбрана]", ""));
                //TechDoc.Facade.GetCountries().Values.Each(c => DropDownList1.Items.Add(new ListItem(c.Name.Text, c.ID.ToString())));
                //foreach (Country country in TechDoc.Facade.GetCountries())
                //{
                //    DropDownList1.Items.Add(new ListItem(country.Name, country.ID.ToString()));
                //}
				//DropDownList1.DataSource = TechDoc.Facade.GetCountries();
				//DropDownList1.DataBind();
			}
		}

		protected override void ExtractValues(IOrderedDictionary dictionary)
		{
			dictionary[Column.Name] = ConvertEditedValue(DropDownList1.SelectedValue);
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
