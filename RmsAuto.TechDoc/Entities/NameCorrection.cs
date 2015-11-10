using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace RmsAuto.TechDoc.Entities
{
	[ScaffoldTable(true)]
	[MetadataType(typeof(NameCorrectionMetadata))]
	public partial class NameCorrection
	{
		partial void OnValidate(ChangeAction action)
		{
			if (action == ChangeAction.Insert)
			{
                using (var dc = new TecdocStoreDataContext())
				{
					if (dc.NameCorrections.SingleOrDefault(
						 m => m.OriginalName == OriginalName) != null)
						throw new ValidationException("это значение уже есть в списке");
				}
			}
		}

		partial void OnCorrectedNameChanged()
		{
			string query = "";
			NameCorrection record;

            using (var dc = new TecdocStoreDataContext())
			{
				record = dc.NameCorrections.SingleOrDefault(n => n.NameCorrectionID == Convert.ToInt32(HttpContext.Current.Request["NameCorrectionID"]));
			}

			switch (record.TableName)
			{
				case "tecdoc_tof_manufacturers":
					query = "update tecdoc_tof_manufacturers set mfa_brand = '" + _CorrectedName + "' where mfa_id = " + record.ID;
					break;
				case "tecdoc_tof_models":
					query =
						"update t set t.tex_text = '" + _CorrectedName +
						@"' from tecdoc_tof_des_texts t 
							join tecdoc_tof_country_designations cd on cd.cds_tex_id = t.tex_id
							join tecdoc_tof_models m on m.mod_cds_id = cd.cds_id
							where m.mod_id = " +
						record.ID;
					break;
			}

			if (query != "")
				using (SqlConnection conn = new SqlConnection(global::RmsAuto.TechDoc.Properties.Settings.Default.ex_tecdocsConnectionString))
				{
					conn.Open();
					using (SqlCommand cmd = new SqlCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = query;
						cmd.ExecuteNonQuery();
					}
				}
		}
	}

	[DisplayName("TecDocs - корректировка названий")]
	public class NameCorrectionMetadata
	{
		[DisplayName("Оригинальное название")]
		[Required(ErrorMessage = "Требуется оригинальное название")]
		public object OriginalName { get; set; }

		[DisplayName("Исправленное название")]
		[Required(ErrorMessage = "Требуется исправленное название")]
		public object CorrectedName { get; set; }

		//[ScaffoldColumn(false)]
		public object ID { get; set; }

		//[ScaffoldColumn(false)]
		public object TableName { get; set; }

	}
}
