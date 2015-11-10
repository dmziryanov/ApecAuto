using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Data.Interfaces;
using Laximo.Guayaquil.Data.net.laximo.ws;

namespace Laximo.Guayaquil.Data
{
    public class CatalogProvider : LaximoWSProviderBase
    {
        public CatalogProvider(string certPath, string certPwd, ICatalogCache cache) : base(certPath, certPwd, cache)
        {
        }

        public CatalogProvider(string certPath, string certPwd, ICatalogCache cache, string locale) : base(certPath, certPwd, cache, locale)
        {
        }

        protected override ILaximoProxy CreateProxy()
        {
            return new Catalog();
        }

        #region commands

        public ListCatalogs CatalogsList()
        {
            return CatalogsList(Locale, null);
        }

        public ListCatalogs CatalogsList(string locale, string ssd)
        {
            return GetData<ListCatalogs>(GetQuery(CommandNames.ListCatalogs, GetLocale(locale), ssd));
        }

        public GetCatalogInfo GetCatalogInfo(string catalogCode)
        {
            return GetCatalogInfo(catalogCode, Locale, null);
        }

        public GetCatalogInfo GetCatalogInfo(string catalogCode, string locale, string ssd)
        {
            return GetData<GetCatalogInfo>(GetQuery(CommandNames.GetCatalogInfo, GetLocale(locale), catalogCode, ssd));
        }

        public FindVehicleByVIN FindVehicleByVIN(string catalogCode, string VIN)
        {
            return FindVehicleByVIN(catalogCode, VIN, Locale, null);
        }

        public FindVehicleByVIN FindVehicleByVIN(string catalogCode, string VIN, string locale, string ssd)
        {
            return GetData<FindVehicleByVIN>(GetQuery(CommandNames.FindVehicleByVIN, GetLocale(locale), catalogCode, VIN, ssd));
        }

        public FindVehicleByFrame FindVehicleByFrame(string catalogCode, string frame, string frameNo)
        {
            return FindVehicleByFrame(catalogCode, frame, frameNo, Locale, null);
        }

        public FindVehicleByFrame FindVehicleByFrame(string catalogCode, string frame, string frameNo, string locale, string ssd)
        {
            return
                GetData<FindVehicleByFrame>(
                    GetQuery(CommandNames.FindVehicleByFrame, GetLocale(locale), catalogCode, frame, frameNo, ssd));
        }

        public FindVehicleByWizard FindVehicleByWizard(string catalogCode, string wizardId)
        {
            return FindVehicleByWizard(catalogCode, wizardId, Locale, null);
        }

        public FindVehicleByWizard FindVehicleByWizard(string catalogCode, string wizardId, string locale, string ssd)
        {
            return
                GetData<FindVehicleByWizard>(
                    GetQuery(CommandNames.FindVehicleByWizard, GetLocale(locale), catalogCode, wizardId, ssd));
        }

        public GetVehicleInfo GetVehicleInfo(string catalogCode, int vehicleId, string ssd)
        {
            return GetVehicleInfo(catalogCode, vehicleId, Locale, ssd);
        }

        public GetVehicleInfo GetVehicleInfo(string catalogCode, int vehicleId, string locale, string ssd)
        {
            return
                GetData<GetVehicleInfo>(
                    GetQuery(CommandNames.GetVehicleInfo, GetLocale(locale), catalogCode, vehicleId, ssd));
        }

        public ListCategories GetCategoriesList(string catalogCode, int vehicleId, int categoryId, string ssd)
        {
            return GetCategoriesList(catalogCode, vehicleId, categoryId, Locale, ssd);
        }

        public ListCategories GetCategoriesList(string catalogCode, int vehicleId, int categoryId, string locale, string ssd)
        {
            return
                GetData<ListCategories>(
                    GetQuery(CommandNames.ListCategories, GetLocale(locale), catalogCode, vehicleId, categoryId, ssd));
        }

        public ListUnits GetUnitsList(string catalogCode, int vehicleId, int categoryId, string ssd)
        {
            return GetUnitsList(catalogCode, vehicleId, categoryId, Locale, ssd);
        }

        public ListUnits GetUnitsList(string catalogCode, int vehicleId, int categoryId, string locale, string ssd)
        {
            return
                GetData<ListUnits>(
                    GetQuery(CommandNames.ListUnits, GetLocale(locale), catalogCode, vehicleId, categoryId, ssd));
        }

        public GetUnitInfo GetUnitInfo(string catalogCode, string unitId, string ssd)
        {
            return GetUnitInfo(catalogCode, unitId, Locale, ssd);
        }

