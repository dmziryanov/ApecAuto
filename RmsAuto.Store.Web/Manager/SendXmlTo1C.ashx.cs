using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using System.Web.SessionState;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SendXmlTo1C : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            if (!LightBO.IsLight()) return;
            
            var ID = int.Parse(context.Request.Params["OrderId"]);
            {
                try
                {
                    LightBO.FormXmlForOrder(ID);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Ok");
                }
                
                catch (Exception ex)
                {
                    //TODO логгировать исключение
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Error");
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
