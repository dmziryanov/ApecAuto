using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Web;
using RmsAuto.Common.Web;
using RmsAuto.Store.Entities;
using System.Collections.Generic;

namespace RmsAuto.Store.Web.Manager.Controls
{
	public partial class ClientPickerLite : System.Web.UI.UserControl
	{

        public bool NoClientsFound { get; set; }

        public bool TooManyClientsFound { get; set; }

        private IEnumerable<BriefClientInfo> searchResults { get { return (IEnumerable<BriefClientInfo>)ViewState["BriefClientInfo"]; } set { ViewState["BriefClientInfo"] = value; } }
        private List<BriefClientInfo> DTSorting { get { return (List<BriefClientInfo>)Session["DTSorting"]; } set { Session["DTSorting"] = value; } }


        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set
            {
                ViewState["sortDirection"] = value;
            }

        }

        protected void Page_Load( object sender, EventArgs e )
		{
		    if (AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite && SiteContext.Current.User.Role == SecurityRole.Manager)
            {
                CaptionHeader.InnerText = Resources.Texts.ClientsList;
            }

            if (!IsPostBack)
            {
              //  _managerList.SelectedValue = SiteContext.Current.User.AcctgID; //По умолчанию выбран текущий менеджер
                _managerList.DataSource = ManagerBO.GetManagerList(Resources.Texts.NotSelected);
                _managerList.DataBind();
            }
            
            AddParams.Visible = AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite;
            _txtClientName.Focus();
		}

		protected void _btnSearchClient_Click( object sender, EventArgs e )
		{
			try
			{
                //Если по каким-то неведомым причинам в лайтовом контроле прочиталась неправильная кукас регином возращаем пустой список.
                if (!AcctgRefCatalog.RmsFranches[SiteContext.Current.InternalFranchName].isLite)
                {
                    //searchResults = new List<BriefClientInfo>(0);
                    throw new Exception("Произошла рассинхронизация региона, повторите операцию");
                }
                else
                {
                    DateTime dateMin;
                    DateTime dateMax;
                    string RegDateStringMin = "";
                    string RegDateStringMax = "";
                    string ManagerCondition = "";

                    if (DateTime.TryParse(RegDateMin.Text, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out dateMin))
                    {
                        RegDateStringMin = " AND CONVERT(varchar(10), a.CreationTime, 126) >= '" + dateMin.ToString("yyyy-MM-dd") + "'";
                    }

                    if (DateTime.TryParse(RegDateMax.Text, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out dateMax))
                    {
                        RegDateStringMax = " AND CONVERT(varchar(10), a.CreationTime, 126) <= '" + dateMax.ToString("yyyy-MM-dd") + "'";
                    }

                    if (_managerList.SelectedValue != "0")
                    {
                        ManagerCondition = " AND ManagerId ='" + _managerList.SelectedValue + "'";
                    }

                    searchResults = ClientSearch.LiteClientSearch(
                    _txtClientName.Text.Trim(),
                    _txtPhone.Text.Trim(),
                    ClientSearchMatching.Fuzzy, /*isAutoOrder.SelectedValue*/ "0", isChecked.SelectedValue, /*_clientType.SelectedValue*/"0", RegDateStringMin, RegDateStringMax, ManagerCondition);
                }

                searchResults.Each<BriefClientInfo>(x => x.Manager = AcctgRefCatalog.RmsEmployees.Items.Where(y => y.EmployeeId == x.ManagerId).Select(y => y.FullName).FirstOrDefault());
                Session["searchResults" + SiteContext.Current.User.UserId] = searchResults;

                if (searchResults.Count() > 0)
                {
                    ClientBootstrap.NavigateUrl = String.Format("../ClientBootStrap.ashx?dateMin={0}&dateMax={1}&manager={2}&client={3}&phone={4}&checked={5}", RegDateMin.Text, RegDateMax.Text, _managerList.SelectedValue, _txtClientName.Text.Trim(), _txtPhone.Text.Trim(), isChecked.SelectedValue);
                    ClientBootstrap.Visible = true;
                }
                else
                {
                    ClientBootstrap.Visible = false;
                }

                searchResults = searchResults.OrderByDescending(x => x.CreationTime).ToList();
                              
                ClientGridView.DataSource = searchResults;
                ClientGridView.DataBind();

                NoClientsFound = false;
				TooManyClientsFound = false;
			}
			catch( AcctgException ex )
			{


                ClientGridView.DataSource = null;
                ClientGridView.DataBind();

                if( ex.ErrorCode == AcctgError.TooManyClientsFound )
					TooManyClientsFound = true;
				else if( ex.ErrorCode == AcctgError.NoDataToRespond )
					NoClientsFound = true;
				else
					throw ex;
			}
		}

