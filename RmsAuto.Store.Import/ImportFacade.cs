using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;

namespace RmsAuto.Store.Import
{
    public static class ImportFacade
    {
        private const string _prices = "prices";
        private const string _priceFactors = "priceFactors";
        private const string _crosses = "crosses";
		private const string _crossesBrands = "crosses_brands";
		private const string _crossesGroups = "crosses_groups";
		private const string _crossesLinks = "crosses_links";
		public static readonly string PricesFormatName = _prices;
        public static readonly string PriceFactorsFormatName = _priceFactors;
        public static readonly string CrossesFormatName = _crosses;
		public static readonly string CrossesBrandsFormatName = _crossesBrands;
		public static readonly string CrossesGroupsFormatName = _crossesGroups;
		public static readonly string CrossesLinksFormatName = _crossesLinks;

        public static ImportReport ImportData(string format, ImportMode mode, Stream stream, string filename)
        {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentException("csv data format is not specified", "format");
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("csv data filename is not specified", "filename");

            var settings = GetSettings(format);
            
            ImporterBase importer = null;
            switch (format)
            {
                case _prices:
					if( mode == ImportMode.BulkInsert ||
					   mode == ImportMode.BulkInsertWithSkipped )
					{
						importer = new PricesImporter( settings, mode );
					}
					break;
				case _priceFactors:
                    if (mode == ImportMode.BulkInsert ||
                        mode == ImportMode.BulkInsertWithSkipped)
                    {
                        importer = new PriceFactorsImporter(settings, mode);
                    }
                    break;
                case _crosses:
				case _crossesBrands:
				case _crossesGroups:
				case _crossesLinks:
					if( mode == ImportMode.BulkInsert ||
                        mode == ImportMode.BulkInsertWithSkipped ||
                        mode == ImportMode.BulkDelete)
                    {
                        importer = new StandardImporter(settings, mode);
                    }
                    break;
            }
            
            if (importer == null)
            {
                throw new InvalidOperationException(
                    string.Format("{0} {1} importer not supported", format, mode));
            }

            try
            {
                return importer.ImportData(stream, filename);
            }
            catch (Exception ex)
            {
                return new ImportReport() { LoadResult = -1, Counters = new LoadCounters() { Aborted = -1 } };
                //TODO: Вернуть объект ImportReport с флагом, чтобы не удалять файл с данными и файл отмашки.
            }
            finally
            {
                importer.Dispose();
            }
        }

        public static LoadCounters ClearData(string format, object key)
        {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentException("csv data format doesn't specified", "format");
            if (key == null)
                throw new ArgumentNullException("key");

            if (format != _prices && format != _priceFactors)
            {
                throw new InvalidOperationException(format + " not supported");
            }

            LoadCounters counters = new LoadCounters();
            (new PricesCleaner(GetSettings(format))).ClearData(key, counters);
            return counters;
        }

        private static ImportSettings GetSettings(string format)
        {
            return (ImportSettings)ConfigurationManager.GetSection("rmsauto.import/" + format);
        }

		/// <summary>
		/// Логгирует в файл (для отладки)
		/// </summary>
		/// <param name="message">Отладочное сообщение</param>
		private static void LogMessageFacadeToFile(string message)
		{
			//По хорошему нужно брать из настроек
			string path = "c:\\_testsvc\\ \\logtempimportfacade.txt";
			using (var sw = new StreamWriter(path, true))
			{
				sw.WriteLine(message);
			}
		}
    }
}
