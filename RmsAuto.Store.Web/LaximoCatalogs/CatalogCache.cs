using System;
using System.Web;
using System.Web.Caching;
using Laximo.Guayaquil.Data.Interfaces;

namespace RmsAuto.Store.Web.LaximoCatalogs
{
    public class CatalogCache : ICatalogCache
    {
        //TODO Laximo видимо это нужно вынести в настройки и заодно снести настройки для laximo из appSettings в свой раздел
        private const int TIMEOUT = 60;
        /// <summary>  
        /// Adds the specified cache object. it will last for max 1hr from last access  
        /// </summary>  
        public void PutCachedData(string request, IEntity entity)
        {
            HttpContext.Current.Cache.Insert(request, entity, null, Cache.NoAbsoluteExpiration,
                                 TimeSpan.FromMinutes(TIMEOUT));
        }

        /// <summary>  
        /// Check if object with the specified request exists.  
        /// </summary>  
        public bool Exists(string request)
        {
            return HttpContext.Current.Cache[request] != null;
        }

        /// <summary>  
        /// Gets the object with the specified request.  
        /// </summary>  
        public IEntity GetCachedData(string request)
        {
            return HttpContext.Current.Cache[request] as IEntity;
        }

        /// <summary>  
        /// Removes object with the specified request.  
        /// </summary>  
        public void Remove(string request)
        {
            HttpContext.Current.Cache.Remove(request);
        }
    }
}
