using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace RmsAuto.Store.Import
{
    class ImportConfigurationSectionHandler : IConfigurationSectionHandler
    {
        const char _DefaultCsvDelimiterChar = ';';

        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, XmlNode section)
        {
            var settings = new ImportSettings();

            var csnAttr = section.Attributes["connectionStringName"];
            if (csnAttr == null || string.IsNullOrEmpty(csnAttr.Value))
                throw new ConfigurationErrorsException("ConnectionString name not specified");
            settings.ConnectionStringName = csnAttr.Value;

            BatchParams batchParams;
            var batchSizeAttr = section.Attributes["batchSize"];
            var batchTimeoutAttr = section.Attributes["batchTimeout"];
            var batchDelayAttr = section.Attributes["batchDelay"];
            if (batchSizeAttr == null || string.IsNullOrEmpty(batchSizeAttr.Value))
                throw new ConfigurationErrorsException("Batch size is not specified");
            if (batchTimeoutAttr == null || string.IsNullOrEmpty(batchTimeoutAttr.Value))
                throw new ConfigurationErrorsException("Batch timeout is not specified");
            if (batchDelayAttr == null || string.IsNullOrEmpty(batchDelayAttr.Value))
                throw new ConfigurationErrorsException("Batch delay is not specified");
            if (!int.TryParse(batchSizeAttr.Value, out batchParams.Size) || batchParams.Size < 0)
                throw new ConfigurationErrorsException("Batch size is not valid");
            if (!int.TryParse(batchTimeoutAttr.Value, out batchParams.Timeout) || batchParams.Timeout < 0)
                throw new ConfigurationErrorsException("Batch timeout is not valid");
            batchParams.DelayMode = batchDelayAttr.Value.EndsWith("%")
                ? DelayMode.Percentage : DelayMode.Мilliseconds;
            if (batchParams.DelayMode == DelayMode.Percentage)
                batchDelayAttr.Value = batchDelayAttr.Value.Substring(0, batchDelayAttr.Value.Length - 1);
            if (!int.TryParse(batchDelayAttr.Value, out batchParams.Delay) || batchParams.Delay < 0)
                throw new ConfigurationErrorsException("Batch delay is not valid");
            settings.BatchParams = batchParams;

            var tableAttr = section.Attributes["targetTable"];
            if (tableAttr == null || string.IsNullOrEmpty(tableAttr.Value))
                throw new ConfigurationErrorsException("Target table is not specified");
            settings.TargetTable = tableAttr.Value;

            var delimAttr = section.Attributes["csvDelimiterChar"];
            settings.CsvDelimiterChar = (delimAttr != null && !string.IsNullOrEmpty(delimAttr.Value)) ?
                Convert.ToChar(System.Text.RegularExpressions.Regex.Unescape(delimAttr.Value)) : 
                _DefaultCsvDelimiterChar;

            var formatAttr = section.Attributes["numberFormatInfo"];
            if (formatAttr != null)
                settings.NumberFormatInfo = formatAttr.Value;

            foreach (XmlNode column in section["csvMetadata"])
            {
                if (column.NodeType == XmlNodeType.Element)
                {
                    bool exclude = false;
                    var exclAttr = column.Attributes["excludeFromMapping"];
                    if (exclAttr != null && !string.IsNullOrEmpty(exclAttr.Value))
                        exclude = bool.Parse(exclAttr.Value);

                    bool isNullable = false;
                    var nullAttr = column.Attributes["isNullable"];
                    if (nullAttr != null && !string.IsNullOrEmpty(nullAttr.Value))
                        isNullable = bool.Parse(nullAttr.Value);

                    int maxLength = -1;
                    var lenAttr = column.Attributes["maxLength"];
                    if (lenAttr != null && !string.IsNullOrEmpty(lenAttr.Value))
                        maxLength = int.Parse(lenAttr.Value);

                    bool deletionKey = false;
                    var keyAttr = column.Attributes["deletionKey"];
                    if (keyAttr != null && !string.IsNullOrEmpty(keyAttr.Value))
                        deletionKey = bool.Parse(keyAttr.Value);

                    settings.AssCsvColumnInfo(new CsvColumnInfo(
                        column.Attributes["name"].Value,
                        column.Attributes["type"].Value,
                        exclude,
                        isNullable,
                        maxLength,
                        deletionKey));
                }
            }
            return settings;
        }

        #endregion
    }
}
