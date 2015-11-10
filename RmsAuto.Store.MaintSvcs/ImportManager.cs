using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using java.util.zip;

using RmsAuto.Common.Misc;
using RmsAuto.Store.Import;
using System.Data.SqlClient;
using System.Configuration;

namespace RmsAuto.Store.MaintSvcs
{
	public class ImportManager : ManagerBase
	{
		private const string _ImpExt = ".imp";
		private const string _DelExt = ".del";
		private const string _LogExt = ".log";
		private const string _ErrExt = ".err";
		private const string _CsvExt = ".csv";
		private const string _ZipExt = ".zip";
        private const int _BufferSize = 4096; //делать буфер больше особо смысла нет, zipStream.read(sbuf) читает блоками ~3K
        
		private Dictionary<ImportEntity, string> _paths = new Dictionary<ImportEntity, string>();
		private Queue<AlarmFile> _fileQueue = new Queue<AlarmFile>();
		private bool _traceMode;

        public ImportManager(string pricePath, string priceFactorPath, string crossPath, string crossBrandsPath, string crossGroupsPath, string crossLinksPath,
        int secondsTimeout, bool traceMode, string pserviceName) : base(secondsTimeout, pserviceName)
		{
			string param = null;
			if( string.IsNullOrEmpty( pricePath ) ) param = "pricePath";
			if( string.IsNullOrEmpty( priceFactorPath ) ) param = "priceFactorPath";
			if( string.IsNullOrEmpty( crossPath ) ) param = "crossPath";
			if( string.IsNullOrEmpty( crossBrandsPath ) ) param = "crossBrandsPath";
			if( string.IsNullOrEmpty( crossGroupsPath ) ) param = "crossGroupsPath";
			if( string.IsNullOrEmpty( crossLinksPath ) ) param = "crossLinksPath";
			if( param != null ) throw new ArgumentException( "Import data path cannot be empty", param );

			_paths[ ImportEntity.Prices ] = pricePath;
			_paths[ ImportEntity.PriceFactors ] = priceFactorPath;
			_paths[ ImportEntity.Crosses ] = crossPath;
			_paths[ ImportEntity.CrossesBrands ] = crossBrandsPath;
			_paths[ ImportEntity.CrossesGroups ] = crossGroupsPath;
			_paths[ ImportEntity.CrossesLinks ] = crossLinksPath;

			_traceMode = traceMode;
		}

