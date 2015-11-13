using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;
using System.Threading;

namespace RmsAuto.Store.Cms.Dac
{
	public static class NewsItemsDac
	{
		static Func<CmsDataContext, int, DateTime, string, IQueryable<NewsItem>> _getTopNews = CompiledQuery.Compile(
			( CmsDataContext dc, int topCount, DateTime lastDate, string locale ) =>
					dc.NewsItems
					.Where( o => o.NewsItemVisible && o.NewsItemDate <= lastDate && o.Localization == locale )
					.OrderByDescending( o => o.NewsItemDate )
					.Take( topCount ) );
		
		static Func<CmsDataContext, int, DateTime, string, IQueryable<NewsItem>> _getTopDiscountNews = CompiledQuery.Compile(
			(CmsDataContext dc, int topCount, DateTime lastDate, string locale) =>
					dc.NewsItems
					.Where(o => o.NewsItemVisible && o.NewsItemDate <= lastDate && o.IsDiscount && o.Localization == locale)
					.OrderByDescending(o => o.NewsItemDate)
					.Take(topCount));

		static Func<CmsDataContext, int, DateTime, NewsItem> _getNewsItem = CompiledQuery.Compile(
			( CmsDataContext dc, int id, DateTime lastDate ) =>
					dc.NewsItems
					.Where( o => o.NewsItemID == id && o.NewsItemVisible && o.NewsItemDate <= lastDate )
					.SingleOrDefault() );

        static Func<CmsDataContext, int?, DateTime, string, IOrderedQueryable<NewsItem>> _getAllNews = CompiledQuery.Compile(
            (CmsDataContext dc, int? year, DateTime lastDate, string locale) =>
                    dc.NewsItems
                    .Where(o => o.NewsItemVisible && o.NewsItemDate <= lastDate && o.Localization == locale && (!year.HasValue || o.NewsItemDate.Year == year.Value))
                    .OrderByDescending(o => o.NewsItemDate));

		public static IEnumerable<NewsItem> GetTopNews( int topCount )
		{
			using( var dc = new DCFactory<CmsDataContext>() )
			{
				dc.DataContext.DeferredLoadingEnabled = false;
				return _getTopNews(dc.DataContext, topCount, DateTime.Now, Thread.CurrentThread.CurrentCulture.Name).ToArray();
			}
		}

		public static IEnumerable<NewsItem> GetTopDiscountNews(int topCount)
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				dc.DataContext.DeferredLoadingEnabled = false;
				return _getTopDiscountNews(dc.DataContext, topCount, DateTime.Now, Thread.CurrentThread.CurrentCulture.Name).ToArray();
			}
		}

		public static NewsItem GetNewsItem( int id )
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				dc.DataContext.DeferredLoadingEnabled = false;
                return _getNewsItem(dc.DataContext, id, DateTime.Now );
			}
		}

        public static List<NewsItem> GetAllNews(int? year)
        {
            using (var dc = new  DCFactory<CmsDataContext>())
            {
                dc.DataContext.DeferredLoadingEnabled = false;
                return _getAllNews(dc.DataContext, year, DateTime.Now, Thread.CurrentThread.CurrentCulture.Name).ToList();
            }
        }
	}
}
