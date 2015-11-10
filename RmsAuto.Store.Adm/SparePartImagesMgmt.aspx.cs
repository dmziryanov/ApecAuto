using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm
{
	public partial class SparePartImagesMgmt : Security.BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            string manufacturer = Page.Request.QueryString["manufacturer"];
            string partNumber = Page.Request.QueryString["partnumber"];
            string supplierId = Page.Request.QueryString["supplierid"];
            string imageNumber = Page.Request.QueryString["imagenumber"];

            if (partNumber != null)
            {
                _txtPartNumber.Text = partNumber;
            }
            if (manufacturer != null)
            {
                _txtManufacturer.Text = manufacturer;
            }
            if (supplierId != null)
            {
                _ddlSupplierID.SelectedValue = supplierId;
            }
		}

		protected void _btnSave_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
                using (var dc = new DCWrappersFactory<StoreDataContext>(false))
				{
					TextBox txtImageNumber = new TextBox();
					TextBox txtDescription = new TextBox();
					FileUpload fuImage = new FileUpload();
					// Выполняем в транзакции
					dc.DataContext.Connection.Open();
					dc.DataContext.Transaction = dc.DataContext.Connection.BeginTransaction();
					try
					{
						for (int i = 1; i <= 5; i++)
						{
							SetControls(i, out txtImageNumber, out txtDescription, out fuImage);
							if (fuImage.HasFile)
							{
								SparePartImage spiSource = dc.DataContext.SparePartImages.SingleOrDefault(item => item.Manufacturer == _txtManufacturer.Text.Trim() &&
																				item.PartNumber == _txtPartNumber.Text.Trim() &&
																				item.SupplierID == Convert.ToInt32(_ddlSupplierID.SelectedValue) &&
																				item.ImageNumber == Convert.ToInt32(txtImageNumber.Text));
								if (spiSource != null) // т.е. пытаемся обновить существующую запись
								{
									// сначала удаляем старую, т.к. Primary Key на полях не позволит их обновить, только пересоздать
									dc.DataContext.SparePartImages.DeleteOnSubmit(spiSource);
									dc.DataContext.SubmitChanges();
								}
								// создаем новую запись
								SparePartImage spi = new SparePartImage();
								spi.Manufacturer = _txtManufacturer.Text.Trim();
								spi.PartNumber = _txtPartNumber.Text.Trim();
								spi.SupplierID = Convert.ToInt32(_ddlSupplierID.SelectedValue);
								spi.ImageNumber = Convert.ToInt32(txtImageNumber.Text);
								spi.Description = txtDescription.Text;
								spi.ImageBody = fuImage.FileBytes;

								dc.DataContext.SparePartImages.InsertOnSubmit(spi);
							}
						}

						dc.DataContext.SubmitChanges();
						dc.DataContext.Transaction.Commit();

						_lblInfo.Text = "Фотографии успешно сохранены.";
						_lblInfo.Style.Add(HtmlTextWriterStyle.Color, "Green");
					}
					catch (Exception ex)
					{
						dc.DataContext.Transaction.Rollback();
						_lblInfo.Text = "Ошибка при сохранении фотографий!";
						_lblInfo.Style.Add(HtmlTextWriterStyle.Color, "Red");
					}
				}

                // перенаправляем на список всех фотографий деталий
                HttpContext.Current.Response.Redirect("ListSparePartImagesMgmt.aspx");
			}
		}

		protected void Uploaders_ServerValidate(object source, ServerValidateEventArgs args)
		{
			CustomValidator cv = (CustomValidator)source;
			FileUpload fuImage = new FileUpload();
			TextBox txtImageNumber = new TextBox();
			switch (cv.ControlToValidate)
			{
				case "_fuImage1":
					txtImageNumber = _txtImageNumber1;
					fuImage = _fuImage1;
					break;
				case "_fuImage2":
					txtImageNumber = _txtImageNumber2;
					fuImage = _fuImage2;
					break;
				case "_fuImage3":
					txtImageNumber = _txtImageNumber3;
					fuImage = _fuImage3;
					break;
				case "_fuImage4":
					txtImageNumber = _txtImageNumber4;
					fuImage = _fuImage4;
					break;
				case "_fuImage5":
					txtImageNumber = _txtImageNumber5;
					fuImage = _fuImage5;
					break;
			}

			int res = -1;
			if ((fuImage.FileBytes.Length / 1024) > 300) //размер загружаемой фотографии не должен превышать 300 Кбайт
			{
				args.IsValid = false;
				cv.Text = "Размер загружаемого файла не должен превышать 300 Кбайт";
			}
			else if (System.IO.Path.GetExtension(fuImage.FileName).ToLower() != ".jpg")
			{
				args.IsValid = false;
				cv.Text = "Неверное расширение файла";
			}
			else if (string.IsNullOrEmpty(txtImageNumber.Text))
			{
				args.IsValid = false;
				cv.Text = "Поле '№' не может быть пустым";
			}
			else if(!int.TryParse(txtImageNumber.Text, out res))
			{
				args.IsValid = false;
				cv.Text = "Поле '№' должно быть целым числом";
			}
		}

		private void SetControls(int index, out TextBox txtIN, out TextBox txtDesc, out FileUpload fu)
		{
			txtIN = new TextBox();
			txtDesc = new TextBox();
			fu = new FileUpload();
			switch (index)
			{
				case 1:
					txtIN = _txtImageNumber1;
					txtDesc = _txtDescription1;
					fu = _fuImage1;
					break;
				case 2:
					txtIN = _txtImageNumber2;
					txtDesc = _txtDescription2;
					fu = _fuImage2;
					break;
				case 3:
					txtIN = _txtImageNumber3;
					txtDesc = _txtDescription3;
					fu = _fuImage3;
					break;
				case 4:
					txtIN = _txtImageNumber4;
					txtDesc = _txtDescription4;
					fu = _fuImage4;
					break;
				case 5:
					txtIN = _txtImageNumber5;
					txtDesc = _txtDescription5;
					fu = _fuImage5;
					break;
			}
		}
	}
}
