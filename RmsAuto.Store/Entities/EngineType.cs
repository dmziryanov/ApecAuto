using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
    public enum EngineType : byte
    {
        [Text("Не задан")]
        NotDefined = 0,
        [Text("Карбюраторный, бензиновый")]
        CarboPertol = 1,
        [Text("Инжекторный, бензиновый")]
        InjectorPetrol = 2,
        [Text("Бензиновый, турбо")]
        TurboPertrol = 3,
        [Text("Бензиновый, битурбо")]
        TwinTurboPetrol = 4,
        [Text("Дизельный")]
        Diesel = 5,
        [Text("Дизельный, турбо")]
        TurboDiesel = 6,
        [Text("Дизельный, битурбо")]
        TwinTurboDiesel = 7,
        [Text("Роторно-поршневой")]
        Rotor = 8
    }
}
