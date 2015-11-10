using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    abstract class DataCleanerBase
    {
        public DataCleanerBase(ImportSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _connString = ConfigurationManager.
                ConnectionStrings[settings.ConnectionStringName].ConnectionString;
            _batchParams = settings.BatchParams;
            _targetTable = settings.TargetTable;
        }

        private string _connString;
        private BatchParams _batchParams;
        private string _targetTable;

        public void ClearData(object key, LoadCounters counters)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                try
                {
                    //Проверка в списке собственных складов
                    var cnt = LockerManager.CheckOwnStore((int)key, connection);

                    //Если поставщик находится в собственных складах наличия то пытаемся получить лок на базу данных, если не получаем то вываливаемся с исключением
                    if (cnt > 0 && LockerManager.AquireCheckLock(connection) < 0)
                    {
                        //Если здесь вываливаемся, то попадаем 
                        counters.Aborted = -1;
                        throw new Exception();
                    }

         

                    BatchUtil.ExecBatched(
                        () => ClearBatch(connection, _batchParams, _targetTable, key, counters),
                        _batchParams, counters);
                }
                catch
                {
                    //Если удалить не удалось, то пытаемся сделать это еще раз (флаг означает, что )
                    counters.Aborted = -1;
                }
                finally
                {
                    //if (counters.Aborted == 0)
                       // LockerManager._releaseAppLock(connection);
                }
            }
        }

        protected abstract bool ClearBatch(SqlConnection connection,
            BatchParams batchParams, string targetTable, object key, LoadCounters counters);
    }
}
