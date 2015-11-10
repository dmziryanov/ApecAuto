using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

using RmsAuto.Common.DataAnnotations;
using RmsAuto.Common.Misc;
using RmsAuto.Common.Web;
using RmsAuto.Store.Cms.Entities;
using System.Collections.Generic;
using System.Threading;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.Banners
{
	public partial class Details : Security.BasePage/*System.Web.UI.Page*/
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

		protected void Page_Init(object sender, EventArgs e)
		{
			table = MetaModel.GetModel( typeof( CmsDataContext ) ).GetTable( typeof( CatalogItem ) );
		}

        protected void Page_Load( object sender, EventArgs e )
		{
			Title = table.DisplayName;

            GoToEditLink.NavigateUrl = GetEditLink();
            GoToListLink.NavigateUrl = GetListLink();
		}

	    protected void CheckBoxVisibilityChecked(object sender, EventArgs e)
        {
	        String s = ((CheckBox) sender).Text;
	        int start = s.IndexOf('[') + 1;
            int catalogItemID = Convert.ToInt32(s.Substring(start, s.IndexOf(']') - start));//Trim(new char[] { '[', ']' });
            start = s.IndexOf('{') + 1;
            byte position = Convert.ToByte(s.Substring(start, s.IndexOf('}') - start));

            Cms.Entities.Banners.ChangeLinkVisibilityToCatalogItem(BannerID, catalogItemID, position, ((CheckBox)sender).Checked);
        }

        protected void CheckBoxLinkChecked(object sender, EventArgs e)
        {
            String s = ((CheckBox)sender).Text;
            int start = s.IndexOf('[') + 1;
            int catalogItemID = Convert.ToInt32(s.Substring(start, s.IndexOf(']') - start));//Trim(new char[] { '[', ']' });
            start = s.IndexOf('{') + 1;
            byte position = Convert.ToByte(s.Substring(start, s.IndexOf('}') - start));

            if (((CheckBox)sender).Checked == false)
                Cms.Entities.Banners.RemoveLinkFromCatalogItem(BannerID, catalogItemID, position);
            else
                Cms.Entities.Banners.AddLinkToCatalogItem(BannerID, catalogItemID, position);
        }

        protected void Page_PreRender( object sender, EventArgs e )
		{
            using (var dc = new DCWrappersFactory<CmsDataContext>())
			{
			    var groups = dc.DataContext.CatalogItems.GroupBy( c => c.ParentItemID ).ToDictionary( g => g.Key ?? 0 );

			    if( groups.Count != 0 )
				{
					var list = new List<object>();
					var stack = new Stack<KeyValuePair<CatalogItemLink,int>>();

                    var tmp = new CatalogItemLink();

					foreach( var item in groups[ 0 ].OrderByDescending( c=>c.CatalogItemPriority ) )
					{
                        if (item.BannerCount > 1)
                        {
                            for (byte i = 0; i < item.BannerCount; i++)
                            {
                                tmp = tmp.FullDeepCopy(dc.DataContext, BannerID, item, i);
                                stack.Push(new KeyValuePair<CatalogItemLink, int>(tmp, 0));
                            }
                        }
                        else
					    {
                            tmp = tmp.FullDeepCopy(dc.DataContext, BannerID, item, 0);
                            stack.Push(new KeyValuePair<CatalogItemLink, int>(tmp, 0));
					    }
					}

					while( stack.Count != 0 )
					{
						var node = stack.Pop();
                        
						list.Add( new { CatalogItemLink = node.Key, Level = node.Value } );

                        if (node.Key.Position > 0) continue;

						if( groups.ContainsKey( node.Key.CatalogItemID ) )
						{
							foreach( var item in groups[ node.Key.CatalogItemID ].OrderByDescending( c => c.CatalogItemPriority ) )
                                if (item.BannerCount > 1)
                                {
                                    for (byte i = 0; i < item.BannerCount; i++)
                                    {
                                        tmp = tmp.FullDeepCopy(dc.DataContext, BannerID, item, i);
                                        stack.Push(new KeyValuePair<CatalogItemLink, int>(tmp, node.Value + 1));
                                    }
                                }
                                else
                                {
                                    tmp = tmp.FullDeepCopy(dc.DataContext, BannerID, item, 0);
                                    stack.Push(new KeyValuePair<CatalogItemLink, int>(tmp, node.Value + 1));
                                }
						}
					}

				    _repeater.DataSource = list;
					_repeater.DataBind();
				}

			}
		}

        protected String GetEditLink()
        {
            String s = Request.RawUrl;
            s = s.Substring(0, s.LastIndexOf("/")) + "/Edit.aspx?" + UrlKeys.Id + "=" + BannerID.ToString();
            return s;
        }

        protected String GetListLink()
        {
            String s = Request.RawUrl;
            s = s.Substring(0, s.LastIndexOf("/")) + "/List.aspx";
            return s;
        }
	}
}