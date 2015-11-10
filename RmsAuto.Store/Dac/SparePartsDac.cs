using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

using RmsAuto.Store.Entities;
using RmsAuto.Store.Web;
using RmsAuto.Store.Data;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Dac
{
    public static class SparePartsDac
    {

        //private static Func<StoreDataContext, SparePartPriceKey, IQueryable<SparePart>> _getPart =
        //    CompiledQuery.Compile<StoreDataContext, SparePartPriceKey, IQueryable<SparePart>>(
        //        (dc, key) =>
        //            from part in dc.SpareParts
        //            where part.Manufacturer == key.Mfr && part.PartNumber == key.PN && part.SupplierID == key.SupplierId
        //            select part);
 
        public static SparePartFranch Load(SparePartPriceKey key)
        {

            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].InternalFranchName == "rmsauto")
                {

                    dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                    return Load(dc.DataContext, key);

                }
                else
                {
                    return SparePartsDacFranch.Load(dc.DataContext, key);
                }
            }
        }

        public static SparePartFranch Load(StoreDataContext context, SparePartPriceKey key)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (key == null)
                throw new ArgumentNullException("key");

            //TODO: Протестировать создание _getPart

            string query = @"SELECT * FROM fSparePartWithCustomFactors({0},{1},{2},{3})";

            return context.ExecuteQuery<SparePartFranch>(query,
                AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].RegionCode /* код франча для "региональных" прайсов */,
                key.PN, key.Mfr, key.SupplierId).SingleOrDefault<SparePartFranch>();
        }

        public static List<SparePartFranch> LoadMassive(IEnumerable<SparePartPriceKey> keys)
        {
            if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].InternalFranchName == "rmsauto")
            {
                using (var dc = new DCWrappersFactory<StoreDataContext>())
                {
                    dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                    return LoadMassive(dc.DataContext, keys);
                }
            }
            else
            {
                using (var dc = new DCWrappersFactory<StoreDataContext>())
                {
                    return SparePartsDacFranch.LoadMassive(dc.DataContext, keys);
                }
            }
        }

        public static List<SparePartFranch> LoadMassive(StoreDataContext context, IEnumerable<SparePartPriceKey> keys)
        {
            if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].InternalFranchName == "rmsauto")
            {
                if (context == null)
                    throw new ArgumentNullException("context");
                if (keys == null)
                    throw new ArgumentNullException("keys");
                String query = "DECLARE @t TABLE(Manufacturer varchar(50), PartNumber varchar(50), SupplierID int) ";
                foreach (var key in keys)
                {
                    query += "INSERT INTO @t VALUES('" + key.Mfr + "','" + key.PN + "','" + key.SupplierId + "') ";
                }

                query += "SELECT s.* " +
                        "FROM SparePartWithCustomFactors AS s " +
                        "JOIN @t AS t on s.Manufacturer = t.Manufacturer AND " +
                        "s.PartNumber = t.PartNumber AND s.SupplierID = t.SupplierID";

                IEnumerable<SparePartFranch> parts = context.ExecuteQuery<SparePartFranch>(query);

                return new List<SparePartFranch>(parts);
            }
            else
            {
                return SparePartsDacFranch.LoadMassive(context, keys);
            }
        }

		

        public static List<SparePartFranch> LoadMassive(IEnumerable<ShoppingCartAddItem> keys)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                return LoadMassive(dc.DataContext, keys);
            }
        }

        public static List<SparePartFranch> LoadMassive(StoreDataContext context, IEnumerable<ShoppingCartAddItem> keys)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (keys == null)
                throw new ArgumentNullException("keys");
            String query = "DECLARE @t TABLE(Manufacturer varchar(50), PartNumber varchar(50), SupplierID int) ";
            foreach (var key in keys)
            {
                query += "INSERT INTO @t VALUES('" + key.Key.Mfr + "','" + key.Key.PN + "','" + key.Key.SupplierId + "') ";
            }

            query += "SELECT s.* " +
                    "FROM SparePartWithCustomFactors AS s " +
                    "JOIN @t AS t on s.Manufacturer = t.Manufacturer AND " +
                    "s.PartNumber = t.PartNumber AND s.SupplierID = t.SupplierID";

            IEnumerable<SparePartFranch> parts = context.ExecuteQuery<SparePartFranch>(
                        query);

            return new List<SparePartFranch>(parts);
        }

        public static List<SparePartFranch> LoadMassive(IEnumerable<ShoppingCartItem> keys)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                return LoadMassive(dc.DataContext, keys);
            }
        }

        public static List<SparePartFranch> LoadMassive(StoreDataContext context, IEnumerable<ShoppingCartItem> keys)
        {
            if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].InternalFranchName == "rmsauto")
            {
                if (context == null)
                    throw new ArgumentNullException("context");
                if (keys == null)
                    throw new ArgumentNullException("keys");
                String query = "DECLARE @t TABLE(Manufacturer varchar(50), PartNumber varchar(50), SupplierID int) ";
                foreach (var key in keys)
                {
                    query += "INSERT INTO @t VALUES('" + key.Manufacturer + "','" + key.PartNumber + "','" + key.SupplierID + "') ";
                }

                query += "SELECT s.* " +
                        "FROM SparePartWithCustomFactors AS s " +
                        "JOIN @t AS t on s.Manufacturer = t.Manufacturer AND " +
                        "s.PartNumber = t.PartNumber AND s.SupplierID = t.SupplierID";

                IEnumerable<SparePartFranch> parts = context.ExecuteQuery<SparePartFranch>(query);

                return new List<SparePartFranch>(parts);
            }
            else
            {

                return SparePartsDacFranch.LoadMassive(context, keys);
                
            }
        }
    }
}
