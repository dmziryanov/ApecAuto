<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLinesWholesaleLite.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderLinesWholesaleLite" %>
<%@ Register src="OrderLineTemplateLite.ascx" tagname="OrderLineTemplateLite" tagprefix="uc1" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>

<script type="text/javascript">
    $(document).ready(function() {


        l = $('input[id*="UpdateLineButton"]').length;
        if (l == 0) {
            $('#<%=ClientID %>_UpdateAllStatusesBtn').attr('disabled', 'disabled');
            $('#<%=ClientID %>_UpdateAllStatusesBtn').removeClass('bluebutton');
            $('#<%=ClientID %>_UpdateAllStatusesBtn').addClass('disabledbutton');
            $('#<%=ClientID %>_UpdateAllStatusesBtn').attr('tooltip', 'Нет статусов для обновления')
        }

        UpdateStatuses = function() {
            $("input[id*='SavedStatuses']").val('');
            var strSavedStatuses = '';

            $('#linesTable tr').each(function(i, l) {
                //i -- индекс; l -- объект строка

                var tmp = l.cells[0].children[1].value; //Статус в сктрытом поле (передается с сервера)
                var id = l.cells[11].children[0].value; //Статус в комбобоксе
                
                if (tmp != id) {
                    var OrderLineID = l.cells[0].children[0].value;
                    var newStatus = l.cells[11].children[0].value;
                    strSavedStatuses = strSavedStatuses + OrderLineID + ',' + newStatus + ';';
                }
            });
            $("input[id*='SavedStatuses']").val(strSavedStatuses);
            location.reload();
        }


        $('#linesTable tr').each(function(i, l) {
            //i -- индекс; l -- объект строка

            var tmp = l.cells[0].children[1].value; //Статус в сктрытом поле (передается с сервера)
            var id = l.cells[11].children[0].value; //Статус в комбобоксе
            l.cells[11].children[0].enabled = (tmp != 240)

            if (tmp != id && tmp != 240 && id != undefined) {
                $(l).children().addClass('active');
            }
            else {
                $(l).children().removeClass('active');
            }
        });

        var previous;

        $("select[id*='_StatusBoxList']").focus(function() {
            // Store the current value on focus and on change
            previous = $(this).val();
        }).change(function() {
            //Выбираем хидден с ид статуса
            var tmp = $(this).parent('td').parent('tr')[0].cells[0];

            var id = tmp.children[1].value; // Значение id храняещееся в скрытом поле
            var ordln = tmp.children[0].value;
            var s = $(this);
            $.get('CheckOrderLineStatus.ashx', { OrderLineID: ordln, newStatus: id }, function(data) {
                if (data == 'False') {
                    s.val(previous);
                    s.parent('td').parent('tr').children().removeClass('active');
                    alert('Данное изменение статуса некорректно!');
                    return false;
                }
            });

            if (id != this.value) {
                $(this).parent('td').parent('tr').children().addClass('active');
            }
            else {
                $(this).parent('td').parent('tr').children().removeClass('active');

            } //Правда жесть?

        });

        function colorLine()
        { }
    });
</script>

