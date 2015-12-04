<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllOrders.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.AllOrders" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<%@ Import Namespace="RmsAuto.Store.BL" %>
 
<%@ Register assembly="RmsAuto.Common" namespace="RmsAuto.Common.Web.UI" tagprefix="cc1" %>

<asp:ObjectDataSource runat="server" ID="_objectDataSource" EnablePaging="True" 
StartRowIndexParameterName="startIndex" MaximumRowsParameterName="size"  
TypeName="RmsAuto.Store.Web.Controls.AllOrders" 
SelectCountMethod="GetOrdersCount" SelectMethod="GetOrders" UpdateMethod="UpdateOrders" onselected="_objectDataSource_Selected" >
<SelectParameters>
    <asp:ControlParameter Name="ClientName" Type="String" DefaultValue=""  ControlID="_nameBox"  />
    <asp:ControlParameter Name="lowDate" Type="DateTime" DefaultValue="01.01.2011" ControlID="lowDate"  />
    <asp:ControlParameter Name="hiDate" Type="DateTime" ControlID="hiDate"  />    
	<asp:ControlParameter Name="statusFilter" ControlID="statusFilter" Type="Int32" DefaultValue="0" />
	<asp:ControlParameter Name="sort" Type="Int32" DefaultValue="0" ControlID="_sortBox" />
	<asp:ControlParameter Name="AcctgOrderLineId" Type="Int32" DefaultValue="0" ControlID="AcctgOrderLineId" />
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

    function ToggleAllOrders() {
        var checkAllOrders = $('input[name=check_all_orders]');
        var orderCheckBoxes = $('input[name=order_id]');
        var checked = checkAllOrders.get(0).checked;
        $.each(orderCheckBoxes, function() { this.checked = checked; });
    }


    function RefreshCheckBoxes() {
        var checkAllOrders = $('input[name=check_all_orders]');
        var orderCheckBoxes = $('input[name=order_id]');

        var isAllChecked = true;
        $.each(orderCheckBoxes, function() { if (!this.checked) isAllChecked = false; });

        if (checkAllOrders.length != 0) checkAllOrders.get(0).checked = isAllChecked;
        return checkAllOrders.length;
    }

    $(function() {
        RefreshCheckBoxes();
    });


    fordersBootstrap = function() {
        
            var orderCheckBoxes = $('input[name=order_id]');
            var IDs = '';
            orderCheckBoxes.each(function() {
                if (this.checked)
                    IDs = IDs + this.value + ';';
            });
        
        if (IDs.length > 0)
        {
            this.href = 'OrderLinesBootStrap.ashx?IDs=' + IDs;
            location.href = 'OrderLinesBootStrap.ashx?IDs=' + IDs;
        }
        else 
        {
            alert("Nothing is selected!");
        }

        return false;
    }

    function DeleteOrder(OrderId) {
        if (confirm("Do you want to delete your order?")) {
            $.ajax({
                data: { OrderId: OrderId },
                url: 'DeleteOrder.ashx',
                success: function() {
                    location.reload();
                }
            });
        }
    }
    
    //Сумма активных заказазов, Сальдо клиента, Сумма заказа, % предоплаты, Сумма заказов до даты минус дата отсрочки, Сальдо до дат отсрочки, лимит по заказам
    function CheckOrder(OrderId) {

             $.ajax({
                 data: { OrderId: OrderId },
                 url: 'BeforeSendCheckOrder.ashx',
                 success: function(data) {
                     ServerCheckOrder = data;
                     proccessOrder(ServerCheckOrder);
                 }
             });

             proccessOrder = function(data) {

                 if (ServerCheckOrder == 'Ok') {
                     $.ajax({
                         data: { OrderId: OrderId },
                         url: 'SendXmlTo1C.ashx',
                         success: function() {
                             location.reload();
                         }
                     });
                     return true;
                 }

                 else {
                     $('#ConfirmText').html(data);

                     $(function() {
                         $("#dialog-confirm").dialog({
                             resizable: false,
                             height: 240,
                             modal: true,
                             buttons: {
                                 "Do not send": function() {
                                     $(this).dialog("close");

                                     //TODO: дописать
                                     return false;
                                 },

                                 "Send": function() {

                                     $.ajax({
                                         data: { OrderId: OrderId },
                                         url: 'SendXmlTo1C.ashx',
                                         success: function() {
                                             location.reload();
                                         }
                                     });

                                     $(this).dialog("close");
                                     return false;
                                 }
                             },
                             open: function() {
                                 $('.ui-dialog-buttonset').find('button:contains("Не отправлять")').addClass('cancelButtonClass');
                                 $('.ui-dialog-buttonset').find('button:contains("Отправить")').addClass('okButtonClass');
                             }
                         });
                     });

                     //return confirm("Отправить в работу? " + firstRuleText + "\n" + secondRuleText + "\n" + firdRuleText + ".\nСумма заказов в работе: " + ActiveOrdersSum + " р." +
                     //     "\nБаланс клиента: " + (-1 * ClientSaldo) + " р." +
                     //"\nСумма заказа: " + Total + " р.");
                 }
             }
    }

    $(function() {
        $('.date').datepicker();
        $("#filter").accordion({ collapsible: true });
    });
