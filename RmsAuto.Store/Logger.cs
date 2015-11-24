using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using RmsAuto.Store.Configuration;
using RmsAuto.Store.Web;
using System.Web;
using RmsAuto.Store.BL;
using System.IO;
using RmsAuto.Store.Acctg;
using System.Configuration;
using System.Threading;

namespace RmsAuto.Store
{
    public static class Logger
    {
        private static IDictionary<string, EventLog> eventLog;
        private static bool _isWork;

        static Logger()
        {
                _isWork = LoggerConfiguration.Current.LogWork == "on";

                eventLog = new Dictionary<string, EventLog>();
                List<String> FranchNames = AcctgRefCatalog.RmsFranches.Items.Select(x => x.InternalFranchName).ToList();
                foreach (string s in FranchNames)
                {
                    eventLog[s] = new EventLog(AcctgRefCatalog.RmsFranches[s].LogName, LoggerConfiguration.Current.MachineName, AcctgRefCatalog.RmsFranches[s].LogName);
                }
        }

        /// <summary>
        /// Протоколирование неизвестного исключения.
        /// </summary>
        /// <param name="e">Исключение</param>
        public static void WriteException( Exception e )
        {
            Exception workE = e;
            if ( e.GetType() == typeof(HttpUnhandledException))
            {
                if ( e.InnerException != null )
                {
                    workE = e.InnerException;
                }
            }
            if ( workE.GetType() == typeof( HttpException ) )
            {
                HttpException httpE = (HttpException)workE;
                Write( httpE.GetHtmlErrorMessage(), EventLogerType.Error, httpE.GetHttpCode(), EventLogerCategory.HttpError, workE );
            }
            else if ( workE.GetType() == typeof( BLException ) )
            {
                Write( "Исключение бизнес логики!", EventLogerType.Warning, (int)EventLogerID.BLException, EventLogerCategory.BLError, workE );
            }
            else
            {
                Write( "Неизвестное исключение!", EventLogerType.Error, (int)EventLogerID.UnknownError, EventLogerCategory.FatalError, workE );
            }
        }

        /// <summary>
        /// Запись информации с типом информация в системный лог
        /// </summary>
        /// <param name="message">Событие</param>
        /// <param name="eventID">Код события</param>
        /// <param name="eventCategory">Категория события</param>
        /// <param name="eventCategory">Выдавать ли стектрейс</param>
        public static void WriteInformation(string message, EventLogerID eventID, EventLogerCategory eventCategory, params Object[] p)
        {
            WriteToLog(message, EventLogerType.Information, eventID, eventCategory, p);
        }

        /// <summary>
        /// Запись информации с типом предупреждение в системный лог
        /// </summary>
        /// <param name="message">Событие</param>
        /// <param name="eventID">Код события</param>
        /// <param name="eventCategory">Категория события</param>
        public static void WriteWarning( string message, EventLogerID eventID, EventLogerCategory eventCategory )
        {
            WriteToLog( message, EventLogerType.Warning, eventID, eventCategory );
        }

        /// <summary>
        /// Запись информации с типом ошибка в системный лог
        /// </summary>
        /// <param name="message">Событие</param>
        /// <param name="eventID">Код события</param>
        /// <param name="eventCategory">Категория события</param>
        public static void WriteError( string message, EventLogerID eventID, EventLogerCategory eventCategory )
        {
             WriteToLog(message, EventLogerType.Error, eventID, eventCategory);
        }

        /// <summary>
        /// Запись информации о исключении в лог
        /// </summary>
        /// <param name="message">Событие</param>
        /// <param name="eventID">Код события</param>
        /// <param name="eventCategory">Категория события</param>
        /// <param name="e">Исключение</param>
        public static void WriteError( string message, EventLogerID eventID, EventLogerCategory eventCategory, Exception e )
        {
            Write(message, EventLogerType.Error, (int)eventID, eventCategory, e);
        }

        /// <summary>
        /// Запись события в системный лог, без типа, кода и категории
        /// </summary>
        /// <param name="message">Событие</param>
        public static void WriteToLog(string message, params Object[] p)
        {
            WriteToLog( message, EventLogerType.Information, EventLogerID.UnknownError, EventLogerCategory.UnknownCategory, p );
        }

        /// <summary>
        /// Запись события в системный лог, без кода и категории
        /// </summary>
        /// <param name="message">Событие</param>
        /// <param name="eventType">Тип события</param>
        public static void WriteToLog( string message, EventLogerType eventType )
        {
            WriteToLog( message, eventType, EventLogerID.UnknownError, EventLogerCategory.UnknownCategory );
        }

        /// <summary>
        /// Запись события в системный лог с не известной категорией
        /// </summary>
        /// <param name="message">Событие</param>
        /// <param name="eventType">Тип события</param>
        /// <param name="eventID">Код события</param>
        public static void WriteToLog( string message, EventLogerType eventType, EventLogerID eventID )
        {
            WriteToLog( message, eventType, eventID, EventLogerCategory.UnknownCategory );
        }

