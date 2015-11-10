using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms;
using RmsAuto.Common.Data;
using RmsAuto.Common.Web;

namespace RmsAuto.Store.Web.Controls
{
    public partial class VinRequestNew : System.Web.UI.UserControl
    {
        public IEnumerable<UserGarageCar> GarageCars { get; set; }
        List<LineItem> LineItems
        {
            get
            {
                return ViewState["__LineItems"] as List<LineItem>;
            }
            set
            {
                ViewState["__LineItems"] = value;
            }
        }

        void BindData()
        {
            if (this.LineItems != null)
            {
                _gvLineItems.DataSource = this.LineItems;
                _gvLineItems.DataBind();
            }
        }

        protected void btnAddItemClick(object sender, EventArgs e)
        {
            if (this.LineItems == null)
            {
                this.LineItems = new List<LineItem>();
            }

            this.LineItems.Add(new LineItem());
            _btnAddItem.Visible = false;
            gvLineItemsRowEditing(_gvLineItems, new GridViewEditEventArgs(this.LineItems.Count - 1));
            ScrollToButtons();
        }

        protected void gvLineItemsRowEditing(object sender, GridViewEditEventArgs e)
        {
            _gvLineItems.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void gvLineItemsRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("GridViewGroup");

            if (Page.IsValid)
            {
                var lineItem = this.LineItems[e.RowIndex];

                lineItem.Name = (_gvLineItems.Rows[e.RowIndex].Cells[0].FindControl("_txtName") as TextBox).Text;
                lineItem.Qty = Convert.ToInt16((_gvLineItems.Rows[e.RowIndex].Cells[1].FindControl("_txtQty") as TextBox).Text);
                lineItem.Description = (_gvLineItems.Rows[e.RowIndex].Cells[2].FindControl("_txtDescription") as TextBox).Text;
                lineItem.IsOldItem = true;
                _btnAddItem.Visible = true;
                _gvLineItems.EditIndex = -1;
                ScrollToGrid();
            }
            BindData();
        }

        protected void gvLineItemsRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            this.LineItems.RemoveAt(e.RowIndex);
            _btnAddItem.Visible = true;
            BindData();
            ScrollToGrid();
        }

        protected void gvLineItemsRowEditCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            var lineItem = this.LineItems[e.RowIndex];
            if (lineItem.IsOldItem)
            {
                //  Если айтем уже был - проверим
                Page.Validate("GridViewGroup");

                if (Page.IsValid)
                {
                    _gvLineItems.EditIndex = -1;
                    _btnAddItem.Visible = true;
                    BindData();
                }
            }
            else
            {
                gvLineItemsRowDeleting(sender, new GridViewDeleteEventArgs(e.RowIndex));
                ScrollToGrid();
            }
        }

        protected void btnAddRequestClick(object sender, EventArgs e)
        {
            if (_gvLineItems.Rows.Count == 0)
            {
                _lblNoItems.Visible = true;
                ScrollToButtons();
                return;
            }

            if (_gvLineItems.EditIndex > -1)
            {
                _lblEditingItems.Visible = true;
                ScrollToButtons();
                return;
            }

            Page.Validate("CarEditGroup");

            if (Page.IsValid && SaveRequest != null)
            {
                var rq = _garageCarEdit.GetNewCarData<VinRequest>();
                this.LineItems.Each(item =>
                    {
                        VinRequestItem rqItem = new VinRequestItem();
                        rqItem.VinRequest = rq;
                        rqItem.Name = item.Name;
                        rqItem.Quantity = item.Qty;
                        rqItem.Description = item.Description;

                        rq.VinRequestItems.Add(rqItem);
                    });

                SaveRequest(this, new VinRequestEventArgs(rq));
            }
        }

        void LoadGarageCars()
        {
            this.GarageCars = ClientCarsDac.GetGarageCars(SiteContext.Current.CurrentClient.Profile.ClientId);

            if (!this.IsPostBack)
            {
                ddCarFromGarage.Items.Add(new ListItem()
                {
                    Value = String.Empty,
                    Text = global::Resources.Texts.NA_Female
                });

                if (this.GarageCars.Count() > 0)
                {
                    ddCarFromGarage.Items[0].Selected = true;
                    this.GarageCars.Each(gc => ddCarFromGarage.Items.Add(new ListItem()
                    {
                        Value = gc.Id.ToString(),
                        Text = gc.GetFullName()
                    }));
                }
            }
        }

        void SelectGarageCar(int id)
        {
            var selectedCar = this.GarageCars.Single(gc => gc.Id == id);
            _garageCarEdit.SetFields(selectedCar);
        }

        void ScrollToGrid()
        {
			Page.ClientScript.RegisterStartupScript( GetType(), "scroll", "<script>$(document).ready(function() { scrollToGrid(); });</script>" );
        }
        void ScrollToButtons()
        {
			Page.ClientScript.RegisterStartupScript( GetType(), "scroll", "<script>$(document).ready(function() { scrollToButtons(); });</script>" );
        }

        protected void ddCarFromGarageChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ddCarFromGarage.SelectedValue) &&
                this.GarageCars != null)
            {
                SelectGarageCar(Convert.ToInt32(ddCarFromGarage.SelectedValue));
            }
            else
            {
				_garageCarEdit.Clear();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGarageCars();

            var cid = Request.QueryString.TryGet<int>(UrlKeys.VinRequests.CarId);
            if (!IsPostBack && cid > 0)
            {
                SelectGarageCar(cid);
                ddCarFromGarage.SetSelected(cid);
            }
        }

        public event VinRequestEventHandler SaveRequest;

        [Serializable]
        class LineItem
        {
            public string Name { get; set; }
            public short Qty { get; set; }
            public string Description { get; set; }
            public bool IsOldItem { get; set; }
        }
    }
}