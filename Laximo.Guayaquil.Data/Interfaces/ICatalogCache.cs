namespace Laximo.Guayaquil.Data.Interfaces
{
    public interface ICatalogCache
    {
        IEntity GetCachedData(string request);
        void PutCachedData(string request, IEntity entity);
        bool Exists(string request);
    }
}
