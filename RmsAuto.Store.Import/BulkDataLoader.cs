using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    class BulkDataLoader : DataLoaderBase
    {
        public BulkDataLoader(SqlConnection connection, BatchParams batchParams,
            string targetTable, IEnumerable<string> columns, bool countSkipped)
            : base(connection, batchParams, targetTable, columns)
        {
            _countSkipped = countSkipped;
        }

        private bool _countSkipped;

        protected override bool LoadBatch(IDataReader reader, BatchParams batchParams, LoadCounters rowCounters)
        {
            bool aborted = false;
            int loaded = 0;
            int affected = 0;
            //загрузка данных
            using (SqlBulkCopy sbc = new SqlBulkCopy(_Connection))
            {
                _Columns.Each(column => sbc.ColumnMappings.Add(column, column));

                sbc.NotifyAfter = 1;
                sbc.SqlRowsCopied += (s, e) =>
                    {
                        if (++loaded >= batchParams.Size && batchParams.Size > 0) e.Abort = true;
                        //batchParams.Size = 0 works as a single batch
                    };
                sbc.BulkCopyTimeout = batchParams.Timeout;
                sbc.DestinationTableName = _TargetTable;
                try
                {
                    sbc.WriteToServer(reader);
                }
                catch (OperationAbortedException)
                {
                    aborted = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            //подсчет числа загруженных (уникальных) строк
            if (!_countSkipped || loaded == 0) //if loaded == 0, @@rowcount may be invalid!
            {
                affected = loaded;
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand("select @@rowcount", _Connection))
                {
                    cmd.CommandTimeout = 2 * 60; //2m
                    affected = (int)cmd.ExecuteScalar();
                }
            }
            rowCounters.Added += affected;
            rowCounters.Skipped += (loaded - affected);

            return aborted;
        }
    }
}
