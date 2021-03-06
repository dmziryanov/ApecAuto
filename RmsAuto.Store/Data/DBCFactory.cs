﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using RmsAuto.Store.Web;
using System.Configuration;
using RmsAuto.Store.Acctg;
using System.Diagnostics;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Data
{
    class DebugTextWriter : System.IO.TextWriter
    {
        public override void Write(char[] buffer, int index, int count)
        {
            System.Diagnostics.Debug.Write(new String(buffer, index, count));
        }

        public override void Write(string value)
        {
            System.Diagnostics.Debug.Write(value);
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.Default; }
        }
    }


    public class DCFactory<T> : IDisposable where T : DataContext, new()
    {
        private bool _openConnection = true; //параметр указывающий нужно ли открывать коннекцию в фабрике или это будет сделано явно вне ее
        private T _dataContext;
        private string _connectionStringTemplate = "Data Source={0};Initial Catalog={1};Integrated Security=False; Max Pool Size=600; Connection Timeout = 100;" + (ConfigurationManager.AppSettings["UseLocalBase"] == "true" ? "Integrated Security=true" : "User Id = sa; password = Newboyintown12");
        private bool _disposed;
        private bool _commit;
        private string _internalFranchName;

        public DCFactory()
            : this(IsolationLevel.ReadUncommitted, true, null, true)
        {
            //отсюда вызывается перегрузка конструктора с параметрами
        }

        private static bool isCallingAssemblyAdm = false;

        static DCFactory()
        {
            StackTrace st = new StackTrace(true);

            for (int i = 0; i < st.FrameCount; i++)
            {
                var CallingAssemblyName = st.GetFrame(i).GetMethod().Module.Assembly.FullName;
                isCallingAssemblyAdm = CallingAssemblyName.Contains("RmsAuto.Store.Adm") || isCallingAssemblyAdm;
            }
        }

        public DCFactory(bool pOpenConnection)
            : this(IsolationLevel.ReadUncommitted, true, null, pOpenConnection)
        {
            _openConnection = pOpenConnection;
        }

        public DCFactory(string pInternalFranchName)
            : this(IsolationLevel.ReadUncommitted, true, pInternalFranchName, true)
        {
        }

        public void SetCommit()
        {
            if (_disposed)
                throw new ObjectDisposedException("DCFactory");

            _commit = true;
        }

        public void SetUnCommit()
        {
            if (_disposed)
                throw new ObjectDisposedException("DCFactory");

            _commit = false;
        }

        public DCFactory(IsolationLevel isolationLevel, bool autoCommit, string pInternalFranchName, bool pOpenConnection)
        {
            _openConnection = pOpenConnection;
            // тоже самое по сути, но через Reflection
            //_dataContext = (T)System.Activator.CreateInstance(typeof(T), _getConnectionString());
            //Reflection медленнее генерик-типов
            _internalFranchName = pInternalFranchName;
            _dataContext = new T();
            _dataContext.Connection.ConnectionString = _getConnectionString();
            _commit = autoCommit;

            if (pOpenConnection)
            {
                _dataContext.Connection.Open();
                _dataContext.Transaction = _dataContext.Connection.BeginTransaction(isolationLevel);
            }
        }

        public T DataContext { get { return _dataContext; } }

        public string InternalFranchName
        {
            get
            {
                try
                {
                    if (_internalFranchName == null)
                    {
                        return SiteContext.Current.InternalFranchName;
                    }
                    else
                    {
                        return _internalFranchName;
                    }
                }
                catch
                {
                    string regionKey = ConfigurationManager.AppSettings["InternalFranchName"];
                    if (string.IsNullOrEmpty(regionKey))
                        throw new ConfigurationErrorsException("Key 'InternalFranchName' is missing!");
                    return regionKey;
                }
            }
        }
        //TODO вынести в словарь франчей
        public string ServerName
        {
            get
            {
                return AcctgRefCatalog.RmsFranches[InternalFranchName].ServerName;
            }
        }
        // Правило именования баз для франчей: ex_ + Служебное название франча + _store || common || log
        public string DbName
        {
            get
            {
                return "ex_" + AcctgRefCatalog.RmsFranches[InternalFranchName].DbName + "_" + _dataContext.Mapping.DatabaseName.Split('_')[2];
            }
        }


        private string _getConnectionString()
        {
            return string.Format(_connectionStringTemplate, ServerName, DbName);
        }

        #region IDisposable Members

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                //Освобождаем управляемые ресурсы
                if (disposing && _openConnection)
                {
                    try
                    {
                        if (_commit)
                            _dataContext.Transaction.Commit();
                        else
                            _dataContext.Transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(ex.Message, EventLogerID.SQLError, EventLogerCategory.FatalError);   
                    }
                    finally
                    {
                        if (_dataContext.Connection.State == ConnectionState.Open) { _dataContext.Connection.Close(); }
                    }
                }

                //Освобождаем неуправляемые ресурсы
                _dataContext.Dispose();

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true /*called by user directly*/);
            GC.SuppressFinalize(this);
        }

        ~DCFactory()
        {
            Dispose(false);
        }

        #endregion
    }

    public class DBCFactory<T> : IDisposable where T : DbContext, new()
    {
        private bool _openConnection = true; //параметр указывающий нужно ли открывать коннекцию в фабрике или это будет сделано явно вне ее
        private T _dataContext;
        private string _connectionStringTemplate = "Data Source={0};Initial Catalog={1};Integrated Security=False; Max Pool Size=600; Connection Timeout = 100;" + (ConfigurationManager.AppSettings["UseLocalBase"] == "true" ? "Integrated Security=true" : "User Id = sa; password = Newboyintown12");
        private bool _disposed;
        private bool _commit;
        private string _internalFranchName;

        public DBCFactory()
            : this(IsolationLevel.ReadUncommitted, true, null, true)
        {
            //отсюда вызывается перегрузка конструктора с параметрами
        }

        private static bool isCallingAssemblyAdm = false;

        static DBCFactory()
        {
            StackTrace st = new StackTrace(true);

            for (int i = 0; i < st.FrameCount; i++)
            {
                var CallingAssemblyName = st.GetFrame(i).GetMethod().Module.Assembly.FullName;
                isCallingAssemblyAdm = CallingAssemblyName.Contains("RmsAuto.Store.Adm") || isCallingAssemblyAdm;
            }
        }

        public DBCFactory(bool pOpenConnection)
            : this(IsolationLevel.ReadUncommitted, true, null, pOpenConnection)
        {
            _openConnection = pOpenConnection;
        }

        public DBCFactory(string pInternalFranchName)
            : this(IsolationLevel.ReadUncommitted, true, pInternalFranchName, true)
        {
        }

        public void SetCommit()
        {
            if (_disposed)
                throw new ObjectDisposedException("DCFactory");

            _commit = true;
        }

        public void SetUnCommit()
        {
            if (_disposed)
                throw new ObjectDisposedException("DCFactory");

            _commit = false;
        }

        public DBCFactory(IsolationLevel isolationLevel, bool autoCommit, string pInternalFranchName, bool pOpenConnection)
        {
            _openConnection = pOpenConnection;
            // тоже самое по сути, но через Reflection
            //_dataContext = (T)System.Activator.CreateInstance(typeof(T), _getConnectionString());
            //Reflection медленнее генерик-типов
            _internalFranchName = pInternalFranchName;
            _dataContext = new T();
            _dataContext.Database.Connection.ConnectionString = _getConnectionString();
            _commit = autoCommit;

           
        }

        public T DataContext { get { return _dataContext; } }

        public string InternalFranchName
        {
            get
            {
                try
                {
                    if (_internalFranchName == null)
                    {
                        return SiteContext.Current.InternalFranchName;
                    }
                    else
                    {
                        return _internalFranchName;
                    }
                }
                catch
                {
                    string regionKey = ConfigurationManager.AppSettings["InternalFranchName"];
                    if (string.IsNullOrEmpty(regionKey))
                        throw new ConfigurationErrorsException("Key 'InternalFranchName' is missing!");
                    return regionKey;
                }
            }
        }
        //TODO вынести в словарь франчей
        public string ServerName
        {
            get
            {
                return AcctgRefCatalog.RmsFranches[InternalFranchName].ServerName;
            }
        }
        // Правило именования баз для франчей: ex_ + Служебное название франча + _store || common || log
        public string DbName
        {
            get
            {
                return "ex_" + AcctgRefCatalog.RmsFranches[InternalFranchName].DbName + "_" + "store";
            }
        }


        private string _getConnectionString()
        {
            return string.Format(_connectionStringTemplate, ServerName, DbName);
        }

        #region IDisposable Members

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                //Освобождаем управляемые ресурсы
                if (disposing && _openConnection)
                {
                    try
                    {
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(ex.Message, EventLogerID.SQLError, EventLogerCategory.FatalError);
                    }
                    finally
                    {
                     
                    }
                }

                //Освобождаем неуправляемые ресурсы
                _dataContext.Dispose();

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true /*called by user directly*/);
            GC.SuppressFinalize(this);
        }

        ~DBCFactory()
        {
            Dispose(false);
        }

        #endregion
    }


    public class StoreContext : DbContext
    {
        public StoreContext()
        {
        }

        public StoreContext(string ConnectionString)
            : base(ConnectionString)
        {
        }

        public DbSet<ClientMessage> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
