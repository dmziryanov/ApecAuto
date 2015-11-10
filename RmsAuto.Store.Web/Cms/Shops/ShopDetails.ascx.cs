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
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Cms.Routing;
using System.Net;
using System.Collections.Generic;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Cms.Shops
{
	public partial class ShopDetails : System.Web.UI.UserControl
	{

		public int ShopId
		{
			get { return ViewState[ "ShopID" ] != null ? (int)ViewState[ "ShopID" ] : 0; }
			set { ViewState[ "ShopID" ] = value; }
		}

       
		protected Shop _item;

		protected void Page_PreRender( object sender, EventArgs e )
		{
            
			using(var dc = new DCWrappersFactory<CmsDataContext>())
			{
				DataLoadOptions dlo = new DataLoadOptions();
				dlo.LoadWith<Shop>(s => s.ShopMapFile);
				dlo.LoadWith<Shop>(s => s.ShopGalleryFolder);
				dlo.LoadWith<Folder>(f => f.Files);
				dc.DataContext.LoadOptions = dlo;
				dc.DataContext.DeferredLoadingEnabled = true;

				_item = dc.DataContext.Shops.Where(s => s.ShopID == ShopId && s.ShopVisible).SingleOrDefault();

				if( _item == null )
					throw new HttpException((int)HttpStatusCode.NotFound, "Not found" );

				_shopVacancyBriefList1.ShopID = _item.ShopID;

				if( _item.ShopMapFile != null )
				{
					_mapLink.Visible = true;
					_mapLink.NavigateUrl = UrlManager.GetFileUrl( _item.ShopMapFile.FileID );
					_mapLink.Attributes[ "onclick" ] = string.Format( "info=window.open(this.href,'','width={0},height={1},scrollbars=yes,resizable=yes'); info.focus(); return false;",
						_item.ShopMapFile.ImageWidth.Value + 40,
						_item.ShopMapFile.ImageHeight.Value + 30 );
					_mapImage.ImageUrl = UrlManager.GetThumbnailUrl( _item.ShopMapFile.FileID, "map", "" );
				}
				else
				{
					_mapLink.Visible = false;
				}

				if( _item.ShopGalleryFolder != null && _item.ShopGalleryFolder.Files.Count != 0 )
				{
					_galleryRow.Visible = true;
                    List<File> AllPhotos = _item.ShopGalleryFolder.Files.Where(f => f.IsImage == true).ToList();
                    var firstPhoto = AllPhotos.Take(1);
                    GalleryHeader.DataSource = firstPhoto;
                    GalleryHeader.DataBind();
                    _galleryRepeater.DataSource = AllPhotos.Except(firstPhoto);
					_galleryRepeater.DataBind();
				}
				else
				{
					_galleryRow.Visible = false;
				}
			}

            lVacancy.Visible = _shopVacancyBriefList1.Count > 0;
		}
        protected void Page_Load(object sender, EventArgs e)
        {
            _shopEmployeeList.ShopID = ShopId;
            _shopVacancyBriefList1.ShopID = ShopId;
        }


	}
}