        /// <summary>
        /// Запись события в системный лог
        /// </summary>
        /// <param name="message">Событие</param>
        /// <param name="eventType">Тип события</param>
        /// <param name="eventID">Код события</param>
        /// <param name="eventCategory">Категория события</param>
        public static void WriteToLog( string message, EventLogerType eventType, EventLogerID eventID, EventLogerCategory eventCategory, params Object[] p )
        {
            Write( message, eventType, (int)eventID, eventCategory, null, p);
        }

        /// <summary>
        /// Запись события в системный лог
        /// </summary>
        /// <param name="message">Событие</param>
        /// <param name="eventType">Тип события</param>
        /// <param name="eventID">Код события</param>
        /// <param name="eventCategory">Категория события</param>
        /// <param name="exc">Исключение</param>
        private static void Write( string message, EventLogerType eventType, int eventID, EventLogerCategory eventCategory, Exception exc, params Object[] p )
        {
            if ( !_isWork )
                return;
            StringBuilder finalMessage = new StringBuilder();
            finalMessage.AppendLine( message );
            finalMessage.AppendLine( "------------------------------------------------------------------------" );



            //Пытаемся получить SiteContext, так как HttpContext.Current.Items не всегда есть в сборке
            SiteContext vSiteContext = null;
            var contextExist = true;

            try
            {
                vSiteContext = SiteContext.Current;
            }
            catch
            {
                contextExist = false;
            }


            if (vSiteContext == null)
            {
                if (contextExist) finalMessage.AppendLine( "Профиль не заполнен!" );

            }
            else if ( SiteContext.Current.IsAnonymous )
            {
                finalMessage.AppendLine( "Пользователь не авторизован." );
            }
            else
            {
                finalMessage.AppendLine( "Пользователь: " + SiteContext.Current.User.UserId.ToString() + " - " + SiteContext.Current.UserDisplayName );
            }
			try
			{
				finalMessage.AppendLine("URL: " + HttpContext.Current.Request.RawUrl);
				finalMessage.AppendLine("ContentType: " + HttpContext.Current.Request.ContentType);
				finalMessage.AppendLine("HttpMethod: " + HttpContext.Current.Request.HttpMethod);
				finalMessage.AppendLine("Browser: " + HttpContext.Current.Request.UserAgent);
				finalMessage.AppendLine("Url: " + HttpContext.Current.Request.Url);
				finalMessage.AppendLine("UrlReferrer: " + HttpContext.Current.Request.UrlReferrer);
				finalMessage.AppendLine("UserHostAddress: " + HttpContext.Current.Request.UserHostAddress);
				finalMessage.AppendLine("UserHostName: " + HttpContext.Current.Request.UserHostName);
			}
			catch (Exception)
			{
				//Данный try catch развернут т.к. когда объект Request не существует его нельзя проверить на = null
			}

            if ( exc != null )
            {
                finalMessage.AppendLine(ExceptionToString(exc, false));
            }
          
            
            StackTrace st = new StackTrace( true );
            if (exc == null && p.Length == 0)
            {
                finalMessage.AppendLine( "------------------------------------------------------------------------" );
                finalMessage.AppendLine("Final stack trace:");
                finalMessage.AppendLine(st.ToString());
            }
            
            eventLog[!SiteContext.isFranchNameAttached() ?ConfigurationManager.AppSettings["InternalFranchName"] : SiteContext._internalFranchName].WriteEntry(finalMessage.ToString(), ConvertType( eventType ), Convert.ToInt32( eventID ), Convert.ToInt16( eventCategory ) );
        }

        private static string ExceptionToString( Exception e, bool inner)
        {
            StringBuilder excMessage = new StringBuilder();
            if ( !inner )
            {
                excMessage.AppendLine( "------------------------------------------------------------------------" );
                excMessage.AppendLine( "Exception:" );
            }
            excMessage.AppendLine( "Source: " + e.Source );
            excMessage.AppendLine( "Message: " + e.Message );
            excMessage.AppendLine( "StackTrace:" );
            excMessage.AppendLine( e.StackTrace );
            if ( e.InnerException != null )
            {
                excMessage.AppendLine( "---------------------------------" );
                excMessage.AppendLine( "InnerException:" );
                excMessage.AppendLine( ExceptionToString( e.InnerException, true ) );
            }
            return excMessage.ToString();
        }

        private static EventLogEntryType ConvertType( EventLogerType eventType )
        {
            switch ( eventType )
            {
                case EventLogerType.Information: return EventLogEntryType.Information;
                case EventLogerType.Warning: return EventLogEntryType.Warning;
                case EventLogerType.Error: return EventLogEntryType.Error;
                default: return EventLogEntryType.Information;
            }
        }        
    }

    public enum EventLogerType
    {
        Information = 1,
        Warning,
        Error
    }

    public enum EventLogerID
    {
        SQLError = 8000,
        UnknownError = 0,
        BLException = 7000,
        AdditionalLogic = 10000
    }

    public enum EventLogerCategory : short
    {
        UnknownCategory = 1,
        FatalError = 2,
        HttpError = 3,
        WebServiceError = 4,
        WebServiceLogic = 5,
        BLError = 7
    }
}
