using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Acctg;

namespace RmsAuto.Store.Web.Manager.Controls
{
    public partial class ClientSearchList : System.Web.UI.UserControl
    {
        public string CurrentUserID { get { return (hfUserID == null ? "" : (string)hfUserID.Value); } set { hfUserID.Value = value; } }
        public string CurrentClientName { get { return (string.IsNullOrEmpty(hfClientName.Value) ? "Customer is not choosen" : (string)hfClientName.Value); } set { hfClientName.Value = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void _btnSearchClient_Click(object sender, EventArgs e)
        {
            var searchResults = ClientSearch.Search(
                    _txtClientName.Text.Trim(),
                    "",
                    ClientSearchMatching.Fuzzy).ToList();

            searchResults.Insert(0, new BriefClientInfo() { UserID = -1, ClientName = "All" });

            _listSearchResults.DataSource = searchResults;
            _listSearchResults.DataBind();
        }

        protected void _listSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            //При смене пользователя перезаполняем ключевые поля
            CurrentUserID = _listSearchResults.SelectedValue;
            CurrentClientName = _listSearchResults.SelectedItem.Text;
        }

    }
}