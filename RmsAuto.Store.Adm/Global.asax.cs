using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using System.Web.Routing;
using System.Web.DynamicData;

using RmsAuto.Store.Entities;
using RmsAuto.TechDoc.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Acctg.Entities;

namespace RmsAuto.Store.Adm
{
    public class Global : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            MetaModel storeModel = new MetaModel();
            MetaModel tecdocModel = new MetaModel();
            MetaModel cmsModel = new MetaModel();
            MetaModel acctgModel = new MetaModel();
            MetaModel commonModel = new MetaModel();

            //                    IMPORTANT: DATA MODEL REGISTRATION 
            // Uncomment this line to register LINQ to SQL classes or an ADO.NET Entity Data
            // model for ASP.NET Dynamic Data. Set ScaffoldAllTables = true only if you are sure 
            // that you want all tables in the data model to support a scaffold (i.e. templates) 
            // view. To control scaffolding for individual tables, create a partial class for 
            // the table and apply the [Scaffold(true)] attribute to the partial class.
            // Note: Make sure that you change "YourDataContextType" to the name of the data context
            // class in your application.
            storeModel.RegisterContext(
                typeof(StoreDataContext),
                new ContextConfiguration() { ScaffoldAllTables = false });

            tecdocModel.RegisterContext(
                typeof(TecdocStoreDataContext),
                new ContextConfiguration() { ScaffoldAllTables = false });

            cmsModel.RegisterContext(
                typeof(RmsAuto.Store.Cms.Entities.CmsDataContext),
                new ContextConfiguration() { ScaffoldAllTables = false });

            acctgModel.RegisterContext(
                typeof(AcctgDataContext),
                new ContextConfiguration() { ScaffoldAllTables = false });

            commonModel.RegisterContext(
                typeof(RmsAuto.Store.Entities.dcCommonDataContext),
                new ContextConfiguration() { ScaffoldAllTables = false });

            // The following statement supports separate-page mode, where the List, Detail, Insert, and 
            // Update tasks are performed by using separate pages. To enable this mode, uncomment the following 
            // route definition, and comment out the route definitions in the combined-page mode section that follows.
            routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                Model = storeModel
            });

            routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                Model = tecdocModel
            });

            routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                Model = cmsModel
            });

            routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new { action = "List|Edit" }),
                Model = acctgModel
            });

            routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                Model = commonModel
            });


            routes.Add(
            "File",
            new Route(
                "Files/{ID}.ashx",
                null,
                new RouteValueDictionary { { "ID", @"^\d+$" } },
                new RouteValueDictionary { { "Url", "~/Cms/File.ashx" } },
                new PageRouteHandler())
                );
            // The following statements support combined-page mode, where the List, Detail, Insert, and
            // Update tasks are performed by using the same page. To enable this mode, uncomment the
            // following routes and comment out the route definition in the separate-page mode section above.
            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.List,
            //    ViewName = "ListDetails",
            //    Model = model
            //});

            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.Details,
            //    ViewName = "ListDetails",
            //    Model = model
            //});
        }

        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
