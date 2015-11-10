using System;

namespace Laximo.Guayaquil.Render
{
    public class ThreeColumnsCatalogList : CatalogsList
    {
        private int _firstDivide;
        private int _secondDivide;

        public ThreeColumnsCatalogList(IGuayaquilExtender extender, ICatalog catalog) : base(extender, catalog)
        {
        }

        private int FirstDivide
        {
            get
            {
                if (_firstDivide <= 0)
                {
                    _firstDivide = (int)Math.Ceiling((double)Catalogs.row.Length / 3);
                }
                return _firstDivide;
            }
        }

        private int SecondDivide
        {
            get
            {
                if(_secondDivide <= 0)
                {
                    _secondDivide = FirstDivide*2;
                }
                return _secondDivide;
            }
        }

        protected override bool IsNeedWriteHeader(int indexRow)
        {
            return indexRow == 1 || indexRow == FirstDivide + 1 || indexRow == SecondDivide + 1;
        }

        protected override bool IsNeedWriteFinishedRow(int indexRow)
        {
            return indexRow == FirstDivide || indexRow == SecondDivide;
        }
    }
}