using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Web;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Data;
using RmsAuto.Store.Web.Manager;

namespace RmsAuto.Store.Web.Controls
{
	public partial class ReclamationRequest : System.Web.UI.UserControl
	{
		#region ==== Fields, Properties ====
		/// <summary>
		/// Заявка
		/// </summary>
		//Здесь раньше использовалась сессия, но теперь используется кеш, так как не получилось серилиазовать Статус
        protected RmsAuto.Store.Entities.Reclamation CurrentRequest
		{
            get
            {
                string request = (string)Session["RmsAuto.Store.Web.Controls.ReclamationRequest"];
                if (String.IsNullOrEmpty(request))
                {
                    var tmpCurrentRequest = new RmsAuto.Store.Entities.Reclamation();
                    tmpCurrentRequest.OrderDate = CurrentOrderLine.Order.OrderDate;
                    Session["RmsAuto.Store.Web.Controls.ReclamationRequest"] = Serializer.Serialize(tmpCurrentRequest);
                    return tmpCurrentRequest;
                }
                return Serializer.Deserialize<RmsAuto.Store.Entities.Reclamation>(request);
            }
            set { Session["RmsAuto.Store.Web.Controls.ReclamationRequest"] = Serializer.Serialize(value); }
		}
		/// <summary>
		/// Строка по которой составляется заявка
		/// </summary>
		protected OrderLine CurrentOrderLine
		{
            get
            {
                string orderLine = (string)Session["RmsAuto.Store.Web.Controls.OrderLine"];
                if (String.IsNullOrEmpty(orderLine))
                {
                    int orderLineID = Convert.ToInt32(Request[UrlKeys.Id]);
                    var tmpOrderLine = OrderBO.LoadOrderLineData(SiteContext.Current.CurrentClient.Profile.ClientId, orderLineID, false);
                    Session["RmsAuto.Store.Web.Controls.OrderLine"] = Serializer.Serialize(tmpOrderLine);
                    return tmpOrderLine;
                }
                return Serializer.Deserialize<OrderLine>(orderLine);
            }
            set { Session["RmsAuto.Store.Web.Controls.OrderLine"] = Serializer.Serialize(value); }
		}

		/// <summary>
		/// Тип заявки (возврат/отказ)
		/// </summary>
		protected ReclamationTracking.ReclamationType ReclamationType
		{
            get { return (ReclamationTracking.ReclamationType)Session["RmsAuto.Store.Web.Controls.ReclamationType"]; }
            set { Session["RmsAuto.Store.Web.Controls.ReclamationType"] = value; }
		}


		#endregion

		#region ==== Events ====
		protected void Page_Load( object sender, EventArgs e )
		{
			if (!IsPostBack)
			{
				var tmpCurrentRequest = new RmsAuto.Store.Entities.Reclamation();
				int orderLineID = Convert.ToInt32(Request[UrlKeys.Id]);
				var tmpOrderLine = OrderBO.LoadOrderLineData(SiteContext.Current.CurrentClient.Profile.ClientId, orderLineID, false);
                this.CurrentOrderLine = tmpOrderLine;
                tmpCurrentRequest.OrderDate = tmpOrderLine.Order.OrderDate;
				if (CurrentOrderLine.CurrentStatus == OrderLineStatusUtil.StatusByte("InStock"))
				{
					ReclamationType = ReclamationTracking.ReclamationType.Rejection; // Если статус 'поступил на склад' - заявка на отказ
				}
				else if (CurrentOrderLine.CurrentStatus == OrderLineStatusUtil.StatusByte( "ReceivedByClient" ))
				{
					ReclamationType = ReclamationTracking.ReclamationType.Reclamation; // Если статус 'получен клиентом' - заявка на возврат
				}
				else
				{
					throw new BLException(Resources.Requests.BLExceptionHint/*"Оформление заявок на отказ/возврат возможно только из статусов 'поступил на склад' и 'получен клиентом'!"*/);
				}
				
                btnSendRequest.CommandName = "send";
				
                if (ReclamationType == ReclamationTracking.ReclamationType.Reclamation) // Если заявка на возврат, то еще раз проверяем валидность сроков
				{
					TimeSpan ts = DateTime.Now - CurrentOrderLine.CurrentStatusDate.Value;

					if (ts.Days > SiteContext.Current.CurrentClient.Profile.ReclamationPeriod)
					{
						btnSendRequest.CommandName = "alert";
						btnSendRequest.OnClientClick = string.Format( "window.alert('" + Resources.Requests.AlertText /*"Все рекламации принимаются не позднее {0} календарных дней со дня отгрузки товара!"*/+"');",
							SiteContext.Current.CurrentClient.Profile.ReclamationPeriod );
					}
				}
                InitControls(tmpCurrentRequest);
                CurrentRequest = tmpCurrentRequest;
			}
		}

