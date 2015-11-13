using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;
using System.Collections.Generic;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates.Custom
{
    public partial class ForeignKey_EditField : System.Web.DynamicData.FieldTemplateUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DropDownList1.Items.Count == 0)
            {
                if (!Column.IsRequired)
                {
                    DropDownList1.Items.Add(new ListItem("[не выбран]", ""));
                }

				

                PopulateCatalogItems(DropDownList1);
            }
        }

		private void PopulateCatalogItems( DropDownList dropDownList )
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				var groups = dc.DataContext.CatalogItems.GroupBy( c => c.ParentItemID ).ToDictionary( g => g.Key ?? 0 );

				if( groups.Count != 0 )
				{
					var stack = new Stack<KeyValuePair<CatalogItem,int>>();

					foreach( var item in groups[ 0 ].OrderByDescending( c=>c.CatalogItemPriority ) )
						stack.Push( new KeyValuePair<CatalogItem, int>( item, 0 ) );

					while( stack.Count != 0 )
					{
						var node = stack.Pop();
						dropDownList.Items.Add(
							new ListItem(
								new string( '.', 3*node.Value ) + node.Key.CatalogItemName,
								node.Key.CatalogItemID.ToString() )
								);

						if( groups.ContainsKey( node.Key.CatalogItemID ) )
						{
							foreach( var item in groups[ node.Key.CatalogItemID ].OrderByDescending( c => c.CatalogItemPriority ) )
								stack.Push( new KeyValuePair<CatalogItem, int>( item, node.Value + 1 ) );
						}						
					}

				}

			}
		}
		

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            if (Mode == DataBoundControlMode.Edit)
            {
                string foreignkey = ForeignKeyColumn.GetForeignKeyString(Row);
                ListItem item = DropDownList1.Items.FindByValue(foreignkey);
                if (item != null)
                {
                    DropDownList1.SelectedValue = foreignkey;
                }
            }
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            // If it's an empty string, change it to null
            string val = DropDownList1.SelectedValue;
            if (val == String.Empty)
                val = null;

            ExtractForeignKey(dictionary, val);
        }

        public override Control DataControl
        {
            get
            {
                return DropDownList1;
            }
        }
    }
}
