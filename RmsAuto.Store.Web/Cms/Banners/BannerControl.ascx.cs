using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Cms.Banners
{
    public partial class BannerControl : System.Web.UI.UserControl
    {
        public Int32 NumOfmSec
        {
            get { return string.IsNullOrEmpty(ConfigurationManager.AppSettings["BannerDelayMilliseconds"]) ? 0 : Convert.ToInt32(ConfigurationManager.AppSettings["BannerDelayMilliseconds"]); }
        }
        
        public Int32 CatalogItemID
		{
			get { return ViewState[ "CatalogItemID" ] != null ? Convert.ToInt32(ViewState[ "CatalogItemID" ]) : 0; }
			set { ViewState[ "CatalogItemID" ] = value.ToString(); }
		}

        public Byte Position
        {
            get { return ViewState["Position"] != null ? Convert.ToByte(ViewState["Position"]) : (byte) 0;}
            set { ViewState["Position"] = value.ToString(); }
        }

		public String DefaultHeader
		{
			get { return (string)ViewState[ "DefaultHeader" ]; }
			set { ViewState[ "DefaultHeader" ] = value; }
		}

		public String DefaultBody
		{
			get { return (string)ViewState[ "DefaultBody" ]; }
			set { ViewState[ "DefaultBody" ] = value; }
		}

        [DefaultValue(false)]
        public Boolean ShowImage
        {
            get { return ViewState["ShowImage"] != null ? (bool)ViewState["ShowImage"] : false; }
            set { ViewState["ShowImage"] = value; }
        }

        private const Int32 DefaultWidth = 200;

        private const Int32 MaxWidth = 1024;

        private const Int32 DefaultHeigth = 300;

        private const Int32 MaxHeigth = 768;

        private const String DefaultBackGroundColor = "#e6f5ff";

        private String ParseHtmlForFile(String html, String fileName, Int32 fileID)
        {
            if (html.Contains("BANNER_HTML_WRAPPER_FOR_FLASH"))
            {
                Int32 width = 0; 
                
                Int32 heigth = 0;

                String backGroundColor = "";

                String[] strs = html.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var digits = new char[] {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};

                foreach (var s in strs)
                {
                    if (s.Contains("BANNER_WIDTH"))
                        try
                        {
                            int wstart = s.IndexOfAny(digits);
                            width = Convert.ToInt32(s.Substring(wstart, s.LastIndexOfAny(digits) - wstart + 1));
                            if (width > MaxWidth)
                                width = DefaultWidth;
                        }
                        catch (Exception)
                        {
                            width = DefaultWidth;
                        }
                        
                    else if (s.Contains("BANNER_HEIGTH"))
                        try
                        {
                            int hstart = s.IndexOfAny(digits);
                            heigth = Convert.ToInt32(s.Substring(hstart, s.LastIndexOfAny(digits) - hstart + 1));
                            if (heigth > MaxHeigth)
                                heigth = DefaultHeigth;
                        }
                        catch (Exception)
                        {
                            heigth = DefaultHeigth;
                        }
                    else if (s.Contains("BANNER_BGCOLOR"))
                        try
                        {
                            backGroundColor = s.Substring(s.LastIndexOf('#'),7);
                            Int32 c = Convert.ToInt32(backGroundColor.Substring(1, 6), 16);
                            if (c < 0 || c > 255255255)
                                backGroundColor = DefaultBackGroundColor;
                        }
                        catch (Exception)
                        {
                            backGroundColor = DefaultBackGroundColor;
                        }
                }

                int pstart = fileName.LastIndexOf("/") + 1;
                int pend = fileName.LastIndexOf(".");

                String flashID = fileName.Substring(pstart, pend-pstart);

                String shortName = FilesDac.GetFile((new DCFactory<CmsDataContext>()).DataContext, fileID).FileName;

                fileName += "/" + shortName;
                
                return  "<div> " +
                            "<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" " +
                            "codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0\" " +
                            "width=\"" + width + "\" height=\"" + heigth + "\" id=\"" + flashID + "\" align=\"middle\">  <param name=\"allowScriptAccess\" value=\"sameDomain\" /> " +
                            "<param name=\"movie\" value=\"" + fileName + "\" /> " +
                            "<param name=\"quality\" value=\"high\" /> " +
                            "<param name=\"wmode\" value=\"transparent\" /> " +
                            "<param name=\"bgcolor\" value=\"" + backGroundColor + "\" /> " +
                            "<embed src=\"" + fileName + "\" quality=\"high\" wmode=\"transparent\" bgcolor=\"" + backGroundColor + "\" " +
                            "width=\"" + width + "\" height=\"" + heigth + "\" name=\"" + flashID + "\" align=\"middle\" allowScriptAccess=\"sameDomain\" " +
                            "type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" /> " +
                            "</object> " +
                        "</div>";
            }
            // пользователь самостоятельно вставил html-обертку, нужно вставить корректные имя и полный путь с учетом роутинга для swf
            else
            {
                String shortName = FilesDac.GetFile((new DCFactory<CmsDataContext>()).DataContext, fileID).FileName;

                fileName += "/" + shortName;

                html = html.Replace("BANNERFULLPATH", fileName).Replace("BANNERFILENAME", shortName);

                return "<div> " + html + " </div>";
            }
        }

        protected void Page_PreRender( object sender, EventArgs e )
		{
            try
            {
				Repeater1.DataSource = BannersForCatalogItem.GetBannersList(CatalogItemID, Position);
				Repeater1.DataBind();
            }
            catch (Exception ex)
            {
				Logger.WriteError("ошибка получения данныз баннера", EventLogerID.BLException, EventLogerCategory.BLError, ex);
            }
		}
	
        //Вариант без переключения баннеров, необходимо вернуть имейдж в разметке на место
        //protected void Page_PreRender( object sender, EventArgs e )
        //{
        //    BannersForCatalogItem bc = null;

        //    if (CatalogItemID != 0)
        //        bc = BannersForCatalogItem.GetRecordRandomBanner(CatalogItemID, Position);

        //    if (bc != null)
        //    {
        //        String fullFilePath = "";
        //        String url = "";
        //        String html = "";
        //        switch (bc.Banners.RenderType)
        //        {
        //            case (int)RenderType.ImageHtml:
        //                _fileUrlPlaceHolder.Visible = false;
        //                fullFilePath = string.Format("{0}/Files/{1}.ashx",
        //                                          ConfigurationManager.AppSettings["WebSiteUrl"],
        //                                          bc.Banners.FileID);
        //                // получение url
        //                url = bc.Banners.URL;
        //                // получение html
        //                html = bc.Banners.Html;

        //                try
        //                {
        //                    if (ShowImage && !string.IsNullOrEmpty(fullFilePath)
        //                    && (new CmsDataContext()).Files.Where(f => f.FileID == bc.Banners.FileID).First().IsImage)
        //                    {
        //                        _imagePlaceHolder.Visible = true;
        //                        _image.ImageUrl = fullFilePath; // путь к image
        //                        if (!string.IsNullOrEmpty(url))
        //                            _url.HRef = url;
        //                    }
        //                    else
        //                    {
        //                        _imagePlaceHolder.Visible = false;
        //                    }
        //                    if (!string.IsNullOrEmpty(html))
        //                    {
        //                        _bodyPlaceHolder.Visible = true;
        //                        _bodyLabel.Text = html;
        //                    }
        //                }
        //                catch
        //                {
        //                    _imagePlaceHolder.Visible = false;
        //                    _bodyPlaceHolder.Visible = false;
        //                }
                        
        //                break;
        //            case (int)RenderType.Html:
        //                _imagePlaceHolder.Visible = false;
        //                _fileUrlPlaceHolder.Visible = false;
                        
        //                // получение html
        //                html = bc.Banners.Html;
        //                if (!string.IsNullOrEmpty(html))
        //                {
        //                    _bodyPlaceHolder.Visible = true;
        //                    _bodyLabel.Text = html;
        //                }
        //                break;
        //            case (int)RenderType.FileHtml:
        //                _imagePlaceHolder.Visible = false;
        //                _bodyPlaceHolder.Visible = false;

        //                // получение url
        //                url = bc.Banners.URL;
        //                if (!string.IsNullOrEmpty(url))
        //                    _fileUrl.HRef = url;
        //                // получение html-обертки
        //                html = bc.Banners.Html;
                        
        //                try
        //                {
        //                    if (!string.IsNullOrEmpty(html) &&
        //                       (new CmsDataContext()).Files.Where(f => f.FileID == bc.Banners.FileID).First().FileMimeType.Contains("flash"))
        //                    {
        //                        fullFilePath = string.Format("{0}/Files/{1}.ashx",
        //                                              ConfigurationManager.AppSettings["WebSiteUrl"],
        //                                              bc.Banners.FileID);

        //                        _fileLinkBodyLabel.Text = ParseHtmlForFile(html, fullFilePath, (int)bc.Banners.FileID);
        //                    }
        //                }
        //                catch (Exception)
        //                {
        //                    _fileUrlPlaceHolder.Visible = false;
        //                }
        //                break; 
        //        }
        //    }
        //    else
        //    {
        //        _imagePlaceHolder.Visible = false;
        //        _bodyPlaceHolder.Visible = false;
        //        _fileUrlPlaceHolder.Visible = false;
        //    }
        //}
    }
}