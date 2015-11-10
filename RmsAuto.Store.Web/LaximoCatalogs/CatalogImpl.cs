using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Render;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    internal class CatalogImpl : ICatalog
    {
        private string _code;
        private int _vehicleId;
        private int _categoryId;
        private string _ssd;
        private string _pathId;
        private GetCatalogInfo _info;

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public int VehicleId
        {
            get { return _vehicleId; }
            set { _vehicleId = value; }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public string Ssd
        {
            get { return _ssd; }
            set { _ssd = value; }
        }

        public string PathId
        {
            get { return _pathId; }
            set { _pathId = value; }
        }

        public GetCatalogInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }
    }
}
