using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    abstract class DataLoaderBase : IDataLoader, IDisposable
    {
        public static DataLoaderBase CreateLoader(ImportMode mode, SqlConnection connection,
            BatchParams batchParams, string targetTable, IEnumerable<string> columns)
        {
            switch (mode)
            {
                case ImportMode.BulkInsert:
                    return new BulkDataLoader(connection, batchParams, targetTable, columns, false);
                case ImportMode.BulkInsertWithSkipped:
                    return new BulkDataLoader(connection, batchParams, targetTable, columns, true);
                case ImportMode.BulkDelete:
                    return new DeleteDataLoader(connection, batchParams, targetTable, columns);
                case ImportMode.Smart:
                    return new SmartDataLoader(connection, batchParams, targetTable, columns);
                default:
                    throw new InvalidOperationException("Unknown ImportMode - '" + mode.ToString() + "'");
            }
        }

        private SqlConnection _connection;
        private BatchParams _batchParams;
        private string _targetTable;
        private IEnumerable<string> _columns;
        private bool _closeConnection;
        private bool _disposed;

        public event EventHandler<LoadErrorEventArgs> LoadError;

        public DataLoaderBase(SqlConnection connection, BatchParams batchParams,
            string targetTable, IEnumerable<string> columns)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (string.IsNullOrEmpty(targetTable))
                throw new ArgumentException("TargetTable name cannot be empty", "targetTable");
            if (columns == null)
                throw new ArgumentNullException("columns");
            if (columns.Count() == 0)
                throw new ArgumentException("Column list cannot be empty", "columns");

            _connection = connection;
            _batchParams = batchParams;
            _targetTable = targetTable;
            _columns = columns;

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
                _closeConnection = true;
            }
        }

        #region IDataLoader Members

        public void LoadData(IDataReader reader, out LoadCounters rowCounters)
        {
            if (_disposed)
                throw new ObjectDisposedException("DataLoaderBase");
            if (reader == null)
                throw new ArgumentNullException("reader");

            var counters = new LoadCounters();

            BatchUtil.ExecBatched(
                () => LoadBatch(reader, _batchParams, counters),
                _batchParams, counters);

            rowCounters = counters;
        }
        
        #endregion

        protected abstract bool LoadBatch(IDataReader reader,
            BatchParams batchParams, LoadCounters rowCounters);
        
        protected SqlConnection _Connection
        {
            get { return _connection; }
        }

        internal protected string _TargetTable
        {
            get { return _targetTable; }
        }

        protected IEnumerable<string> _Columns
        {
            get { return _columns; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_closeConnection && _connection.State == ConnectionState.Open)
                    _connection.Close();
                _disposed = true;
            }
        }

        #endregion

        protected virtual void OnLoadError(Exception errorInfo)
        {
            if (LoadError != null)
                LoadError(this, new LoadErrorEventArgs(errorInfo));
            else
                throw errorInfo;
        }
    }
}
