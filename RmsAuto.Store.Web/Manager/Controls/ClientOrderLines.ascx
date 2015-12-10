<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientOrderLines.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.ClientOrderLines" EnableViewState="true" %>
<%@ Register src="~/Controls/OrderLineTemplateToLoad.ascx" tagname="OrderLineTemplate" tagprefix="uc1" %>

<script type="text/javascript">

	function ToggleAllLines() {
		var checkAllOrders = $('input[name=check_all_lines]');
		var orderCheckBoxes = $('input[name=orderline_id]');
		var checked = checkAllOrders.get(0).checked;

		$.each(orderCheckBoxes, function() { this.checked = checked; });
    }

	function GatherStatuses(sender) {

	    sender.enabled = false;
		$("input[id*='hfOrderlinesIDS']").val("");
		strOrderlinesIDS = "";
		var orderCheckBoxes = $('input[name=orderline_id]');
		orderCheckBoxes.each(function() {
			if (this.checked) {
				strOrderlinesIDS = strOrderlinesIDS + this.value + ";";
			}
		});

		$("input[id*='hfOrderlinesIDS']").val(strOrderlinesIDS);
		if (strOrderlinesIDS.length > 0)
		{
		    if (sender.id.indexOf('lbPackingInvoice') > 0) {
		        Packing = true;
		    }

		    if (sender.id.indexOf('lbCommercialInvoice') > 0) {
		        Invoice = true;
		    }


            if (Packing && Invoice)
		        $('#lbShip').css('visibility', 'visible');

		    
		    if (sender.id == 'lbShip' /*Если отгружаем товар*/)
		          { return  CheckOrder(strOrderlinesIDS); } 
		       
		    
		    return true;
		    
		  }
		else
		    { alert('Nothing is selected'); return false; }
	}

	function RefreshCheckBoxes() {
		var checkAllOrders = $('input[name=check_all_lines]');
		var orderCheckBoxes = $('input[name=orderline_id]');

		var isAllChecked = true;
		$.each(orderCheckBoxes, function() { if (!this.checked) isAllChecked = false; });

		if (checkAllOrders.length != 0) checkAllOrders.get(0).checked = isAllChecked;
}
	

	$(function() {
	    RefreshCheckBoxes();
	    Invoice = false;
	    Packing = false;
	    $('#filter').accordion({ collapsible: true });
    });


function CheckOrder(strOrderlinesIDS) {

    $.ajax({
    data: { OrderLinesIDs: strOrderlinesIDS },
    url: 'BeforeOrderlinesShipment.ashx',
        success: function(data) {

        proccessOrder(data, strOrderlinesIDS);
        }
    });
  }

  proccessOrder = function(data, OrderLinesIDs) {

  if (data == 'Ok') {
            ClientId = $("input[id*='hfClientId']").val()

            $.ajax({
                data: { OrderLinesIDs: OrderLinesIDs, CurrentUserId: ClientId },
                url: 'ProcessShipment.ashx',
                success: function (data) {
                    alert(data);
                    window.location = 'ClientOrdelinesLoad.aspx';
                
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
                        "Do not dispatch": function () {
                            $(this).dialog("close");

                            //TODO: дописать
                            return false;
                        },

                        "Dispatch": function () {
                                
                            ClientId = $("input[id*='hfClientId']").val() 

                            $.ajax({
                            data: { OrderLinesIDs: OrderLinesIDs, CurrentUserId: ClientId },
                                url: 'ProcessShipment.ashx',
                                success: function(data) {
                                    alert(data);
                                    window.location = 'ClientOrdelinesLoad.aspx';
                                }
                            });

                            $(this).dialog("close");
                            return false;
                        }
                    },
                    open: function() {
                        $('.ui-dialog-buttonset').find('button:contains("Не отгружать")').addClass('cancelButtonClass');
                        $('.ui-dialog-buttonset').find('button:contains("Отгрузить")').addClass('okButtonClass');
                    }
                });
            });

            //return confirm("Отправить в работу? " + firstRuleText + "\n" + secondRuleText + "\n" + firdRuleText + ".\nСумма заказов в работе: " + ActiveOrdersSum + " р." +
            //     "\nБаланс клиента: " + (-1 * ClientSaldo) + " р." +
            //"\nСумма заказа: " + Total + " р.");
        }
}
	
</script>

