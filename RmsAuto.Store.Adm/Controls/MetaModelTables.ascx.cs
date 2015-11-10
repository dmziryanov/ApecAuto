using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;

namespace RmsAuto.Store.Adm.Controls
{
    public partial class MetaModelTables : System.Web.UI.UserControl
    {
        public string DataContextType
        {
            get { return (string)ViewState["__dataContextType"]; }
            set { ViewState["__dataContextType"] = value; }
        }

        public string HeaderText
        {
            get { return (string)ViewState["__headerText"]; }
            set { ViewState["__headerText"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Collections.IList visibleTables = MetaModel.GetModel(
                Type.GetType(DataContextType, true)).VisibleTables;
            if (visibleTables.Count == 0)
            {
                throw new InvalidOperationException("There are no accessible tables. Make sure that at least one data model is registered in Global.asax and scaffolding is enabled or implement custom pages.");
            }
            
            Menu1.DataSource = visibleTables;
            Menu1.DataBind();
        }
    }
}