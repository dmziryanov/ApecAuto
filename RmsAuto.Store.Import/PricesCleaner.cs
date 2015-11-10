using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    sealed class PricesCleaner : DataCleanerBase
    {
        public PricesCleaner(ImportSettings settings) : base(settings)
        {

        }

        protected override bool ClearBatch(SqlConnection connection,
            BatchParams batchParams, string targetTable, object key, LoadCounters counters)
        {
            if (!(key is int)) throw new
                ArgumentException("SupplierID key must be integer", "key");

            using (SqlCommand cmd = new SqlCommand(string.Format(
                "DELETE " + (batchParams.Size == 0 ? "" : "TOP({0}) ") +
                "FROM {1} WHERE SupplierID = @SupplierID",
                batchParams.Size, targetTable), connection))
            {
                cmd.CommandTimeout = batchParams.Timeout;
                cmd.Parameters.AddWithValue("@SupplierID", key);
                int rowsAffected = cmd.ExecuteNonQuery();

              

                counters.Deleted += rowsAffected;
                return rowsAffected >= batchParams.Size && batchParams.Size > 0;
                //batchParams.Size = 0 works as a single batch
            }

        }
    }
}
