extern alias DataStreams1;

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using DataStreams1.DataStreams.Common;
using DataStreams1.DataStreams.Xls;
using DataStreams1.DataStreams.Xlsx;
using RmsAuto.Store.Adm.Catalogs;
using RmsAuto.Store.Adm.scripts;
using RmsAuto.Store.Cms.Entities;

namespace RmsAuto.Store.Adm
{
    public partial class Disc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptsManager.RegisterMsoFramework(Page);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            LoadDiscs(FileUpload1);
        }
        private LoadResult LoadDiscs(FileUpload fileUpload)
        {
            HttpPostedFile file = fileUpload.PostedFile;
            LoadResult result = new LoadResult { FileName = file.FileName };

            bool xlsx; switch (Path.GetExtension(file.FileName))
            {
                case ".xls": xlsx = false; break;
                case ".xlsx": xlsx = true; break;
                default: return result.AddError(0, 0, "");
            }

            SpreadsheetReader reader = null;
            try
            {
                reader = xlsx ? (SpreadsheetReader)(new XlsxReader(file.InputStream))
                    : (SpreadsheetReader)(new XlsReader(file.InputStream));
                try
                {
                    LoadDiscsFromReader(reader, result);
                }
                catch (Exception)
				{
					//TODO допилить обработку ошибок
					//result.AddError(ErrorReason.UnknownFailure, 0, ex.Message);
                }
            }
            catch
            {
				//TODO допилить обработку ошибок
                //result.AddError(ErrorReason.ReaderFailed, 0, file.FileName);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return result;
        }

        private void LoadDiscsFromReader(SpreadsheetReader reader, LoadResult result)
        {
            //проверка наличия в файле единственного листа с заказом
            reader.CurrentSheet = 0;
            int numOfFields = 9;

            List<RmsAuto.Store.Cms.Entities.Disc> t = new List<RmsAuto.Store.Cms.Entities.Disc>();
            for (int r = 1; r < reader.RecordCount; r++)
            {
                string[] rec = new string[numOfFields];
                for (int i = 0; i < numOfFields; i++)
                    rec[i] = reader[r, i];

                rec = rec.Each(s => s.Trim()).ToArray();

                if (rec.All(s => s == ""))
                    continue;

                if (rec.All(s => string.IsNullOrEmpty(s)))
                {
                    break;
                }

                var t1 = new RmsAuto.Store.Cms.Entities.Disc();

                for (int c = 0; c < numOfFields; c++)
                {
                    string value = rec[c];
                    try
                    {
                        switch (c)
                        {
                            case 0: t1.Manufacturer = value; break;
                            case 1: t1.ModelName = value; break;                            
                            case 2: t1.PartNumber = value; break;
                            case 3: t1.Width = Decimal.Parse(value); break;
                            case 4: t1.Diameter = Decimal.Parse(value); break;
                            case 5: t1.Gab = Decimal.Parse(value); break;
                            case 6: t1.PCD = value; break;
                            case 7: t1.Dia = Decimal.Parse(value); break;
                        }
                    }
                    catch
                    {
						//TODO допилить обработку ошибок
                    }
                }
                t.Add(t1);
            }

            using (CmsDataContext cms = new CmsDataContext())
            {
                try
                {
                    cms.Connection.Open();
                    cms.Transaction = cms.Connection.BeginTransaction();
                    var current = cms.Discs.Select(x => x).ToList();
                    current.Sort(new DiscComparer());
                    t.Sort(new DiscComparer());

                    t = t.Select(x => x).Distinct(new DiscComparer()).ToList();

                    //Пробегаем по каждому объекту каталога и смотрим какая у него была картинка
                    foreach (var RefObj in t)
                    {
						var tmp = current.Where(x => x.PartNumber == RefObj.PartNumber).FirstOrDefault();
						RefObj.ImageUrl = tmp != null && tmp.ImageUrl.HasValue ? tmp.ImageUrl : (int)NoPhotoID.Disc;
                    }

					// "мерджим" старое и новое
                    t = t.UnionAndDistinct<RmsAuto.Store.Cms.Entities.Disc>(current, new DiscComparer());

                    cms.Discs.DeleteAllOnSubmit(current);
                    cms.Discs.InsertAllOnSubmit(t);
                    cms.SubmitChanges(ConflictMode.ContinueOnConflict);
                    cms.Transaction.Commit();
                    Response.Redirect(Request.Url.ToString());
                }
				catch (ChangeConflictException)
				{
					//TODO допилить обработку ошибок
					//Если не удалось изменить какую-то одну запись это не значит что нужно делать откат всего
				}
                catch (Exception)
                {
                    cms.Transaction.Rollback();
                    //ShowMessage("Указано неиспользуемое имя бренда");
                    Logger.WriteError("Ошибка при сохранении результатов загрузки дисков", EventLogerID.BLException, EventLogerCategory.BLError);
                }
                finally
                {
                    if (cms.Connection.State == System.Data.ConnectionState.Open) { cms.Connection.Close(); }
                }
            }
        }

        void ShowMessage(string message)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(), "__messageBox", "<script type='text/javascript'>alert('" + message + "');</script>");
        }
    }
}
