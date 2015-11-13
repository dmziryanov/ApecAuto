using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RmsAuto.Store.Entities;
using RmsAuto.TechDoc.Entities;
using System.Data;
using System.Data.Linq;
using RmsAuto.Common.Data;
using System.Configuration;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Web;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.BL
{
	public static class PricingSearch
	{
		static bool _useNewPricingSearch = ConfigurationManager.AppSettings[ "PricingSearch" ] == "new";

		public static string NormalizePartNumber( string partNumber )
		{
			//if( _useNewPricingSearch )
				return PricingSearchNEW.NormalizePartNumber( partNumber );
			//else
			//    return PricingSearchOLD.NormalizePartNumber( partNumber );
		}

		public static SparePartManufacturerItem[] SearchSparePartManufactures( string partNumber, bool searchCrosses )
		{
			//if( _useNewPricingSearch )
				return PricingSearchNEW.SearchSparePartManufactures( partNumber, searchCrosses );
			//else
			//    return PricingSearchOLD.SearchSparePartManufactures( partNumber, searchCrosses );
		}

		public static SparePartItem[] SearchSpareParts( string partNumber, string manufacturer, bool searchCrosses )
		{
			//if( _useNewPricingSearch )
				return PricingSearchNEW.SearchSpareParts( partNumber, manufacturer, searchCrosses );
			//else
			//    return PricingSearchOLD.SearchSpareParts( partNumber, manufacturer, searchCrosses );
		}
	}

	
    public static class PricingSearchNEW
    {
        /// <summary>
        /// Генерация нормализованного номера детали
        /// </summary>
        /// <param name="partNumber"></param>
        /// <returns></returns>
        public static string NormalizePartNumber( string partNumber )
        {
            return partNumber == null ? null : PartKey.NormalizePartNumber( partNumber );
        }

        public static SparePartManufacturerItem[] SearchSparePartManufactures( string partNumber, bool searchCrosses )
        {
            string normalizedPartNumber = NormalizePartNumber( partNumber );

            return SearchSpareParts( partNumber, searchCrosses )
                .GroupBy( p => p.ManufacturerGroup, StringComparer.InvariantCultureIgnoreCase /* add by Daniil */ )
                .Select( g =>
                    new SparePartManufacturerItem
                    {
                        Manufacturer = g.Key,
                        PartNumber = partNumber,
                        PartDescription = g.OrderBy( p => p.ItemType ).Select( p => p.SparePart.PartDescription ).FirstOrDefault()
                    } )
                .OrderBy( g => g.Manufacturer )
                .ToArray();
        }

        public static SparePartItem[] SearchSpareParts( string partNumber, string manufacturer, bool searchCrosses )
        {
            return SearchSpareParts( partNumber, searchCrosses ).Where( p => p.ManufacturerGroup == manufacturer ).ToArray();
        }

     
        class SearchResult
        {
            public string ManufacturerGroup { get; set; }
            public SparePartItemType ItemType { get; set; }

            /// <summary>
            /// Отображаемый минимальный срок поставки
            /// </summary>
            public int DisplayDeliveryDaysMin
            {
                get { return DeliveryDaysMin > 0 ? DeliveryDaysMin : 1; }
                set { }
            }

            /// <summary>
            /// Отображаемый максимальный срок поставки
            /// </summary>
            public int DisplayDeliveryDaysMax
            {
                get { return DeliveryDaysMax > 0 ? DeliveryDaysMax : 1; }
                set { }
            }

            public string Manufacturer { get; set; }
            public string PartNumber { get; set; }
            public string InternalPartNumber { get; set; }
            public string PartName { get; set; }
            public string PartDescription { get; set; }
            public int SupplierID { get; set; }
            public int DeliveryDaysMin { get; set; }
            public int DeliveryDaysMax { get; set; }
            public decimal InitialPrice { get; set; }
            public string RgCode { get; set; }
            public decimal? WeightPhysical { get; set; }
            public decimal? WeightVolume { get; set; }
            public int? QtyInStock { get; set; }
            public int? MinOrderQty { get; set; }
            public decimal? PriceConstantTerm { get; set; }
            public decimal CorrectionFactor { get; set; }
            public decimal CorrectionFactor39 { get; set; }
            public decimal CustomFactor1 { get; set; }
            public decimal CustomFactor2 { get; set; }
            public decimal CustomFactor3 { get; set; }
            public decimal CustomFactor4 { get; set; }
            public decimal CustomFactor5 { get; set; }
            public decimal CustomFactor6 { get; set; }
            public decimal CustomFactor7 { get; set; }
            public decimal CustomFactor8 { get; set; }
            public decimal CustomFactor9 { get; set; }
            public decimal CustomFactor10 { get; set; }
            public decimal CustomFactor11 { get; set; }
            public decimal CustomFactor12 { get; set; }
            public decimal CustomFactor13 { get; set; }
            public decimal CustomFactor14 { get; set; }
            public decimal CustomFactor15 { get; set; }
            public decimal CustomFactor16 { get; set; }
            public decimal CustomFactor17 { get; set; }
            public decimal CustomFactor18 { get; set; }
            public decimal CustomFactor19 { get; set; }
            public decimal CustomFactor20 { get; set; }
            public decimal CustomFactor21 { get; set; }
            public decimal CustomFactor22 { get; set; }
            public decimal CustomFactor23 { get; set; }
            public decimal CustomFactor24 { get; set; }
            public decimal CustomFactor25 { get; set; }
            public decimal CustomFactor26 { get; set; }
            public decimal CustomFactor27 { get; set; }
            public decimal CustomFactor28 { get; set; }
            public decimal CustomFactor29 { get; set; }
            public decimal CustomFactor30 { get; set; }
            public decimal CustomFactor31 { get; set; }
            public decimal CustomFactor32 { get; set; }
            public decimal CustomFactor33 { get; set; }
            public decimal CustomFactor34 { get; set; }
            public decimal CustomFactor35 { get; set; }
            public decimal CustomFactor36 { get; set; }
            public decimal CustomFactor37 { get; set; }
            public decimal CustomFactor38 { get; set; }
            public decimal CustomFactor39 { get; set; }
            public decimal CustomFactor40 { get; set; }
            public decimal CustomFactor41 { get; set; }
            public decimal CustomFactor42 { get; set; }
            public decimal CustomFactor43 { get; set; }
            public decimal CustomFactor44 { get; set; }
            public decimal CustomFactor45 { get; set; }
            public decimal CustomFactor46 { get; set; }
            public decimal CustomFactor47 { get; set; }
            public decimal CustomFactor48 { get; set; }
            public decimal CustomFactor49 { get; set; }
            public decimal CustomFactor50 { get; set; }
            public decimal CustomFactor51 { get; set; }
            public decimal CustomFactor52 { get; set; }
            public decimal CustomFactor53 { get; set; }
            public decimal CustomFactor54 { get; set; }
            public decimal CustomFactor55 { get; set; }
            public decimal CustomFactor56 { get; set; }
            public decimal CustomFactor57 { get; set; }
            public decimal CustomFactor58 { get; set; }
            public decimal CustomFactor59 { get; set; }
            public decimal CustomFactor60 { get; set; }
            public decimal CustomFactor61 { get; set; }
            public decimal CustomFactor62 { get; set; }
            public decimal CustomFactor63 { get; set; }
            public decimal CustomFactor64 { get; set; }
            public decimal CustomFactor65 { get; set; }
            public decimal CustomFactor66 { get; set; }
            public decimal CustomFactor67 { get; set; }
            public decimal CustomFactor68 { get; set; }
            public decimal CustomFactor69 { get; set; }
            public decimal CustomFactor70 { get; set; }
            public decimal CustomFactor71 { get; set; }
            public decimal CustomFactor72 { get; set; }
            public decimal CustomFactor73 { get; set; }
            public decimal CustomFactor74 { get; set; }
            public decimal CustomFactor75 { get; set; }
            public decimal CustomFactor76 { get; set; }
            public decimal CustomFactor77 { get; set; }
            public decimal CustomFactor78 { get; set; }
            public decimal CustomFactor79 { get; set; }
            public decimal CustomFactor80 { get; set; }
            public decimal CustomFactor81 { get; set; }
            public decimal CustomFactor82 { get; set; }
            public decimal CustomFactor83 { get; set; }
            public decimal CustomFactor84 { get; set; }
            public decimal CustomFactor85 { get; set; }
            public decimal CustomFactor86 { get; set; }
            public decimal CustomFactor87 { get; set; }
            public decimal CustomFactor88 { get; set; }
            public decimal CustomFactor89 { get; set; }
            public decimal CustomFactor90 { get; set; }
            public decimal CustomFactor91 { get; set; }
            public decimal CustomFactor92 { get; set; }
            public decimal CustomFactor93 { get; set; }
            public decimal CustomFactor94 { get; set; }
            public decimal CustomFactor95 { get; set; }
            public decimal CustomFactor96 { get; set; }
            public decimal CustomFactor97 { get; set; }
            public decimal CustomFactor98 { get; set; }
            public decimal CustomFactor99 { get; set; }
            public decimal CustomFactor100 { get; set; }
            public DateTime PriceDate { get; set; }
            public int? SparePartGroupID { get; set; }
            public int? BeforeTime { get; set; }
            public int? OnTime { get; set; }
            public int? Delay { get; set; }
            public int? NonDelivery { get; set; }
            public int SizeID { get; set; }
            public decimal AdditionalCF1 { get; set; }
            public decimal AdditionalCF2 { get; set; }
            public decimal AdditionalCF3 { get; set; }
            public decimal AdditionalCF4 { get; set; }
            public decimal AdditionalCF5 { get; set; }
            public decimal AdditionalCF6 { get; set; }
            public decimal AdditionalCF7 { get; set; }
            public decimal AdditionalCF8 { get; set; }
            public decimal AdditionalCF9 { get; set; }
            public decimal AdditionalCF10 { get; set; }
            public decimal AdditionalCF11 { get; set; }
        }

        static SparePartItem[] SearchSpareParts( string partNumber, bool searchCrosses )
        {
            string normalizedPartNumber = NormalizePartNumber( partNumber );
            SparePartItem[] result = null;

            using ( var dc = new DCFactory<StoreDataContext>() )
            {
                try
                {
                    dc.DataContext.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                    //dc.Transaction = dc.Connection.BeginTransaction( IsolationLevel.ReadUncommitted );

                    string query;
                    IEnumerable<SearchResult> parts;

					if (SiteContext.Current.InternalFranchName == "rmsauto")
					{
						if (!searchCrosses)
						{
							query = @"select P.Manufacturer as ManufacturerGroup, {1} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactors({4}, {0}, null, null) P";
						}
						else
						{
							query = @"select P.Manufacturer as ManufacturerGroup, {1} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactors({4}, {0}, null, null) P

							UNION

							select B2.Manufacturer as ManufacturerGroup, {3} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactors({4}, {0}, null, null) P
							join SparePartCrossesBrands B1 on P.Manufacturer=B1.Manufacturer
							join SparePartCrossesBrands B2 on B1.ManufacturerMain=B2.ManufacturerMain 
							where B2.Manufacturer!=P.Manufacturer

							UNION

							select isnull(B1.Manufacturer,G1.Manufacturer) as ManufacturerGroup, 
								case when isnull(B1.Manufacturer,G1.Manufacturer)=P.Manufacturer and G1.GreatGroupID=G2.GreatGroupID and G1.GroupId=G2.GroupID then {2}
								else {3} end as ItemType,
								0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax,
								P.*
							from SparePartCrossesGroups G1
							join SparePartCrossesLinks L on G1.GreatGroupID=L.GreatGroupID1
							join SparePartCrossesGroups G2 on L.GreatGroupID2=G2.GreatGroupID
							left join SparePartCrossesBrands B1 on G1.Manufacturer=B1.ManufacturerMain
							left join SparePartCrossesBrands B2 on G2.Manufacturer=B2.ManufacturerMain
							/*join SparePartWithCustomFactors P on P.PartNumber=G2.PartNumber and P.Manufacturer=isnull(B2.Manufacturer,G2.Manufacturer)*/
							cross apply fSparePartWithCustomFactors({4}, G2.PartNumber, isnull(B2.Manufacturer,G2.Manufacturer), null) P
							where G1.PartNumber={0} 
							and not (isnull(B1.Manufacturer,G1.Manufacturer)=P.Manufacturer and G1.PartNumber=P.PartNumber)";
						}

						parts = dc.DataContext.ExecuteQuery<SearchResult>(
								query,
								normalizedPartNumber,
								(int)SparePartItemType.Exact,
								(int)SparePartItemType.Transition,
								(int)SparePartItemType.Analogue,
								"rms"); //для франча тут будет код франча (для региональных прайсов)
					}
					else
					{


						if (!searchCrosses)
						{
							query = @"select P.Manufacturer as ManufacturerGroup, {1} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactorsRMS({4}, {0}, null, null, {5}) P
							
							UNION ALL

							select P.Manufacturer as ManufacturerGroup, {1} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactors({4}, {0}, null, null, {5}) P";

							#region ==== попытка сделать через openquery (с cross apply работать не будет, поэтому отказались в итоге) ====
							/*string openquerystring = string.Format(
							@"openquery([SQLWEBCLUSTER\SQL_WEB], 'select * from ex_rmsauto_store.dbo.fSparePartWithCustomFactorsTEST(''{0}'', ''{1}'', null, null, {2})')",
							FranchConfiguration.Current.RegionCode,
							normalizedPartNumber,
							FranchConfiguration.Current.AdditionalPeriod);
						query = @"select P.Manufacturer as ManufacturerGroup, {1} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from " + openquerystring + @" P
							
							UNION ALL

							select P.Manufacturer as ManufacturerGroup, {1} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactors({4}, {0}, null, null, {5}) P";*/
							#endregion
						}
						else
						{
							query = @"select P.Manufacturer as ManufacturerGroup, {1} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactorsRMS({4}, {0}, null, null, {5}) P

							UNION

							select B2.Manufacturer as ManufacturerGroup, {3} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactorsRMS({4}, {0}, null, null, {5}) P
							join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesBrands B1 on P.Manufacturer=B1.Manufacturer
							join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesBrands B2 on B1.ManufacturerMain=B2.ManufacturerMain 
							where B2.Manufacturer!=P.Manufacturer

							UNION

							select isnull(B1.Manufacturer,G1.Manufacturer) as ManufacturerGroup, 
								case when isnull(B1.Manufacturer,G1.Manufacturer)=P.Manufacturer and G1.GreatGroupID=G2.GreatGroupID and G1.GroupId=G2.GroupID then {2}
								else {3} end as ItemType,
								0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax,
								P.*
							from[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesGroups G1
							join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesLinks L on G1.GreatGroupID=L.GreatGroupID1
							join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesGroups G2 on L.GreatGroupID2=G2.GreatGroupID
							left join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesBrands B1 on G1.Manufacturer=B1.ManufacturerMain
							left join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesBrands B2 on G2.Manufacturer=B2.ManufacturerMain
							cross apply fSparePartWithCustomFactorsRMS({4}, G2.PartNumber, isnull(B2.Manufacturer,G2.Manufacturer), null, {5}) P
							where G1.PartNumber={0} 
							and not (isnull(B1.Manufacturer,G1.Manufacturer)=P.Manufacturer and G1.PartNumber=P.PartNumber)

							UNION
							
							select P.Manufacturer as ManufacturerGroup, {1} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactors({4}, {0}, null, null, {5}) P

							UNION

							select B2.Manufacturer as ManufacturerGroup, {3} as ItemType, 0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax, P.* 
							from fSparePartWithCustomFactors({4}, {0}, null, null, {5}) P
							join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesBrands B1 on P.Manufacturer=B1.Manufacturer
							join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesBrands B2 on B1.ManufacturerMain=B2.ManufacturerMain 
							where B2.Manufacturer!=P.Manufacturer

							UNION

							select isnull(B1.Manufacturer,G1.Manufacturer) as ManufacturerGroup, 
								case when isnull(B1.Manufacturer,G1.Manufacturer)=P.Manufacturer and G1.GreatGroupID=G2.GreatGroupID and G1.GroupId=G2.GroupID then {2}
								else {3} end as ItemType,
								0 as DisplayDeliveryDaysMin, 0 as DisplayDeliveryDaysMax,
								P.*
							from[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesGroups G1
							join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesLinks L on G1.GreatGroupID=L.GreatGroupID1
							join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesGroups G2 on L.GreatGroupID2=G2.GreatGroupID
							left join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesBrands B1 on G1.Manufacturer=B1.ManufacturerMain
							left join[w2008r2sql1\sql1].ex_apecauto_store.dbo.SparePartCrossesBrands B2 on G2.Manufacturer=B2.ManufacturerMain
							cross apply fSparePartWithCustomFactors({4}, G2.PartNumber, isnull(B2.Manufacturer,G2.Manufacturer), null, {5}) P
							where G1.PartNumber={0} 
							and not (isnull(B1.Manufacturer,G1.Manufacturer)=P.Manufacturer and G1.PartNumber=P.PartNumber)";
						}
						
                        if (LightBO.IsLight() && AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].DbName.Equals("LightsApec"))
						{
                            query = query.Replace(@"[w2008r2sql1\sql1].ex_apecauto_store", @"[w2008r2sql1\sql1].ex_apecautoR_store");
						}

                        if (LightBO.IsLight() && AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].DbName.Equals("LightsAPEC"))
                        {
                            query = query.Replace(@"[w2008r2sql1\sql1].ex_apecauto_store", @"[SRVDB2\SRVDB2].ex_apecauto_store");
                        }

						parts = dc.DataContext.ExecuteQuery<SearchResult>(
						 query,
						 normalizedPartNumber,
						 (int)SparePartItemType.Exact,
						 (int)SparePartItemType.Transition,
						 (int)SparePartItemType.Analogue,
						 AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].RegionCode, /*код франча для "региональных" прайсов */
						 AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].AdditionalPeriod /*добавочный период к срокам поставки*/);
					}

                    var res = parts.Select(p => new SparePartItem
                    {
                        ItemType = p.ItemType,
                        ManufacturerGroup = p.ManufacturerGroup,
                        SparePart = new SparePartFranch
                        {
                            Manufacturer = p.Manufacturer,
                            PartNumber = p.PartNumber,
                            InternalPartNumber = p.InternalPartNumber,
                            PartName = p.PartName,
                            PartDescription = p.PartDescription,
                            SupplierID = p.SupplierID,
                            DeliveryDaysMin = p.DeliveryDaysMin,
                            DeliveryDaysMax = p.DeliveryDaysMax,
                            InitialPrice = p.InitialPrice,
                            RgCode = p.RgCode,
                            WeightPhysical = p.WeightPhysical,
                            WeightVolume = p.WeightVolume,
                            QtyInStock = p.QtyInStock,
                            MinOrderQty = p.MinOrderQty,
                            PriceConstantTerm = p.PriceConstantTerm,
                            CorrectionFactor = p.CorrectionFactor,
                            CorrectionFactor39 = p.CorrectionFactor39,
                            CustomFactor1 = p.CustomFactor1,
                            CustomFactor2 = p.CustomFactor2,
                            CustomFactor3 = p.CustomFactor3,
                            CustomFactor4 = p.CustomFactor4,
                            CustomFactor5 = p.CustomFactor5,
                            CustomFactor6 = p.CustomFactor6,
                            CustomFactor7 = p.CustomFactor7,
                            CustomFactor8 = p.CustomFactor8,
                            CustomFactor9 = p.CustomFactor9,
                            CustomFactor10 = p.CustomFactor10,
                            CustomFactor11 = p.CustomFactor11,
                            CustomFactor12 = p.CustomFactor12,
                            CustomFactor13 = p.CustomFactor13,
                            CustomFactor14 = p.CustomFactor14,
                            CustomFactor15 = p.CustomFactor15,
                            CustomFactor16 = p.CustomFactor16,
                            CustomFactor17 = p.CustomFactor17,
                            CustomFactor18 = p.CustomFactor18,
                            CustomFactor19 = p.CustomFactor19,
                            CustomFactor20 = p.CustomFactor20,
                            CustomFactor21 = p.CustomFactor21,
                            CustomFactor22 = p.CustomFactor22,
                            CustomFactor23 = p.CustomFactor23,
                            CustomFactor24 = p.CustomFactor24,
                            CustomFactor25 = p.CustomFactor25,
                            CustomFactor26 = p.CustomFactor26,
                            CustomFactor27 = p.CustomFactor27,
                            CustomFactor28 = p.CustomFactor28,
                            CustomFactor29 = p.CustomFactor29,
                            CustomFactor30 = p.CustomFactor30,
                            CustomFactor31 = p.CustomFactor31,
                            CustomFactor32 = p.CustomFactor32,
                            CustomFactor33 = p.CustomFactor33,
                            CustomFactor34 = p.CustomFactor34,
                            CustomFactor35 = p.CustomFactor35,
                            CustomFactor36 = p.CustomFactor36,
                            CustomFactor37 = p.CustomFactor37,
                            CustomFactor38 = p.CustomFactor38,
                            CustomFactor39 = p.CustomFactor39,
                            CustomFactor40 = p.CustomFactor40,
                            CustomFactor41 = p.CustomFactor41,
                            CustomFactor42 = p.CustomFactor42,
                            CustomFactor43 = p.CustomFactor43,
                            CustomFactor44 = p.CustomFactor44,
                            CustomFactor45 = p.CustomFactor45,
                            CustomFactor46 = p.CustomFactor46,
                            CustomFactor47 = p.CustomFactor47,
                            CustomFactor48 = p.CustomFactor48,
                            CustomFactor49 = p.CustomFactor49,
                            CustomFactor50 = p.CustomFactor50,
                            CustomFactor51 = p.CustomFactor51,
                            CustomFactor52 = p.CustomFactor52,
                            CustomFactor53 = p.CustomFactor53,
                            CustomFactor54 = p.CustomFactor54,
                            CustomFactor55 = p.CustomFactor55,
                            CustomFactor56 = p.CustomFactor56,
                            CustomFactor57 = p.CustomFactor57,
                            CustomFactor58 = p.CustomFactor58,
                            CustomFactor59 = p.CustomFactor59,
                            CustomFactor60 = p.CustomFactor60,
                            CustomFactor61 = p.CustomFactor61,
                            CustomFactor62 = p.CustomFactor62,
                            CustomFactor63 = p.CustomFactor63,
                            CustomFactor64 = p.CustomFactor64,
                            CustomFactor65 = p.CustomFactor65,
                            CustomFactor66 = p.CustomFactor66,
                            CustomFactor67 = p.CustomFactor67,
                            CustomFactor68 = p.CustomFactor68,
                            CustomFactor69 = p.CustomFactor69,
                            CustomFactor70 = p.CustomFactor70,
                            CustomFactor71 = p.CustomFactor71,
                            CustomFactor72 = p.CustomFactor72,
                            CustomFactor73 = p.CustomFactor73,
                            CustomFactor74 = p.CustomFactor74,
                            CustomFactor75 = p.CustomFactor75,
                            CustomFactor76 = p.CustomFactor76,
                            CustomFactor77 = p.CustomFactor77,
                            CustomFactor78 = p.CustomFactor78,
                            CustomFactor79 = p.CustomFactor79,
                            CustomFactor80 = p.CustomFactor80,
                            CustomFactor81 = p.CustomFactor81,
                            CustomFactor82 = p.CustomFactor82,
                            CustomFactor83 = p.CustomFactor83,
                            CustomFactor84 = p.CustomFactor84,
                            CustomFactor85 = p.CustomFactor85,
                            CustomFactor86 = p.CustomFactor86,
                            CustomFactor87 = p.CustomFactor87,
                            CustomFactor88 = p.CustomFactor88,
                            CustomFactor89 = p.CustomFactor89,
                            CustomFactor90 = p.CustomFactor90,
                            CustomFactor91 = p.CustomFactor91,
                            CustomFactor92 = p.CustomFactor92,
                            CustomFactor93 = p.CustomFactor93,
                            CustomFactor94 = p.CustomFactor94,
                            CustomFactor95 = p.CustomFactor95,
                            CustomFactor96 = p.CustomFactor96,
                            CustomFactor97 = p.CustomFactor97,
                            CustomFactor98 = p.CustomFactor98,
                            CustomFactor99 = p.CustomFactor99,
                            CustomFactor100 = p.CustomFactor100,
                            PriceDate = p.PriceDate,
                            SparePartGroupID = p.SparePartGroupID,
                            BeforeTime = p.BeforeTime,
                            OnTime = p.OnTime,
                            Delay = p.Delay,
                            NonDelivery = p.NonDelivery,
                            SizeID = p.SizeID,
                            AdditionalCF1 = p.AdditionalCF1,
                            AdditionalCF2 = p.AdditionalCF2,
                            AdditionalCF3 = p.AdditionalCF3,
                            AdditionalCF4 = p.AdditionalCF4,
                            AdditionalCF5 = p.AdditionalCF5,
                            AdditionalCF6 = p.AdditionalCF6,
                            AdditionalCF7 = p.AdditionalCF7,
                            AdditionalCF8 = p.AdditionalCF8,
                            AdditionalCF9 = p.AdditionalCF9,
                            AdditionalCF10 = p.AdditionalCF10,
                            AdditionalCF11 = p.AdditionalCF11
                        }
                    });

                    // deas 06.05.2011 task4045 Замена русских букв в поиске
                    result = res.ToArray();
                    
                }
                catch(Exception ex)
                {
					string str = ex.Message;
                    Logger.WriteError("Ошибка при получении результатов поиска цен PricingSearch", EventLogerID.BLException, EventLogerCategory.FatalError);
                }
                finally
                {
                    //Теперь закрываемся в DCWrappersFacttory
                }
                return result;
            }
        }
    }

	public enum SparePartItemType
	{
		//OwnStore = -1,
		Exact = 0,
		Transition = 1,
		Analogue = 2
	}

	public class SparePartManufacturerItem
	{
		public string Manufacturer { get; set; }
		public string PartNumber { get; set; }
		public string PartDescription { get; set; }
	}

	public class SparePartItem
	{
		public string ManufacturerGroup { get; set; }
		public SparePartItemType ItemType { get; set; }
        public SparePartFranch SparePart { get; set; }
	}
}
