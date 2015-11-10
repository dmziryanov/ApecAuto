using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    [Obsolete]
    class SmartDataLoader : DataLoaderBase
    {
        private SqlCommand _selectCmd;
        private SqlCommand _insertCmd;
        private SqlCommand _updateCmd;
        
        public SmartDataLoader(
            SqlConnection connection,
            BatchParams batchParams,
            string targetTable,
            IEnumerable<string> columns) 
            : base(connection, batchParams, targetTable, columns) 
        {
            BuildCommands();           
        }

        protected override bool LoadBatch(IDataReader reader, BatchParams batchParams, LoadCounters rowCounters)
        {
            bool read = true;
            //batch size = 0 works as a single batch since --batchParams.Size != 0 will be never reached
            while (--batchParams.Size != 0 && (read = reader.Read()))
            {
                try
                {
                    if (RowExists(reader))
                    {
                        if (_updateCmd != null)
                        {
                            DoUpdate(reader);
                            rowCounters.Updated++;
                        }
                    }
                    else
                    {
                        DoInsert(reader);
                        rowCounters.Added++;
                    }
                }
                catch (Exception ex)
                {
                    OnLoadError(ex);
                }
            }

            return read;
        }
        
        private bool RowExists(IDataRecord record)
        {
            _selectCmd.AssignParamValues(record);
            using (SqlDataReader reader = _selectCmd.ExecuteReader(CommandBehavior.SingleRow))
            {
                return reader.Read();
            }
        }

        private void DoInsert(IDataRecord record)
        {
            _insertCmd.AssignParamValues(record);
            _insertCmd.ExecuteNonQuery();
        }

        private void DoUpdate(IDataRecord record)
        {
            _updateCmd.AssignParamValues(record);
            _updateCmd.ExecuteNonQuery();
        }

        private void BuildCommands()
        {
            var cmd = new SqlCommand("SELECT * FROM " + _TargetTable, _Connection);
            using (var rdr = cmd.ExecuteReader(CommandBehavior.KeyInfo))
            {
                var builder = new CommandBuilder(rdr.GetSchemaTable());
                _selectCmd = builder.BuildSelectCommand(_Connection);
                _insertCmd = builder.BuildInsertCommand(_Connection);
                if (builder.HasNonKeyColumns)
                    _updateCmd = builder.BuildUpdateCommand(_Connection);
            }
        }
    }

    static class Util
    {
        public static void AssignParamValues(this SqlCommand command, IDataRecord record)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (record == null)
                throw new ArgumentNullException("record");

            foreach (SqlParameter param in command.Parameters)
                param.Value = record[param.SourceColumn];
        }
    }
}
