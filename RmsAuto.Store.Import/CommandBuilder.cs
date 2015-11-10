using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    class CommandBuilder
    {
        class ColumnInfo
        {
            public string Name;
            public SqlDbType SqlDbType;
            public bool IsKey;
            public bool IsNullable;
        }

        private List<ColumnInfo> _keyColumns = new List<ColumnInfo>();
        private List<ColumnInfo> _nonKeyColumns = new List<ColumnInfo>();
        private string _tableName;

        public CommandBuilder(DataTable schemaTable)
        {
            if (schemaTable == null)
                throw new ArgumentNullException("schemaTable");
            Init(schemaTable);            
        }

        public CommandBuilder(string table, SqlConnection connection)
        {
            if (string.IsNullOrEmpty(table))
                throw new ArgumentException("Table name cannot be empty", "table");
            if (connection == null)
                throw new ArgumentNullException("connection");

            var cmd = new SqlCommand("SELECT * FROM " + table, connection);
            using (var rdr = cmd.ExecuteReader(CommandBehavior.KeyInfo))
            {
                Init(rdr.GetSchemaTable());
            }
        }

        private void Init(DataTable schemaTable)
        {
            foreach (DataRow dr in schemaTable.Rows)
            {
                if (_tableName == null)
                    _tableName = (string)dr["BaseSchemaName"] + "." + (string)dr["BaseTableName"];

                var columnInfo = new ColumnInfo()
                {
                    Name = (string)dr["BaseColumnName"],
                    SqlDbType = (SqlDbType)dr["ProviderType"],
                    IsKey = (bool)dr["IsKey"],
                    IsNullable = Convert.ToBoolean(dr["AllowDBNull"])
                };

                if (columnInfo.IsKey)
                    _keyColumns.Add(columnInfo);
                else
                    _nonKeyColumns.Add(columnInfo);
            }
        }

        public bool HasNonKeyColumns
        {
            get { return _nonKeyColumns.Count > 0; }
        }
         

        public SqlCommand BuildSelectCommand(SqlConnection connection)
        {
            CheckConnection(connection);

            var cmd = connection.CreateCommand();
            var cmdTextBuilder = new StringBuilder("SELECT * FROM " + _tableName);

            BuildWhereClause(cmd.Parameters, cmdTextBuilder);
            cmd.CommandText = cmdTextBuilder.ToString();

            return cmd;
        }

        public SqlCommand BuildDeleteCommand(SqlConnection connection)
        {
            CheckConnection(connection);

            var cmd = connection.CreateCommand();
            var cmdTextBuilder = new StringBuilder("DELETE FROM " + _tableName);

            BuildWhereClause(cmd.Parameters, cmdTextBuilder);
            cmd.CommandText = cmdTextBuilder.ToString();
            return cmd;
        }

        public SqlCommand BuildInsertCommand(SqlConnection connection)
        {
            CheckConnection(connection);
            var cmd = connection.CreateCommand();

            var columnList = new StringBuilder();
            var valuesClause = new StringBuilder();

            foreach (ColumnInfo col in _keyColumns.Concat(_nonKeyColumns))
            {
                if (columnList.Length > 0)
                    columnList.Append(',');
                columnList.Append("[" + col.Name + "]");

                if (valuesClause.Length > 0)
                    valuesClause.Append(',');
                valuesClause.Append("@" + col.Name);
                cmd.Parameters.Add(CreateParameter(col));
            }

            cmd.CommandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                _tableName,
                columnList.ToString(),
                valuesClause.ToString());

            return cmd;
        }

        public SqlCommand BuildUpdateCommand(SqlConnection connection)
        {
            CheckConnection(connection);
            if (_nonKeyColumns.Count == 0)
                throw new InvalidOperationException(_tableName + " doesn't have any non-key columns to update");

            var cmd = connection.CreateCommand();

            var cmdTextBuilder = new StringBuilder("UPDATE " + _tableName + " SET ");

            for (int i = 0; i < _nonKeyColumns.Count; i++)
            {
                if (i > 0)
                    cmdTextBuilder.Append(',');
                cmdTextBuilder.AppendFormat("[{0}] = @{0}", _nonKeyColumns[i].Name);
                cmd.Parameters.Add(CreateParameter(_nonKeyColumns[i]));
            }

            BuildWhereClause(cmd.Parameters, cmdTextBuilder);
            cmd.CommandText = cmdTextBuilder.ToString();
            return cmd;
        }
               
        private SqlParameter CreateParameter(ColumnInfo columnInfo)
        {
            var param = new SqlParameter()
                    {
                        ParameterName = "@" + columnInfo.Name,
                        SourceColumn = columnInfo.Name,
                        SqlDbType = columnInfo.SqlDbType
                    };
            if (columnInfo.IsNullable)
                param.Value = Convert.DBNull;
            return param;
        }

        private void CheckConnection(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
        }

        private void BuildWhereClause(SqlParameterCollection cmdParameters, StringBuilder cmdTextBuilder)
        {
            if (_keyColumns.Count == 0)
                throw new InvalidOperationException(_tableName + " doesn't have primary key");
            cmdTextBuilder.Append(" WHERE ");
            for (int i = 0; i < _keyColumns.Count; i++)
            {
                if (i > 0)
                    cmdTextBuilder.Append(" AND ");
                cmdTextBuilder.AppendFormat("[{0}] = @{0}", _keyColumns[i].Name);
                cmdParameters.Add(CreateParameter(_keyColumns[i]));
            }
        }
    }
}