<div class="orders">
    <div class="tab-text">
        <div class="t-hold">
            <div class="br"></div>
            <div class="bl"></div>
            <table class="table-big">
	            <tbody class="list2 list3">
	            <tr >
		            <th style="width:0px"></th>
		            <th><asp:Literal ID="lOrderNumber" runat="server" Text="Order number" 
				            meta:resourcekey="lOrderNumberResource1" /></th>
		            <asp:PlaceHolder runat="server" ID="_headerCustOrderNumPlaceHolder"><th>
			            <asp:Literal ID="lClientOrderNumber" runat="server" Text="Customer's order number" 
				            meta:resourcekey="lClientOrderNumberResource1" /></th></asp:PlaceHolder>
		            <th><asp:Literal ID="lOrderDate" runat="server" Text="Order's date" 
				            meta:resourcekey="lOrderDateResource1" /></th>
                    <%--<th><asp:Literal ID="lOrderCode" runat="server" Text="код заказа" 
				            meta:resourcekey="lOrderCodeResource1" /></th>--%>
	                <th><asp:Literal ID="Literal1" runat="server" Text="Linenumber" 
				            meta:resourcekey="lOrderCodeResource1" /></th>
		            <th><asp:Literal ID="lManufact" runat="server" Text="Brand" 
				            meta:resourcekey="lManufactResource1" /></th>
		            <th><asp:Literal ID="lDetailNumber" runat="server" Text="Partnumber" 
				            meta:resourcekey="lDetailNumberResource1" /></th>
		            <th><asp:Literal ID="lName" runat="server" Text="Description" 
				            meta:resourcekey="lNameResource1" /></th>
		            <th class="nowrap"><asp:Literal ID="lQty" runat="server" Text="Q-ty" 
				            meta:resourcekey="lQtyResource1" /></th>
		            <th><asp:Literal ID="lPrice" runat="server" Text="Price" 
				            meta:resourcekey="lPriceResource1" /></th>
		            <th><asp:Literal ID="lSumm" runat="server" Text="Total" 
				            meta:resourcekey="lSummResource1" /></th>
		            <th><asp:Literal ID="lDateOnStock" runat="server" 
				            Text="Est. delivery date " 
				            meta:resourcekey="lDateOnStockResource1" /><span style="color:red">*</span></th>
		            <th style="margin-left: 40px"><asp:Literal ID="lStatus" runat="server" Text="Status" 
				            meta:resourcekey="lStatusResource1" /></th>
	            </tr>
	            </tbody>
	            <tbody runat="server" id="_filters1Block" class="filters">
	            <tr>
	             <td></td>
	             <td class="td1">
		            <asp:TextBox runat="server" ID="_filterOrderIdTextBox" 
			             meta:resourcekey="_filterOrderIdTextBoxResource1" />
		             <asp:DropDownList runat="server" ID="_filterOrderIdDropDownBox" />
		            <asp:RegularExpressionValidator runat="server" ID="_filterOrderIdValidator" 
			             ControlToValidate="_filterOrderIdTextBox" SetFocusOnError="True" 
			             Display="Dynamic" ValidationExpression="(\d+\/)?\d+" 
			             Text="Введите номер заказа в формате 99/999" 
			             meta:resourcekey="_filterOrderIdValidatorResource1" />
	             </td>
	             <asp:PlaceHolder runat="server" ID="_filterCustOrderNumPlaceHolder"><td>
		            <asp:TextBox runat="server" ID="_filterCustOrderNumBox" 
			             meta:resourcekey="_filterCustOrderNumBoxResource1" />
	             </td></asp:PlaceHolder>
	             <td><asp:TextBox runat="server" ID="_filterOrderDateBox" 
			             meta:resourcekey="_filterOrderDateBoxResource1" /></td>
	             <td>&nbsp;</td>
	             <td style="padding-left:5px;"><asp:DropDownList runat="server" ID="_filterManufacturerBox" /></td>
	             <td><asp:TextBox runat="server" ID="_filterPartNumberBox" 
			             meta:resourcekey="_filterPartNumberBoxResource1" /></td>
	             <td><asp:TextBox runat="server" ID="_filterPartNameBox" 
			             meta:resourcekey="_filterPartNameBoxResource1" /></td>
	             <td>
		            <asp:TextBox runat="server" ID="_filterEstSupplyDateBox" 
			             meta:resourcekey="_filterEstSupplyDateBoxResource1" />
		            <asp:RegularExpressionValidator runat="server" 
			             ID="_filterEstSupplyDateValidator" ControlToValidate="_filterEstSupplyDateBox" 
			             SetFocusOnError="True" Display="Dynamic" 
			             ValidationExpression="\d{1,2}\.\d{1,2}\.\d{4}" 
			             Text="Введите дату в формате дд.мм.гггг" 
			             meta:resourcekey="_filterEstSupplyDateValidatorResource1" />
	            </td>
	             <td colspan="4" class="td2">
		             <asp:DropDownList style="position: relative; width: 180px; top: 49px; left: 1217px;" 
                         runat="server"  ID="_filterStatusBox" AppendDataBoundItems="True" 
                         AutoPostBack="True" >
                     </asp:DropDownList>
                    </td>
	            </tr>
	            </tbody>
	            <tbody runat="server" id="_filters2Block"  class="filters2">
	            <tr>
	             <td></td>
	             <td colspan="12"><div style="float:right;"><asp:LinkButton runat="server" 
			             ID="_clearFilterButton" onclick="_clearFilterButton_Click" CausesValidation="False" 
			             meta:resourcekey="_clearFilterButtonResource1" Text="очистить фильтр">
            </asp:LinkButton></div>
                    <asp:Button runat="server" ID="_searchButton" CssClass="button" ImageUrl="<%$ Resources:ImagesURL, filter_btn %>" onclick="_searchButton_Click" 
			             meta:resourcekey="_searchButtonResource1" /></td>
	            </tr>
	            </tbody>
	            <input type="hidden" runat="server" value="" id = "SavedStatuses" />
	            <asp:ListView runat="server" ID="_listView" DataSourceID="_objectDataSource" 
			                  ondatabound="_listView_DataBound" ondatabinding="_listView_DataBinding" 
                              onitemupdated="_listView_ItemUpdated" 
                              onitemcommand="_listView_ItemCommand" 
                    onitemdatabound="_listView_ItemDataBound1">
	            <LayoutTemplate>
		            <tbody class="list2 list3" id="linesTable" >
		                <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
		            </tbody>
	            </LayoutTemplate>
	            <ItemTemplate>
		            <asp:PlaceHolder runat="server" ID="_placeHolder">
			            <uc1:OrderLineTemplateLite 
					            ID="OrderLineTemplate1" runat="server" ShowCustOrderNum='<%#ShowCustOrderNum%>' 
					            OnChangeReaction="OnClientChangeReaction"  />
		            </asp:PlaceHolder>					
	            </ItemTemplate>
	            <EmptyDataTemplate>
		            <tr><td colspan="14"><asp:Literal ID="lEmptyRowList" runat="server" 
						            Text="список строк пуст" meta:resourcekey="lEmptyRowListResource1" /></td></tr>
		
	            </EmptyDataTemplate>
            </asp:ListView>


            </table>
        </div>
        <div class="bottom">
            <div class="left">
                <div class="pages">
                    <ul>
                        <li>
                            <asp:DataPager ID="_dataPager" runat="server" 
	                            PagedControlID="_listView">
	                            <Fields>
		                            <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="False" PreviousPageText="" ButtonCssClass="prev" />
		                            <asp:NumericPagerField />
		                            <asp:NextPreviousPagerField ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText="" ButtonCssClass="next"/>
	                            </Fields>
                            </asp:DataPager>
                        </li>
                        <li runat="server" id="_sortBlock">
	                        <asp:Literal ID="lOrderBy" runat="server" Text="<%$ Resources:Texts, OrderBy %>" />
	                        <asp:DropDownList runat="server" ID="_sortBox" AutoPostBack="True" OnSelectedIndexChanged="_sortBox_SelectedIndexChanged" />
                        </li>
                        <li>
                            <asp:Literal 
		                        ID="lCountView" runat="server" Text="display by:" 
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
            <div class="right">
                <div runat="server" id="_totalsBlock" class="right_block2">
                <asp:Literal ID="lSummOnPage" runat="server" Text="Total on page" 
		                meta:resourcekey="lSummOnPageResource1" />&nbsp;<asp:Label runat="server" ID="_pageTotalLabel" CssClass="price" />&nbsp;<%=global::Resources.Texts.DollarShort %> <br />
                <asp:Literal ID="lAllSumm" runat="server" Text="Total" 
		                meta:resourcekey="lAllSummResource1" />&nbsp;<asp:Label runat="server" ID="_totalLabel" CssClass="price" />&nbsp;<%=global::Resources.Texts.DollarShort%>
                </div>
                <div runat="server" id="_pagerSettingsBlock"></div>
            </div>
        </div>
    </div>
