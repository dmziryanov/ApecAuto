extern alias DataStreams1;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DataStreams1.DataStreams.Common;
using DataStreams1.DataStreams.Xls;
using DataStreams1.DataStreams.Xlsx;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using RmsAuto.Store.Web.Manager.BasePages;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Manager
{
	[LightPage]
	public partial class UploadStatuses : RMMPage
	{
        private List<TempOLStatuse> _res = new List<TempOLStatuse>();

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			pButtons.Visible = false;
			lSummaryInfo.Visible = false;
			lPreloadInfo.Visible = false;
		}

		public string GetTemplateUrl()
		{
            string templateFileName = "statusTemplate_eng.xlsx";
			int fileID = FilesDac.GetFileIDByName(templateFileName);
			return UrlManager.GetFileUrl(fileID);
		}
		
        protected void Page_Load(object sender, EventArgs e)
		{
		}

        private void ShowMessage(string message)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>");
        }

		/// <summary>
		/// Загрузка Excel файла и его разбор
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnUpload_Click(object sender, EventArgs e)
		{
			HttpPostedFile file = fuMain.PostedFile;
			if (string.IsNullOrEmpty(fuMain.FileName))
			{
				AddError(ErrorReason.FileNotSpecified, "");
                ShowMessage("File is not choosen");
				return;
			}
			if (!fuMain.HasFile)
			{
				AddError(ErrorReason.FileNotFoundOrEmpty, file.FileName);
				return;
			}

			bool xlsx;

			switch (Path.GetExtension(file.FileName))
			{
				case ".xls": xlsx = false; break;
				case ".xlsx": xlsx = true; break;
				default:
					{
						AddError(ErrorReason.InvalidFileFormat, "");
						return;
					}
			}
			
            SpreadsheetReader reader = null;
			try
			{
				reader = xlsx ? (SpreadsheetReader)(new XlsxReader(file.InputStream))
					: (SpreadsheetReader)(new XlsReader(file.InputStream));
				try
				{
					//разбор загруженного файла
					LoadStatusesFromReader(reader);
					//зачищаем старые данные
					LightBO.DeleteOrderLinesNewStatuses(SiteContext.Current.User.UserId);
					//закидываем новые данные
					LightBO.InsertOrderLinesNewStatuses(_res);
					//цепляем загруженные данные с имеющимися строками и выводим
					var items = LightBO.SelectOrderLinesNewStatuses(SiteContext.Current.User.UserId);
					if (items.Count() > 0)
					{
						int successCount = items.Where(i => i.CurrentStatus.HasValue).Count();
						lPreloadInfo.Text = string.Format("Quantity of lines found by loaded data = {0}", successCount);
						lPreloadInfo.Visible = true;
						pButtons.Visible = true;
						rptMain.DataSource = items;
						rptMain.DataBind();
					}
				}
				catch (Exception ex)
				{
				    AddError(ErrorReason.UnknownFailure, ex.Message);
				}
			}
			catch(Exception ex)
			{
				AddError(ErrorReason.ReaderFailed, file.FileName);
			}
			finally
			{
				if (reader != null) reader.Close();
			}
		}

		/// <summary>
        /// Разбор Excel файла
        /// </summary>
        /// <param name="reader">объект чтения</param>
		private void LoadStatusesFromReader(SpreadsheetReader reader)
		{
			//проверка наличия в файле единственного листа с заказом
			reader.CurrentSheet = 0;
			if (reader.SheetCount > 1)
			{
				AddError(ErrorReason.SingleSheetRequired, "");
				return;
			}

			int numOfFields = 9;
			
			// ID текущего менеджера по идее
			int managerUserID = SiteContext.Current.User.UserId;
			
			for (int r = 1; r < reader.RecordCount; r++)
			{
				string[] rec = new string[numOfFields];
				for (int i = 0; i < numOfFields; i++)
				{
				    rec[i] = reader[r, i];
				}

				rec = rec.Each(s => s.Trim()).ToArray();

				if (rec.All(s => s == "")) { continue; }

				if (rec.All(s => string.IsNullOrEmpty(s))) { break; }
				
				_res.Add(new TempOLStatuse()
				{
					ManagerUserID = managerUserID,
					SupplierID = rec[1],
					Manufacturer = rec[2],
					PartNumber = rec[3],
					Qty = rec[4],
					AcctgOrderLineID = rec[5],
					CurrentStatus = byte.Parse(rec[6]),
                    NewStatus = rec[7],
					OrderID = rec[8],
                    
				});
			}
		}
		private void AddError(ErrorReason reason, string arg)
		{
			lSummaryInfo.Text = lSummaryInfo.Text + TextError(reason, arg) + Environment.NewLine;
		}

	    public enum ErrorReason
		{
			FileNotSpecified, FileNotFoundOrEmpty, InvalidFileFormat,
			ReaderFailed, SingleSheetRequired, 
			InvalidSupplierID, InvalidManufacturer, InvalidPartNumber, InvalidQuantity, InvalidAcctgOrderLineID,
			InvalidStatus, InvalidOrderID,
			UnknownFailure
		}

		private string TextError(ErrorReason reason, string args)
		{
			//TODO implement this method
			return string.Empty;
		}

		protected string GetCurrentStatusName(object status)
		{
			byte statusID = 0;
			if (status == null)
			{
				return "<span style='color:Red'>NOT FOUND</span>";
			}
			else
			{
				if (byte.TryParse(status.ToString(), out statusID))
				{
					return OrderLineStatusUtil.DisplayName(statusID);
				}
				else
				{
					return "-";
				}
			}
		}

		protected string GetNewStatusName(string status)
		{
			byte statusID = 0;
			if(byte.TryParse(status, out statusID))
			{	
				return OrderLineStatusUtil.DisplayName(statusID);
			}
			else
			{
				return "-";
			}
		}

		protected void lbClear_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Manager/UploadStatuses.aspx");
		}

		protected void lbUpdate_Click(object sender, EventArgs e)
		{
			int cnt = LightBO.UpdateOrderLinesNewStatuses(SiteContext.Current.User.UserId);

			rptMain.Visible = false;
			pButtons.Visible = false;
			lSummaryInfo.Text = string.Format("Succssefully updated {0} lines", cnt);
			lSummaryInfo.Visible = true;
		}
	}
}
