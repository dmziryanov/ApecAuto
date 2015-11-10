using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    class CsvColumnInfo
    {
        public readonly string Name;
        public readonly string TypeName;
        public readonly bool ExcludeFromMapping;
        public readonly bool IsNullable;
        public readonly int MaxLength;
        public readonly bool DeletionKey;

        internal CsvColumnInfo(
            string name, 
            string typeName,
            bool excludeFromMapping,
            bool isNullable,
            int maxLength,
            bool deletionKey)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("CsvColumnInfo.Name cannot be empty", "name");
            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentException("CsvColumnInfo.TypeName cannot be empty", "typeName");
            Name = name;
            TypeName = typeName;
            ExcludeFromMapping = excludeFromMapping;
            IsNullable = isNullable;
            MaxLength = maxLength;
            DeletionKey = deletionKey;
        }
    }
}
