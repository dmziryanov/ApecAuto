using System;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;
using RmsAuto.Store.Acctg;
using RmsAuto.Common.Web;
using RmsAuto.Store.Adm.scripts;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.Banners
{
	public partial class Edit : Security.BasePage/*System.Web.UI.Page*/
	{
		protected MetaTable table;

        protected int BannerID
        {
            get
            {
                Int32? bannerID = !string.IsNullOrEmpty(Request[UrlKeys.Id]) ? (int?)Convert.ToInt32(Request[UrlKeys.Id]) : null;
                return (int)bannerID;
            }
        }

        public int FileID
        { get; set; }

        protected void renderTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetVisibility();
        }

        private void SetVisibility()
        {
            switch (_renderTypeList.SelectedIndex)
            {
                case (int)RenderType.ImageHtml:
                    AddFilePlaceHolder.Visible = true;
                    AddFileControl1.RenderType = this._renderTypeList.SelectedIndex;
                    URLPlaceHolder.Visible = true;
                    HtmlPlaceHolder.Visible = true;
                    HtmlRequeredPlaceHolder.Visible = false;
                    HtmlRequeredValidatorPlaceHolder.Visible = false;
                    FlashPastePlaceHolder.Visible = false;
                    FileHtmlEditPlaceHolder.Visible = true;
                    break;
                case (int)RenderType.Html:
                    AddFilePlaceHolder.Visible = false;
                    URLPlaceHolder.Visible = false;
                    HtmlPlaceHolder.Visible = true;
                    HtmlRequeredPlaceHolder.Visible = true;
                    HtmlRequeredValidatorPlaceHolder.Visible = true;
                    FlashPastePlaceHolder.Visible = false;
                    FileHtmlEditPlaceHolder.Visible = true;
                    break;
                case (int)RenderType.FileHtml:
                    AddFilePlaceHolder.Visible = true;
                    URLPlaceHolder.Visible = true;
                    AddFileControl1.RenderType = this._renderTypeList.SelectedIndex;
                    HtmlPlaceHolder.Visible = true;
                    HtmlRequeredPlaceHolder.Visible = true;
                    HtmlRequeredValidatorPlaceHolder.Visible = true;
                    FlashPastePlaceHolder.Visible = true;
                    FileHtmlEditPlaceHolder.Visible = false;
                    break;
            }
        }

	    private void renderTypeListBind()
        {
            _renderTypeList.Items.Clear();

            foreach (var el in Enum.GetValues(typeof(Cms.Entities.RenderType)))
            {
                Int32 i = Convert.ToInt32(el);
                String s = ((RenderType)i).ToTextOrName();
                _renderTypeList.Items.Insert(i, s);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            GoToListLink.NavigateUrl = GetListLink();

            if (!IsPostBack)
            renderTypeListBind();

            var bannerID = BannerID;
            // Режим создания нового баннера
            if (bannerID == 0)
            {
                using (var dc = new DCWrappersFactory<CmsDataContext>(false))
	            {
                    dc.DataContext.Connection.Open();
	                Cms.Entities.Banners banner = new Cms.Entities.Banners ();
	                dc.DataContext.Banners.InsertOnSubmit( banner );
				    dc.DataContext.Transaction = dc.DataContext.Connection.BeginTransaction();
				    try
				    {
					    dc.DataContext.SubmitChanges();
					    dc.DataContext.Transaction.Commit();
				    }
				    catch
				    {
					    dc.DataContext.Transaction.Rollback();
					    throw;
				    }
				    finally
				    {
					    if( dc.DataContext.Connection.State == System.Data.ConnectionState.Open )
						    dc.DataContext.Connection.Close();

				        int newbID = Cms.Entities.Banners.GetMaxID();
                        String s = Request.RawUrl;
                        s = s.Substring(0, s.LastIndexOf("/")) + "/Edit.aspx?" + UrlKeys.Id + "=" + newbID.ToString();
                        Response.Redirect(s);
				    }
	            }
            }
            // Режим редактирования
            else if (bannerID != 0 && !IsPostBack)
            {
                Cms.Entities.Banners b = RmsAuto.Store.Cms.Entities.Banners.GetBannerByID(BannerID);
                _txtName.Text = b.Name;
                _renderTypeList.SelectedIndex = b.RenderType;
                _txtURL.Text = b.URL;
                _txtHtml.Text = b.Html;
                SetVisibility();
            }
        }

	    protected void Page_Load(object sender, EventArgs e )
		{
	        _txtHtml.TextMode = TextBoxMode.MultiLine;
            _txtHtml.Columns = 80;
            _txtHtml.Rows = 10;

            ScriptsManager.RegisterJQuery(Page);
            ScriptsManager.RegisterMsoFramework(Page);
            ScriptsManager.RegisterRmsTinyMce(Page);

            _editButton.OnClientClick = string.Format(
                "mso.textEditorPopup.editField($('#{0}'));return false;",
                _txtHtml.ClientID);

	        AddFileControl1.RenderType = this._renderTypeList.SelectedIndex;
		}

        protected void ButtonUpplyClick( object sender, EventArgs e )
        {
            Page.Validate("EditGroup");
            if (this.IsValid)
                using (var dc = new DCWrappersFactory<CmsDataContext>())
                {
                    Cms.Entities.Banners b = RmsAuto.Store.Cms.Entities.Banners.GetBannerByID(dc.DataContext, BannerID);
                    b.Name = _txtName.Text;
                    b.RenderType = (byte)_renderTypeList.SelectedIndex;
                    b.URL = _txtURL.Text;
                    b.Html = _txtHtml.Text;
                    dc.DataContext.SubmitChanges();
                }
        }

        protected void ButtonUpplyAndGoToLinkClick ( object sender, EventArgs e )
        {
            ButtonUpplyClick(this, e);
            if (this.IsValid)
            {
                String s = Request.RawUrl;
                s = s.Substring(0, s.LastIndexOf("/")) + "/Details.aspx?" + UrlKeys.Id + "=" + BannerID.ToString();
                Response.Redirect(s);
            }
        }

	    protected void ButtonDeleteClick(object sender, EventArgs e)
        {
            using (var dc = new DCWrappersFactory<CmsDataContext>())
            {
                try
                {
                    Cms.Entities.Banners b = null;

                    b = Cms.Entities.Banners.GetBannerByID(dc.DataContext, BannerID);

                    try
                    {
                        var file = dc.DataContext.Files.Where(f => f.FileID == b.FileID).First();

                        if (file != null)
                        {
                            dc.DataContext.Files.DeleteOnSubmit(file);

                            b.FileID = null;

                            dc.DataContext.Files.DeleteOnSubmit(file);
                        }
                    }
                    catch
                    {
                    }

                    b = Cms.Entities.Banners.GetBannerByID(dc.DataContext, BannerID);
                    dc.DataContext.Banners.DeleteOnSubmit(b);

                    foreach (var r in dc.DataContext.BannersForCatalogItems)
                    {
                        if (r.BannerID == BannerID)
                            dc.DataContext.BannersForCatalogItems.DeleteOnSubmit(r);
                    }

                    dc.DataContext.SubmitChanges();
                }
                catch (Exception)
                {
                }

            }
            String s = Request.RawUrl;
            s = s.Substring(0, s.LastIndexOf("/")) + "/List.aspx";
            Response.Redirect(s);
        }

        protected void ButtonHtmlForFlashClick(object sender, EventArgs e)
        {
            using (var dc = new DCWrappersFactory<CmsDataContext>())
            {
                Cms.Entities.Banners b = RmsAuto.Store.Cms.Entities.Banners.GetBannerByID(dc.DataContext, BannerID);
                b.Html = "BANNER_HTML_WRAPPER_FOR_FLASH" + Environment.NewLine +
                         "BANNER_WIDTH=200" + Environment.NewLine +
                         "BANNER_HEIGTH=300" + Environment.NewLine +
                         "BANNER_BGCOLOR=#e6f5ff";
                b.RenderType = (byte) RenderType.FileHtml;
                _txtHtml.Text = b.Html;
                dc.DataContext.SubmitChanges();
            }
        }

        protected String GetListLink()
        {
            String s = Request.RawUrl;
            s = s.Substring(0, s.LastIndexOf("/")) + "/List.aspx";
            return s;
        }
	}
}