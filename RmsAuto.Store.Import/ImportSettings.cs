using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    class ImportSettings
    { 
        private List<CsvColumnInfo> _csvMetadata = new List<CsvColumnInfo>();

        public string ConnectionStringName
        {
            get; set;
        }

        public BatchParams BatchParams
        {
            get; set;
        }

        public string TargetTable
        {
            get; set;
        }

        public char CsvDelimiterChar
        {
            get; set;
        }

        public string NumberFormatInfo
        {
            get; set;
        }

        public IEnumerable<CsvColumnInfo> CsvMetadata
        {
            get { return _csvMetadata; }
        }

        public void AssCsvColumnInfo(CsvColumnInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            _csvMetadata.Add(info);
        }
    }
}
