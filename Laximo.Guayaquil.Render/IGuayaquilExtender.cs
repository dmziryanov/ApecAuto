namespace Laximo.Guayaquil.Render
{
    public interface IGuayaquilExtender
    {
        string GetLocalizedString(string name, GuayaquilTemplate renderer, params string[] p);
        string FormatLink(string type, object dataItem, ICatalog catalog, GuayaquilTemplate renderer);
    }
}