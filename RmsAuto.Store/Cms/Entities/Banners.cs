using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using RmsAuto.Common.Linq;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Cms.Entities
{
    partial class CmsDataContext
    {
        [Function(Name = "NEWID", IsComposable = true)]
        public Guid Random()
        { // to prove not used by our C# code... 
            throw new NotImplementedException();
        }
    }

    public class CatalogItemLink: CatalogItem
    {
        public Boolean Visible { get; set; }
        // Позиция (номер баннеро-места)
        public Byte Position { get; set; }
        // Признак привязки
        public Boolean Banded { get; set; }

        public static Boolean IsVisibleByBannerID(CmsDataContext dc, int bannerID, int catalogItemID, byte position)
        {
            var el =
                   from o in dc.BannersForCatalogItems
                   where o.BannerID == bannerID && o.CatalogItemID == catalogItemID && o.Position == position
                   select o.IsVisible;

            try
            {
                return (Boolean)el.First();
            }
            catch
            {
                return false;
            }
        }

        [Obsolete]
		public static Boolean IsVisibleByBannerID(int bannerID, int catalogItemID, byte position)
        {
            using (var dc = new CmsDataContext())
            {
                return IsVisibleByBannerID(dc, bannerID, catalogItemID, position);
            }
        }

        public static Boolean IsBanded(CmsDataContext dc, int bannerID, int catalogItemID, byte position)
        {
            var el =
                   from o in dc.BannersForCatalogItems
                   where o.BannerID == bannerID && o.CatalogItemID == catalogItemID && o.Position == position
                   select o;

            try
            {
                if (el.First() != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static Boolean IsBanded(int bannerID, int catalogItemID, byte position)
        {
            using (var dc = new CmsDataContext())
            {
                return IsBanded(dc, bannerID, catalogItemID, position);
            }
        }

        public void DeepPropertiesCopy(CatalogItem catalogItem)
        {
            this.BannerCount = catalogItem.BannerCount;
            this.CatalogItemCode = catalogItem.CatalogItemCode;
            this.CatalogItemID = catalogItem.CatalogItemID;
            this.CatalogItemImageUrl = catalogItem.CatalogItemImageUrl;
            this.CatalogItemMenuType = catalogItem.CatalogItemMenuType;
            this.CatalogItemName = catalogItem.CatalogItemName;
            this.CatalogItemOpenNewWindow = catalogItem.CatalogItemOpenNewWindow;
            this.CatalogItemPath = catalogItem.CatalogItemPath;
            this.CatalogItemPriority = catalogItem.CatalogItemPriority;
            this.CatalogItemQueryString = catalogItem.CatalogItemQueryString;
            this.IsServicePage = catalogItem.IsServicePage;
            this.PageBody = catalogItem.PageBody;
            this.PageDescription = catalogItem.PageDescription;
            this.PageFooter = catalogItem.PageFooter;
            this.PageKeywords = catalogItem.PageKeywords;
            this.PageTitle = catalogItem.PageTitle;
            this.ParentItemID = catalogItem.ParentItemID;
        }

        public CatalogItemLink FullDeepCopy(CmsDataContext dc, int bannerID, CatalogItem item, byte position)
        {
            var tmp = new CatalogItemLink();
            tmp.DeepPropertiesCopy(item);
            tmp.Position = position;
            tmp.Visible = IsVisibleByBannerID(dc, bannerID, tmp.CatalogItemID, position);
            tmp.Banded = IsBanded(dc, bannerID, tmp.CatalogItemID, position);
            return tmp;
        }
    }

    public partial class BannersForCatalogItem
    {
        // Для исключения проблем с конкуретным доступом
        public static Boolean Locked = false;

        public static BannersForCatalogItem GetRecord(CmsDataContext dc, int bannerID, int catalogItemID, byte position)
        {
            var el =
                   from o in dc.BannersForCatalogItems
                   where o.BannerID == bannerID && o.CatalogItemID == catalogItemID && o.Position == position
                   select o;
            try
            {
                return el.First();
            }
            catch
            {
                return null;
            }
        }
        
		[Obsolete]
		public static BannersForCatalogItem GetRecord(int bannerID, int catalogItemID, byte position)
        {
            using (var dc = new CmsDataContext())
            {
                return GetRecord(dc, bannerID, catalogItemID, position);
            }
        }

        public static BannersForCatalogItem GetRecord(CmsDataContext dc, int catalogItemID, byte position)
        {
            var el =
                   from o in dc.BannersForCatalogItems
                   where o.CatalogItemID == catalogItemID && o.Position == position
                   select o;
            try
            {
                return el.First();
            }
            catch
            {
                return null;
            }
        }
        public static BannersForCatalogItem GetRecord(int catalogItemID, byte position)
        {
            using (var dc = new CmsDataContext())
            {
                return GetRecord(dc, catalogItemID, position);
            }
        }

        //private static Expression<Func<BannersForCatalogItem, bool>> WorkingBannersExpression { get; set; }

        [Obsolete]
        public static BannersForCatalogItem GetRecordRandomBanner(int catalogItemID, byte position)
        {
            using (var dc = new CmsDataContext())
            {
                try
                {
                    List<BannersForCatalogItem> wBanners = new List<BannersForCatalogItem>();
                    //var workingBannersExpression = PredicateBuilder.False<BannersForCatalogItem>();
                    foreach (var bannersForCatalogItem in dc.BannersForCatalogItems)
                    {
                        if ((bannersForCatalogItem.Banners.RenderType == (byte)RenderType.ImageHtml
                            && bannersForCatalogItem.Banners.FileID != null && bannersForCatalogItem.CatalogItemID == catalogItemID
                            && bannersForCatalogItem.Position == position && bannersForCatalogItem.IsVisible)
                            ||
                            (bannersForCatalogItem.Banners.RenderType == (byte)RenderType.Html
                            && bannersForCatalogItem.Banners.Html != null && bannersForCatalogItem.CatalogItemID == catalogItemID
                            && bannersForCatalogItem.Position == position && bannersForCatalogItem.IsVisible)
                            ||
                            (bannersForCatalogItem.Banners.RenderType == (byte)RenderType.FileHtml
                            && bannersForCatalogItem.Banners.FileID != null && bannersForCatalogItem.Banners.Html != null
                            && bannersForCatalogItem.CatalogItemID == catalogItemID
                            && bannersForCatalogItem.Position == position && bannersForCatalogItem.IsVisible)
                            )
                            //workingBannersExpression = workingBannersExpression.Or(l => l.IsVisible == true);
                            wBanners.Add(bannersForCatalogItem);
                    }

                    Random r = new Random();

                    return wBanners.OrderBy(x => r.Next()).First();
                    //var lines = dc.BannersForCatalogItems.Where(workingBannersExpression);
                    //var el = from o in dc.BannersForCatalogItems
                    //         where o.IsVisible
                    //         orderby dc.Random()
                    //         select o;
                    //return el.First();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static List<Banners> GetBannersList(int catalogItemID, byte position)
        {
			using (var dc = new DCWrappersFactory<CmsDataContext>())
			{
				try
				{

					List<BannersForCatalogItem> wBanners = new List<BannersForCatalogItem>();
					//var workingBannersExpression = PredicateBuilder.False<BannersForCatalogItem>();
					foreach (var bannersForCatalogItem in dc.DataContext.BannersForCatalogItems)
					{
						if ((bannersForCatalogItem.Banners.RenderType == (byte)RenderType.ImageHtml
							&& bannersForCatalogItem.Banners.FileID != null && bannersForCatalogItem.CatalogItemID == catalogItemID
							&& bannersForCatalogItem.Position == position && bannersForCatalogItem.IsVisible)
							||
							(bannersForCatalogItem.Banners.RenderType == (byte)RenderType.Html
							&& bannersForCatalogItem.Banners.Html != null && bannersForCatalogItem.CatalogItemID == catalogItemID
							&& bannersForCatalogItem.Position == position && bannersForCatalogItem.IsVisible)
							||
							(bannersForCatalogItem.Banners.RenderType == (byte)RenderType.FileHtml
							&& bannersForCatalogItem.Banners.FileID != null && bannersForCatalogItem.Banners.Html != null
							&& bannersForCatalogItem.CatalogItemID == catalogItemID
							&& bannersForCatalogItem.Position == position && bannersForCatalogItem.IsVisible)
							)
							//workingBannersExpression = workingBannersExpression.Or(l => l.IsVisible == true);
							wBanners.Add(bannersForCatalogItem);
					}
					return wBanners.Select(x => x.Banners).ToList();
					//var lines = dc.BannersForCatalogItems.Where(workingBannersExpression);
					//var el = from o in dc.BannersForCatalogItems
					//         where o.IsVisible
					//         orderby dc.Random()
					//         select o;
					//return el.First();
				}

				catch
				{
					if (dc.DataContext.Connection.State == System.Data.ConnectionState.Open)
						dc.DataContext.Connection.Close();
					return null;
				}
				finally
				{
					
				}
			}
        }
    }
    



    [ScaffoldTable(true)]
    [MetadataType(typeof(BannerMetadata))]
    public partial class Banners
    {
        public static int GetBannerIDbyCatalogItemName(CmsDataContext dc, String catalogItemName)
        {
            var el =
                   from o in dc.BannersForCatalogItems
                   where o.CatalogItem.CatalogItemName == catalogItemName
                   select o.BannerID;

            try
            {
                return (int)el.First();
            }
            catch
            {
                return 0;
            }
        }

        public static int GetBannerIDbyCatalogItemName(String catalogItemName)
        {
            var dc = new CmsDataContext();
            return GetBannerIDbyCatalogItemName(dc,catalogItemName);
        }

        public static int GetBannerIDbyCatalogItemID(CmsDataContext dc, int catalogItemID)
        {
            var el =
                   from o in dc.BannersForCatalogItems
                   where o.CatalogItemID == catalogItemID
                   select o.BannerID;
            try
            {
                return (int)el.First();
            }
            catch
            {
                return 0;
            }
        }

        public static int GetBannerIDbyCatalogItemID(int catalogItemID)
        {
            var dc = new CmsDataContext();
            return GetBannerIDbyCatalogItemID(dc, catalogItemID);
        }

        public static int GetRenderTypeByBannerID(CmsDataContext dc, int bannerID)
        {
            var el =
                   from o in dc.Banners
                   where o.BannerID == bannerID
                   select o.RenderType;
            try
            {
                return (int)el.First();
            }
            catch
            {
                return 0;
            }
        }

        public static int GetRenderTypeByBannerID(int bannerID)
        {
            var dc = new CmsDataContext();
            return GetRenderTypeByBannerID(dc, bannerID);
        }

        public static int GetFileIDByBannerID(CmsDataContext dc, int bannerID)
        {
            var el =
                   from o in dc.Banners
                   where o.BannerID == bannerID
                   select o.FileID;
            try
            {
                return (int)el.First();
            }
            catch
            {
                return 0;
            }
        }

        public static int GetFileIDByBannerID(int bannerID)
        {
            var dc = new CmsDataContext();
            return GetFileIDByBannerID(dc, bannerID);
        }

        public static int GetMaxID(CmsDataContext dc)
        {
            var el =
                   from o in dc.Banners
                   select (o.BannerID);
            try
            {
                return (int)el.Max();
            }
            catch
            {
                return 0;
            }
        }

        public static int GetMaxID()
        {
            var dc = new CmsDataContext();
            return GetMaxID(dc);
        }

        public static Banners GetBannerByID(CmsDataContext dc, int bannerID)
        {
            var el =
                   from o in dc.Banners
                   where o.BannerID == bannerID
                   select o;
            try
            {
                return el.First();
            }
            catch
            {
                return null;
            }
        }

        public static Banners GetBannerByID(int bannerID)
        {
            using (var dc = new CmsDataContext())
            {
                return GetBannerByID(dc, bannerID);
            }
        }

        public static void ChangeLinkVisibilityToCatalogItem(int bannerID, int catalogItemID, byte position, bool isVisible)
        {
            using (var dc = new CmsDataContext())
            {
                Cms.Entities.BannersForCatalogItem b;
                try
                {
                    if (BannersForCatalogItem.Locked == false)
                    {
                        BannersForCatalogItem.Locked = true;
                        b = Cms.Entities.BannersForCatalogItem.GetRecord(dc, bannerID, catalogItemID, position);
                        b.IsVisible = isVisible;
                        dc.SubmitChanges();
                        BannersForCatalogItem.Locked = false;
                    }
                }
                catch (Exception)
                {
                    // значит записи такой нет
                    BannersForCatalogItem.Locked = false;
                }
            }
        }

        public static void AddLinkToCatalogItem(int bannerID, int catalogItemID, byte position)
        {
            using (var dc = new CmsDataContext())
            {
                try
                {
                    if (BannersForCatalogItem.Locked == false)
                    {
                        BannersForCatalogItem.Locked = true;
                        Cms.Entities.BannersForCatalogItem b = Cms.Entities.BannersForCatalogItem.GetRecord(dc, bannerID, catalogItemID, position);
                        if (b == null)
                        {
                            b = new BannersForCatalogItem();
                            b.BannerID = bannerID;
                            b.CatalogItemID = catalogItemID;
                            b.IsVisible = true;
                            b.Position = position;
                            dc.BannersForCatalogItems.InsertOnSubmit(b);
                            dc.SubmitChanges();
                        }
                        else
                        {
                            b.BannerID = bannerID;
                            b.CatalogItemID = catalogItemID;
                            b.IsVisible = true;
                            b.Position = position;
                            dc.SubmitChanges();
                        }
                        BannersForCatalogItem.Locked = false;
                    }
                }
                
                catch (Exception)
                {
                    // Записать не получилось, освободим
                    BannersForCatalogItem.Locked = false;
                }
            }
        }
        
        public static void RemoveLinkFromCatalogItem(int bannerID, int catalogItemID, byte position)
        {
            using (var dc = new CmsDataContext())
            {
                try
                {
                    if (BannersForCatalogItem.Locked == false)
                    {
                        BannersForCatalogItem.Locked = true;
                        Cms.Entities.BannersForCatalogItem b = Cms.Entities.BannersForCatalogItem.GetRecord(dc, bannerID, catalogItemID, position);
                        if (b != null)
                        {
                            dc.BannersForCatalogItems.DeleteOnSubmit(b);
                            dc.SubmitChanges();
                        }
                        BannersForCatalogItem.Locked = false;
                    }
                }

                catch (Exception)
                {
                    // Записать не получилось, освободим
                    BannersForCatalogItem.Locked = false;
                }
            }
        }

        public String RenderTypeText
        {
            get
            {
                return ( (Entities.RenderType) this.RenderType).ToTextOrName();
            }
        }
    }

    [DisplayName("Баннеры")]
    public partial class BannerMetadata
    {
        [ScaffoldColumn(true)]
        [DisplayName("ID")]
        public object BannerID { get; set; }
        
        [ScaffoldColumn(true)]
        [DisplayName("Название")]
        public object Name { get; set; }

        [ScaffoldColumn(true)]
        [DisplayName("URL")]
        public object URL { get; set; }

        [ScaffoldColumn(true)]
        [DisplayName("Вид отображения")]
        public object RenderTypeText { get; private set; }

        [ScaffoldColumn(false)]
        public object RenderType { get; set; }

        [ScaffoldColumn(false)]
        public object FileID { get; set; }

        [ScaffoldColumn(false)]
        public object BannersForCatalogItems { get; set; }

        [ScaffoldColumn(false)]
        public object File { get; set; }

    }
    public enum RenderType : byte
    {
        [Text("Изображение + HTML")]
        ImageHtml = 0,
        [Text("HTML")]
        Html = 1,
        [Text("Файл + HTML (Flash)")]
        FileHtml = 2
    }
}