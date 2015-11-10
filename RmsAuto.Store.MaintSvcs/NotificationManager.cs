using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Store.BL;
using System.Diagnostics;

namespace RmsAuto.Store.MaintSvcs
{
	public class NotificationManager : ManagerBase
	{
        public NotificationManager(int secondsTimeout, string pserviceName) : base(secondsTimeout, pserviceName)
		{

		}

		protected override void DoWorkInternal()
		{
            if (SheduledExecution())
            {
                using (SendOrderLineTrackingAlertsLog log = new SendOrderLineTrackingAlertsLog(LogError, LogMessage, this.ServiceName))
                {
                    OrderBO.SendOrderLineTrackingAlerts(log);
                }
            }
		}

        private bool SheduledExecution()
        {
            int hour = DateTime.Now.Hour;
            return (9 <= hour && hour < 21) || hour == 3;
        }

		class SendOrderLineTrackingAlertsLog : OrderBO.ISendOrderLineTrackingAlertsLog, IDisposable
		{
			Action<Exception, string> _logErrorAction;
			Action<string, string> _logMessageAction;
			Stopwatch _stopWatch;
            string _ServiceName;

			int _successfulCount = 0;
            int _managerListFailedCount = 0;
			int _emptyEmailCount = 0;
			int _invalidEmailCount = 0;
			int _errorCount = 0;

            public SendOrderLineTrackingAlertsLog(Action<Exception, string> logErrorAction, Action<string, string> logMessageAction, string pServiceName)
            {
                _logErrorAction = logErrorAction;
                _logMessageAction = logMessageAction;
                _ServiceName = pServiceName;
                _stopWatch = Stopwatch.StartNew();
            }


			public void LogSuccessfulAlert( string clientId )
			{
				_successfulCount++;
			}

            public void LogManagerListRequestFailed()
            {
                _managerListFailedCount++;
                _logMessageAction("Manager list request failed", _ServiceName);
            }

            public void LogEmptyEmail(string clientId)
			{
				_emptyEmailCount++;
				_logMessageAction(string.Format( "Send alert to client {0}: empty email", clientId ), _ServiceName );
			}

			public void LogInvalidEmail( string clientId, string email )
			{
				_invalidEmailCount++;
				_logMessageAction( string.Format( "Send alert to client {0}: invalid email: {1}", clientId, email ), _ServiceName );
			}

			public void LogError( string clientId, Exception ex )
			{
				_errorCount++;
                _logErrorAction(ex, this._ServiceName);
			}

			public void Dispose()
			{
				_stopWatch.Stop();

				StringBuilder sb = new StringBuilder();
				sb.AppendFormat( "Send alerts completed.\r\n" );
				sb.AppendFormat( "Successful alerts: {0}\r\n", _successfulCount );
				sb.AppendFormat( "Failed manager list requests: {0}\r\n", _managerListFailedCount );
				sb.AppendFormat( "Empty emails: {0}\r\n", _emptyEmailCount );
				sb.AppendFormat( "Invalid emails: {0}\r\n", _invalidEmailCount );
				sb.AppendFormat( "Errors: {0}\r\n", _errorCount );
				sb.AppendFormat( "Elapsed time: {0}", _stopWatch.Elapsed );

				_logMessageAction( sb.ToString(), this._ServiceName );
			}

		}
	}
}
