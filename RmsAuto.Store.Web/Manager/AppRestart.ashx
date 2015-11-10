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

        string InternalFranchName = (string)context.Request["InternalFranchName"];
        if (InternalFranchName.Contains(","))
            { context.Session["InternalFranchName"] = InternalFranchName.Split(',')[0]; }
        else
            { context.Session["InternalFranchName"] = InternalFranchName;  }

        HttpCookie coockie = new HttpCookie("InternalFranchName");
        coockie.Value = InternalFranchName; //по умолчанию русская локаль
        coockie.Expires = DateTime.Now.AddMonths(1);
        coockie.Domain = HttpContext.Current.Request.Url.Host;
        context.Response.Cookies.Add(coockie);
        
        context.Response.ContentType = "plain/text";
        context.Response.Write("ok");

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}