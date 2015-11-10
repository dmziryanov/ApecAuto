using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.Acctg;
using System.Web.SessionState;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Web.Manager.Reports.DataHandlers
{
    public class Client
    {
        public string ClientID;
        public string ClientName;
    }

    
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Clients : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var searchResults = ClientSearch.Search(
                       "",
                       "",
                       ClientSearchMatching.Fuzzy).Select(x => new Client { ClientID = x.ClientID, ClientName = x.ClientName }).ToArray();

            context.Response.ContentType = "application/json";
            context.Response.Write(this.JsonSerializer(searchResults));
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
