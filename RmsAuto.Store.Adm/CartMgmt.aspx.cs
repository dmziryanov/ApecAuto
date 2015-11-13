using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using System.IO;
using System.Text;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm
{
	public partial class CartMgmt : Security.BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void _btnGetCart_Click(object sender, EventArgs e)
		{
			string clientID = _txtClientID.Text;
			FillData(clientID);
		}

		protected void _btnExcel_Click(object sender, ImageClickEventArgs e)
		{
			string clientID = _txtClientID.Text;
			FillData(clientID);

			Response.Clear();
			Response.ClearHeaders();
			Response.ContentType = "application/vnd.ms-excel";
			Response.AddHeader("Content-Disposition", string.Format( "attachment; filename=Cart_of_{0}_{1}.xls", clientID, DateTime.Now.ToShortDateString()));

			this.EnableViewState = false;
			StringWriter sw = new StringWriter();
			HtmlTextWriter htw = new HtmlTextWriter(sw);

			Encoding win1251 = Encoding.GetEncoding(1251);
			Response.ContentEncoding = win1251;
			_gvToExcel.RenderControl(htw);/* данная фишка работает с DataGrid, но не работает с GridView */

			Response.Write(sw.ToString());
			Response.End();
		}

		protected void _btnClean_Click(object sender, ImageClickEventArgs e)
		{
			string clientID = _txtClientID.Text;
			using (var context = new DCFactory<StoreDataContext>())
			{
				var cartItems = context.DataContext.ShoppingCartItems.Where(i => i.ClientID == clientID);
				if (cartItems.Count() > 0)
				{
					context.DataContext.ShoppingCartItems.DeleteAllOnSubmit(cartItems);
					context.DataContext.SubmitChanges();
				}
			}
			
			_gvToExcel.DataSource = null;
			_gvToExcel.DataBind();
		}

		private void FillData(string clientId)
		{
			if (!string.IsNullOrEmpty(clientId))
			{
				List<ShoppingCartItem> cartItems = new List<ShoppingCartItem>();
				using (var context = new DCFactory<StoreDataContext>())
				{
					cartItems = context.DataContext.ShoppingCartItems.Where(i => i.ClientID == clientId).ToList();
				}
				_gvToExcel.DataSource = cartItems;
				_gvToExcel.DataBind();
			}
		}
	}
}
