extern alias DataStreams1; //to make sure it is DataStreams1.dll, reference alias is set to DataStreams1

using DataStreams1.DataStreams.Csv;
using DataStreams1.DataStreams.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    class ImportActivity : IDisposable
    {
        public event EventHandler<ValidateRecordEventArgs> ValidateRecord;
        public event EventHandler<CompleteEventArgs> Complete;

        private ImportMode _mode;
        private DataLoaderBase _loader;
        private CsvDataReader _reader;
        private char _csvDelimiterChar;
        private string _numberFormatInfo;
        private IEnumerable<CsvColumnInfo> _csvMetadata;
        private bool _disposed;
        
        public ImportActivity(
            ImportMode mode,
            SqlConnection connection,
            BatchParams batchParams,
            string targetTable,
            char csvDelimiterChar,
            string numberFormatInfo,
            IEnumerable<CsvColumnInfo> csvMetadata)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (string.IsNullOrEmpty(targetTable))
                throw new ArgumentException("Target table name cannot be empty", "targetTable");
            if (csvMetadata == null)
                throw new ArgumentNullException("csvMetadata");

            _mode = mode;
            _csvMetadata = csvMetadata;
            _csvDelimiterChar = csvDelimiterChar;
            _numberFormatInfo = numberFormatInfo;

            _loader = DataLoaderBase.CreateLoader(
                mode,
                connection,
                batchParams,
                targetTable,
                _csvMetadata
                .Where(sci => !sci.ExcludeFromMapping && (mode != ImportMode.BulkDelete || sci.DeletionKey))
                .Select(sci => sci.Name));
        }

        public ImportMode Mode
        {
            get { return _mode; }
        }

        internal string TargetTable
        {
            get { return _loader._TargetTable; }
        }
        
        public void Run(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (_disposed)
                throw new ObjectDisposedException("ImportActivity");
                       
            using (_reader = new CsvDataReader(stream, _csvDelimiterChar, Encoding.Default))
            {
				_reader.Settings.UseTextQualifier = false;
                foreach (var col in _csvMetadata)
                {
                    CsvDataReader.Column column = new CsvDataReader.Column(col.TypeName);
                    if (!string.IsNullOrEmpty(_numberFormatInfo))
                        column.FormatProvider = new CultureInfo(_numberFormatInfo);
                    _reader.Columns.Add(column, col.Name);
                }
                                
                _reader.ReadRecord += e => e.SkipRecord = OnValidateRecord(e.Values);

                LoadCounters rowCounters;

                _loader.LoadData((IDataReader)_reader, out rowCounters);

                OnComplete(new CompleteEventArgs() { Counters = rowCounters });
            }
        }

        public event EventHandler<LoadErrorEventArgs> DbError
        {
            add
            {
                _loader.LoadError += value;
            }
            remove
            {
                _loader.LoadError -= value;
            }
        }

        protected virtual void OnComplete(CompleteEventArgs args)
        {
           if (Complete != null)
               Complete(this, args);
        }
        
        protected virtual bool OnValidateRecord(DataReaderBase.RecordValuesCollection values)
        {
            if (ValidateRecord != null)
            {
                var details = ValidateCurrentRecord();
                var args = new ValidateRecordEventArgs(
                    (IDataRecord)_reader,
					values,
					this );

				foreach( var validationErrorDetail in details )
					args.AddValidationErrorDetail( validationErrorDetail );

                ValidateRecord(this, args);
                return !args.IsValid || args.SkipRecord;
            }
            return true;
        }

        private List<ValidationErrorDetail> ValidateCurrentRecord()
        {
            var details = new List<ValidationErrorDetail>();
            foreach (var sci in _csvMetadata)
            {
                var reason = ValidateColumnValue(sci);
                if (reason != ValidationFailReason.None)
                    details.Add(new ValidationErrorDetail()
                    {
                        ColumnName = sci.Name,
                        FailReason = reason
                    });
            }
            return details;
        }
                        
        #region IDisposable Members

        public void Dispose()
        {
            if (!_disposed)
            {
                _loader.Dispose();
                _disposed = true;
            }
        }

        #endregion

        public string RestoreRawRecord(DataReaderBase.RecordValuesCollection values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string value in values)
            {
                if (sb.Length > 0)
                    sb.Append(_csvDelimiterChar);
                sb.Append(!string.IsNullOrEmpty(value) ? value : "[NULL]");
            }
            return sb.ToString();
        }

        private ValidationFailReason ValidateColumnValue(CsvColumnInfo info)
        {
            object value = null;
            try
            {
                checked
                {
                    value = ((IDataRecord)_reader)[info.Name];
                }
            }
            catch (Exception)
            {
                return ValidationFailReason.TypeConvertionFailure;
            }

            if (Convert.IsDBNull(value) && !info.IsNullable &&
                (_mode != ImportMode.BulkDelete || info.DeletionKey))
                return ValidationFailReason.NullabilityViolation;

            if (value is string)
            {
                if (info.MaxLength != -1 && ((string)value).Length > info.MaxLength)
                    return ValidationFailReason.MaxLengthViolation;
            }

            return ValidationFailReason.None;
        }
    }
}