</script>

<div id="dialog-confirm" style="display:none" title="Attention">
   <div id="ConfirmText">Если вы видите этот текст, то нужно скрыть див для окна</div>
</div>


<div id="filter">
	<h3> <a href="#"><asp:Literal ID="lFilter" runat="server" Text="Filter:" 
			meta:resourcekey="lFilterResource1"></asp:Literal></a></h3>
    <div>
        <table style="width:auto;">
            <tr>
                <td><asp:Literal runat="server" ID="lCustomersName" Text="Customer's name" meta:resourcekey="lCustomersName"></asp:Literal></td>
                <td><asp:TextBox runat="server" ID="_nameBox" AutoPostBack="True" OnSelectedIndexChanged="_sortBox_SelectedIndexChanged" Width="120" /></td>
                <td><%= global::Resources.Texts.OrderStatus %></td>
                <td>
                    <asp:DropDownList runat="server" ID="statusFilter" Width="128">
                        <asp:ListItem Value="0" Text="<%$ Resources:Texts, All %>"></asp:ListItem>
                        <asp:ListItem Value="1" Text="<%$ Resources:Texts, New %>"></asp:ListItem>
                        <asp:ListItem Value="2" Text="<%$ Resources:Texts, InProcess %>"></asp:ListItem>
                        <asp:ListItem Value="3" Text="<%$ Resources:Texts, Archive %>"></asp:ListItem>
                        <asp:ListItem Value="4" Text="<%$ Resources:Texts, Active %>"></asp:ListItem>
	                </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td><%= global::Resources.Texts.BeginPeriod %></td>
                <td><asp:TextBox CssClass="date" runat="server" ID="lowDate" AutoPostBack="True" Width="120" /></td>
                <td><%= global::Resources.Texts.EndPeriod %></td>
                <td><asp:TextBox CssClass="date" runat="server" ID="hiDate" AutoPostBack="True" Width="120" /></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="3"><asp:Button ID="_btnSearchClient" runat="server" Text="<%$ Resources:Texts, Search %>" CssClass="button" Width="54px" onclick="_btnSearchClient_Click" /></td>
            </tr>
        </table>
    </div>
</div>


<!---&nbsp&nbsp&nbsp&nbsp<asp:Label runat="server">Номер&nbspстроки&nbspзаказа&nbsp</asp:Label>&nbsp<asp:TextBox runat="server" ID="AcctgOrderLineId" AutoPostBack="True"/>-->
       

