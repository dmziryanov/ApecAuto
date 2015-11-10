using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Data.Interfaces;
using Laximo.Guayaquil.Data.net.laximo.ws.aftermarket;

namespace Laximo.Guayaquil.Data
{
    public class AftermarketProvider : LaximoWSProviderBase
    {
        public AftermarketProvider(string certPath, string certPwd, ICatalogCache cache) : base(certPath, certPwd, cache)
        {
        }

        public AftermarketProvider(string certPath, string certPwd, ICatalogCache cache, string locale) : base(certPath, certPwd, cache, locale)
        {
        }

        protected override ILaximoProxy CreateProxy()
        {
            return new Aftermarket();
        }

        #region commands

        public find_detail GetFindOEM(string oem, string options)
        {
            return GetFindOEM(oem, options, null);
        }

        public find_detail GetFindOEM(string oem, string options, string locale)
        {
            return GetData<find_detail>(GetQuery(CommandNames.FindOEM, GetLocale(locale), oem, options));
        }

        public FindDetails GetFindDetail(string detailId, string options)
        {
            return GetFindDetail(detailId, options, null);
        }

        public FindDetails GetFindDetail(string detailId, string options, string locale)
        {
            return GetData<FindDetails>(GetQuery(CommandNames.FindDetail, GetLocale(locale), detailId, options));
        }

        public FindReplacements GetFindReplacements(string detailId)
        {
            return GetFindReplacements(detailId, null);
        }

        public FindReplacements GetFindReplacements(string detailId, string locale)
        {
            return GetData<FindReplacements>(GetQuery(CommandNames.FindReplacements, GetLocale(locale), detailId));
        }

        public ManufacturerInfo GetManufacturerInfo(string manufacturerId)
        {
            return GetManufacturerInfo(manufacturerId, null);
        }

        public ManufacturerInfo GetManufacturerInfo(string manufacturerId, string locale)
        {
            return
                GetData<ManufacturerInfo>(GetQuery(CommandNames.GetManufacturerInfo, GetLocale(locale), manufacturerId));
        }

        #endregion
    }
}