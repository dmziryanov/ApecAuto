using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

using RmsAuto.Store.Web.Resource;

namespace RmsAuto.Store.Entities
{
    public enum OrderStatus : byte
    {
        //[Text("новый")]
		[Text("new")]
        [LocalizedDescription("OrderStatus_New", typeof(EnumResource))]
		New = 1,

        //[Text("в работе")]
		[Text("processing")]
        [LocalizedDescription("OrderStatus_Processing", typeof(EnumResource))]
		Processing = 2,

        //[Text("завершен")]
		[Text("completed")]
        [LocalizedDescription("OrderStatus_Completed", typeof(EnumResource))]
		Completed = 3,

        //[Text("принят")]
		[Text("apply")]
        [LocalizedDescription("OrderStatus_Apply", typeof(EnumResource))]
		Apply = 4
    }
}
