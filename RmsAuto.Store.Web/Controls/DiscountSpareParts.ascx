<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscountSpareParts.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.DiscountSpareParts" %>
<%@ Register Src="~/Controls/SparePartInformationExt.ascx" TagName="SparePartInfoExt" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/RMSPagerControl.ascx" TagName="RMSPagerControl" TagPrefix="uc1" %>
<%@ Import Namespace="RmsAuto.Store.Web.Controls" %>

<script type="text/javascript">

	function validate_qty(qtyBoxId) {
		var val = $.trim($('#' + qtyBoxId).val());
		$('#' + qtyBoxId).val(val);

		if (val == '') {
			alert('<%= global::Resources.Texts.AlertQty %>');
			return false;
		}
		else if (!val.match(/^\d+$/)) {
		alert('<%= global::Resources.Texts.AlertCorrectQty %>');
			return false;
		}
		return true;
	}

</script>
<asp:PlaceHolder runat="server" ID="_resultsPlaceHolder">
	<table cellspacing="0" cellpadding="0"  class="list" style="width:100%">
		<tr>
			<th><%=global::Resources.Texts.Manufacturer %></th>
			<th><%=global::Resources.Texts.Article %></th>
			<th><%=global::Resources.Texts.PartName %></th>
			<th><%=global::Resources.Texts.Availability %></th>
			<th><%=global::Resources.Texts.DeliveryPeriod %></th>
			<th>Информация</th>
			<th><%=global::Resources.Texts.Price %></th>
			<th class="empty" colspan="2">&nbsp;</th>
		</tr>
		<asp:Repeater ID="_partsRepeater" runat="server"
			onitemcommand="_partsRepeater_ItemCommand" 
			onitemdatabound="_partsRepeater_ItemDataBound">
			<ItemTemplate>
				<tr class='bgfon'>
					<td>
						<%# Server.HtmlEncode(((SearchDiscountResultItem)Container.DataItem).SparePart.Manufacturer)%>
					</td>
					<td nowrap><asp:Label id="_lblKey" runat="server" Visible="false"></asp:Label>
						<%# Server.HtmlEncode(((SearchDiscountResultItem)Container.DataItem).SparePart.PartNumber)%>
					</td>
					<td>
					   <%# Server.HtmlEncode(((SearchDiscountResultItem)Container.DataItem).SparePart.PartDescription)%>
					</td>
					<td style="text-align:center">
						<%# (((SearchDiscountResultItem)Container.DataItem).SparePart.QtyInStock ?? 0) != 0 ? string.Format("{0}", ((SearchDiscountResultItem)Container.DataItem).SparePart.QtyInStock) : ""%>
						<asp:Image ID="Image1" runat="server" ImageUrl="~/images/qty_full.gif" Width="14" Height="15" Visible='<%#(((SearchDiscountResultItem)Container.DataItem).SparePart.QtyInStock??0)==0%>' ToolTip="В наличии на складе поставщика"  />
					</td>           
					<td><%# ((SearchDiscountResultItem)Container.DataItem).SparePart.DisplayDeliveryDaysMin%>-<%# ((SearchDiscountResultItem)Container.DataItem).SparePart.DisplayDeliveryDaysMax%></td>
					<td>
						<uc1:SparePartInfoExt ID="_sparePartInfoExt" runat="server" AdditionalInfoExt="<%# ((SearchDiscountResultItem)Container.DataItem).AdditionalInfoExt %>" />
						<img id="Img5" runat="server" alt="" src="~/images/search_info.png" title='<%# string.Format( "Количество в заказе должно быть кратно {0}", ((SearchDiscountResultItem)Container.DataItem).SparePart.DefaultOrderQty ) %>' width="14" height="15" border="0" Visible='<%# ( (SearchDiscountResultItem)Container.DataItem ).SparePart.DefaultOrderQty > 1 %>' />
					</td>
					<td class="price" nowrap>
						<font><%# GetOldPriceString(((SearchDiscountResultItem)Container.DataItem).FinalSalePrice, ((SearchDiscountResultItem)Container.DataItem).SparePart.SupplierID) %></font>&nbsp;
						<%# string.Format( "{0:### ### ##0.00}", ((SearchDiscountResultItem)Container.DataItem).FinalSalePrice  ) %>
					</td>
					<td width="25" align="center" 
					onkeypress="if( event.keyCode==13 ) { $('input[name$=_btnAddToCart]',this.parentElement).click(); return false; }">
						<asp:PlaceHolder runat="server" ID="_qtyPlaceHolder" Visible='<%# !IsRestricted %>'>
						  <nobr>
						  <asp:TextBox ID="_txtQty" runat="server" Columns="3" Text='<%# ((SearchDiscountResultItem)Container.DataItem).SparePart.DefaultOrderQty %>'></asp:TextBox>
						  </nobr>
						   <asp:Label ID="_lblDefaultOrderQty" runat="server" Visible="false" Text='<%# ( (SearchDiscountResultItem)Container.DataItem ).SparePart.DefaultOrderQty %>' />
						</asp:PlaceHolder>
					</td>
					<td align="center">
					   <asp:ImageButton ImageUrl="~/images/search_basket2.gif" ToolTip="В корзину" ID="_btnAddToCart" runat="server" CommandName="AddToCart" Text="<%$ Resources:Texts, AddToCartLC %>"  Visible='<%# !IsRestricted && Convert.ToDecimal(((SearchDiscountResultItem)Container.DataItem).FinalSalePrice) != 0 %>' />
					</td>
				</tr>
			</ItemTemplate>
		</asp:Repeater>
	</table>
	<uc1:RMSPagerControl ID="_searchResultPager" runat="server" Visible="false" />
</asp:PlaceHolder>