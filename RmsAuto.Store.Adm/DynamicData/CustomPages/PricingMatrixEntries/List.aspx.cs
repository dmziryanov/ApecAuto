using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;
using RmsAuto.Common.Web;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.PricingMatrixEntries
{
	public partial class List : Security.BasePage/*System.Web.UI.Page*/
	{
        private const string _supplierIdField = "ddlSupplierID";
        private const string _manufacturerField = "tbManufacturer";
        private const string _rgCodeField = "RgCodeSpec";
        private const string _partNumberField = "tbPartNumber";

		protected MetaTable table;

		protected void Page_Init(object sender, EventArgs e)
		{
			DynamicDataManager1.RegisterControl(GridView1, true /*setSelectionFromUrl*/);
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			table = GridDataSource.GetTable();
			Title = table.DisplayName;
			InsertHyperLink.NavigateUrl = table.GetActionPath(PageAction.Insert);

			// Disable various options if the table is readonly
			if (table.IsReadOnly)
			{
				GridView1.Columns[0].Visible = false;
				InsertHyperLink.Visible = false;
			}
            
			if (!Page.IsPostBack)
			{
                InitSupplierList();
                InitFilterLiterals();
                InitFilterInputs();
              	ApplySort();
			}
		}

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView1.BottomPagerRow.Visible = true;
        } 

        private void InitSupplierList()
        {
            DropDownSp.Items.Add(new ListItem("[не выбран]", "0"));
			//foreach (var item in RefCatalog.RmsSuppliers.Items.OrderBy(s => s.SupplierName))
			//{
			//    DropDownSp.Items.Add(new ListItem(item.SupplierName, item.SupplierId.ToString()));
			//}
        }

        private void InitFilterLiterals()
        {
            LiteralSp.Text = table.GetColumn(_supplierIdField).DisplayName;
            LiteralMn.Text = table.GetColumn(_manufacturerField).DisplayName;
            LiteralRg.Text = table.GetColumn(_rgCodeField).DisplayName;
            LiteralPn.Text = table.GetColumn(_partNumberField).DisplayName;
        }

        private void InitFilterInputs()
        {
            string[] FieldNames = { _manufacturerField, _rgCodeField, _partNumberField };
            TextBox[] TextBoxes = { TextBoxMn, TextBoxRg, TextBoxPn };
            RegularExpressionValidator[] Validators =
                { RegexValidatorMn, RegexValidatorRg, RegexValidatorPn };

            for (int i = 0; i < FieldNames.Length; i++)
            {
                MetaColumn column = table.GetColumn(FieldNames[i]);
                RegularExpressionAttribute regexattr = column.Attributes.
                    OfType<RegularExpressionAttribute>().FirstOrDefault();
                TextBoxes[i].MaxLength = column.MaxLength;
                if (regexattr != null)
                {
                    Validators[i].ErrorMessage = regexattr.ErrorMessage;
                    Validators[i].ValidationExpression = regexattr.Pattern;
                    Validators[i].Enabled = true;
                }
                else Validators[i].Enabled = false;
            }
        }
        
		private void ApplySort()
		{
			var sortAttr = table.Attributes.OfType<SortAttribute>().FirstOrDefault();
			if (sortAttr != null)
			{
				GridDataSource.OrderBy = sortAttr.Expression;
			}
		}

        protected void ButtonApplyFilter_Click(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
            ApplySort();
        }

        protected void GridViewPager_PageSizeChanged(object sender, EventArgs e)
        {
            ApplySort();
        }
	}
}
