using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.BL;
using RmsAuto.TechDoc.Entities;
using RmsAuto.Store.BL;
using RmsAuto.Store.Cms.Entities;
using System.Collections.Generic;
using RmsAuto.Store.Entities;
using RmsAuto.TechDoc.Entities.Helpers;
using RmsAuto.Common.Web;
using RmsAuto.Common.Data;
using RmsAuto.Store.Dac;

namespace RmsAuto.Store.Web.Controls
{
	public partial class SparePartsManufacturers : System.Web.UI.UserControl
	{
		public bool SearchCounterParts
		{
			get { return Request[ UrlKeys.StoreAndTecdoc.SearchCounterparts ] == "1"; }
		}
		public string EnteredPartNumber
		{
			get { return Request[ UrlKeys.StoreAndTecdoc.EnteredPartNumber ] ?? ""; }
		}

		protected string GetSearchSparePartsUrl( string mfr, string pn )
		{
			string url;
			if( !SiteContext.Current.IsAnonymous && SiteContext.Current.User.Role == SecurityRole.Manager )
			{
				url = ResolveUrl( Manager.Store.SearchSpareParts.GetUrl( mfr, pn, this.SearchCounterParts ) );
                //url = UrlManager.GetSearchSparePartsUrl(mfr, pn, this.SearchCounterParts);
			}
			else
			{
				url = UrlManager.GetSearchSparePartsUrl( mfr, pn, this.SearchCounterParts );
			}
			return url;
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			_callToManagerHint_TextItemControl.Visible = false;

			CmsContext.Current.CatalogItem = UrlManager.CatalogItems.PrivateOfficeCatalogItem;

			var partNumber = PricingSearch.NormalizePartNumber( EnteredPartNumber );
			if( !String.IsNullOrEmpty( partNumber ) )
			{
				SearchSparePartsLogDac.AddLog( DateTime.Now, partNumber, Request.UserHostAddress );

				var manufacturers = PricingSearch.SearchSparePartManufactures( partNumber, SearchCounterParts );

				if( manufacturers.Length == 0 )
				{
					_errorLabel.Text = Resources.Texts.NoneFound;//"Ничего не найдено";
					if( !this.SearchCounterParts )
					{
						_errorLabel.Text += "<br />" + Resources.Texts.TryWithAnalogs; //Попробуйте поискать с аналогами.";
					}
					_manufacturerRepeater.Visible = false;

					_callToManagerHint_TextItemControl.Visible = true;
				}
				else if( manufacturers.Length == 1 )
				{
					Response.Redirect( GetSearchSparePartsUrl(
													manufacturers[ 0 ].Manufacturer,
													manufacturers[ 0 ].PartNumber ) );
				}
				else
				{
					//var manufacturerLogos = ManufacturerBO.GetManufacturers( manufacturers.Select( m => m.Manufacturer ) ).ToDictionary( m => m.Name, m => m.LogoFile );
					_manufacturerRepeater.DataSource = manufacturers.Select(
						m => new
						{
							Manufacturer = m.Manufacturer,
							PartNumber = m.PartNumber,
							PartDescription = m.PartDescription/*,
							LogoFile = manufacturerLogos.ContainsKey( m.Manufacturer ) ? manufacturerLogos[ m.Manufacturer ] : null*/
						} );
					_manufacturerRepeater.DataBind();
				}

			}
			else
			{
				_errorLabel.Text = "Search criteria is not determined";
				_manufacturerRepeater.Visible = false;
			}
            //deas 16.03.2011 task3302
            //вывод сообщения пользователю
            if ( Session["FindRedirectMess"] != null )
            {
                ShowMessage( Session["FindRedirectMess"].ToString() );
                Session["FindRedirectMess"] = null;
            }
		}

        //deas 16.03.2011 task3302
        //вывод сообщения пользователю
        private void ShowMessage( string message )
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>" );
        }

	}
}