		protected override void DoWorkInternal()
		{
			AlarmFile alarmFile;

                while ((alarmFile = DequeueAlarmFile()) != null)
                {
                    Encoding encoding = Encoding.GetEncoding(Configuration.LogEncoding);
                    using (var log = new StreamWriter(
                        Path.ChangeExtension(alarmFile.FileInfo.FullName, _LogExt), false, encoding))
                    {
                        //log.WriteLine( alarmFile.Extension.Substring( 1 ) );
                        switch (alarmFile.FileType)
                        {
                            case AlarmFile.AlarmFileType.Import: log.WriteLine("imp"); break;
                            case AlarmFile.AlarmFileType.Delete: log.WriteLine("del"); break;
                            default: throw new Exception("Unknown alarm file type: " + alarmFile.FileType.ToString());
                        }

                        LoadCounters counters = new LoadCounters();
                        int rowcount = 0;
                        int skiprowcount = 0;
                        int errorcode = 0;
                        FileInfo dataFile = null;

                        if (_traceMode)
                            //TODO: Необходимо записывать тип события в лог
                            LogMessage("Start processing {0} file \"{1}\".", this.ServiceName, alarmFile.ImportEntity, alarmFile.FileInfo.Name);
                        try
                        {
                            switch (alarmFile.ImportEntity)
                            {
                                //Тот же самый case ImportEntity.PriceFactors.
                                case ImportEntity.Prices:
                                case ImportEntity.PriceFactors:
                                    if (alarmFile.FileType == AlarmFile.AlarmFileType.Import)
                                    {
                                        dataFile = PickUpDataFile(alarmFile.FileInfo);
                                        //Вернуть флаг, что отвалились
                                        counters = ProcessImportTask(
                                            alarmFile.ImportEntity,
                                            ImportMode.BulkInsert,
                                            dataFile,
                                            Path.ChangeExtension(alarmFile.FileInfo.FullName, _ErrExt),
                                            ref rowcount, ref skiprowcount, ref errorcode);
                                    }
                                    else
                                    {
                                        counters = ProcessDeleteTask(alarmFile.ImportEntity, alarmFile.FileInfo, ref rowcount);
                                    }
                                    break;
                                
                                case ImportEntity.Crosses:
                                case ImportEntity.CrossesBrands:
                                case ImportEntity.CrossesGroups:
                                case ImportEntity.CrossesLinks:

                                    dataFile = PickUpDataFile(alarmFile.FileInfo);
                                    counters = ProcessImportTask(
                                        alarmFile.ImportEntity,
                                        alarmFile.FileType == AlarmFile.AlarmFileType.Import ? ImportMode.BulkInsertWithSkipped : ImportMode.BulkDelete,
                                        dataFile,
                                        Path.ChangeExtension(alarmFile.FileInfo.FullName, _ErrExt),
                                        ref rowcount, ref skiprowcount, ref errorcode);

                                    break;
                            }

                        }
                        catch (Exception ex)
                        {
                            LogException(ex, log);
                            errorcode = 2900;
                        }
                        finally
                        {
                          
                            //проверить counters.Aborted и не удалять если отвалились
                            if (counters.Aborted == 0)
                            {
                                log.WriteLine("rowcount={0}", rowcount);
                                if (alarmFile.ImportEntity == ImportEntity.Crosses)
                                    log.WriteLine("skiprowcount={0}", skiprowcount);
                                log.WriteLine("errorcode={0}", errorcode);

                                alarmFile.Delete();


                                if (dataFile != null)
                                {
                                    DeleteDataFile(dataFile);
                                }
                            }
                            
                            if (_traceMode)
                            {
                                LogMessage("Finished processing {0} file \"{1}\" (ErrorCode = {2}, " +
                                "RowCount = {3}, BatchCount = {4}, BatchTime = {5}, DelayTime = {6}).", this.ServiceName,
                                alarmFile.ImportEntity, alarmFile.FileInfo.Name, errorcode, rowcount + skiprowcount,
                                counters.BatchCount, counters.BatchTime, counters.DelayTime);
                            }
                        }
                    }
                }
		}

		private AlarmFile DequeueAlarmFile()
		{
			if( _fileQueue.Count == 0 )
			{
				List<AlarmFile> list = new List<AlarmFile>();
				foreach( ImportEntity importEntity in Enum.GetValues( typeof( ImportEntity ) ) )
				{
					list.AddRange(
						Directory.GetFiles( _paths[ importEntity ], "*" + _ImpExt )
							.Select( f => new AlarmFile( new FileInfo( f ), importEntity, AlarmFile.AlarmFileType.Import ) )
						);

					list.AddRange(
						Directory.GetFiles( _paths[ importEntity ], "*" + _DelExt )
							.Select( f => new AlarmFile( new FileInfo( f ), importEntity, AlarmFile.AlarmFileType.Delete ) )
						);
				}
				foreach( var item in list.OrderBy( f => f.Index ) )
				{
					_fileQueue.Enqueue( item );
				}
			}

			return _fileQueue.Count != 0 ? _fileQueue.Dequeue() : null;
		}

		private LoadCounters ProcessDeleteTask( ImportEntity impEntity, FileInfo delFile, ref int rowcount )
		{
			using( var reader = delFile.OpenText() )
			{
				int supplierId = int.Parse(reader.ReadLine());
				var counters = ImportFacade.ClearData( impEntity.GetFormatName(), supplierId );
				rowcount = counters.Deleted;
				return counters;
			}
		}

