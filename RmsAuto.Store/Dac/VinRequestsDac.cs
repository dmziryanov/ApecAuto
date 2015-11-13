using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
    public static class VinRequestsDac
    {
        private static Func<StoreDataContext, string, IOrderedQueryable<VinRequest>> _getClientVinRequests = 
            CompiledQuery.Compile((StoreDataContext dc, string clientId) =>
                from vr in dc.VinRequests
                where vr.ClientId == clientId
                orderby vr.RequestDate descending
                select vr);

        private static Func<StoreDataContext, int, IQueryable<VinRequest>> _getVinRequest =
            CompiledQuery.Compile((StoreDataContext dc, int requestId) =>
                from vr in dc.VinRequests
                where vr.Id == requestId
                select vr);

        private static Func<StoreDataContext, int, IQueryable<VinRequestItem>> _getRequestItems =
            CompiledQuery.Compile((StoreDataContext dc, int requestId) =>
                from item in dc.VinRequestItems
                where item.RequestId == requestId
                select item);

        private static Func<StoreDataContext, int, IQueryable<VinRequestItem>> _getRequestItem =
           CompiledQuery.Compile((StoreDataContext dc, int requestItemId) =>
               from item in dc.VinRequestItems
               where item.Id == requestItemId
               select item);


        public static IEnumerable<VinRequest> GetRequests(string clientId)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                return _getClientVinRequests(ctx.DataContext, clientId).ToList();
            }
        }

        public static VinRequest GetRequest(int requestId)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                return _getVinRequest(ctx.DataContext, requestId).Single();
            }
        }

        public static void SetRequestProceeded(int requestId, DateTime answerDate)
        {
            using (StoreDataContext ctx = new StoreDataContext())
            {
                var request = _getVinRequest(ctx, requestId).Single();
                request.Proceeded = true;
                request.AnswerDate = answerDate;
                ctx.SubmitChanges();
            }
        }

        public static IEnumerable<VinRequestItem> GetRequestItems(int requestId)
        {
            using (StoreDataContext ctx = new StoreDataContext())
            {
                return _getRequestItems(ctx, requestId).ToList();
            }
        }

        public static void UpdateVinRequestItem(VinRequestItem item)
        {
            using (StoreDataContext ctx = new StoreDataContext())
            {
                var old = _getRequestItem(ctx, item.Id).Single();

                old.Name = item.Name;
                old.ManagerComment = item.ManagerComment;
                old.Manufacturer = item.Manufacturer;
                old.PartNumber = item.PartNumber;
                old.PartNumberOriginal = item.PartNumberOriginal;
                old.DeliveryDays = item.DeliveryDays;
                old.PricePerUnit = item.PricePerUnit;
                old.Quantity = item.Quantity;

                ctx.SubmitChanges();
            }
        }
    }
}
