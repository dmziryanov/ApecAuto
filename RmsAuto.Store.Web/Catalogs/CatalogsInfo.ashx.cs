using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Data;
using System.Configuration;
using System.Web.SessionState;

namespace RmsAuto.Store.Web.Catalogs
{

    public class CatalogJson
    {
        public int ID;
        public string Name;
        public string Description;
        public int? ImageUrl;
        public string @ref;
    }
    
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CatalogsInfo : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            if (SiteContext.Current.InternalFranchName == ConfigurationManager.AppSettings["InternalFranchName"])
                {
                    using (var cms = new CmsDataContext())
                    {
                        var Cats = (from x in cms.OurCatalogs.Where(x => x.Visible == true).OrderBy(x => x.Priority)
                                select new CatalogJson() { ID = x.Id, Name = x.Name, Description = x.Description, ImageUrl = x.ImageUrl, @ref = x.@ref,  }).ToList();
                        context.Response.ContentType = "application/json";
                        context.Response.Write(this.JsonSerializer(Cats));
                    }
                }
                else
                {
                    using (var cms = new DCWrappersFactory<CmsDataContext>())
                    {
                        var Cats = (from x in cms.DataContext.spSelCatalogsFromRms().Where(x => x.Visible == true).OrderBy(x => x.Priority)
                                    select new CatalogJson() { ID = x.Id, Name = x.Name, Description = x.Description, ImageUrl = x.ImageUrl, @ref = x.@ref, }).ToList();
                        context.Response.ContentType = "application/json";
                        context.Response.Write(this.JsonSerializer(Cats));
                    }

                }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
