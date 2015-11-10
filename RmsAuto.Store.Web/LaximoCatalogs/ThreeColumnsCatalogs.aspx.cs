using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;
using RmsAuto.Store.Web.LaximoCatalogs.Extenders;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public partial class ThreeColumnsCatalogs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LaximoMaster master = Page.Master as LaximoMaster;
            if (master != null)
            {
                ListCatalogs catalogs = master.CatalogProvider.CatalogsList();
				//т.к. картинка производителя для каталогов подгружается как web-resource по наименованию, а именования могут приходит
				//как ford.png и как Ford.png, то держим все наименования картинок в low-case и все приходящие наименования также переводим
				//в low-case
				foreach (var r in catalogs.row)
				{
					r.icon = r.icon.ToLower();
				}

                ThreeColumnsCatalogList threeColumnsCatalogList = new ThreeColumnsCatalogList(new CatalogExtender(), master.Catalog);
                threeColumnsCatalogList.Catalogs = catalogs;

                treeColumnsCatalogs.Controls.Add(threeColumnsCatalogList);
            }
        }
    }
}
