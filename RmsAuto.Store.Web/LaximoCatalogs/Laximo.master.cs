using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Laximo.Guayaquil.Data;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public partial class LaximoMaster : System.Web.UI.MasterPage
    {
        private CatalogImpl _catalog;

        public CatalogProvider CatalogProvider
        {
            get
            {
				try
				{
					CatalogProvider catalogProvider = (CatalogProvider)Application["CatalogProvider"];
					return catalogProvider;
				}
				catch (Exception ex)
				{
					Logger.WriteError("Не подгрузились каталоги Laximo (LaximoMaster)", EventLogerID.BLException, EventLogerCategory.BLError, ex);
					return null;
				}
            }
        }

        public ICatalog Catalog
        {
            get { return _catalog; }
            private set { _catalog = (CatalogImpl)value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Catalog = new CatalogImpl();
            Catalog.Code = Request.QueryString["c"];
            Catalog.Ssd = Request.QueryString["ssd"];

            string cid = Request.QueryString["cid"];
            Catalog.CategoryId = string.IsNullOrEmpty(cid) ? -1 : Convert.ToInt32(cid);

            Catalog.VehicleId = Convert.ToInt32(Request.QueryString["vid"]);
            Catalog.PathId = Request.QueryString["path_id"];
        }
    }
}
