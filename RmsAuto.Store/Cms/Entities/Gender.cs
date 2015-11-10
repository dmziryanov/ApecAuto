using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Cms.Entities
{
	public enum Gender
	{
		[Text("Не указан")]
		Any = 0,

		[Text("Мужской")]
		Male = 1,
		
		[Text("Женский")]
		Female = 2
	}
}
