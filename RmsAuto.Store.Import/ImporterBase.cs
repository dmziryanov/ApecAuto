using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    //TODO: Вынести в общую базу
    public static class LockerManager
    {
        const string _SEMAPHORE = "PRICE_LOAD_LOCK";

        //Флаг, говорящий, что лок активировали отсюда
        private static bool isLockActivated = false;

        public static int AquireCheckLock(SqlConnection _Connection)
        {
            if (ConfigurationManager.AppSettings["UseLock"] != "true") { return 0; }

            if (!isLockActivated)
            {

                SqlCommand cmd = new SqlCommand(@"EXEC @result = sp_getapplock @Resource = '" + _SEMAPHORE + "', @LockTimeout = 10, @LockOwner='Session', @LockMode = 'Exclusive';", _Connection);
                cmd.Parameters.Add("@result", System.Data.SqlDbType.Int);
                cmd.Parameters[0].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                var res = (int)cmd.Parameters[0].Value;
                if (res == 0) { isLockActivated = true; }
                return res;
            }
            else
            {
                return 0;
            }
        }

        public static void _releaseAppLock(SqlConnection _Connection)
        {
            if (ConfigurationManager.AppSettings["UseLock"] != "true") { return; }
            if (isLockActivated)
            {
                string query = "exec sp_releaseapplock @Resource = '" + _SEMAPHORE + "', @LockOwner = 'Session'";
                SqlCommand cmd = new SqlCommand(query, _Connection);
                cmd.ExecuteNonQuery();
                isLockActivated = false;
            }
        }

        public static int CheckOwnStore(int supplierId, SqlConnection _Connection)
        {
            var cmd = _Connection.CreateCommand();
            cmd.CommandText = @"EXEC @result = [dbo].[spSelOwnStoreSuppliers] " + supplierId;

            cmd.Parameters.Add("@result", System.Data.SqlDbType.Int);
            cmd.Parameters[0].Direction = System.Data.ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            return (int)cmd.Parameters[0].Value;
        }

    }

	abstract class ImporterBase : IDisposable
	{
		protected SqlConnection _Connection;
		protected ImportActivity _Activity;
		protected ImportReport _CurrentReport;
		private bool _disposed;
        

		public ImporterBase( ImportSettings settings, ImportMode mode )
		{
          
            if( settings == null )
				throw new ArgumentNullException( "settings" );
			_Connection = new SqlConnection( ConfigurationManager.ConnectionStrings[ settings.ConnectionStringName ].ConnectionString );
			_Connection.Open();
			_Activity = new ImportActivity(
						mode,
						_Connection,
						settings.BatchParams,
						settings.TargetTable,
						settings.CsvDelimiterChar,
						settings.NumberFormatInfo,
						settings.CsvMetadata );

            
			_Activity.ValidateRecord += OnValidateRecord;

			_Activity.Complete += ( s, e ) => _CurrentReport.Counters = e.Counters;

			_Activity.DbError += ( s, e ) => _CurrentReport.AddError( e.ErrorInfo );
		}

		public void Dispose()
		{
			if( !_disposed )
			{
				_Activity.Dispose();
				_Connection.Dispose();
				_disposed = true;
			}
		}

		public ImportReport ImportData( Stream stream, string filename )
		{
			if( _disposed )
				throw new ObjectDisposedException( "ImporterBase" );
			if( stream == null )
				throw new ArgumentNullException( "stream" );
			if( string.IsNullOrEmpty( filename ) )
				throw new ArgumentException( "Filename cannot be empty", "filename" );

			_CurrentReport = new ImportReport() { Filename = filename, Mode = _Activity.Mode };
  
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			_Activity.Run( stream );

			sw.Stop();
			_CurrentReport.Elapsed = sw.Elapsed;
            LockerManager._releaseAppLock(_Connection);
			return _CurrentReport;
		}

		private void OnValidateRecord( object sender, ValidateRecordEventArgs e )
		{
			_CurrentReport.TotalCount++;

			if( e.IsValid )
			{
				OnValidRecord( e );
			}

			if( !e.IsValid )
			{
				_CurrentReport.AddError( e.Error );
			}
		}

		protected virtual void OnValidRecord( ValidateRecordEventArgs e ) 
        { 


        }
	}
}
