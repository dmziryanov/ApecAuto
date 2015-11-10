using Laximo.Guayaquil.Data.Interfaces;

namespace Laximo.Guayaquil.Data.Entities
{
    public partial class ListCatalogs : IEntity {}
    public partial class GetCatalogInfo : IEntity {}
    public partial class FindVehicleByVIN : IEntity {}
    public partial class FindVehicleByFrame : IEntity {}
    public partial class FindVehicleByWizard : IEntity {}
    public partial class GetVehicleInfo : IEntity {}
    public partial class ListCategories : IEntity {}
    public partial class ListImageMapByUnit : IEntity {}
    public partial class GetFilterByDetail : IEntity {}
    public partial class GetFilterByUnit : IEntity {}
    public partial class GetUnitInfo : IEntity {}
    public partial class GetWizard : IEntity {}
    public partial class ListDetailsByUnit : IEntity {}
    public partial class ListQuickDetail : IEntity {}
    public partial class ListQuickGroups : IEntity {}
    public partial class ListUnits : IEntity {}
    public partial class ListDetailByOEM : IEntity {}

    public partial class VehicleInfo
    {
        private string _pathData;
        public string PathData
        {
            get { return _pathData; }
            set { _pathData = value; }
        }
    }

    public partial class GetUnitInfo
    {
        public GetUnitInfoRow FirstRow
        {
            get
            {
                if(row != null && row.Length > 0)
                {
                    return row[0];
                }
                return null;
            }
        }
    }

    //Aftermarket
    public partial class find_detail : IEntity {}
    public partial class FindReplacements : IEntity {}
    public partial class ManufacturerInfo : IEntity { }

    public struct CommandNames
    {
        //Catalog
        public const string ListCatalogs = "ListCatalogs";
        public const string GetCatalogInfo = "GetCatalogInfo";
        public const string FindVehicleByVIN = "FindVehicleByVIN";
        public const string FindVehicleByFrame = "FindVehicleByFrame";
        public const string FindVehicleByWizard = "FindVehicleByWizard";
        public const string GetVehicleInfo = "GetVehicleInfo";
        public const string ListCategories = "ListCategories";
        public const string ListImageMapByUnit = "ListImageMapByUnit";
        public const string GetFilterByDetail = "GetFilterByDetail";
        public const string GetFilterByUnit = "GetFilterByUnit";
        public const string GetUnitInfo = "GetUnitInfo";
        public const string GetWizard = "GetWizard";
        public const string ListDetailByUnit = "ListDetailByUnit";
        public const string ListQuickDetail = "ListQuickDetail";
        public const string ListQuickGroup = "ListQuickGroup";
        public const string ListUnits = "ListUnits";
        public const string ListDetailByOEM = "ListDetailByOEM";

        //Aftermarket
        public const string FindOEM = "FindOEM";
        public const string FindDetail = "FindDetail";
        public const string GetManufacturerInfo = "ManufacturerInfo";
        public const string FindReplacements = "FindReplacements";
    }
}