        public GetUnitInfo GetUnitInfo(string catalogCode, string unitId, string locale, string ssd)
        {
            return
                GetData<GetUnitInfo>(
                    GetQuery(CommandNames.GetUnitInfo, GetLocale(locale), catalogCode, unitId, ssd));
        }

        public ListImageMapByUnit GetListImageMapByUnit(string catalogCode, string unitId, string ssd)
        {
            return GetListImageMapByUnit(catalogCode, unitId, Locale, ssd);
        }

        public ListImageMapByUnit GetListImageMapByUnit(string catalogCode, string unitId, string locale, string ssd)
        {
            return
                GetData<ListImageMapByUnit>(
                    GetQuery(CommandNames.ListImageMapByUnit, GetLocale(locale), catalogCode, unitId, ssd));
        }

        public ListDetailsByUnit GetListDetailByUnit(string catalogCode, string unitId, string ssd)
        {
            return GetListDetailByUnit(catalogCode, unitId, Locale, ssd);
        }

        public ListDetailsByUnit GetListDetailByUnit(string catalogCode, string unitId, string locale, string ssd)
        {
            return
                GetData<ListDetailsByUnit>(
                    GetQuery(CommandNames.ListDetailByUnit, GetLocale(locale), catalogCode, unitId, ssd));
        }

        public ListDetailByOEM GetListDetailByOEM(string catalogCode, string oem, string ssd)
        {
            return GetListDetailByOEM(catalogCode, oem, Locale, ssd);
        }

        public ListDetailByOEM GetListDetailByOEM(string catalogCode, string oem, string locale, string ssd)
        {
            return
                GetData<ListDetailByOEM>(GetQuery(CommandNames.ListDetailByOEM, GetLocale(locale), catalogCode, oem, ssd));
        }

        public GetWizard GetWizard(string catalogCode, string wizardId, string valueId)
        {
            return GetWizard(catalogCode, wizardId, valueId, Locale);
        }

        public GetWizard GetWizard(string catalogCode, string wizardId, string valueId, string locale)
        {
            return
                GetData<GetWizard>(
                    GetQuery(CommandNames.GetWizard, GetLocale(locale), catalogCode, wizardId, valueId));
        }

        public GetFilterByUnit GetFilterByUnit(string catalogCode, int vehicleId, string unitId, string filter, string ssd)
        {
            return GetFilterByUnit(catalogCode, vehicleId, unitId, filter, Locale, ssd);
        }

        public GetFilterByUnit GetFilterByUnit(string catalogCode, int vehicleId, string unitId, string filter, string locale, string ssd)
        {
            return
                GetData<GetFilterByUnit>(
                    GetQuery(CommandNames.GetFilterByUnit, GetLocale(locale), catalogCode, filter, vehicleId, unitId, ssd));
        }

        public GetFilterByDetail GetFilterByDetail(string catalogCode, string vehicleId, string unitId, string filter, string detailId, string ssd)
        {
            return GetFilterByDetail(catalogCode, vehicleId, unitId, filter, detailId, Locale, ssd);
        }

        public GetFilterByDetail GetFilterByDetail(string catalogCode, string vehicleId, string unitId, string filter,
                                                   string detailId, string locale, string ssd)
        {
            return
                GetData<GetFilterByDetail>(
                    GetQuery(CommandNames.GetFilterByDetail, GetLocale(locale), catalogCode, filter,
                             vehicleId, unitId, detailId, ssd));
        }

        public ListQuickGroups GetListQuickGroup(string catalogCode, int vehicleId)
        {
            return GetListQuickGroup(catalogCode, vehicleId, Locale, null);
        }

        public ListQuickGroups GetListQuickGroup(string catalogCode, int vehicleId, string locale, string ssd)
        {
            return GetData<ListQuickGroups>(
                GetQuery(CommandNames.ListQuickGroup, GetLocale(locale), catalogCode, vehicleId, ssd));
        }

        public ListQuickDetail GetListQuickDetail(string catalogCode, int vehicleId, string groupId, bool all)
        {
            return GetListQuickDetail(catalogCode, vehicleId, groupId, all, Locale, null);
        }

        public ListQuickDetail GetListQuickDetail(string catalogCode, int vehicleId, string groupId, bool all, string locale, string ssd)
        {
            return
                GetData<ListQuickDetail>(
                    GetQuery(CommandNames.ListQuickDetail, GetLocale(locale), catalogCode, vehicleId, groupId, ssd, all));
        }

        #endregion
    }
}
