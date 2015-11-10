using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RmsAuto.Store.BL
{
    public class BLException : Exception
    {
        public BLException() { }

        public BLException(string message) : base(message) { }

        public BLException( string message, bool showDetail ) : base(message)
        {
            if ( showDetail )
            {
                Logger.WriteException( this );
                HttpContext.Current.Server.ClearError();
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Session["ErrorBL"] = message;
                HttpContext.Current.Response.Redirect( "~/ErrorBL.aspx", true );
            }
        }

        public BLException(string message, Exception inner) : base(message, inner) { }
    }
}
