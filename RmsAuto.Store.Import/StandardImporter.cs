using System;

namespace RmsAuto.Store.Import
{
    sealed class StandardImporter : ImporterBase
    {
        public StandardImporter(ImportSettings settings, ImportMode mode)
            : base(settings, mode)
        {
        }
   }
}
