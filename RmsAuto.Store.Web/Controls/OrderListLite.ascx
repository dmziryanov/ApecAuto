<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderListLite.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderListLite" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<%@ Register assembly="RmsAuto.Common" namespace="RmsAuto.Common.Web.UI" tagprefix="cc1" %>

<asp:ObjectDataSource runat="server" ID="_objectDataSource" EnablePaging="True" 
StartRowIndexParameterName="startIndex" MaximumRowsParameterName="size"  
TypeName="RmsAuto.Store.Web.Controls.OrderListLite" 
SelectCountMethod="GetOrdersCount" SelectMethod="GetOrders" UpdateMethod="UpdateOrders" onselected="_objectDataSource_Selected" >
<SelectParameters>
	<asp:Parameter Name="statusFilter" Type="Int32" DefaultValue="0" />
	<asp:ControlParameter Name="sort" Type="Int32" DefaultValue="0" ControlID="_sortBox" />
	<asp:Parameter Name="startIndex" Type="Int32" />
	<asp:Parameter Name="size" Type="Int32" />
</SelectParameters>
<UpdateParameters>
	<asp:Parameter Name="statusFilter" Type="Int32" DefaultValue="0" />
	<asp:ControlParameter Name="sort" Type="Int32" DefaultValue="0" ControlID="_sortBox" />
	<asp:Parameter Name="startIndex" Type="Int32" />
	<asp:Parameter Name="size" Type="Int32" />
</UpdateParameters>
</asp:ObjectDataSource>

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

$(function() { 
	RefreshCheckBoxes(); 
});

</script>