<div class="tab-text">
    <div class="t-hold">
        <cc1:ExtendedListView ID="_listView" runat="server" DataSourceID="_objectDataSource" 
	        ondatabinding="_listView_DataBinding" 
            onitemupdated="_listView_ItemUpdated" 
            onitemdatabound="_listView_ItemDataBound">
	        <LayoutTemplate>
		        <table class="table-big">
		        <tr>
			        <th style="width:20px"><input type="checkbox" name="check_all_orders" onclick="ToggleAllOrders()" /></th>
                    <th><asp:Literal ID="Literal3" runat="server" meta:resourcekey="lSendDelete" Text="Send/Delete"/></th>
			        <th><asp:Literal ID="Literal1" runat="server" meta:resourcekey="lCustomersName" Text="Customer's Name"/></th>
			        <th><asp:Literal ID="Literal2" runat="server" meta:resourcekey="lCustomersNumber" Text="Customer's Number"  /></th>
			        <th><asp:Literal ID="lOrderNumber" runat="server" Text="Order's Number" meta:resourcekey="lOrderNumber" /></th>
			        <th><asp:Literal ID="lClientOrderNumber" runat="server" Text="Customer order number" meta:resourcekey="lClientOrderNumber" /></th>
			        <th><asp:Literal ID="lSendDate" runat="server" Text="Date of placement" meta:resourcekey="lSendDate" /></th>
			        <th style="width:10%" colspan="2"><asp:Literal ID="lSumm" runat="server" Text="Total" meta:resourcekey="lSumm" /></th>
			        <asp:PlaceHolder ID="PlaceHolder1" runat="server" visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				        <th><asp:Literal ID="Literal4" runat="server" meta:resourcekey="lOrderStatus" Text="Order status"/></th>
				        <th><asp:Literal ID="lPrint" runat="server" Text="Print" meta:resourcekey="lPrint" /></th>
			        </asp:PlaceHolder>
			        <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Convert.ToBoolean(ShowPaymentOrderLink) %>'>
				        <th><asp:Literal ID="lEndDate" runat="server" Text="Date of complete" meta:resourcekey="lEndDate" /></th>
			        </asp:PlaceHolder>
		        </tr>
		        <tr>
			        <td colspan="6" class="empty" style=""><img id="Img1" runat="server" src="~/images/1pix.gif" width="1" height="3" border="0"/> </td>
		        </tr>
		        <tr runat="server" id="itemPlaceHolder" />
		        </table>			

	        </LayoutTemplate>
	        <ItemTemplate>
		        <tr runat="server" id="tr1">
			        <td id="Td1" style="width:20px" runat="server"><input type="checkbox" name="order_id" value='<%# Eval("OrderID") %>' onclick="RefreshCheckBoxes()" /></td>
                    <td class="nowrap" style="text-align:center" id="tdxml" runat="server" visible='<%# OrderStatusFilter==RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				        <input type="hidden" value='<%# Eval("Total")  %>' />
				        <input type="hidden" value='<%# Eval("ActiveOrdersSum")  %>' />
				        <input type="hidden" value='<%# Eval("ClientSaldo")  %>' />
				        <input type="hidden" value='<%# Eval("PrepaymentPercent")  %>' />
                        <img alt="Send" title="Send" style='cursor:pointer; <%# LightBO.GetOrderNoXmlSign((int)Eval("OrderID")) ? "" : "display:none" %>' id="ibFormXML" src="/images/play.png" onclick='<%#String.Format("javascript:return CheckOrder({0});", Eval("OrderID")) %>' />&nbsp;				  
                        <img alt="Delete" title="Delete" style='cursor:pointer; <%# LightBO.GetOrderNoXmlSign((int)Eval("OrderID")) ? "" : "display:none" %>' id="ibDeleteOrder" src="/images/trash.png" onclick='<%#String.Format("javascript:return DeleteOrder({0});", Eval("OrderID")) %>' />				  
			        </td>
		            <td id="Td10" runat="server">
			            <%# Eval("ClientName") %>
                    </td>
                    <td id="Td11" runat="server">
			            <%# Eval("ClientID") %>
                    </td>
			        <td id="Td2" runat="server" class="nowrap">
				        <asp:HyperLink ID="_orderLink" runat="server" 
					        NavigateUrl='<%# GetOrderDetailsUrl((int)Eval("OrderID")) %>'
					        Text='<%#  "Order #" + GetOrderDisplayNumber((RmsAuto.Store.Entities.Order)Container.DataItem ) %>' />
			        </td>
			        <td id="Td3" runat="server">
			            <%# Eval("CustOrderNum") %>
                    </td>
			        <td id="Td4" runat="server">
				        <%# ((DateTime)Eval("OrderDate")).ToShortDateString() %>
			        </td>
			        <td id="Td5" class="price right_align" runat="server"><nobr>
				        <asp:PlaceHolder runat="server" ID="PlaceHolder1" 
					        Visible='<%# !Convert.ToBoolean(ShowPaymentOrderLink) %>'>
					        <%# Eval("Total","{0:### ### ##0.00}") %>
				        </asp:PlaceHolder>
				        <asp:PlaceHolder runat="server" ID="PlaceHolder2" 
					        Visible='<%# Convert.ToBoolean(ShowPaymentOrderLink) %>'>
					        <asp:HyperLink runat="server" ID="HyperLink1" 
						        Target="_blank"
						        NavigateUrl='<%= GetPaymentOrderPrintUrl((int)Eval("OrderID")) %>'
						        Visible='<%# Convert.ToBoolean(ShowPaymentOrderLink) %>' 
						        ToolTip="Распечатать квитанцию для оплаты в Сбербанке РФ"> <%# Eval("Total","{0:### ### ##0.00}") %> </asp:HyperLink>
				        </asp:PlaceHolder>
				        </nobr>
			        </td>

			        <td ID="td7" runat="server" Visible='<%# OrderStatusFilter == RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				        <%--<asp:DropDownList DataSourceID="AllStatuses" DataTextField="Client" 
                            DataValueField="OrderLineStatusID" style="width: 60%;" 
                            runat="server"  ID="_StatusBoxList"      />--%>
				        <%--<asp:Button CommandName="Update" class="bluebutton" runat="server" ID="ImageButton2" Text="Обновить" />--%>
				        <%--  SelectedValue='<%# Bind("Status") %>'--%> 
				        <td><%# ((OrderStatus)Eval("Status")).ToTextOrName() %></td>
			        </td>
			
			        <td ID="td8" runat="server" Visible='<%# OrderStatusFilter == RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ActiveOrders %>'>
				        <asp:HyperLink runat="server" ID="HyperLink3" 
					        Target="_blank" Width="30"  Height="30"
					        NavigateUrl='<%# "~/Cabinet/Orders/OrderPrint.aspx?OrderID=" + Eval("OrderID").ToString() %>'
					        ToolTip="Print order"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/print.png" ImageAlign="AbsBottom" />
				        </asp:HyperLink>
			        </td>
			
			  
			
			        <td ID="td9" runat="server" 
				        Visible='<%# OrderStatusFilter == RmsAuto.Store.Web.OrderTracking.OrderStatusFilter.ArchiveOrders %>'>
				        <%# Eval("CompletedDate") != null ? ((DateTime)Eval("CompletedDate")).ToShortDateString() : "" %>
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
            <div runat="server" id="_totalsBlock" class="right_block2">
                <%= global::Resources.Texts.Total%> 
                <asp:Label runat="server" 
		            ID="_totalLabel" CssClass="price" 
		             /> <%= global::Resources.Texts.RussianRouble%>
            </div>
        </div>
    </div>
