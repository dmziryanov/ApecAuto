using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
    public enum BodyType : byte
    {
        [Text("Не задан")]
        NotDefined = 0,
        [Text("Легковые/Седан")]
        Sedan = 1,
        [Text("Легковые/Универсал")]
        Wagon = 2,
        [Text("Легковые/Хэтчбек")]
        Hatchback = 3,
        [Text("Легковые/Купе")]
        Coupe = 4,
        [Text("Легковые/Седан")]
        Limousine = 5,
        [Text("Легковые/Лимузин")]
        Minivan = 6,
        [Text("Легковые/Минивэн")]
        Hardtop = 7,
        [Text("Легковые/Хардтоп")]
        TwoDoor = 8,
        [Text("Легковые/Тудор")]
        TownCar = 9,
        [Text("Легковые/Таун-кар")]
        Combi = 10,
        [Text("Легковые/Комби")]
        LiftBack = 11,
        [Text("Легковые/Лифтбэк")]
        Cabri = 12,
        [Text("Легковые открытые/Кабриолет")]
        Phaeton = 13,
        [Text("Легковые открытые/Фаэтон")]
        Landau = 14,
        [Text("Легковые открытые/Ландо")]
        Targa = 15,
        [Text("Легковые открытые/Тарга")]
        Roadster = 16,
        [Text("Легковые открытые/Спайдер")]
        Spider = 17,
        [Text("Грузовые/Пикап")]
        Pickup = 18,
        [Text("Грузовые/Фургон")]
        Van = 19
    }
}