<div class="tab-text">
    <div class="t-hold">
        <cc1:ExtendedListView ID="_listView" runat="server" DataSourceID="_objectDataSource" 
	        ondatabinding="_listView_DataBinding" onitemcommand="_listView_ItemCommand" 
            onitemupdated="_listView_ItemUpdated" 
            onitemdatabound="_listView_ItemDataBound">
	        <LayoutTemplate>
		        <table>
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
			        <asp:PlaceHolder ID="PlaceHolder1" runat="server" 
				        Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				        <th><asp:Literal ID="lLineStatus" runat="server" Text="Статус строк" 
						        meta:resourcekey="lLineStatus1" /></th>
				        <th><asp:Literal ID="lOrderStatus" runat="server" Text="Статус заказа" 
						        meta:resourcekey="lOrderStatus1" />
				        <th><asp:Literal ID="lPrint" runat="server" Text="Печать" 
						        meta:resourcekey="lPrintResource1" /></th>
			        </asp:PlaceHolder>
			        <asp:PlaceHolder ID="PlaceHolder2" runat="server" 
				        Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ArchiveOrders %>'>
				        <th><asp:Literal ID="lEndDate" runat="server" Text="Дата завершения" 
						        meta:resourcekey="lEndDateResource1" /></th>
			        </asp:PlaceHolder>
		        </tr>
		        <tr runat="server" id="itemPlaceHolder" />
		        </table>			

	        </LayoutTemplate>
	        <ItemTemplate>
		        <tr runat="server" id="tr1">
			        <td id="Td1" style="width:20px" runat="server"><input type="checkbox" name="order_id" 
					        value='<%# Eval("OrderID") %>' onclick="RefreshCheckBoxes()" /></td>
			        <td id="Td2" runat="server">
				        <asp:HyperLink ID="_orderLink" runat="server" 
					        NavigateUrl='<%# GetOrderDetailsUrl((int)Eval("OrderID")) %>'
					        Text='<%#  "Order #" + GetOrderDisplayNumber( (RmsAuto.Store.Entities.Order)Container.DataItem ) %>' />
			        </td>
			        <td id="Td3" runat="server">
			            <%# Eval("CustOrderNum") %>
                    </td>
			        <td id="Td4" runat="server">
				        <%# Eval("OrderDate", "{0:dd.MM.yyyy}" ) %>
			        </td>
			        <td id="Td5" class="price" runat="server"><nobr>
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
						        ToolTip="Распечатать квитанцию для оплаты в Сбербанке РФ"> <%# Eval("Total","{0:### ### ##0.00}") %> </asp:HyperLink>
				        </asp:PlaceHolder>
				        </nobr>
			        </td>
			        <td id="Td6" runat="server">
				        <asp:HyperLink runat="server" ID="HyperLink2" 
					        Target="_blank"
					        NavigateUrl='<%# GetPaymentOrderPrintUrl((int)Eval("OrderID")) %>'
					        Visible='<%# ShowPaymentOrderLink %>'
					         ToolTip="Распечатать квитанцию для оплаты в Сбербанке РФ"><asp:Image ID="Image1" runat="server" ImageUrl="~/images/s-blank.gif" ImageAlign="AbsBottom" />
        </asp:HyperLink>
			        </td>
			        <td ID="td7" runat="server" 
				        Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				
				        <asp:DropDownList DataSourceID="AllStatuses" DataTextField="Client" 
                            DataValueField="OrderLineStatusID" style="width: 60%;" 
                            runat="server"  ID="_StatusBoxList"      />
				        <asp:Button CommandName="Update" class="button" runat="server" ID="ImageButton2" Text="Update" />
				        <%--  SelectedValue='<%# Bind("Status") %>'--%> 
				        <td><%# ((OrderStatus)Eval("Status")).ToTextOrName() %></td>
			        </td>
			        <td ID="td8" runat="server" 
				        Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				        <asp:HyperLink runat="server" ID="HyperLink3" 
					        Target="_blank"
					        NavigateUrl='<%# "~/Cabinet/Orders/OrderPrint.aspx?OrderID=" + Eval("OrderID").ToString() %>'
					        ToolTip="Print order"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/print.png" ImageAlign="AbsBottom" />
        </asp:HyperLink>
			        </td>
			        <td ID="td9" runat="server" 
				        Visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ArchiveOrders %>'>
				        <%# Eval( "CompletedDate", "{0:dd.MM.yyyy}" ) %>
			        </td>
		        </tr>
	        </ItemTemplate>
	        <EmptyDataTemplate>
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
		                            ShowNextPageButton="False" ShowPreviousPageButton="False" 
		                            PreviousPageText="" ButtonCssClass="prev" />
	                            <asp:NumericPagerField />
	                            <asp:NextPreviousPagerField ShowLastPageButton="False" 
		                            ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText="" ButtonCssClass="next" />
                            </Fields>
                        </asp:DataPager>
                    </li>
                    <li runat="server" id="_sortBlock">
	                    <%= global::Resources.Texts.OrderBy%>
	                    <asp:DropDownList runat="server" ID="_sortBox" AutoPostBack="True" 
		                    OnSelectedIndexChanged="_sortBox_SelectedIndexChanged" />
                    </li>
                    <li>
                        <asp:Literal ID="lCountView" runat="server" Text="<%$ Resources:Texts, DisplayOn %>" />
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
        </div>
        <div class="right">
            <asp:PlaceHolder runat="server" ID="_footerBlock">
                <div class="right_block2" id="_activeOrderTotalsBlock" runat="server" visible="false">
	            <br />
	            <b><asp:Literal ID="lOrderOn" runat="server" Text="Заказов на" 
			            meta:resourcekey="lOrderOnResource1" /></b> <asp:Label runat="server" 
			            ID="_processingOrdersTotalLabel" CssClass="price" /> <%=global::Resources.Texts.RoubleShort%>
	            <br />
	            <b><asp:Literal ID="lWorkDetails" runat="server" Text="Позиций в работе на" 
			            meta:resourcekey="lWorkDetailsResource1" /></b> <asp:Label runat="server" 
			            ID="_processingLinesTotalLabel" CssClass="price" /> <%=global::Resources.Texts.RoubleShort%>
	            </div>

	            <div class="right_block2" id="_archiveOrderTotalsBlock" runat="server" visible="false">
	            <br />
	            <b><asp:Literal ID="lAllSumm" runat="server" Text="Общая сумма заказов" 
			            meta:resourcekey="lAllSummResource1" /></b> <asp:Label runat="server" 
			            ID="_completedOrdersTotalLabel" CssClass="price" /> <%=global::Resources.Texts.RoubleShort%>
	            <br />
	            <b><asp:Literal ID="lSaleOn" runat="server" Text="Выдано на" 
			            meta:resourcekey="lSaleOnResource1" /></b> <asp:Label runat="server" 
			            ID="_receivedByClientSumLabel" CssClass="price" /> <%=global::Resources.Texts.RoubleShort%>
	            </div>
            </asp:PlaceHolder>
        </div>
    </div>
</div>

<asp:Button runat="server" ID="_showOrdersButton" CssClass="button" Text="View order" onclick="_showOrdersButton_Click" 
			meta:resourcekey="_showOrdersButtonResource1" />

<asp:ObjectDataSource ID="AllStatuses" runat="server" 
        TypeName="RmsAuto.Store.Web.Controls.OrderListLite" SelectMethod="GetStatuses" >
    
</asp:ObjectDataSource>