		private LoadCounters ProcessImportTask( ImportEntity impEntity, ImportMode impMode,
			FileInfo dataFile, string logname, ref int rowcount, ref int skiprowcount, ref int errorcode )
		{
			using( var stream = ReadDataFile( dataFile ) )
			{
				var report = ImportFacade.ImportData( impEntity.GetFormatName(), impMode, stream, dataFile.Name );

				if( Configuration.LogDetails && ( report.HasValidationErrors || report.HasErrors ) )
				{
					Encoding encoding = Encoding.GetEncoding( Configuration.LogEncoding );
					using( var log = new StreamWriter( logname, false, encoding ) )
					{
						log.WriteLine( "found={0}", report.TotalCount );

						if( report.HasValidationErrors )
						{
							log.WriteLine( "*** Parse and validation errors ***" );
							foreach( var error in report.ValidationErrors )
							{
								log.WriteLine( "source record: " + error.RawRecord );
								foreach( var detail in error.Details )
								{
									detail.WriteToLog( log );
								}
							}
						}

						if( report.HasErrors )
						{
							foreach( var error in report.Errors )
								LogException( error, log );
						}
					}
				}

				rowcount = impMode == ImportMode.BulkDelete ? report.Counters.Deleted : report.Counters.Added;
				skiprowcount = report.Counters.Skipped;
				int totalrows = rowcount + skiprowcount;
				errorcode = totalrows == report.TotalCount ? 0 : totalrows == 0 ? 2920 : 2910;
               

				return report.Counters;
			}
		}

		private FileInfo PickUpDataFile( FileInfo alarmFile )
		{
			//TODO: сделать расширение txt
            FileInfo csvFile = new FileInfo( Path.ChangeExtension( alarmFile.FullName, _CsvExt ) );
			FileInfo zipFile = new FileInfo( Path.ChangeExtension( alarmFile.FullName, _ZipExt ) );
			//zip - файл приоритетнее csv
			return zipFile.Exists ? zipFile : csvFile;
		}

		private Stream ReadDataFile( FileInfo dataFile )
		{
			Stream stream;
			if( dataFile.Extension == _CsvExt )
			{
				stream = dataFile.OpenRead();
			}
			else if( dataFile.Extension == _ZipExt )
			{
				if( dataFile.Length == 0 )
					throw new Exception( "DataFile size is 0 bytes." );

				//раззиповать файл
				string csvFileName = Path.ChangeExtension( dataFile.FullName, ".csv" );
				ZipFile zf = new ZipFile( dataFile.FullName );
				try
				{
					string csvEntry = dataFile.GetNameOnly() + _CsvExt;
					ZipEntry ze = zf.getEntry( csvEntry );
					if( ze == null ) throw new Exception(
						 csvEntry + " entry not found in " + dataFile.Name );
					var zipStream = zf.getInputStream( ze );
					try
					{
						using( FileStream stm = new FileStream( csvFileName, FileMode.Create, FileAccess.Write ) )
						{
							int len;
							byte[] buf = new byte[ _BufferSize ];
							sbyte[] sbuf = new sbyte[ _BufferSize ];
							while( ( len = zipStream.read( sbuf ) ) > 0 )
							{
								Buffer.BlockCopy( sbuf, 0, buf, 0, len );
								stm.Write( buf, 0, len );
							}
						}						
					}
					finally
					{
						zipStream.close();
					}
				}
				finally
				{
					zf.close();
				}

				//прочитать раззипованный файл
				stream = new FileStream( csvFileName, FileMode.Open, FileAccess.Read );
			}
			else
			{
				throw new Exception( "Unknown data file extension: " + dataFile.Extension );
			}

			return stream;
		}

		private void DeleteDataFile( FileInfo dataFile )
		{
			if( dataFile.Extension == _CsvExt )
			{
				dataFile.Delete();
			}
			else if( dataFile.Extension == _ZipExt )
			{
				string csvFileName = Path.ChangeExtension( dataFile.FullName, ".csv" );
				File.Delete( csvFileName );
				dataFile.Delete();
			}
			else
			{
				throw new Exception( "Unknown data file extension: " + dataFile.Extension );
			}
		}

		private void LogException( Exception ex, StreamWriter log )
		{
			log.WriteLine( ex.Message );
			log.WriteLine( ex.StackTrace );
		}
	}

	class AlarmFile
	{
		public enum AlarmFileType
		{
			Import,
			Delete
		}

		public int Index { get; private set; }
		public FileInfo FileInfo { get; private set; }
		public ImportEntity ImportEntity { get; private set; }
		public AlarmFileType FileType { get; private set; }

		public AlarmFile( FileInfo fileInfo, ImportEntity importEntity, AlarmFileType fileType )
		{
			FileInfo = fileInfo;
			ImportEntity = importEntity;
			FileType = fileType;
			Index = int.Parse( fileInfo.GetNameOnly() );
		}

		public void Delete()
		{
			FileInfo.Delete();
		}
	}


}
