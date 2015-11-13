using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Web;
using RmsAuto.Store.Configuration;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
    public static class SparePartsDacFranch
    {
	

        public static SparePartFranch Load(SparePartPriceKey key)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                try
                {
                    dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                    return Load(dc.DataContext, key);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex.Message, EventLogerID.BLException, EventLogerCategory.FatalError, ex);
                    return default(SparePartFranch);
                }
                finally
                {
                  //  dc.DataContext.Connection.Close();
                }
            }
        }

        public static SparePartFranch Load(StoreDataContext context, SparePartPriceKey key)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (key == null)
                throw new ArgumentNullException("key");

		
			string query = @"SELECT * FROM fSparePartWithCustomFactorsRMS({0},{1},{2},{3},{4})
							UNION
							SELECT * FROM fSparePartWithCustomFactors({0},{1},{2},{3},{4})";

			return context.ExecuteQuery<SparePartFranch>( query,
                AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].RegionCode /* код франча для "региональных" прайсов */, 
				key.PN, key.Mfr, key.SupplierId,
				AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].AdditionalPeriod /*добавочный период к срокам поставки*/).SingleOrDefault<SparePartFranch>();
        }

        public static List<SparePartFranch> LoadMassive(IEnumerable<SparePartPriceKey> keys)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                try
                {
                    dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                    return LoadMassive(dc.DataContext, keys);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex.Message, EventLogerID.BLException, EventLogerCategory.FatalError, ex);
                    return default(List<SparePartFranch>);
                }
                finally
                {
                    dc.DataContext.Connection.Close();
                }
            }
        }

        public static List<SparePartFranch> LoadMassive(StoreDataContext context, IEnumerable<SparePartPriceKey> keys)
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

		
			query += @"
				select s.* from @t as t cross apply fSparePartWithCustomFactorsRMS({0}, t.PartNumber, t.Manufacturer, t.SupplierID, {1}) s
				union
				select s.* from @t as t cross apply fSparePartWithCustomFactors({0}, t.PartNumber, t.Manufacturer, t.SupplierID, {1}) s";

			IEnumerable<SparePartFranch> parts = context.ExecuteQuery<SparePartFranch>(
						query, AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].RegionCode, AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].AdditionalPeriod);

            return new List<SparePartFranch>(parts);
        }

        public static List<SparePartFranch> LoadMassive(IEnumerable<ShoppingCartAddItem> keys)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                try
                {
                    dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                    return LoadMassive(dc.DataContext, keys);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex.Message, EventLogerID.BLException, EventLogerCategory.FatalError, ex);
                    return default(List<SparePartFranch>);
                }
                finally
                {
                    dc.DataContext.Connection.Close();
                }
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

			query += @"
				select s.* from @t as t cross apply fSparePartWithCustomFactorsRMS({0}, t.PartNumber, t.Manufacturer, t.SupplierID, {1}) s
				union
				select s.* from @t as t cross apply fSparePartWithCustomFactors({0}, t.PartNumber, t.Manufacturer, t.SupplierID, {1}) s";

			IEnumerable<SparePartFranch> parts = context.ExecuteQuery<SparePartFranch>(
						query, AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].RegionCode, AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].AdditionalPeriod);

            return new List<SparePartFranch>(parts);
        }

        public static List<SparePartFranch> LoadMassive(IEnumerable<ShoppingCartItem> keys)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                try
                {
                    dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                    return LoadMassive(dc.DataContext, keys);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex.Message, EventLogerID.BLException, EventLogerCategory.FatalError, ex);
                    return default(List<SparePartFranch>);
                }
                finally
                {
                   //Закрываемся в фабрике
                }
            }
        }

        public static List<SparePartFranch> LoadMassive(StoreDataContext context, IEnumerable<ShoppingCartItem> keys)
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

			query += @"
				select s.* from @t as t cross apply fSparePartWithCustomFactorsRMS({0}, t.PartNumber, t.Manufacturer, t.SupplierID, {1}) s
				union
				select s.* from @t as t cross apply fSparePartWithCustomFactors({0}, t.PartNumber, t.Manufacturer, t.SupplierID, {1}) s";

			IEnumerable<SparePartFranch> parts = context.ExecuteQuery<SparePartFranch>(
						query, AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].RegionCode, AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].AdditionalPeriod);

            return new List<SparePartFranch>(parts);
        }
    }
}