		protected void btnSendRequest_Click( object sender, EventArgs e )
		{
			Button btn = (Button)sender;
			if (btn.CommandName == "send" && Page.IsValid)
			{
                RmsAuto.Store.Entities.Reclamation tmpCurrentRequest = CurrentRequest;
				tmpCurrentRequest.ContactPerson = txtContactPerson.Text.Trim();
				tmpCurrentRequest.ContactPhone = txtContactPhone.Text.Trim();
				tmpCurrentRequest.Qty = Convert.ToInt32(txtQty.Text.Trim());
                
				tmpCurrentRequest.ReclamationReason = ddlReclamationReason.SelectedItem.Text;
				tmpCurrentRequest.ReclamationDescription = txtReclamationDescription.Text.Trim();

				if (ReclamationType == ReclamationTracking.ReclamationType.Reclamation)
				{
					//tmpCurrentRequest.Torg12Number = txtTorg12Number.Text.Trim();
					tmpCurrentRequest.InvoiceNumber = txtInvoiceNumber.Text.Trim();
					tmpCurrentRequest.Torg12Price = Convert.ToDecimal( txtTorg12Price.Text.Trim() );
				}
				tmpCurrentRequest.CurrentStatus = 10; // Присваиваем статус "отправлено"
				tmpCurrentRequest.CurrentStatusDate = DateTime.Now;

				tmpCurrentRequest.sReclamationNumber = string.Empty;

				ReclamationTracking.SendReclamation(tmpCurrentRequest);
								
				//Response.Redirect( UrlManager.GetReclamationUrl( "list" ), true );
				string url = string.Empty;
				switch (SiteContext.Current.User.Role)
				{
					case SecurityRole.Client:
						url = UrlManager.GetReclamationUrl( "list" );
						break;
					case SecurityRole.Manager:
						url = ClientReclamations.GetUrl();
						break;
					default:
						throw new Exception( "Unknown user role" );
				}
                CurrentRequest = tmpCurrentRequest;
				Response.Redirect( url, true );
			}
		}
		#endregion

		#region ==== Validators ====
		protected void ValidateQty( object source, ServerValidateEventArgs args )
		{
			int res = 0;
			if (int.TryParse( args.Value.Trim(), out res ))
			{
				if (res > 0 && res <= CurrentOrderLine.Qty)
				{
					args.IsValid = true;
					return;
				}
			}
			args.IsValid = false;
		}

		protected void ValidateTorg12Price( object source, ServerValidateEventArgs args )
		{
			decimal res = 0;
			if (decimal.TryParse( args.Value.Trim(), out res ))
			{
				if (res > 0)
				{
					args.IsValid = true;
					return;
				}
			}
			args.IsValid = false;
		}

		protected void ValidateReclamationReason( object source, ServerValidateEventArgs args )
		{
			if (args.Value != "noselected")
			{
				args.IsValid = true;
				return;
			}
			args.IsValid = false;
		}
		#endregion

