<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderList.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderList" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>

<%@ Register assembly="RmsAuto.Common" namespace="RmsAuto.Common.Web.UI" tagprefix="cc1" %>

<asp:ObjectDataSource runat="server" ID="_objectDataSource" EnablePaging="True" 
StartRowIndexParameterName="startIndex" MaximumRowsParameterName="size"  
TypeName="RmsAuto.Store.Web.Controls.OrderList" 
SelectCountMethod="GetOrdersCount" SelectMethod="GetOrders" 
	onselected="_objectDataSource_Selected" >
<SelectParameters>
	<asp:Parameter Name="statusFilter" Type="Int32" DefaultValue="0" />
	<asp:ControlParameter Name="sort" Type="Int32" DefaultValue="0" ControlID="_sortBox" />
	<asp:Parameter Name="startIndex" Type="Int32" />
	<asp:Parameter Name="size" Type="Int32" />
</SelectParameters>
</asp:ObjectDataSource>


<div class="tab-text">
<script type="text/javascript">

function ToggleAllOrders()
{
	var checkAllOrders = $('input[name=check_all_orders]');
	var orderCheckBoxes = $('input[name=order_id]');
	var checked = checkAllOrders.get(0).checked;

	$.each( orderCheckBoxes, function() { this.checked = checked; } );
}

function RefreshCheckBoxes()
{
	var checkAllOrders = $('input[name=check_all_orders]');
	var orderCheckBoxes = $('input[name=order_id]');
	
	var isAllChecked = true;
	$.each( orderCheckBoxes, function() { if( !this.checked ) isAllChecked = false; } );
	
	if( checkAllOrders.length!=0 ) checkAllOrders.get(0).checked = isAllChecked;
}

$( function() { 
	RefreshCheckBoxes(); 
} );

</script>
<div class="t-hold">
<cc1:ExtendedListView ID="_listView" runat="server" DataSourceID="_objectDataSource" 
	ondatabinding="_listView_DataBinding">
	<LayoutTemplate>
		<table cellspacing="0" cellpadding="0" class="list2" width="100%">
		<tr>
			<th style="width:20px"><input type="checkbox" name="check_all_orders" onclick="ToggleAllOrders()" /></th>
			<th><asp:Literal ID="lOrderNumber" runat="server" Text="Номер заказа" 
					meta:resourcekey="lOrderNumberResource1" /></th>
			<th><asp:Literal ID="lClientOrderNumber" runat="server" Text="Номер заказа клиента" 
					meta:resourcekey="lClientOrderNumberResource1" /></th>
			<th><asp:Literal ID="lSendDate" runat="server" Text="Дата размещения" 
					meta:resourcekey="lSendDateResource1" /></th>
			<th style="width:10%" colspan="2"><asp:Literal ID="lSumm" runat="server" Text="Сумма" 
					meta:resourcekey="lSummResource1" /></th>
			<asp:PlaceHolder runat="server" 
				Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				<th><asp:Literal ID="lStatus" runat="server" Text="Статус" 
						meta:resourcekey="lStatusResource1" /></th>
				<th><asp:Literal ID="lPrint" runat="server" Text="Печать" 
						meta:resourcekey="lPrintResource1" /></th>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" 
				Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ArchiveOrders %>'>
				<th><asp:Literal ID="lEndDate" runat="server" Text="Дата завершения" 
						meta:resourcekey="lEndDateResource1" /></th>
			</asp:PlaceHolder>
		</tr>
<%--		<tr>
			<td colspan="6" class="empty" style=""><img id="Img1" runat="server" src="~/images/1pix.gif" width="1" height="3" border="0"/> </td>
		</tr>--%>
		<tr runat="server" id="itemPlaceHolder" />
		</table>			

	</LayoutTemplate>
	<ItemTemplate>
		<tr runat="server" id="tr1">
			<td style="width:20px" runat="server"><input type="checkbox" name="order_id" 
					value='<%# Eval("OrderID") %>' onclick="RefreshCheckBoxes()" /></td>
			<td runat="server">
				<asp:HyperLink ID="_orderLink" runat="server" 
					NavigateUrl='<%# GetOrderDetailsUrl((int)Eval("OrderID")) %>'
					Text='<%# GetOrderDisplayNumber( (RmsAuto.Store.Entities.Order)Container.DataItem ) %>' />
			</td>
			<td runat="server">
			    <%# Eval("CustOrderNum") %>
            </td>
			<td runat="server">
				<%# Eval("OrderDate", Resources.Format.Date ) %>
			</td>
			<td class="price right_align" runat="server">
				<asp:PlaceHolder runat="server" ID="PlaceHolder1" 
					Visible='<%# !ShowPaymentOrderLink %>'>
					<span class="blue"><%# Eval("Total","{0:### ### ##0.00}") %></span>
				</asp:PlaceHolder>
				<asp:PlaceHolder runat="server" ID="PlaceHolder2" 
					Visible='<%# ShowPaymentOrderLink %>'>
					<asp:HyperLink runat="server" ID="HyperLink1" 
						Target="_blank"
						NavigateUrl='<%# GetPaymentOrderPrintUrl((int)Eval("OrderID")) %>'
						Visible='<%# ShowPaymentOrderLink %>' 
						ToolTip="Распечатать квитанцию для оплаты в Сбербанке РФ">
						<%# Eval("Total","{0:### ### ##0.00}") %>
					</asp:HyperLink>
				</asp:PlaceHolder>
			</td>
			<td runat="server">
				<%--<asp:HyperLink runat="server" ID="HyperLink2" 
					Target="_blank"
					NavigateUrl='<%# GetPaymentOrderPrintUrl((int)Eval("OrderID")) %>'
					Visible='<%# ShowPaymentOrderLink %>'
					 ToolTip="Распечатать квитанцию для оплаты в Сбербанке РФ"><asp:Image ID="Image1" runat="server" ImageUrl="~/images/s-blank.gif" ImageAlign="AbsBottom" />
