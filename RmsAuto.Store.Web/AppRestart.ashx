<%@ WebHandler Language="C#" Class="AppRestart" %>

using System;
using System.Web;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;

public class AppRestart : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        //TODO: Возможно здесь можно как-то перегрузить кеш и коллекции без выгрузки домена
        //HttpRuntime.UnloadAppDomain();
        
        ////Очистка кеша
        //List<string> keys = new List<string>();

        //// retrieve application Cache enumerator
        //IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();

        //// copy all keys that currently exist in Cache
        //while (enumerator.MoveNext())
        //{
        //    keys.Add(enumerator.Key.ToString());
        //}

        //// delete every key from cache
        //for (int i = 0; i < keys.Count; i++)
        //{
        //    HttpRuntime.Cache.Remove(keys[i]);
        //}

        //TODO: Возможно переключение по урлу понадобиться в дальнейшем
        //string InternalFranchName = (string)context.Request["InternalFranchName"];
        //if (InternalFranchName.Contains(","))
        //    { context.Session["InternalFranchName"] = InternalFranchName.Split(',')[0]; }
        //else
        //    { context.Session["InternalFranchName"] = InternalFranchName;  }

        //HttpCookie cookie = context.Request.Cookies["InternalFranchName"];
        //if (cookie != null)
        //{
        //    cookie.Expires = DateTime.Now.AddDays(-1);
        //    cookie.Domain = ".rmsauto.ru";
        //    context.Response.Cookies.Add(cookie);
        //}

        //cookie = new HttpCookie("InternalFranchName");
        //cookie.Value = InternalFranchName; //по умолчанию русская локаль
        //cookie.Expires = DateTime.Now.AddMonths(1);
        //context.Response.Cookies.Add(cookie);
        
        //context.Response.ContentType = "plain/text";
        //context.Response.Write("ok");

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}