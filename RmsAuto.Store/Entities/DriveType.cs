using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
    public enum DriveType : byte
    {
        [Text("Не задан")]
        NotDefined = 0,
        [Text("Передний")]
        Front = 1,
        [Text("Задний")]
        Rear = 2,
        [Text("Полный")]
        Full = 3
    }
}