</asp:HyperLink>--%>
			</td>
			<td ID="td3" runat="server" 
				Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				<%# ((OrderStatus)Eval("Status")).GetDescription() %>
			</td>
			<td ID="td4" runat="server" 
				Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				<asp:HyperLink runat="server" ID="HyperLink3" 
					Target="_blank"
					NavigateUrl='<%# "~/Cabinet/Orders/OrderPrint.aspx?OrderID=" + Eval("OrderID").ToString() %>'
					ToolTip="Распечатать заказ"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/print.png" ImageAlign="AbsBottom" />
</asp:HyperLink>
			</td>
			<td ID="td5" runat="server" 
				Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ArchiveOrders %>'>
				<%# Eval( "CompletedDate", Resources.Format.Date ) %>
			</td>
		</tr>
	</ItemTemplate>
	<EmptyDataTemplate>
        <div class="clearfloat"></div>
		<table width="100%">
		<tr><td>
		<asp:Literal ID="lEmptyOrderList" runat="server" Text="Order list is empty" 
				meta:resourcekey="lEmptyOrderListResource1" />	
		</td></tr>
		</table>			
	</EmptyDataTemplate>
</cc1:ExtendedListView>
    </div>
<div class="bottom">
    <div class="left">
        <div class="pages">
            <ul>
                <li>
                    <asp:DataPager ID="_dataPager" runat="server" PagedControlID="_listView">
                        <Fields>
	                        <asp:NextPreviousPagerField ShowFirstPageButton="False" 
		                        ShowNextPageButton="False" ShowPreviousPageButton="False" PreviousPageText="" ButtonCssClass="prev" />
	                        <asp:NumericPagerField />
	                        <asp:NextPreviousPagerField ShowLastPageButton="False" 
		                        ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText="" ButtonCssClass="next" />
                        </Fields>
                    </asp:DataPager>
                </li>
                <li runat="server" id="_sortBlock">
                    <span class="text"><%= global::Resources.Texts.OrderBy%></span>
	                    <asp:DropDownList runat="server" ID="_sortBox" AutoPostBack="True" 
		                    OnSelectedIndexChanged="_sortBox_SelectedIndexChanged" />
                </li>
                <li>
                    <asp:Label ID="lCountView" CssClass="text" runat="server" Text="<%$ Resources:Texts, DisplayOn %>" />
		            <asp:DropDownList runat="server" ID="_pageSizeBox" AutoPostBack="True" 
			            OnSelectedIndexChanged="_pageSizeBox_SelectedIndexChanged">
			            <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
			            <asp:ListItem Text="40" Value="40"></asp:ListItem>
	    	            <asp:ListItem Text="100" Value="100"></asp:ListItem>
		                <asp:ListItem Text="400" Value="400"></asp:ListItem>
		            </asp:DropDownList>
                </li>
            </ul>
        </div>
        <asp:Button runat="server" ID="_showOrdersButton" CssClass="button" Text="<%$ Resources:Texts, ToShowTheOrder %>" onclick="_showOrdersButton_Click"  />
        
	</div>
    <div class="right">
        <asp:PlaceHolder runat="server" ID="_footerBlock">

	<div class="right_block2" id="_activeOrderTotalsBlock" runat="server" visible="false">
	<br />
	<asp:Literal ID="lOrderOn" runat="server" Text="Orders total" 
			meta:resourcekey="lOrderOnResource1" /> <asp:Label runat="server" 
			ID="_processingOrdersTotalLabel" CssClass="price" /> <%=global::Resources.Texts.DollarShort%>
	<br />
	<asp:Literal ID="lWorkDetails" runat="server" Text="Processing lines total" 
			meta:resourcekey="lWorkDetailsResource1" /> <asp:Label runat="server" 
			ID="_processingLinesTotalLabel" CssClass="price" /> <%=global::Resources.Texts.DollarShort%>
	</div>

	<div class="right_block2" id="_archiveOrderTotalsBlock" runat="server" visible="false">
	<br />
	<asp:Literal ID="lAllSumm" runat="server" Text="Общая сумма заказов" 
			meta:resourcekey="lAllSummResource1" /> <asp:Label runat="server" 
			ID="_completedOrdersTotalLabel" CssClass="price" /> <%=global::Resources.Texts.DollarShort%>
	<br />
	<asp:Literal ID="lSaleOn" runat="server" Text="Выдано на" 
			meta:resourcekey="lSaleOnResource1" /> <asp:Label runat="server" 
			ID="_receivedByClientSumLabel" CssClass="price" /> <%=global::Resources.Texts.DollarShort%>
	</div>


</asp:PlaceHolder>
    </div>
    </div>
</div>