        protected void _rptSearchResults_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.IsDataBound())
            {
                var info = (BriefClientInfo)e.Item.DataItem;
                if (((ManagerSiteContext)SiteContext.Current).ClientSet.Contains(info.ClientID))
                {
                    e.Item.FindControl("_lbAddToHandySet").Visible = false;
                    ((Label)e.Item.FindControl("_lblWarning")).Text = "already added in list";
                }
            }
        }

        protected void SortList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _btnSearchClient_Click(null, null);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        { 

        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {

        }


        //protected void ClientGridView_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    if (e.SortExpression == "ClientName")
        //        if (e.SortDirection == SortDirection.Ascending)
        //            ClientGridView.DataSource = searchResults.OrderBy(x => x.ClientName);
        //        else
        //            ClientGridView.DataSource = searchResults.OrderByDescending(x => x.ClientName);
                    
        //    //if (SortList.SelectedValue == "1")
        //    //if (SortList.SelectedValue == "2")
        //    //    searchResults = searchResults.OrderBy(x => x.CreationTime);
        //}


        protected void ClientGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string sortExpression = e.SortExpression;
                ViewState["z_sortexpresion"] = e.SortExpression;
                
                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    GridViewSortDirection = SortDirection.Descending;
                    SortGridView(sortExpression, "DESC");
                }
                else
                {
                    GridViewSortDirection = SortDirection.Ascending;
                    SortGridView(sortExpression, "ASC");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при сортировке: " + ex.Message);
            }
        }
      
        //TODO: Возможно стоит сделать общий GridView, который позволяет делать все
        private void SortGridView(string sortExpression, string direction)
        {

            var sorting = new string[14] { "ClientNameASC", "ClientNameDESC", "MainPhoneASC", "MainPhoneDESC", "CreationTimeASC", "CreationTimeDESC", "TradingVolumeASC", "TradingVolumeDESC", "IsCheckedASC", "IsCheckedDESC", "IsAutoOrderASC", "IsAutoOrderDESC", "ManagerASC", "ManagerDESC" };
          
            
            switch (Array.IndexOf(sorting, sortExpression+direction)+1) 
            {

                case 1: DTSorting = searchResults.OrderBy(x => x.ClientName).ToList(); break;
                case 2: DTSorting = searchResults.OrderByDescending(x => x.ClientName).ToList(); break;
                case 3: DTSorting = searchResults.OrderBy(x => x.MainPhone).ToList(); break;
                case 4: DTSorting = searchResults.OrderByDescending(x => x.MainPhone).ToList(); break;
                case 5: DTSorting = searchResults.OrderBy(x => x.CreationTime).ToList(); break;
                case 6: DTSorting = searchResults.OrderByDescending(x => x.CreationTime).ToList(); break;
                case 7: DTSorting = searchResults.OrderBy(x => x.TradingVolume).ToList(); break;
                case 8: DTSorting = searchResults.OrderByDescending(x => x.TradingVolume).ToList(); break;
                case 9: DTSorting = searchResults.OrderBy(x => x.IsChecked).ToList(); break;
                case 10: DTSorting = searchResults.OrderByDescending(x => x.IsChecked).ToList(); break;
                case 11: DTSorting = searchResults.OrderBy(x => x.IsAutoOrder).ToList(); break;
                case 12: DTSorting = searchResults.OrderByDescending(x => x.IsAutoOrder).ToList(); break;
                case 13: DTSorting = searchResults.OrderBy(x => x.Manager).ToList(); break;
                case 14: DTSorting = searchResults.OrderByDescending(x => x.Manager).ToList(); break;
                //case "MainPhone": ;
            }

            //DTSorting = new DataView(DTSorting, "", sortExpression + " " + direction, DataViewRowState.CurrentRows).ToTable();
            

            ClientGridView.DataSource = DTSorting;
            ClientGridView.DataBind();
        }
        
     
        protected void ClientGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    if (GridViewSortDirection == SortDirection.Ascending)
                        e.Row.Cells[sortColumnIndex].Controls.Add(new Label() {Text = "↑", Visible = true});
                    else
                        e.Row.Cells[sortColumnIndex].Controls.Add(new Label() { Text = "↓", Visible = true }); 
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    
                }
            }

        }

        int GetSortColumnIndex()
        {

            // Iterate through the Columns collection to determine the index
            // of the column being sorted.
            foreach (DataControlField field in ClientGridView.Columns)
            {
                if (field.SortExpression == (string)ViewState["z_sortexpresion"])
                {
                    return ClientGridView.Columns.IndexOf(field);
                }
            }

            return -1;
        }

        protected void ClientGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var clientId = (string)e.CommandArgument;
            if (e.CommandName == "AddToHandySet")
            {
              
                    var context = (ManagerSiteContext)SiteContext.Current;
                    context.ClientSet.AddClient(clientId);
                    context.ClientSet.SetDefaultClient(clientId);
                    Response.Redirect(ClientProfile.GetUrl(), true);
              
              
            }
        }

        protected void ClientGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem != null)
            {
                var info = (BriefClientInfo)e.Row.DataItem;
                if (!string.IsNullOrEmpty(info.ClientID) )
                {
                    if (((ManagerSiteContext)SiteContext.Current).ClientSet.Contains(info.ClientID))
                    {
                        e.Row.Cells[5].FindControl("_lbAddToHandySet").Visible = false;
                        ((Label)e.Row.Cells[5].FindControl("_lblWarning")).Text = "already added in list";
                    }
                }
                else
                {
                    e.Row.Cells[5].FindControl("_lbAddToHandySet").Visible = false;
                    ((Label)e.Row.Cells[5].FindControl("_lblWarning")).Visible = false;
                    e.Row.Cells[5].FindControl("_lblNotActivated").Visible = string.IsNullOrEmpty(info.ClientID);
                }
            }
        }

        protected void ClientGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

            
            
            
        }

        protected void ClientGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            
            ClientGridView.DataSource = DTSorting ?? searchResults;
            ClientGridView.PageIndex = e.NewPageIndex;
            ClientGridView.DataBind();
        }


        private void ShowMessage(string message)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "__messageBox",
                "<script type='text/javascript'>alert('" + message + "');</script>");
        }
	}
}