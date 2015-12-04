<%@ WebHandler Language="C#" Class="answerSubmit" %>

using System;
using System.Web;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;

public class answerSubmit : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
   
        using (var dc = new DCFactory<LogDataContext>().DataContext)
        {
            var data = string.IsNullOrEmpty(context.Request["data"]) ? "" : (string) context.Request["data"];
            foreach (var VARIABLE in data.Split(';'))
            {
                dc.CreateEvent(VARIABLE);    
            }
            
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}