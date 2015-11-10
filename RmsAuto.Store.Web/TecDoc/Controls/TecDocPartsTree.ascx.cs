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
using RmsAuto.TechDoc;
using RmsAuto.TechDoc.Entities.Helpers;
using RmsAuto.Store.Cms.Routing;
using System.Collections.Generic;

namespace RmsAuto.Store.Web.TecDoc.Controls
{
	public partial class TecDocPartsTree : System.Web.UI.UserControl
	{
		public int ModificationId { get; set; }

		protected string GetPartsUrl( int strId )
		{
			return UrlManager.GetTecDocSearchTreeNodeDetailsUrl( ModificationId, strId );
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			List<SearchTreeNodeHelper> tree = Facade.GetTree( ModificationId );

			//отрезать фиктивный корень
			SearchTreeNodeHelper root = tree.SingleOrDefault();

			if( root != null )
			{
				tree = root.Children;
				foreach( SearchTreeNodeHelper node in tree )
					node.ParentSearchTreeNodeID = null;
			}
		
			_repeater.DataSource = tree;
			_repeater.DataBind();
		}

		protected void _repeater_ItemDataBound( object sender, RepeaterItemEventArgs e )
		{
			if( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
			{
				SearchTreeNodeHelper node = (SearchTreeNodeHelper)e.Item.DataItem;
				if( node.Children.Count != 0 )
				{
					Control ctl = e.Item.FindControl( "_childsPlaceHolder" );
					Repeater rpt = new Repeater();
					rpt.ItemTemplate = _repeater.ItemTemplate;
					rpt.HeaderTemplate = _repeater.HeaderTemplate;
					rpt.FooterTemplate = _repeater.FooterTemplate;
					rpt.ItemDataBound += _repeater_ItemDataBound;
					ctl.Controls.Add( rpt );
					rpt.DataSource = node.Children;
					rpt.DataBind();
				}
			}
		}

	}
}