		#region ==== Helpers ====
		/// <summary>
		/// Инициализация контролов и текущей заявки известными значениями
		/// </summary>
        private void InitControls(RmsAuto.Store.Entities.Reclamation tmpCurrentRequest)
		{
			if (ReclamationType == ReclamationTracking.ReclamationType.Reclamation) // Если заявка на возврат
			{
				_HeaderText.Text = Resources.Reclamations.ReturnRequest;//"Заявка на возврат";
				_Qty.Text = Resources.Reclamations.ReturnQty;//"Количество на возврат";
				_ReclamationReason.Text = Resources.Reclamations.ReturnReason;//"Причина возврата";
				_ReclamationDescription.Text = Resources.Reclamations.ReturnReasonFull;//"Полное описание причины возврата товара";
				phTorg12Number.Visible = phTorg12Price.Visible = true;
				_lSupplyDate.Text = Resources.Reclamations.ReturnInvoiceDate;//"Дата накладной";
				_SupplyDate.Text = CurrentOrderLine.CurrentStatusDate.Value.ToString( "dd.MM.yyyy" );
			}
			else // Если заявка на отказ
			{
				_HeaderText.Text = Resources.Reclamations.RefusalRequest;//"Заявка на отказ";
				_Qty.Text = Resources.Reclamations.RefusalQty;//"Количество на отказ от получения";
				_ReclamationReason.Text = Resources.Reclamations.RefusalReason;//"Причина отказа";
				_ReclamationDescription.Text = Resources.Reclamations.RefusalReasonFull;//"Полное описание причины отказа от получения со склада";
				phUnitQty.Visible = true;
				_lSupplyDate.Text = Resources.Reclamations.RefusalDeliveryDate;//"Дата поступления товара на склад";
				tmpCurrentRequest.UnitQty = CurrentOrderLine.Qty; _UnitQty.Text = CurrentOrderLine.Qty.ToString();
			}

			tmpCurrentRequest.SupplyDate = CurrentOrderLine.CurrentStatusDate;
			tmpCurrentRequest.ReclamationType = ReclamationType;

			_ClientName.Text = tmpCurrentRequest.ClientName = SiteContext.Current.CurrentClient.Profile.ClientName;
			_ClientID.Text = tmpCurrentRequest.ClientID = SiteContext.Current.CurrentClient.Profile.ClientId;
			_ManagerName.Text = tmpCurrentRequest.ManagerName = GetManagerName( SiteContext.Current.CurrentClient.Profile.ManagerId );
			tmpCurrentRequest.ReclamationDate = DateTime.Now; _ReclamationDate.Text = DateTime.Now.ToString( "dd.MM.yyyy" );
			tmpCurrentRequest.OrderID = CurrentOrderLine.OrderID; _OrderID.Text = CurrentOrderLine.OrderID.ToString();
            _OrderDate.Text = tmpCurrentRequest.OrderDate.ToString("dd.MM.yyyy");
			tmpCurrentRequest.EstSupplyDate = CurrentOrderLine.EstSupplyDate; _EstSupplyDate.Text = CurrentOrderLine.EstSupplyDate.HasValue ? CurrentOrderLine.EstSupplyDate.Value.ToString( "dd.MM.yyyy" ) : string.Empty;
			_Manufacturer.Text = tmpCurrentRequest.Manufacturer = CurrentOrderLine.Manufacturer;
			_PartNumber.Text = tmpCurrentRequest.PartNumber = CurrentOrderLine.PartNumber;
			_PartName.Text = tmpCurrentRequest.PartName = CurrentOrderLine.PartName;
			tmpCurrentRequest.SupplierID = CurrentOrderLine.SupplierID;
			tmpCurrentRequest.UnitPrice = CurrentOrderLine.UnitPrice; _UnitPrice.Text = CurrentOrderLine.UnitPrice.ToString();
			tmpCurrentRequest.OrderLineID = CurrentOrderLine.OrderLineID;
			tmpCurrentRequest.AcctgOrderLineID = CurrentOrderLine.AcctgOrderLineID;
		}

		private string GetManagerName( string managerID )
		{
			try
			{
				//TODO переписать в дальнейшем (возможно добавить метод в BL для извлечения только имени менеджера)
				return RmsAuto.Store.Acctg.ClientProfile.Load( managerID ).ClientName;
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
		#endregion
	}
}