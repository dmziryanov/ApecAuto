using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Routing;
using System.Collections.Generic;
using RmsAuto.Store.Configuration;
using RmsAuto.Store.Entities;
using RmsAuto.Common.Web;
using RmsAuto.Store.BL;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Store
{
    public partial class StockSupplierParts : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		public int PageNum
		{
			get
			{
				return Request.QueryString.TryGet<int>( UrlKeys.Paging.PageNum, 1 );
			}
		}
		public string Mfr { get; set; }

		protected void Page_Load( object sender, EventArgs e )
		{
            using (var ctx = new DCFactory<StoreDataContext>())
			{
				this.Mfr = Request.QueryString.TryGet<string>( UrlKeys.StoreAndTecdoc.ManufacturerName, String.Empty );
				_pageTitleLiteral.Text = String.Format( "{0} / {1}", CmsContext.Current.CatalogItem.CatalogItemName, this.Mfr );

				var ids = StocksConfiguration.Current.SupplierIds;
				ctx.DataContext.Log = new RmsAuto.Common.Misc.DebuggerWriter();
                
                throw new NotImplementedException();
			    
                //TODO: Возможно тут стоит переписать через SparePartsDac
                //var sps = ctx.DataContext.SpareParts.Where( sp => sp.Manufacturer == this.Mfr )
                //            .Where( sp => ids.Contains( sp.SupplierID ) )
                //            .OrderBy( sp => sp.PartDescription )
                //            .ThenBy( sp => sp.InitialPrice );

                //var pageSize = StocksConfiguration.Current.PageSize;

                //var pages = new int[ (int)Math.Ceiling( (decimal)sps.Count() / (decimal)pageSize ) ];
                //for( int j = 0 ; j < pages.Length ; j++ )
                //{
                //    pages[ j ] = j + 1;
                //}

                //rptPaging.DataSource = pages;
                //rptPaging.DataBind();

                //rptParts.DataSource = sps.Skip( ( this.PageNum - 1 ) * pageSize ).Take( pageSize ).ToList();
                //rptParts.DataBind();
			}
		}

        protected string GetSparePartDetailsUrl(SparePartFranch sparePart)
        {
            SparePartPriceKey key = new SparePartPriceKey(
                sparePart.Manufacturer,
                sparePart.PartNumber,
                sparePart.SupplierID);
            return UrlManager.GetSparePartDetailsUrl(key.ToUrlString());
        }
	}
}
