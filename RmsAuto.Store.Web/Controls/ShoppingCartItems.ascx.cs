using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;

using RmsAuto.Common.Misc;
using RmsAuto.Store.Web;
using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web.Resource;

namespace RmsAuto.Store.Web.Controls
{
    public partial class ShoppingCartItems : System.Web.UI.UserControl
    {
        // deas 01.03.2011 task2401
        // хеш для хранения сгрупированной корзины, для контроля превышения количества, кратности и т.д.
        private Dictionary<SparePartPriceKey, ShoppingCartItem> _groupCart = new Dictionary<SparePartPriceKey, ShoppingCartItem>();
 
        public bool DisplayVinOptions
        {
            get { return Convert.ToBoolean(ViewState["__DisplayVinOptions"]); }
            set { ViewState["__DisplayVinOptions"] = value; }
        }
        
        public int CartItemsCount
        {
            get { return Convert.ToInt32(ViewState["__itemsCount"]); }
            private set { ViewState["__itemsCount"] = value; }
        }

        public bool ContainsBadItems
        {
            get { return Convert.ToBoolean(ViewState["__containsBadItems"]); }
            private set { ViewState["__containsBadItems"] = value; }
        }

        protected int PartsCount
        {
            get { return Convert.ToInt32(ViewState["__partsCount"]); }
            set { ViewState["__partsCount"] = value; }
        }

        protected decimal Total
        {
            get { return Convert.ToDecimal(ViewState["__total"]); }
            set { ViewState["__total"] = value; }
        }

        protected bool IsGuest
        {
            get { return SiteContext.Current.CurrentClient.IsGuest; }
        }

		public enum SortOptions
		{
			//[Text("Производитель, артикул")]
			[Text("Brand, part number")]
            [LocalizedDescription("SortOptions_ManufacturerArticle", typeof(EnumResource))]
			ManufacturerArticle,

			//[Text("В порядке ввода")]
			[Text("In the order entered")]
            [LocalizedDescription("SortOptions_InputOrder", typeof(EnumResource))]
            InputOrder
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
				foreach( SortOptions sortField in Enum.GetValues( typeof( SortOptions ) ) )
				{
					_sortBox.Items.Add( new ListItem( sortField.GetDescription(), ( (int)sortField ).ToString() ) );
				}
				_sortBox.SelectedValue = Convert.ToString( (int)SortOptions.ManufacturerArticle );

                DisplayVinOptions = 
                    !SiteContext.Current.IsAnonymous &&
                    SiteContext.Current.User.Role != SecurityRole.Manager &&
                    ClientCarsDac.GetGarageCars(SiteContext.Current.CurrentClient.Profile.ClientId).Count() > 0;
                BindData();
            }
        }
        
        private void BindData()
        {
            var items = SiteContext.Current.CurrentClient.Cart.GetItems();
            this.CartItemsCount = items.Count();
            this.ContainsBadItems = items.Any(i => i.HasIssues);
            this.PartsCount = items.Sum(i => i.Qty);
            this.Total = items.Sum(i => i.ItemTotal);
            			
			switch( (SortOptions)Convert.ToInt32(_sortBox.SelectedValue) )
			{
                case SortOptions.InputOrder: 
                    _cartItemsRepeater.DataSource = items.OrderBy(o => o.ItemID);
                    break;
                case SortOptions.ManufacturerArticle:
                    _cartItemsRepeater.DataSource = items
                        .OrderBy(o => o.Manufacturer)
                        .ThenBy(o => o.PartNumber);
                        break;
			}

            // deas 01.03.2011 task2401
            // заполнение сгруппированной корзины
            // deas 16.03.2011 task2983
            // не проверять количественные показатели у позиций отсутствующих в прайс листе
            _groupCart.Clear();
            foreach ( var oneI in items )
            {
                SparePartPriceKey key = new SparePartPriceKey( oneI.Manufacturer, oneI.PartNumber, oneI.SupplierID );
                if ( _groupCart.ContainsKey( key ) )
                {
                    _groupCart[key].Qty += oneI.Qty;
                    if ( oneI.SparePart != null )
                    {
                        _groupCart[key].UpdateQtyIssues( oneI.SparePart );
                    }
                }
                else
                {
                    _groupCart.Add( key, new ShoppingCartItem
                    {
                        Manufacturer = oneI.Manufacturer,
                        PartNumber = oneI.PartNumber,
                        SupplierID = oneI.SupplierID,
                        DeliveryDaysMin = oneI.DeliveryDaysMin,
                        DeliveryDaysMax = oneI.DeliveryDaysMax,
                        Qty = oneI.Qty
                    } );
                    if ( oneI.SparePart != null )
                    {
                        _groupCart[key].UpdateQtyIssues( oneI.SparePart );
                    }
                }
            }

            _cartItemsRepeater.DataBind();
            _btnRecalc.Enabled = CartItemsCount > 0;

        }


