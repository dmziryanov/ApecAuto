using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace RmsAuto.Store.MaintSvcs
{
    public abstract class ManagerBase
    {
        private int _millisecondsTimeout;
        private Thread _workerThread;
        private EventWaitHandle _waitHandle;
        protected string ServiceName;

        public ManagerBase(int secondsTimeout, string serviceName)
        {
            if (secondsTimeout <= 0)                throw new ArgumentException(     "Timeout value must be positive",      "secondsTimeout");
            _millisecondsTimeout = secondsTimeout * 1000;
            _waitHandle = new ManualResetEvent(false);
            
            LogMessage("Параметр имени: " + serviceName, serviceName);
            if (serviceName == null) { throw new Exception("Не задано имя сервиса при инициализации базового класса сервиса, проверьте файл настроек Params.txt"); }
            this.ServiceName = serviceName;
        }

        public void Start()
        {
            if (_workerThread != null)
                throw new InvalidOperationException("Manager already started");

            _waitHandle.Reset();
            _workerThread = new Thread(WorkerProc);
            _workerThread.Start();
       }

        public void Stop()
        {
            if (_workerThread == null)
                throw new InvalidOperationException("Manager already stopped");

            _waitHandle.Set();
            _workerThread.Join();
            _workerThread = null;
        }

        protected abstract void DoWorkInternal();

        private void WorkerProc()
        {
            do
            {
                try
                {
                    DoWorkInternal();
                }
                catch (Exception ex)
                {
                    LogError(ex, this.ServiceName);
                }
            } while (!_waitHandle.WaitOne(_millisecondsTimeout));
        }

        private void LogEvent(string msg, EventLogEntryType etype, string eventSource)
        {
            if (!EventLog.SourceExists(eventSource))
                EventLog.CreateEventSource( eventSource, null);
            EventLog.WriteEntry(eventSource, msg, etype);
        }

        protected void LogMessage(string msg, string source)
        {
            LogEvent(msg, EventLogEntryType.Information, source);
        }

        protected void LogMessage(string msg, string source, params object[] args)
        {
            LogEvent(string.Format(msg, args), EventLogEntryType.Information, source);
        }

        protected void LogError(Exception ex, string source)
        {
            LogEvent(ex.ToString(), EventLogEntryType.Error, source);
        }

		/// <summary>
		/// Логгирует в файл (для отладки)
		/// </summary>
		/// <param name="message">Отладочное сообщение</param>
		protected void LogMessageToFile(string message)
		{
			//По хорошему нужно брать из настроек
			string path = "c:\\_testsvc\\apec\\logtemp.txt";
			using (var sw = new StreamWriter(path, true))
			{
				sw.WriteLine(message);
			}
		}
    }
}
