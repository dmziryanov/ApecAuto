using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
    public static class ClientCarsDac
    {
        private static Func<StoreDataContext, string, IOrderedQueryable<UserGarageCar>> _getClientCars =
            CompiledQuery.Compile(
            (StoreDataContext dc, string clientId) =>
                from car in dc.UserGarageCars
                where car.ClientId == clientId
                orderby car.Brand, car.Model, car.Modification
                select car);

        private static Func<StoreDataContext, int, IQueryable<UserGarageCar>> _getCar =
            CompiledQuery.Compile(
            (StoreDataContext dc, int carId) =>
                from car in dc.UserGarageCars
                where car.Id == carId
                select car);

        public static IEnumerable<UserGarageCar> GetGarageCars(string clientId)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                return _getClientCars(ctx.DataContext, clientId).ToList();
            }
        }

        public static UserGarageCar GetGarageCar(int carId)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                return _getCar(ctx.DataContext, carId).Single();
            }
        }

        public static void UpdateGarageCar(int carId, Action<UserGarageCar> fillerAction)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                var car = _getCar(ctx.DataContext, carId).Single();
                fillerAction(car);
                ctx.DataContext.SubmitChanges();
            }
        }

        public static void DeleteGarageCar(int carId)
        {
            using (var ctx = new DCFactory<StoreDataContext>())
            {
                var car = _getCar(ctx.DataContext, carId).Single();
                ctx.DataContext.UserGarageCars.DeleteOnSubmit(car);
                ctx.DataContext.SubmitChanges();
            }
        }
    }
}
