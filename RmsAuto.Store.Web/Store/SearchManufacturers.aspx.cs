using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RmsAuto.TechDoc;
using RmsAuto.TechDoc.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web.Controls;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.BL;
using System.Text.RegularExpressions;
using RmsAuto.Store.BL;
//using RmsAuto.Store.Cms;

namespace RmsAuto.Store.Web
{
    public partial class SearchManufacturers : RmsAuto.Store.Web.BasePages.LocalizablePage
	{

        public static string GetUrl()
        {
            return "~/Store/SearchManufacturers.aspx";
        }

		protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}
