extern alias DataStreams1;

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataStreams1.DataStreams.Common;
using DataStreams1.DataStreams.Xls;
using DataStreams1.DataStreams.Xlsx;
using DevExpress.Web.ASPxGridView;
using RmsAuto.Store.Adm.Catalogs;
using RmsAuto.Store.Adm.scripts;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using System.Data.SqlClient;
//using DevExpress.Web.ASPxGridView;

namespace RmsAuto.Store.Adm
{
    public class LoadResult
    {
        public LoadResult AddError(object cartNotEmpty, int param1, string param2)
        {
			//TODO допилить обработку ошибок
            throw new NotImplementedException();
        }

        public LoadResult()
        {
        }
        public object CustOrderNum { get; set; }
        public string FileName { get; set; }
    }

    public class TireManager
    {
        public List<Tires> GetAllTires()
       {
           using (CmsDataContext cms = new CmsDataContext())
           {
               cms.Tires.Select(x => x);
               return null;
           }
       }
    }

    public partial class Tires : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // var Gr = new ASPxGridView();
            //Gr.Visible = false;
            ScriptsManager.RegisterMsoFramework(Page);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            LoadTires(FileUpload1);
        }

        private LoadResult LoadTires(FileUpload fileUpload)
        {
            HttpPostedFile file = fileUpload.PostedFile;
            LoadResult result = new LoadResult { FileName = file.FileName };

            if (string.IsNullOrEmpty(file.FileName))
            {
				//TODO допилить обработку ошибок
                //return result.AddError(ErrorReason.FileNotSpecified, 0, "");
            }

            if (!fileUpload.HasFile)
            {
				//TODO допилить обработку ошибок
                //return result.AddError(ErrorReason.FileNotFoundOrEmpty, 0, file.FileName);
            }

            bool xlsx;

            switch (Path.GetExtension(file.FileName))
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
                    LoadTiresFromReader(reader, result);
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

        private void LoadTiresFromReader(SpreadsheetReader reader, LoadResult result)
        {
			//проверка наличия в файле единственного листа с заказом
            reader.CurrentSheet = 0;
            int numOfFields = 9;

            List<RmsAuto.Store.Cms.Entities.Tires> t = new List<RmsAuto.Store.Cms.Entities.Tires>();
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

                var t1 = new RmsAuto.Store.Cms.Entities.Tires();

                for (int c = 1; c < numOfFields; c++)
                {
                    string value = rec[c];
                    try
                    {
                        switch (c)
                        {
                            case 1: t1.TireNumber = value; break;
                            case 2: t1.Profile = Double.Parse(value); break;
                            case 3: t1.Height = Double.Parse(value); break;
                            case 4: t1.Radius = value; break;
                            case 5: t1.TireIndex = value; break;
                            case 6: t1.ModelName = value; break;
                            case 7: t1.Manufacturer = value; break;
                            case 8: t1.Season = value; break;
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
					var current = cms.Tires.Select(x => x).ToList();
					current.Sort(new MyComparer());
					t.Sort(new MyComparer());

					t = t.Select(x => x).Distinct(new MyComparer()).ToList();

					//Пробегаем по каждому объекту каталога и смотрим какая у него была картинка
					foreach (var RefObj in t)
					{
						var tmp = current.Where(x => x.TireNumber == RefObj.TireNumber).FirstOrDefault();
						RefObj.ImageUrl = tmp != null && tmp.ImageUrl.HasValue ? tmp.ImageUrl : (int)NoPhotoID.Tire;
					}

					// "мерджим" старое и новое
					t = t.UnionAndDistinct<RmsAuto.Store.Cms.Entities.Tires>(current, new MyComparer());

					cms.Tires.DeleteAllOnSubmit(current);
					cms.Tires.InsertAllOnSubmit(t);
					cms.SubmitChanges();
					//cms.SubmitChanges(ConflictMode.ContinueOnConflict);
					cms.Transaction.Commit();
				}


				catch (ChangeConflictException ex)
				{
					//TODO допилить обработку ошибок
					//Если не удалось изменить какую-то одну запись это не значит что нужно делать откат всего
				}
				catch (Exception ex)
				{
					cms.Transaction.Rollback();
					Logger.WriteError("Ошибка при сохранении результатов загрузки шин", EventLogerID.BLException, EventLogerCategory.BLError, ex);
				}
				finally
				{
					if (cms.Connection.State == System.Data.ConnectionState.Open) { cms.Connection.Close(); }
				}
			}
        }
    }
}