<input runat="server" id="hfOrderlinesIDS" type="hidden" value=""  />
<input runat="server" enableviewstate="true" id="hfClientId" type="hidden" value="" />
<input runat="server"  enableviewstate="true" id="hfClientName" type="hidden" value="" />
<input runat="server"  enableviewstate="true" id="hfVaryParam" type="hidden" value="" />
<div style="width:20%;float:left; height: 400px; min-width: 300px;">
		<asp:TextBox id="_txtClientName" runat="server" Width="194px" EnableViewState="true"></asp:TextBox>&nbsp;
		<asp:Button ID="_btnSearchClient" CssClass="button" runat="server" Text="Search" onclick="_btnSearchClient_Click" />
	
	<br />
	<br />
	<asp:ListBox  ID="_listSearchResults" runat="server" 
		onselectedindexchanged="_listSearchResults_SelectedIndexChanged" 
		 DataTextField="ClientName" AutoPostBack="True" EnableViewState="true"
		CausesValidation="True" DataValueField="ClientID" Height="80%" Width="259px">
	</asp:ListBox>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
</div>

<asp:ObjectDataSource runat="server" ID="_objectDataSource" EnablePaging="True" 
    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="size"  
    TypeName="RmsAuto.Store.Web.Manager.Controls.ClientOrderLines" 
    SelectCountMethod="GetOrderLinesCount" SelectMethod="GetOrderLines" onselected="_objectDataSource_Selected">
    <SelectParameters>
        <asp:ControlParameter Name="ClientId" PropertyName="value" ControlID="hfClientId" Type="String" DefaultValue="0" />
        <asp:ControlParameter Name="VaryParam" PropertyName="value" ControlID="hfVaryParam" Type="String" DefaultValue="0" />
    </SelectParameters>
</asp:ObjectDataSource>

<div style="width:70%; float:left; min-width: 500px;">
<h1 runat="server" id="ClientName"><%= CurrentClientName %></h1>
<div class="tab-text">
    <div class="t-hold">
<table class="table-big">
	<tbody>
	<tr >
	    <th><asp:Literal ID="Literal1" runat="server" Text="" /><input type="checkbox" name="check_all_lines" onclick="ToggleAllLines()" /></th>
		<th><asp:Literal ID="lOrderNumber" runat="server" Text="Order number" meta:resourcekey="lOrderNumberResource1" /></th>
		<th><asp:Literal ID="lOrderDate" runat="server" Text="Order date" meta:resourcekey="lOrderDateResource1" /></th>
		<th><asp:Literal ID="lOrderCode" runat="server" Text="Order code" meta:resourcekey="lOrderCodeResource1" /></th>
		<th><asp:Literal ID="lManufact" runat="server" Text="Brand"	meta:resourcekey="lManufactResource1" /></th>
		<th><asp:Literal ID="lDetailNumber" runat="server" Text="Part number" meta:resourcekey="lDetailNumberResource1" /></th>
		<th><asp:Literal ID="lName" runat="server" Text="Description" meta:resourcekey="lNameResource1" /></th>
		<th nowrap><asp:Literal ID="lQty" runat="server" Text="Q-ty" meta:resourcekey="lQtyResource1" /></th>
		<th><asp:Literal ID="lPrice" runat="server" Text="Price" meta:resourcekey="lPriceResource1" /></th>
		<th><asp:Literal ID="lSumm" runat="server" Text="Total" meta:resourcekey="lSummResource1" /></th>
		<th><asp:Literal ID="lDateOnStock" runat="server" Text="Date of arrival to franchisee's stock" meta:resourcekey="lDateOnStockResource1" /><span style="color:red">*</span></th>
		<th style="margin-left: 40px"><asp:Literal ID="lStatus" runat="server" Text="Status" meta:resourcekey="lStatusResource1" /></th>
		<%--<th></th>
		<th></th>
		<th></th>--%>
	</tr>
	</tbody>
	
