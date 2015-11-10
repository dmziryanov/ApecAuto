using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
	public static class SearchSparePartsLogDac
	{
		private static Func<LogDataContext, DateTime, DateTime, IQueryable<SearchSparePartsLog>> _getSearchSparePartsLog =
			CompiledQuery.Compile(
			( LogDataContext dc, DateTime date1, DateTime date2 ) =>
				dc.SearchSparePartsLogs.Where( l => date1 <= l.SearchDate && l.SearchDate <= date2 ) );

		public static void AddLog( DateTime searchDate, string partNumber, string clientIP )
		{
            using (var dc = new DCWrappersFactory<LogDataContext>())
            {
				dc.DataContext.ExecuteCommand(
					"insert into SearchSparePartsLog (SearchDate,PartNumber,ClientIP) values ({0},{1},{2})",
					searchDate, partNumber, clientIP );
                dc.SetCommit();
			}
		}

		public static void AddLog( DateTime searchDate, string partNumber, string manufacturer, string clientIP )
		{
            using (var dc = new DCWrappersFactory<LogDataContext>())
			{
                dc.DataContext.ExecuteCommand(
					"insert into SearchSparePartsLog (SearchDate,PartNumber,Manufacturer,ClientIP) values ({0},{1},{2},{3})",
					searchDate, partNumber, manufacturer, clientIP );
                dc.SetCommit();
			}
		}

        public static void AddWebServiceLog(DateTime searchDate, string partNumber, string manufacturer, string clientIP, string ClientID)
        {
            using (var dc = new DCWrappersFactory<LogDataContext>())
            {
                dc.DataContext.ExecuteCommand(
                    "insert into SearchSparePartsWebServiceLog (SearchDate,PartNumber,Manufacturer,ClientIP, ClientID) values ({0},{1},{2},{3},{4})",
                    searchDate, partNumber, manufacturer, clientIP, ClientID);
                dc.SetCommit();
            }
        }

		public static IQueryable<SearchSparePartsLog> GetLog( LogDataContext dc, DateTime date1, DateTime date2 )
		{
			return _getSearchSparePartsLog( dc, date1, date2 );
		}
	}
}
