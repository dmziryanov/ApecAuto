using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.Cms.Entities;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Web.SessionState;

namespace RmsAuto.Store.Web.Cms
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class RegionList : IHttpHandler, IRequiresSessionState
    {

        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }


        RmsAuto.Store.Entities.City[] Cities = null;
        List<RmsAuto.Store.Cms.Entities.Shop> AllShops = new List<RmsAuto.Store.Cms.Entities.Shop>();
        List<RmsAuto.Store.Entities.Region> RegionsThroughCities = null;

        public void ProcessRequest(HttpContext context)
        {
            var StoreCtx = new CmsDataContext();
            var CommonCtx = new RmsAuto.Store.Entities.dcCommonDataContext();
         
            //Извлекаем наборы данных в списки, так как LINQ to SQL не дает выполнять запросы к различным контекстам
            AllShops = StoreCtx.Shops.Where(x => x.ShopVisible).ToList();
            var Regs = CommonCtx.Regions.Select(x => x).ToList();
            var Cits = CommonCtx.Cities.Select(x => x).ToList();

            Cities = (from shops in AllShops
                      join cities in Cits on shops.CityID equals cities.CityID
                      orderby cities.Name
                      where shops.ShopVisible
                      select cities).Distinct().ToArray();

            Cities = Cities.Union(Cits.Where(x => x.Visible.HasValue && x.Visible.Value)).OrderBy(x => x.Name).Distinct().Where(x => AllShops.Select(y => y.CityID).Contains(x.CityID)).ToArray();

            if (SiteContext.Current.IsAnonymous)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonSerializer(Cities.OrderByDescending(x => x.Priority).ToArray()));
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Для смены региона, пожалуйста выйдите из системы");
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
