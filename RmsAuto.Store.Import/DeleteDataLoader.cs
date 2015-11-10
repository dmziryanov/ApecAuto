using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    sealed class DeleteDataLoader : BulkDataLoader
    {
        public DeleteDataLoader(SqlConnection connection, BatchParams batchParams,
            string targetTable, IEnumerable<string> columns) : base(connection, batchParams,
            "#" + targetTable.Replace('.', '_') + "_TMP_FOR_DELETION", columns, false)
        {
            _dataTable = targetTable;
        }

        private string _dataTable;

        protected override bool LoadBatch(IDataReader reader, BatchParams batchParams, LoadCounters rowCounters)
        {
            LoadCounters impCounters = new LoadCounters();
            bool aborted;
            int deleted;

            CreateTempTable(_TargetTable, _dataTable);
            try
            {
                //вставка ключей удаляемых записей во временную таблицу
                aborted = base.LoadBatch(reader, batchParams, impCounters);
                //удаление данных на основе содержимого временной таблицы
                deleted = this.DeleteData(batchParams, _TargetTable, _dataTable);
                //подсчет строк
                rowCounters.Deleted += deleted;
                rowCounters.Skipped += (impCounters.Added - deleted);
            }
            finally
            {
                DropTempTable(_TargetTable);
            }

            return aborted;
        }

        private void CreateTempTable(string tempTable, string dataTable)
        {
            string selectList = _Columns.Aggregate((acc, col) => acc + ", " + col);
            string cmdText = string.Format(@"
begin try drop table {0} end try
begin catch end catch
select top 0 {2} into {0} from {1}", tempTable, dataTable, selectList);
            using (SqlCommand cmd = new SqlCommand(cmdText, _Connection))
            {
                cmd.CommandTimeout = 5 * 60; //5m
                cmd.ExecuteNonQuery();
            }
        }

        private int DeleteData(BatchParams batchParams, string tempTable, string dataTable)
        {
            string joinCondition = _Columns
                .Select(col => "dat." + col + "=tmp." + col)
                .Aggregate((acc, c) => acc + " and " + c);
            string cmdText = string.Format(@"
delete {1}
from {1} as dat
join {0} as tmp on {2}
select @@rowcount", tempTable, dataTable, joinCondition);
            using (SqlCommand cmd = new SqlCommand(cmdText, _Connection))
            {
                cmd.CommandTimeout = batchParams.Timeout;
                return (int)cmd.ExecuteScalar();
            }

        }

        private void DropTempTable(string tempTable)
        {
            string cmdText = string.Format("drop table {0}", tempTable);
            using (SqlCommand cmd = new SqlCommand(cmdText, _Connection))
            {
                cmd.CommandTimeout = 5 * 60; //5m
                cmd.ExecuteNonQuery();
            }
        }
    }
}
