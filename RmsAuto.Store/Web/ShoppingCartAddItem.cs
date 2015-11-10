using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    public class ShoppingCartAddItem
    {
        // deas 28.02.2011 task2401
        // Изменен класс ключа с SparePartPriceKey
        // для работы с ReferenceID
        public ShoppingCartKey Key;
        public string ReferenceID;
        public static Int32 ReferenceIDLength
        {
            get { return 50; }
        }
        public int Quantity;
        public bool StrictlyThisNumber;
        /// <summary>
        /// Устанавливается в методе ShoppingCart.Add в true, если запчасть найдена
        /// </summary>
        public bool PartNotFound;

		public decimal? priceClientYesterday { get; set; } //сюда записываем (для "особых" пользователей): "вчерашняя" цена + допустимый % превышения
    }
}
