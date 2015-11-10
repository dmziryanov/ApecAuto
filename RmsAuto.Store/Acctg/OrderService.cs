using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Acctg
{
    public static class OrderService
    {
        public static void SendOrder(StoreDataContext dc, ref OrderInfo order)
        {
            if (order == null)
                throw new ArgumentNullException("order");
			ServiceProxy.Default.SendOrder(dc, order );
			#region старый вариант ориентированный на веб-сервис
			//var handlingInfo = ServiceProxy.Default.SendOrder(order);
			//foreach (var line in order.OrderLines)
			//{
			//    var lineHandling = handlingInfo.LineHandlings
			//        .Single(l => l.WebOrderLineId == line.WebOrderLineId);
			//    line.AcctgOrderLineId = lineHandling.AcctgOrderLineId;
			//    line.OrderLineStatus = lineHandling.OrderLineStatus;
			//}
			#endregion
		}

		//[Obsolete]
		//public static OrderLineInfo[] GetChanges(DateTime checkpoint)
		//{
		//    try
		//    {
		//        return ServiceProxy.Default.GetChanges(checkpoint);
		//    }
		//    catch (AcctgException ex)
		//    {
		//        if (ex.ErrorCode == AcctgError.NoDataToRespond)
		//            return new OrderLineInfo[0];
		//        throw ex;
		//    }
		//}

		//public static Acknowledgement PostOrderLineChangeRequest(
		//    int acctgOrderLineId,
		//    OrderLineChangeReaction reaction)
		//{
		//    return ServiceProxy.Default.PostChangeRequest(new OrderLineChangeRequest
		//    {
		//        AcctgOrderLineId = acctgOrderLineId,
		//        AcceptOrderLineChange = reaction == OrderLineChangeReaction.Accept ? 1 : 0
		//    }
		//    ) == "OK" ? Acknowledgement.OK : Acknowledgement.Unknown;
		//}

		//public static SellerInfo GetSellerInfo()
		//{
		//    return ServiceProxy.Default.GetSellerInfo();
		//}
    }
}
