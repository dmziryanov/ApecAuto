using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Cms.Entities;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using RmsAuto.Store.Cms.Routing;
using System.Web.SessionState;

namespace RmsAuto.Store.Web.Cms
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ShopYaCoordList : IHttpHandler, IRequiresSessionState
    {

     public static string JsonSerializer<T> (T t)
     {
         DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
         MemoryStream ms = new MemoryStream();
         ser.WriteObject(ms, t);
         string jsonString = Encoding.UTF8.GetString(ms.ToArray());
         ms.Close();
         return jsonString;
     }  

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["ID"] != null && context.Request.Params["Region"] != null)
            {
                Shop[] shops = null;
                if (context.Request.Params["Region"] == "Region")
                    shops = ShopsDac.GetShopByRegionID(Convert.ToInt32(context.Request.Params["ID"])).ToArray();
                if (context.Request.Params["Region"] == "City")
                    shops = ShopsDac.GetShopByCityID(Convert.ToInt32(context.Request.Params["ID"])).ToArray();
                if (context.Request.Params["Region"] == "Shop")
                    shops = ShopsDac.GetShopByID(Convert.ToInt32(context.Request.Params["ID"])).ToArray();

                ShopDTO[] Coords = new ShopDTO[shops.Length];
                for (int i = 0; i < shops.Length; i++)
                {
                    Coords[i].X = shops[i].XCoord;
                    Coords[i].Y = shops[i].YCoord;
                    Coords[i].SiteUrl = shops[i].SiteUrl ?? "";
                    Coords[i].Description = "<div style='float:left'>" + (shops[i].MapComment ?? (shops[i].ShopAddress)) + "</div>" + "<br><br><div style='float:right'><a class='GrayTextStyle' href ='" + UrlManager.MakeAbsoluteUrl("/About/Shops/" + shops[i].ShopID + ".aspx") + "'>Подробнее...</a></div>";
                    Coords[i].Name = shops[i].ShopAddress;
                    Coords[i].FranchName = shops[i].FranchName;
                    Coords[i].InternalFranchName = shops[i].InternalFranchName;
                }
                
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonSerializer(Coords));
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

    [Serializable]
    struct ShopDTO
    {
        public double X;
        public double Y;
        public string SiteUrl;
        public string Description;
        public string Name;
        public string FranchName;
        public string InternalFranchName;
        public int? Priority;
    }
}
