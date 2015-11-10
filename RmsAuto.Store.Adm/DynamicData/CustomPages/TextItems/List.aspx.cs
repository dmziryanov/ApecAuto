using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages
{
	public partial class List : Security.BasePage/*System.Web.UI.Page*/
	{
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
				ApplyStaticFilters();
				ApplySort();
			}
		}

		protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
		{
			GridView1.PageIndex = 0;
		}

		private void ApplyStaticFilters()
		{
			foreach (var col in table.Columns)
			{
				var sfAttr = col.Attributes.OfType<StaticFilterAttribute>().FirstOrDefault();
				if (sfAttr != null)
					GridDataSource.WhereParameters.Add(new StaticParameter(col.Name, sfAttr.Value));
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

	}
}
