using Laximo.Guayaquil.Data.Entities;

namespace Laximo.Guayaquil.Render
{
    public interface ICatalog
    {
        string Code { get; set; }
        int VehicleId { get; set; }
        int CategoryId { get; set; }
        string Ssd { get; set; }
        string PathId { get; set; }
        GetCatalogInfo Info { get; set; }
    }
}
