using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Store
{
	public partial class SparePartDefects : System.Web.UI.Page
	{
        private static string UniqueKeyPhoto = "";
        public static string ReloadPage = "true";
        private string CurrentUniqueKeyPhoto;

        public int ImageHeight;

		protected string GetImageUrl(SparePartImage spi)
		{
            using (System.IO.MemoryStream stm = new System.IO.MemoryStream(spi.ImageBody.ToArray()))
            {
                using (System.Drawing.Image img = System.Drawing.Image.FromStream(stm))
                {
                    // получаем размер загруженного фото
                    ImageHeight = img.Height;
                }
            }

			return UrlManager.GetSparePartDefectImageUrl(spi.Manufacturer + ',' + spi.PartNumber + ',' + spi.SupplierID + ',' + spi.ImageNumber);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			(dsImages.SelectParameters[0] as QueryStringParameter).QueryStringField = "id";//UrlKeys.StoreAndTecdoc.ArticleId;
		}
	}
}
