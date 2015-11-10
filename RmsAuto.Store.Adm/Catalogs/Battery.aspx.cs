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
    public partial class Battery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

            List<RmsAuto.Store.Cms.Entities.Battery> t = new List<RmsAuto.Store.Cms.Entities.Battery>();
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

                var t1 = new RmsAuto.Store.Cms.Entities.Battery();

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
                            case 3: t1.Capacity = Decimal.Parse(value); break;
                            case 4: t1.polarity = value; break;
                            case 5: t1.cleat = value; break;
                            case 6: t1.Size = value; break;
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
                    var current = cms.Batteries.Select(x => x).ToList();
                    current.Sort(new BatteryComparer());
                    t.Sort(new BatteryComparer());

                    t = t.Select(x => x).Distinct(new BatteryComparer()).ToList();

                    //Пробегаем по каждому объекту каталога и смотрим какая у него была картинка
                    foreach (var RefObj in t)
                    {
						var tmp = current.Where(x => x.PartNumber == RefObj.PartNumber).FirstOrDefault();
						RefObj.ImageUrl = tmp != null && tmp.ImageUrl.HasValue ? tmp.ImageUrl : (int)NoPhotoID.Battery;
                    }

                    t = t.UnionAndDistinct<RmsAuto.Store.Cms.Entities.Battery>(current, new BatteryComparer());

                    cms.Batteries.DeleteAllOnSubmit(current);
                    cms.Batteries.InsertAllOnSubmit(t);
                    cms.SubmitChanges(ConflictMode.ContinueOnConflict);
                    cms.Transaction.Commit();
                }
				catch (ChangeConflictException)
				{
					//TODO допилить обработку ошибок
				}
                catch (Exception)
                {
                    cms.Transaction.Rollback();
                    Logger.WriteError("Ошибка при сохранении результатов загрузки аккумуляторов", EventLogerID.BLException, EventLogerCategory.BLError);
                }
                finally
                {
                    if (cms.Connection.State == System.Data.ConnectionState.Open) { cms.Connection.Close(); }
                }
            }
        }
    } 
}