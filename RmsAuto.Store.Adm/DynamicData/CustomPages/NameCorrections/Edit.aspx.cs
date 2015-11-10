using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.TechDoc.Entities;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.NameCorrections
{
	public partial class Edit : Security.BasePage/*System.Web.UI.Page*/
	{
		protected MetaTable table;

		protected void Page_Init(object sender, EventArgs e)
		{
			DynamicDataManager1.RegisterControl(DetailsView1);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			table = DetailsDataSource.GetTable();
			Title = table.DisplayName;

		}

		protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
		{
			
			if (e.CommandName == DataControlCommands.CancelCommandName)
					Response.Redirect(table.ListActionPath);
		}

		protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
		{
			if (e.Exception == null || e.ExceptionHandled)
			{
				Response.Redirect(table.ListActionPath);
			}
		}

		protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
		{
			e.NewValues.Add("IsNew", false);
		}
	}
}