<%--	<tbody runat="server" id="_filters2Block"  class="filters2">
	<tr>
	 <td></td>
	 <td colspan="15"><div style="float:right;"></div>
        </td>
	</tr>
	</tbody>--%>
	<input type="hidden" runat="server" value="" id = "SavedStatuses" />
	<asp:ListView runat="server" ID="_listView" DataSourceID="_objectDataSource" 
			      ondatabound="_listView_DataBound" ondatabinding="_listView_DataBinding" >
        
	<LayoutTemplate>
		<tbody class="list2 list3" id="linesTable" >
		    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
		</tbody>
	</LayoutTemplate>
	<ItemTemplate>
		<asp:PlaceHolder runat="server" ID="_placeHolder">
			<uc1:OrderLineTemplate ID="OrderLineTemplate1" runat="server" />
		</asp:PlaceHolder>					
	</ItemTemplate>
	<EmptyDataTemplate>
		<tr><td colspan="14"><asp:Literal ID="lEmptyRowList" runat="server" 
						Text="List of lines is empty" meta:resourcekey="lEmptyRowListResource1" /></td></tr>
		
	</EmptyDataTemplate>
</asp:ListView>

</table>
</div>
    <div class="bottom">
        <div class="pages">
            <ul>
                <li>
                    <asp:DataPager ID="_dataPager" runat="server" PagedControlID="_listView">
		                <Fields>
			                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="False" PreviousPageText="" ButtonCssClass="prev" />
			                <asp:NumericPagerField />
			                <asp:NextPreviousPagerField ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText="" ButtonCssClass="next"/>
		                </Fields>
	                </asp:DataPager>
                </li>
                <li runat="server" id="_totalsBlock">
                    <asp:Literal 
			            ID="lCountView" runat="server" Text="Display by:" 
			            meta:resourcekey="lCountViewResource1" />
		            <asp:DropDownList runat="server" 
			            ID="_pageSizeBox" AutoPostBack="True" 
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
	

	<div class="right_block2">
	<%--<b><asp:Literal ID="lSummOnPage" runat="server" Text="Итого на странице" 
			meta:resourcekey="lSummOnPageResource1" /></b> <asp:Label runat="server" ID="_pageTotalLabel" 
			CssClass="price" /><%=global::Resources.Texts.RoubleShort%> <br />
	<b><asp:Literal ID="lAllSumm" runat="server" Text="Итого" 
			meta:resourcekey="lAllSummResource1" /></b> <asp:Label runat="server" 
			ID="_totalLabel" CssClass="price" /><%=global::Resources.Texts.RoubleShort%> </div><div runat="server" id="_pagerSettingsBlock">--%>
		
	</div>
	<a href='#' style="visibility:hidden" id="lbShip" class="btn btn-success" onclick="return GatherStatuses(this);">Dispatch</a>
    <div id="filter" style="margin-top:10px;">
        <h3>Commercial Invoice</h3>
        <div>
            <table style="width:auto;">
                <tr>
                    <td>Description of goods: </td>
                    <td><asp:TextBox ID="txtDescrOfGoods1" runat="server" Width="180px" /></td>
                </tr>
                <tr>
                    <td>Country of origin: </td>
                    <td><asp:TextBox ID="txtCountryOfOrigin1" runat="server" Width="180px" /></td>
                </tr>
            </table>
            <asp:LinkButton ID="lbCommercialInvoice" runat="server" class="btn btn-success" OnClientClick="return GatherStatuses(this);" onclick="lbCommercialInvoice_Click">Commercial Invoice</asp:LinkButton>
        </div>
        <h3>Invoice Cum Packing List</h3>
        <div>
            <table style="width:auto;">
                <tr>
                    <td>Description of goods: </td>
                    <td><asp:TextBox ID="txtDescrOfGoods2" runat="server" Width="180px" /></td>
                    <td>&nbsp;&nbsp;&nbsp;Total Pkgs</td>
                    <td><asp:TextBox ID="txtTotalPkgs" runat="server" Width="80px" /></td>
                </tr>
                <tr>
                    <td>Country of origin: </td>
                    <td><asp:TextBox ID="txtCountryOfOrigin2" runat="server" Width="180px" /></td>
                    <td>&nbsp;&nbsp;&nbsp;Total weight, kg</td>
                    <td><asp:TextBox ID="txtTotalWeight" runat="server" Width="80px" /></td>
                </tr>
            </table>
            <asp:LinkButton runat="server" ID="lbPackingInvoice" class="btn btn-success" OnClientClick="return GatherStatuses(this);" onclick="lbPackingInvoice_Click">Invoice Cum Packing List</asp:LinkButton>
        </div>
    </div>
</div>
</div>

<div id="dialog-confirm" style="display:none" title="Attention">
   <div id="ConfirmText">Если вы видите этот текст, то нужно скрыть див для окна</div>
</div>


