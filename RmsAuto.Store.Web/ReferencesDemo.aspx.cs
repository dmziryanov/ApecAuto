using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Common.Misc;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web
{
    public partial class ReferencesDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void _rptReferences_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    var reference = AcctgRefCatalog.GetReference((string)e.Item.DataItem);

                    ((Literal)e.Item.FindControl("_ltrRefName")).Text = reference.Description ?? reference.Name;
                    var refItems = (GridView)e.Item.FindControl("_refItems");
                    refItems.DataSource = reference.Items;
                    refItems.DataBind();
                    
                }
                catch (Exception ex)
                {
                    ((Label)e.Item.FindControl("_lblErrorMessage")).Text = "Ошибка: " + 
                        ex.Message.Replace("\n", "<br/>");
                }
            }
        }
        
        private void BindData()
        {
            _rptReferences.DataSource = AcctgRefCatalog.RefNames;
            _rptReferences.DataBind();
        }
    }
}
