using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.NameCorrections
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
			// Disable various options if the table is readonly
			if (table.IsReadOnly)
			{
				GridView1.Columns[0].Visible = false;
				//InsertHyperLink.Visible = false;
			}
		}

		protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
		{
			GridView1.PageIndex = 0;
		}
	}
}