</div>

<asp:Button class="button" OnClientClick="UpdateStatuses();" 
    runat="server"  ID="UpdateAllStatusesBtn" Text="Update all statuses" 
    EnableTheming="True" />

	<uc1:TextItemControl ID="_estSupplyDateHint_TextItemControl" runat="server" 
	TextItemID="Orders.EstSupplyDateHint"
	ShowHeader="false" />

<asp:PlaceHolder 
	ID="PartNumberTransitionHintPlaceHolder"  runat="server" Visible="False"><span style="font-size: xx-small;">
        <span style="color: #f09399;">**</span> <asp:Literal ID="lChangeNumber" 
		runat="server" Text="Переход номера" 
		meta:resourcekey="lChangeNumberResource1" /></span>
</asp:PlaceHolder>


<asp:ObjectDataSource runat="server" ID="_objectDataSource" EnablePaging="True" 
	StartRowIndexParameterName="startIndex" MaximumRowsParameterName="size"  
	TypeName="RmsAuto.Store.Web.Controls.OrderLinesWholesaleLite" 
	SelectCountMethod="GetOrderLinesCount" SelectMethod="GetOrderLines" 
	onselected="_objectDataSource_Selected" UpdateMethod="UpdateOrderLines"  >
	<SelectParameters>
		<asp:Parameter Name="strOrderIDs" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="orderID" Type="Int32" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="custOrderNum" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="orderDate" Type="DateTime" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="manufacturer" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="partNumber" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="partName" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="estSupplyDate" Type="DateTime" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="status" Type="String" ConvertEmptyStringToNull="true" />
		<asp:ControlParameter Name="sort" ControlID="_sortBox" Type="Int32" DefaultValue="0" />
		<asp:Parameter Name="startIndex" Type="Int32" />
		<asp:Parameter Name="size" Type="Int32" />
	</SelectParameters>
	<UpdateParameters>
		<asp:Parameter Name="strOrderIDs" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="orderID" Type="Int32" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="custOrderNum" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="orderDate" Type="DateTime" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="manufacturer" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="partNumber" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="partName" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="estSupplyDate" Type="DateTime" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="status" Type="String" ConvertEmptyStringToNull="true" />
		<asp:ControlParameter Name="sort" ControlID="_sortBox" Type="Int32" DefaultValue="0" />
	</UpdateParameters>
</asp:ObjectDataSource>