</div>

<asp:PlaceHolder runat="server" ID="_footerBlock">
	<div class="right_block2" id="_archiveOrderTotalsBlock" runat="server" visible="false">
	<br />
	<b><asp:Literal ID="lAllSumm" runat="server" Text="Total orders" 
			meta:resourcekey="lAllSummResource1" /></b> <asp:Label runat="server" 
			ID="_completedOrdersTotalLabel" CssClass="price" /> <%=global::Resources.Texts.RoubleShort%>
	<br />
	<b><asp:Literal ID="lSaleOn" runat="server" Text="Issued on" 
			meta:resourcekey="lSaleOnResource1" /></b> <asp:Label runat="server" 
			ID="_receivedByClientSumLabel" CssClass="price" /> <%=global::Resources.Texts.DollarShort%>
	</div>
	<div>
		<asp:Button runat="server" ID="_showOrdersButton" CssClass="button" onclick="_showOrdersButton_Click" Text="Show Orders" />&nbsp
		<a runat="server" ID="_ordersBootstrap" onclick="return fordersBootstrap();" class="button" href="#">Download orders lines</a>
	</div>
	
</asp:PlaceHolder>


<asp:ObjectDataSource ID="AllStatuses" runat="server" 
        TypeName="RmsAuto.Store.Web.Controls.OrderListLite" SelectMethod="GetStatuses" >
</asp:ObjectDataSource>