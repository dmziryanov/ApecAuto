using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using Laximo.Guayaquil.Data.Entities;
using Laximo.Guayaquil.Data.Interfaces;

namespace Laximo.Guayaquil.Data.net.laximo.ws
{
    public partial class Catalog : ILaximoProxy
    {
    }
}

namespace Laximo.Guayaquil.Data.net.laximo.ws.aftermarket
{
    public partial class Aftermarket : ILaximoProxy
    {
    }
}

namespace Laximo.Guayaquil.Data
{
    public abstract class LaximoWSProviderBase : IDisposable
    {
        private const string DefaultLocal = "ru_RU";

        private const string ResponseStartTag = "<response>";
        private const string ResponseEndTag = "</response>";

        private readonly string _locale;
        private ILaximoProxy _proxy;
        private readonly ICatalogCache _cache;

        protected LaximoWSProviderBase(string certPath, string certPwd, ICatalogCache cache)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            Proxy.ClientCertificates.Add(new X509Certificate2(certPath, certPwd));

            _cache = cache;
        }

        protected LaximoWSProviderBase(string certPath, string certPwd, ICatalogCache cache, string locale)
            : this(certPath, certPwd, cache)
        {
            _locale = locale;
        }

        private ILaximoProxy Proxy
        {
            get { return _proxy ?? (_proxy = CreateProxy()); }
        }

        protected abstract ILaximoProxy CreateProxy();

        public string Locale
        {
            get
            {
                return string.IsNullOrEmpty(_locale) ? DefaultLocal : _locale;
            }
        }

        protected static string GetQuery(string command, params object[] p)
        {
            string parameters;
            switch (command)
            {
                case CommandNames.ListCatalogs:
                    parameters = String.Format("Locale={0}|ssd={1}", p);
                    break;
                case CommandNames.GetCatalogInfo:
                    parameters = String.Format("Locale={0}|Catalog={1}|ssd={2}", p);
                    break;
                case CommandNames.FindVehicleByVIN:
                    parameters = String.Format("Locale={0}|Catalog={1}|VIN={2}|ssd={3}", p);
                    break;
                case CommandNames.FindVehicleByFrame:
                    parameters = String.Format("Locale={0}|Catalog={1}|Frame={2}|FrameNo={3}|ssd={4}", p);
                    break;
                case CommandNames.FindVehicleByWizard:
                    parameters = String.Format("Locale={0}|Catalog={1}|WizardId={2}|ssd={3}", p);
                    break;
                case CommandNames.GetVehicleInfo:
                case CommandNames.ListQuickGroup:
                    parameters = String.Format("Locale={0}|Catalog={1}|VehicleId={2}|ssd={3}", p);
                    break;
                case CommandNames.ListCategories:
                case CommandNames.ListUnits:
                    parameters = String.Format("Locale={0}|Catalog={1}|VehicleId={2}|CategoryId={3}|ssd={4}", p);
                    break;
                case CommandNames.GetUnitInfo:
                case CommandNames.ListImageMapByUnit:
                case CommandNames.ListDetailByUnit:
                    parameters = String.Format("Locale={0}|Catalog={1}|UnitId={2}|ssd={3}", p);
                    break;
                case CommandNames.GetWizard:
                    parameters = String.Format("Locale={0}|Catalog={1}|WizardId={2}|ValueId={3}", p);
                    break;
                case CommandNames.GetFilterByUnit:
                    parameters = String.Format("Locale={0}|Catalog={1}|Filter={2}|VehicleId={3}|UnitId={4}|ssd={5}", p);
                    break;
                case CommandNames.GetFilterByDetail:
                    parameters =
                        String.Format(
                            "Locale={0}|Catalog={1}|Filter={2}|VehicleId={3}|UnitId={4}}DetailId={5}|ssd={6}", p);
                    break;
                case CommandNames.ListQuickDetail:
                    parameters = String.Format("Locale={0}|Catalog={1}|VehicleId={2}|QuickGroupId={3}|ssd={4}", p);
                    if (p.Length > 5 && Convert.ToBoolean(p[5]))
                    {
                        parameters += "|All=1";
                    }
                    break;
                case CommandNames.ListDetailByOEM:
                    parameters = String.Format("Locale={0}|Catalog={1}|OEM={2}|ssd={3}", p);
                    break;
                //Aftermarket
                case CommandNames.FindOEM:
                    parameters = String.Format("Locale={0}|OEM={1}|Options={2}", p);
                    break;
                case CommandNames.FindDetail:
                    parameters = String.Format("Locale={0}|DetailId={1}|Options={2}", p);
                    break;
                case CommandNames.GetManufacturerInfo:
                    parameters = String.Format("Locale={0}|ManufacturerId={1}", p);
                    break;
                case CommandNames.FindReplacements:
                    parameters = String.Format("Locale={0}|DetailId={1}", p);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(command);
            }
            return String.Format("{0}:{1}", command, parameters);
        }

        protected T GetData<T>(string query) where T : IEntity
        {
            return GetEntity<T>(query);
            return _cache != null ? GetCachedData<T>(query) : GetEntity<T>(query);
        }

        private T GetCachedData<T>(string query) where T : IEntity
        {
            if (_cache.Exists(query))
            {
                return (T)_cache.GetCachedData(query);
            }

            T entity = GetEntity<T>(query);
            _cache.PutCachedData(query, entity);
            return entity;
        }

        private T GetEntity<T>(string query) where T : IEntity
        {
            string data = _proxy.QueryData(query);
            T entity = Deserialize<T>(data);
            return entity;
        }

        private static T Deserialize<T>(string data) where T : IEntity
        {
            StringBuilder sb = new StringBuilder(data);
            if (data.StartsWith(ResponseStartTag))
            {
                sb.Remove(0, ResponseStartTag.Length);
            }
            if (data.EndsWith(ResponseEndTag))
            {
                sb.Remove(sb.Length - ResponseEndTag.Length, ResponseEndTag.Length);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(sb.ToString());
            return (T)serializer.Deserialize(stringReader);
        }

        protected string GetLocale(string locale)
        {
            return String.IsNullOrEmpty(locale) ? Locale : locale;
        }

        public void Dispose()
        {
            if (_proxy != null)
            {
                _proxy.Dispose();
                _proxy = null;
            }
        }
    }
}