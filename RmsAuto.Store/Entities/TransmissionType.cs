using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
	public enum TransmissionType : byte
	{
		[Text( "Не задан" )]
		NotDefined = 0,
		[Text( "Механика" )]
		Mechanic = 1,
		[Text( "Автомат" )]
		Auto = 2,
		[Text( "Вариатор" )]
		Variator = 3,
		[Text( "Роботизированная механика" )]
		Robot = 4
	}
}
