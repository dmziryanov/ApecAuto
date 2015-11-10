using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace RmsAuto.Store.Web.TecDoc
{
	/// <summary>
	/// Марка автомобиля (информация из CMS (Brand), и соответствующий ей объект Manufacturer из Текдока)
	/// </summary>
	public class BrandItem
	{
		public RmsAuto.Store.Cms.Entities.Brand Brand { get; private set; }
		public RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer Manufacturer { get; private set; }
		public BrandItem( RmsAuto.Store.Cms.Entities.Brand brand, RmsAuto.TechDoc.Entities.TecdocBase.Manufacturer manufacturer )
		{
			if( brand == null ) throw new ArgumentNullException( "brand" );
			if( manufacturer == null ) throw new ArgumentNullException( "manufacturer" );

			Brand = brand;
			Manufacturer = manufacturer;
		}
	}
}
