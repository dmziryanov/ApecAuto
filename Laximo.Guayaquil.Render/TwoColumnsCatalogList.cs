using System;

namespace Laximo.Guayaquil.Render
{
    public class TwoColumnsCatalogList : CatalogsList
    {
        private int _divide;

        public TwoColumnsCatalogList(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        private int Divide
        {
            get
            {
                if(_divide <= 0)
                {
                    _divide = (int)Math.Ceiling((double)Catalogs.row.Length / 2);
                }
                return _divide;
            }
        }

        protected override bool IsNeedWriteHeader(int indexRow)
        {
            return indexRow == 1 || indexRow == Divide + 1;
        }

        protected override bool IsNeedWriteFinishedRow(int indexRow)
        {
            return indexRow == Divide;
        }
    }
}
