using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;
using RmsAuto.Common.Web;
using RmsAuto.Store.Cms.Entities;
using System.Collections.Generic;
using System.Threading;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.Banners
{
	public partial class List : Security.BasePage/*System.Web.UI.Page*/
	{
		protected MetaTable table;

        protected String GetEditURL(object dataItem)
        {
            String s = Request.RawUrl;
            var dItem = (Cms.Entities.Banners) dataItem;
            s = s.Substring(0, s.LastIndexOf("/")) + "/Edit.aspx?" + UrlKeys.Id + "=" + dItem.BannerID.ToString();
            return s;
        }

        protected String GetAddURL()
        {
            String s = Request.RawUrl;
            s = s.Substring(0, s.LastIndexOf("/")) + "/Edit.aspx?" + UrlKeys.Id + "=" + "0";
            return s;
        }

        protected void OnDeleteClick(object sender, CommandEventArgs e)
        {
            if (e.CommandName == DataControlCommands.DeleteCommandName)
            {
                int id = Convert.ToInt32(e.CommandArgument);

                using (var dc = new DCWrappersFactory<CmsDataContext>())
                {
                    try
                    {
                        Cms.Entities.Banners b = null;

                        b = Cms.Entities.Banners.GetBannerByID(dc.DataContext, id);

                        try
                        {
                            var file = dc.DataContext.Files.Where(f => f.FileID == b.FileID).First();

                            if (file != null)
                            {
                                dc.DataContext.Files.DeleteOnSubmit(file);

                                b.FileID = null;

                                dc.DataContext.Files.DeleteOnSubmit(file);
                            }
                        }
                        catch
                        {
                        }

                        b = Cms.Entities.Banners.GetBannerByID(dc.DataContext, id);
                        dc.DataContext.Banners.DeleteOnSubmit(b);

                        foreach (var r in dc.DataContext.BannersForCatalogItems)
                        {
                            if (r.BannerID == id)
                                dc.DataContext.BannersForCatalogItems.DeleteOnSubmit(r);
                        }
                        
                        dc.DataContext.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }
                    
                }
            }
        }
        
	    protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(GridView1, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            table = GridDataSource.GetTable();
            Title = table.DisplayName;
            InsertHyperLink.NavigateUrl = GetAddURL();

            // Disable various options if the table is readonly
            if (table.IsReadOnly)
            {
                GridView1.Columns[0].Visible = false;
                InsertHyperLink.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                ApplyStaticFilters();
                ApplySort();
            }
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
        }

        private void ApplyStaticFilters()
        {
            foreach (var col in table.Columns)
            {
                var sfAttr = col.Attributes.OfType<StaticFilterAttribute>().FirstOrDefault();
                if (sfAttr != null)
                    GridDataSource.WhereParameters.Add(new StaticParameter(col.Name, sfAttr.Value));
            }
        }

        private void ApplySort()
        {
            var sortAttr = table.Attributes.OfType<SortAttribute>().FirstOrDefault();
            if (sortAttr != null)
            {
                GridDataSource.OrderBy = sortAttr.Expression;
            }
        }
	}
}
