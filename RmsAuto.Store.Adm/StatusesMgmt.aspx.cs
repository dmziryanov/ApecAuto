using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Adm.scripts;
using System.Globalization;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm
{
	public partial class StatusesMgmt : Security.BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ScriptsManager.RegisterJQuery( this );
			if (!IsPostBack)
			{
                using (DCWrappersFactory<StoreDataContext> dc = new DCWrappersFactory<StoreDataContext>())
				{
					foreach (var statusElement in dc.DataContext.OrderLineStatuses)
					{
						byte status = statusElement.OrderLineStatusID;
						if (status != OrderLineStatusUtil.StatusByte("Rejected"))
						{
							string displayName = OrderLineStatusUtil.DisplayName(status);
							_ddlStatuses.Items.Add(new ListItem(displayName, status.ToString()));
						}
					}
				}
			}
		}

		protected void _btnGetOrderLines_Click(object sender, EventArgs e)
		{
			FillData();
		}

		protected void _btnChangeStatus_Click(object sender, EventArgs e)
		{
            int orderID = -1; int.TryParse(_txtOrderID.Text.Trim(), out orderID); if (orderID < 0) return;
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                for (int i = 0; i < _gvOrderLines.Rows.Count; i++)
                {
                    if (!((CheckBox)_gvOrderLines.Rows[i].Cells[0].Controls[0]).Checked) continue;
                    DataControlFieldCell cb = (DataControlFieldCell)_gvOrderLines.Rows[i].Cells[2];
                    int orderLineID; try { orderLineID = Convert.ToInt32(cb.Text); }
                    catch { continue; }
                    OrderLine line = dc.DataContext.OrderLines.FirstOrDefault(l => l.OrderLineID == orderLineID && l.OrderID == orderID);
                    if (line != null && line.CurrentStatus != OrderLineStatusUtil.StatusByte("Rejected"))
                    {
                        byte newStatus = 0;
                        byte.TryParse(_ddlStatuses.SelectedValue, out newStatus);
                        if (newStatus > 0)
                        {
                            //новая дата статуса
                            DateTime orderLineStatusDate;
                            try { orderLineStatusDate = DateTime.ParseExact(_txtStatusDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture); }
                            catch { orderLineStatusDate = DateTime.Now; }
                            line.CurrentStatus = newStatus;
                            line.CurrentStatusDate = orderLineStatusDate;
                        }
                    }
                }
                dc.DataContext.SubmitChanges();
			}

			FillData();
        }

        protected void _ibEdit_Click( object sender, ImageClickEventArgs e )
		{
			ImageButton ib = (ImageButton)sender;
			if (ib.CommandName == "edit")
			{
				int orderLineId = Convert.ToInt32( ib.CommandArgument );

                using (var dc = new DCWrappersFactory<StoreDataContext>())
				{
					var orderLine = dc.DataContext.OrderLines.Where( l => l.OrderLineID == orderLineId ).SingleOrDefault();
					if (orderLine != null)
					{
						_txtOrderID.Text = orderLine.OrderID.ToString();
						_ddlStatuses.SelectedValue = orderLine.CurrentStatus.ToString();
						_txtStatusDate.Text = orderLine.CurrentStatusDate.HasValue ? orderLine.CurrentStatusDate.Value.ToString( "dd.MM.yyyy" ) : string.Empty;
					}
				}
			}
		}

		protected void _gvOrderLines_RowEditing( object sender, GridViewEditEventArgs e )
		{
		}

		private void FillData()
		{
			int orderID = -1;
			int.TryParse(_txtOrderID.Text.Trim(), out orderID);

			if (orderID > 0)
			{
				List<OrderLine> orderLines = new List<OrderLine>();
                using (var context = new DCWrappersFactory<StoreDataContext>())
				{
					orderLines = context.DataContext.OrderLines.Where(l => l.OrderID == orderID && l.CurrentStatus != OrderLineStatusUtil.StatusByte("Rejected")).ToList();
				}
				_gvOrderLines.DataSource = orderLines;
				_gvOrderLines.DataBind();
			}
		}
	}
}
