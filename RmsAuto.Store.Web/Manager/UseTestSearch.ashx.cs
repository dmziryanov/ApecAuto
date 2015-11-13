using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Services;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UseTestSearch : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            
             using (var dc = new DCFactory<StoreDataContext>())
             {
                 var i = dc.DataContext.ExecuteCommand(@"USE [ex_LiteFranchTest_store];
                                                        DROP function [dbo].[fSparePartWithCustomFactorsRMS];");

                 i = dc.DataContext.ExecuteCommand(@"

                CREATE function [dbo].[fSparePartWithCustomFactorsRMS] 
                (@Region varchar(50), @PartNumber varchar(50), @Manufacturer varchar(50), @SupplierID int, @AdditionalPeriod int) returns table as return (

                select
                p.Manufacturer,
                p.PartNumber,
                p.InternalPartNumber,
                p.PartName,
                p.PartDescription,
                p.SupplierID,
                p.DeliveryDaysMin + @AdditionalPeriod as DeliveryDaysMin,
                p.DeliveryDaysMax + @AdditionalPeriod as DeliveryDaysMax,
                p.InitialPrice,
                p.RgCode,
                p.WeightPhysical,
                p.WeightVolume,
                p.QtyInStock,
  p.MinOrderQty,
  p.PriceConstantTerm,
  p.PriceDate,
  p.SparePartGroupID,
  p.Region,
	e.PricingMatrixEntryID,
	e.CorrectionFactor,
	e.CorrectionFactor39,
	e.CustomFactor1,  -- Roz1
	e.CustomFactor2,  -- Roz2
	e.CustomFactor3,  -- Roz3
	e.CustomFactor4,  -- Roz4
	e.CustomFactor5,  -- Roz5
	0.00 as CustomFactor6, -- e.CustomFactor6,
	0.00 as CustomFactor7, -- e.CustomFactor7,
	e.CustomFactor8,  -- Opt3
	0.00 as CustomFactor9, -- e.CustomFactor9,
	e.CustomFactor10, -- Opt4
	0.00 as CustomFactor11, -- e.CustomFactor11,
	0.00 as CustomFactor12, -- e.CustomFactor12,
	e.CustomFactor13, -- Opt5
	0.00 as CustomFactor14, -- e.CustomFactor14,
	e.CustomFactor15, -- Opt6
	0.00 as CustomFactor16, -- e.CustomFactor16,
	0.00 as CustomFactor17, -- e.CustomFactor17,
	0.00 as CustomFactor18, -- e.CustomFactor18,
	0.00 as CustomFactor19, -- e.CustomFactor19,
	e.CustomFactor20, -- Opt1
	e.CustomFactor21, -- Opt2
	0.00 as CustomFactor22, -- e.CustomFactor22,
	0.00 as CustomFactor23, -- e.CustomFactor23,
	0.00 as CustomFactor24, -- e.CustomFactor24,
	e.CustomFactor25 as CustomFactor25, -- e.CustomFactor25, КОСТЫЛЬ!!!
	e.BeforeTime,
	e.OnTime,
	e.Delay,
	e.NonDelivery,
  p.SizeID,
  isnull(ea.AdditionalCF1, 1.00) as AdditionalCF1,
  isnull(ea.AdditionalCF2, 1.00) as AdditionalCF2,
  isnull(ea.AdditionalCF3, 1.00) as AdditionalCF3,
  isnull(ea.AdditionalCF4, 1.00) as AdditionalCF4,
  isnull(ea.AdditionalCF5, 1.00) as AdditionalCF5,
  isnull(ea.AdditionalCF6, 1.00) as AdditionalCF6,
  isnull(ea.AdditionalCF7, 1.00) as AdditionalCF7,
  isnull(ea.AdditionalCF8, 1.00) as AdditionalCF8,
  isnull(ea.AdditionalCF9, 1.00) as AdditionalCF9,
  isnull(ea.AdditionalCF10,1.00) as AdditionalCF10,
  isnull(ea.AdditionalCF11,1.00) as AdditionalCF11
from [1Ctest].ex_rmsauto_store/*ex_rmsauto_crosses*/.dbo.SpareParts /* 'Наши' остатки */ as p /*with (NoLock)*/
	join [1Ctest].ex_rmsauto_store/*ex_rmsauto_crosses*/.dbo.PricingMatrixEntries /* 'Наши' наценки */ e on e.PricingMatrixEntryID = ( select top 1 
                                                              PricingMatrixEntryID 
                                                            from ( select 
                                                                     PricingMatrixEntryID, 10 as OrderBy
                                                                   from [1Ctest].ex_rmsauto_store/*ex_rmsauto_crosses*/.dbo.PricingMatrixEntries e1 /*  наценки */
                                                                   where e1.SupplierID = p.SupplierID 
                                                                     and e1.Manufacturer = p.Manufacturer 
                                                                     and e1.PartNumber = p.PartNumber 
                                                                     and e1.RgCode is null
		                                                               union all 
		                                                               select 
		                                                                 PricingMatrixEntryID, 20 as OrderBy
		                                                               from [1Ctest].ex_rmsauto_store/*ex_rmsauto_crosses*/.dbo.PricingMatrixEntries e2 /*  наценки */
		                                                               where e2.SupplierID = p.SupplierID 
		                                                                 and e2.Manufacturer = p.Manufacturer 
		                                                                 and e2.PartNumber is null 
		                                                                 and e2.RgCode = p.RgCode
		                                                               union all 
		                                                               select 
		                                                                 PricingMatrixEntryID, 30 as OrderBy
		                                                               from [1Ctest].ex_rmsauto_store/*ex_rmsauto_crosses*/.dbo.PricingMatrixEntries e3 /* наценки */
		                                                               where e3.SupplierID = p.SupplierID 
		                                                                 and e3.Manufacturer = p.Manufacturer 
		                                                                 and e3.PartNumber is null 
		                                                                 and e3.RgCode is null
		                                                               union all
		                                                               select 
		                                                                 PricingMatrixEntryID, 40 as OrderBy
		                                                               from [1Ctest].ex_rmsauto_store/*ex_rmsauto_crosses*/.dbo.PricingMatrixEntries e4 /*  наценки */
		                                                               where e4.SupplierID = p.SupplierID 
		                                                                 and e4.Manufacturer is NULL
		                                                                 and e4.PartNumber is null 
		                                                                 and e4.RgCode = p.RgCode
		                                                               union all 
		                                                               select 
		                                                                 PricingMatrixEntryID, 50 as OrderBy
		                                                               from [1Ctest].ex_rmsauto_store/*ex_rmsauto_crosses*/.dbo.PricingMatrixEntries e5 /* наценки */
		                                                               where e5.SupplierID = p.SupplierID 
		                                                                 and e5.Manufacturer is null 
		                                                                 and e5.PartNumber is null 
		                                                                 and e5.RgCode is null
		                                                             ) T order by OrderBy
																	 )
	left join [dbo].[PricingMatrixAddEntries] ea on ea.PricingMatrixAddEntryID = ( select top 1 PricingMatrixAddEntryID
		from ( select PricingMatrixAddEntryID, 100 as OrderBy
			   from dbo.PricingMatrixAddEntries ea1
			   where ea1.SupplierID = p.SupplierID
			     and ea1.Manufacturer = p.Manufacturer
			   union all
			   select PricingMatrixAddEntryID, 200
			   from dbo.PricingMatrixAddEntries ea2
			   where ea2.SupplierID = p.SupplierID
			     and ea2.Manufacturer is null
			   union all
			   select PricingMatrixAddEntryID, 300
			   from dbo.PricingMatrixAddEntries ea3
			   where ea3.SupplierID is null
			     and ea3.Manufacturer is null
			   ) T2 order by OrderBy
			   )
where (p.PartNumber = @PartNumber) and (p.Manufacturer = @Manufacturer or @Manufacturer is null) and (p.SupplierID = @SupplierID or @SupplierID is null)
)");
                 context.Response.ContentType = "text/plain";
                 context.Response.Write(i);
             }

           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