		protected void _sortBox_OnSelectedIndexChanged( object sender, EventArgs e )
		{
			BindData();
		}

        protected void _cartItemsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // deas 01.03.2011 task2401
                // обрабатывается элемент из сгруппированной корзины, а не отображаемый
                var cartItem = (ShoppingCartItem)e.Item.DataItem;
                var cartItemG = _groupCart[new SparePartPriceKey( cartItem.Manufacturer, cartItem.PartNumber, cartItem.SupplierID )];
                ((Label)e.Item.FindControl("_hiddenItemKey")).Text = string.Format(
                    "{0},{1},{2}", cartItem.Manufacturer, cartItem.PartNumber, cartItem.SupplierID);
                var options = (OrderLineOptions)e.Item.FindControl("_options");
                string warning = null;

                if (cartItem.Discontinued)
                {
                    ((TextBox)e.Item.FindControl("_txtQty")).Enabled = false;
                    ((CheckBox)e.Item.FindControl("_chkRemove")).Checked = true;
                    ((HtmlAnchor)e.Item.FindControl("_sparePartDetails")).Disabled = true;
                    options.Visible = false;

                    // 2014-05-30 zborovskiy
                    // Компенсирует скрытие контрола options, из-за которого пропадала ячейка таблицы
                    HtmlTableCell optEmpty = e.Item.FindControl("optEmpty") as HtmlTableCell;
                    if (optEmpty != null)
                        optEmpty.Visible = true;

					warning = Resources.ValidatorsMessages.CartItemNotInStock + "<br/>" + Resources.ValidatorsMessages.CartToProceedPleaseDelete;//"Товара нет в наличии.<br/>Для оформления заказа необходимо удалить товар из корзины";

                    var btnSearch = (LinkButton)e.Item.FindControl("_btnSearch");
                    btnSearch.Visible = true;
					btnSearch.PostBackUrl = UrlManager.GetSearchSparePartsUrl( cartItem.Manufacturer, cartItem.PartNumber, true );
                }
                else
                {
                    options.IsAnalogsNotSupported = cartItem.IsAnalogsNotSupported;
                    options.StrictlyThisNumber = cartItem.StrictlyThisNumber;
                    options.VinCheckupDataId = cartItem.VinCheckupDataID;
                    //Возможно стоит перенести это до стрикилти
					
                    

                    if ( cartItemG.QtyBelowMinimum )
                        warning += String.Format(
							Resources.ValidatorsMessages.CartMinOrderQty
                            /*"Минимальное необходимое количество для заказа: {0}"*/,
                            cartItem.SparePart.MinOrderQty );
                    else if ( cartItemG.QtyMultiplicityViolation )
                        warning += Resources.ValidatorsMessages.CartQtyShouldBeMultiply /*"Количество должно быть кратным числу деталей в комплекте ("*/ +
                        cartItem.SparePart.DefaultOrderQty
                        .Progression( cartItem.SparePart.DefaultOrderQty, 5 )
                        .Select(i => i.ToString())
                        .Aggregate((acc, s) => acc + "," + s) + Resources.ValidatorsMessages.CartAndSoOn /*" и т.д.)"*/;

					if (cartItemG.QtyAboveAvailableInStock)
					{
						warning += /*String.Format(*/
							Resources.ValidatorsMessages.CartQtyExceedsLimit /*"Количество превышает допустимый лимит:"*/ /*{0}",
							cartItem.SparePart.QtyInStock)*/;
						string key = Guid.NewGuid().ToString().ToLower();
						Session[key] = cartItem.SparePart.QtyInStock;
						((HtmlImage)e.Item.FindControl("_imgQtyInStock")).Src = ResolveUrl("~/qty.ashx") + "?id=" + key;
					}

                    if (cartItem.PriceChanged)
                        warning += "<br/>" + Resources.ValidatorsMessages.CartPriceChangedRecountPlease /*"Цена на данную позицию изменилась. Выполните пересчет корзины"*/; 
                }

                // deas 25.05.2011 task4218 запрет заказа товаров по нулевой цене
                if ( cartItem.UnitPrice == 0 )
                {
                    if ( warning == null )
                    {
                        warning += Resources.ValidatorsMessages.CartIncorrectItemPleaseDelete /*"Ошибочная позиция. Удалите ее из корзины."*/;
                    }
                    else
                    {
                        warning += "<br/>" + Resources.ValidatorsMessages.CartIncorrectItemPleaseDelete /*"Ошибочная позиция. Удалите ее из корзины."*/;
                    }
                }
                                                
