using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web.PrivateOffice
{
	public partial class ReclamationPrint : LocalizablePage
	{
		protected void Page_PreRender( object sender, EventArgs e )
		{
			Response.Cache.SetCacheability( HttpCacheability.NoCache );

			var reclamationId = Convert.ToInt32( Request["id"] );
			var client = SiteContext.Current.CurrentClient;

			RmsAuto.Store.Entities.Reclamation rec = ReclamationTracking.GetReclamation( client.Profile.ClientId, reclamationId );

			if (rec == null)
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not Found" );

			if (rec.ReclamationType == ReclamationTracking.ReclamationType.Reclamation) // Если заявка на возврат
			{
				_HeaderText.Text = Resources.Reclamations.ReturnRequest; //"Заявка на возврат";
				_lQty.Text = Resources.Reclamations.ReturnQty; //"Количество на возврат";
				_lReclamationReason.Text = Resources.Reclamations.ReturnReason; //"Причина возврата";
				_lReclamationDescription.Text = Resources.Reclamations.ReturnReasonFull; //"Полное описание причины возврата товара";
			    phTorg12Number.Visible = phTorg12Price.Visible = true;
				_lSupplyDate.Text = Resources.Reclamations.ReturnInvoiceDate; //"Дата накладной";
			}
			else // Если заявка на отказ
			{
				_HeaderText.Text = Resources.Reclamations.RefusalRequest; //"Заявка на отказ";
				_lQty.Text = Resources.Reclamations.RefusalQty; //"Количество на отказ от получения";
				_lReclamationReason.Text = Resources.Reclamations.RefusalReason; //"Причина отказа";
				_lReclamationDescription.Text = Resources.Reclamations.RefusalReasonFull; //"Полное описание причины отказа от получения со склада";
			    phUnitQty.Visible = true;
				_lSupplyDate.Text = Resources.Reclamations.RefusalDeliveryDate; //"Дата поступления товара на склад";
			}

			_ManagerName.Text = rec.ManagerName;
			_ClientName.Text = client.Profile.ClientName;
			_ClientID.Text = client.Profile.ClientId;
			_ContactPerson.Text = rec.ContactPerson;
			_ContactPhone.Text = rec.ContactPhone;
			_ReclamationDate.Text = rec.ReclamationDate.ToString( "dd.MM.yyyy" );

			_OrderID.Text = rec.OrderID.ToString();
			_OrderDate.Text = rec.OrderDate.ToString( "dd.MM.yyyy" );
			_EstSupplyDate.Text = rec.EstSupplyDate.HasValue ? rec.EstSupplyDate.Value.ToString( "dd.MM.yyyy" ) : string.Empty;
			if (phTorg12Number.Visible)
			{
				//_Torg12Number.Text = rec.Torg12Number;
				_InvoiceNumber.Text = rec.InvoiceNumber;
			}
			_SupplyDate.Text = rec.SupplyDate.HasValue ? rec.SupplyDate.Value.ToString( "dd.MM.yyyy" ) : string.Empty;

			_Manufacturer.Text = rec.Manufacturer;
			_PartNumber.Text = rec.PartNumber;
			_PartName.Text = rec.PartName;
			_Qty.Text = rec.Qty.ToString();
			_UnitPrice.Text = string.Format( "{0:### ### ##0.00}", rec.UnitPrice );
			if (phUnitQty.Visible) { _UnitQty.Text = rec.UnitQty.ToString(); }
			if (phTorg12Price.Visible) { _Torg12Price.Text = rec.Torg12Price.HasValue ? string.Format( "{0:### ### ##0.00}", rec.Torg12Price.Value ) : string.Empty; }

			_ReclamationReason.Text = rec.ReclamationReason;
			_ReclamationDescription.Text = rec.ReclamationDescription;
		}
	}
}
