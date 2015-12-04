using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    /// <summary>
    /// Summary description for answerSubmit2
    /// </summary>
    public class answerSubmit2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (var dc = new LogDataContext())
            {
                
                var data = string.IsNullOrEmpty(context.Request["data"]) ? "" : (string)context.Request["data"];
                foreach (var VARIABLE in data.Split(';'))
                {
                    dc.CreateEvent(VARIABLE);
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