               ((Label)e.Item.FindControl("_lblWarning")).Text = warning;
            }
        }

		public void Recalc()
		{
			if( Page.IsValid )
			{
				List<ShoppingCartItem> cartItems = new List<ShoppingCartItem>( _cartItemsRepeater.Items.Count );
				foreach( RepeaterItem item in _cartItemsRepeater.Items )
					if( item.ItemType == ListItemType.Item ||
						item.ItemType == ListItemType.AlternatingItem )
						cartItems.Add( GetCartItem( item ) );

				SiteContext.Current.CurrentClient.Cart.Update( cartItems );
				BindData();
			}
		}

        protected void _btnRecalc_Click(object sender, EventArgs e)
        {
			Recalc();
        }

        private ShoppingCartItem GetCartItem(RepeaterItem item)
        {
            var key = SparePartPriceKey.Parse(((Label)item.FindControl("_hiddenItemKey")).Text);
            int qty = ((CheckBox)item.FindControl("_chkRemove")).Checked ? 0 : 
                Convert.ToInt32(((TextBox)item.FindControl("_txtQty")).Text);
            string refid = ((TextBox) item.FindControl("_txtReferenceID")).Text.ToString();
            // deas 28.02.2011 task2401
            // добавлен учет ItemID для разделения строк с разными референсами
            int itemID = 0;
            int.TryParse( ( (HiddenField)item.FindControl( "_itemID" ) ).Value, out itemID );
            // deas 28.04.2011 task3929 добавление в корзине флага отправить в заказ
            bool addToOrder = ( (CheckBox)item.FindControl( "_chkAddToOrder" ) ).Checked;
            var options = (OrderLineOptions)item.FindControl("_options");
            return new ShoppingCartItem()
            {
                AddToOrder = addToOrder,
                ItemID = itemID,
                Manufacturer = key.Mfr,
                PartNumber = key.PN,
                SupplierID = key.SupplierId,
                Qty = qty,
                ReferenceID = refid,                
                VinCheckupDataID = options.VinCheckupDataId,
                StrictlyThisNumber = options.StrictlyThisNumber
            };
        }

        public void _qtyValidator_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            var parent = ((Control)source).Parent;
            if (!((CheckBox)parent.FindControl("_chkRemove")).Checked)
            {
                args.IsValid = !string.IsNullOrEmpty(args.Value);
            }
        }

        protected string GetSparePartDetailsUrl(SparePartFranch sparePart)
        {
            if (sparePart == null)
                return string.Empty;

            SparePartPriceKey key = new SparePartPriceKey(
                sparePart.Manufacturer,
                sparePart.PartNumber,
                sparePart.SupplierID);
            return UrlManager.GetSparePartDetailsUrl(key.ToUrlString());
        }

        protected void CheckBoxRemoveAllCheckedChanged(object sender, EventArgs e)
        {
            foreach (var item in _cartItemsRepeater.Items)
                ((CheckBox)(item as Control).FindControl("_chkRemove")).Checked = ((CheckBox)sender).Checked;
        }

        protected void CheckBoxAddToOrderAllCheckedChanged( object sender, EventArgs e )
        {
            foreach ( var item in _cartItemsRepeater.Items )
                ( (CheckBox)( item as Control ).FindControl( "_chkAddToOrder" ) ).Checked = ( (CheckBox)sender ).Checked;
        }

		protected void _chkStrictlyAll_CheckedChanged( object sender, EventArgs e )
		{
			foreach (var item in _cartItemsRepeater.Items)
			{
				var options = (OrderLineOptions)(item as Control).FindControl( "_options" );
				if (options.IsAnalogsNotSupported.HasValue && !options.IsAnalogsNotSupported.Value) 
                    { options.StrictlyThisNumber = ((CheckBox)sender).Checked; }
			}
		}

        protected void _cartItemsRepeater_ItemCommand( object source, RepeaterCommandEventArgs e )
        {
            // deas 01.03.2011 task2401
            // команда копирования позиции для добавления нового ReferenceID
            if ( e.CommandName == "CopyPosition" )
            {
                Recalc();
                if ( SiteContext.Current.CurrentClient.Cart.GetTotals().ItemsCount != 0 )
                {
                    var key = SparePartPriceKey.Parse( ( (Label)e.Item.FindControl( "_hiddenItemKey" ) ).Text );
                    if ( !SiteContext.Current.CurrentClient.Cart.GetItems()
                        .Any( t => t.Manufacturer == key.Mfr
                        && t.PartNumber == key.PN
                        && t.SupplierID == key.SupplierId
                        && t.ReferenceID == "" ) )
                    {
                        // deas 27.04.2011 task3929 добавление в корзине флага отправить в заказ
                        bool addToOrder = true;
                        try
                        {
                            addToOrder = ( (CheckBox)e.Item.FindControl( "_chkAddToOrder" ) ).Checked;
                        }
                        catch { }
                        SiteContext.Current.CurrentClient.Cart.Add( key, 1, addToOrder );
                        BindData();
                    }
                }
            }
        }

    }
}