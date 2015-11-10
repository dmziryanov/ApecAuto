using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Adm
{
	public partial class OrderResendMgmt : Security.BasePage
	{
		protected override void  OnPreRender(EventArgs e)
		{
 			base.OnPreRender(e);
			//Данный функционал устарел, т.к. теперь отправка заказа идет не через веб-сервис
			this.Visible = false;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				_txtOrderId.Text = string.Empty;
				_lblInfoText.Text = string.Empty;
			}
		}

		protected void _btnResendOrder_Click(object sender, EventArgs e)
		{
			try
			{
				OrderBO.ResendOrder(Convert.ToInt32(_txtOrderId.Text));

				_lblInfoText.Text = "Заказ успешно отправлен.";
				_lblInfoText.Style.Add(HtmlTextWriterStyle.Color, "Green");
			}
			catch (Exception ex)
			{
				_lblInfoText.Text = "Ошибка отправки заказа!";
				_lblInfoText.Style.Add(HtmlTextWriterStyle.Color, "Red");
			}
		}
	}
}
