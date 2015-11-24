<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineList.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderLineList" %>
<%@ Register src="OrderLineTemplate.ascx" tagname="OrderLineTemplate" tagprefix="uc1" %>

<div class="tab-text">
    <div class="t-hold">
<script type="text/javascript">
	$(document).ready(function() {
//		InputHelperCreate($('#<%=_filterOrderIDs.ClientID%>'), 'введите номера заказов');
//		InputHelperCreate($('#<%=_filterPartNumbers.ClientID%>'), 'введите номера деталей');
//		InputHelperCreate($('#<%=_filterReferenceIDs.ClientID%>'), 'введите коды заказов');

	    $("#filter").accordion({ collapsible: true });
	    $('.date').datepicker();
	});
</script>

<asp:ScriptManager ID="spMain" runat="server"></asp:ScriptManager>

<div id="filter">
	<h3> <a href="#"><asp:Literal ID="lFilter" runat="server" Text="Filter:" 
			meta:resourcekey="lFilterResource1"></asp:Literal></a></h3>

	<div>
		<table style="width: 900px;">
		<thead>
			<tr>
				<th style="width: 200px;"></th>
				<th style="width: 450px;"></th>
				<th style="width: 100px;"></th>
				<th style="width: 150px;"></th>
			</tr>	
		</thead>
			<tr>
				<td><asp:Literal ID="lStartDate" runat="server" Text="Дата размещения заказа, с " 
						meta:resourcekey="lStartDateResource1"></asp:Literal></td>
				<td>
					<asp:TextBox ID="_filterStartDate" runat="server" 
						meta:resourcekey="_filterStartDateResource1" CssClass="date"></asp:TextBox>
					&nbsp;<%= global::Resources.Texts.AC_To%>&nbsp;
					<asp:TextBox ID="_filterEndDate" runat="server" 
						meta:resourcekey="_filterEndDateResource1" CssClass="date"></asp:TextBox>
                    <asp:Label CssClass="info" ToolTip="<%$ Resources:Hints, dateFormatHint %>" runat="server"></asp:Label>
					<asp:CustomValidator ID="_filterStartDateCustomValidator" runat="server" Display="Dynamic"
						ControlToValidate="_filterStartDate" 
						ErrorMessage="Неверный формат данных" 
						onservervalidate="ValidateDate" meta:resourcekey="_filterStartDateCustomValidatorResource1"></asp:CustomValidator>
					<asp:CustomValidator ID="_filterEndDateCustomValidator" runat="server" Display="Dynamic"
						ControlToValidate="_filterEndDate" 
						ErrorMessage="Неверный формат данных" 
						onservervalidate="ValidateDate" meta:resourcekey="_filterEndDateCustomValidatorResource1"></asp:CustomValidator>
				</td>
				<td><asp:Literal ID="lStatus" runat="server" Text="Статус заказа" 
						meta:resourcekey="lStatusResource1"></asp:Literal></td>
				<td>
					<asp:DropDownList ID="ddlStatusesPresets" runat="server" Width="153px" 
						AutoPostBack="True" 
						OnSelectedIndexChanged="ddlStatusesPresets_SelectedIndexChanged">
						<asp:ListItem Text="все" Value="all" meta:resourcekey="ListItemResource1"></asp:ListItem>
						<asp:ListItem Text="все рабочие" Value="allworked" 
							meta:resourcekey="ListItemResource2"></asp:ListItem>
						<asp:ListItem Text="все закрытые" Value="allclosed" 
							meta:resourcekey="ListItemResource3"></asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td><asp:Literal ID="lStartPrice" runat="server" Text="Диапазон цены детали, c " 
						meta:resourcekey="lStartPriceResource1"></asp:Literal></td>
				<td>
					<asp:TextBox ID="_filterStartPrice" runat="server" 
						meta:resourcekey="_filterStartPriceResource1"></asp:TextBox>
					&nbsp;<%= global::Resources.Texts.AC_To%>&nbsp;
					<asp:TextBox ID="_filterEndPrice" runat="server" 
						meta:resourcekey="_filterEndPriceResource1"></asp:TextBox>
                    <asp:Label ID="Label1" CssClass="info" ToolTip="<%$ Resources:Hints, numbersAllowedHint %>" runat="server"></asp:Label>
					<asp:CustomValidator ID="_filterStartPriceCustomValidator" runat="server" Display="Dynamic"
						ControlToValidate="_filterStartPrice" 
						ErrorMessage="Неверный формат данных" 
						onservervalidate="ValidatePrice" 
						meta:resourcekey="_filterStartPriceCustomValidatorResource1"></asp:CustomValidator>
					<asp:CustomValidator ID="_filterEndPriceCustomValidator" runat="server" Display="Dynamic"
						ControlToValidate="_filterEndPrice" 
						ErrorMessage="Неверный формат данных" 
						onservervalidate="ValidatePrice" meta:resourcekey="_filterEndPriceCustomValidatorResource1"></asp:CustomValidator>
				</td>
				<td rowspan="7" colspan="2">
					<div style="width: 255px; height: 164px; overflow-y: scroll; border: solid 1px #bbbbbb;">
						<asp:UpdatePanel ID="upStatuses" runat="server">
							<ContentTemplate>
								<asp:CheckBoxList ID="_filterStatuses" runat="server"></asp:CheckBoxList>
							</ContentTemplate>
							<Triggers>
								<asp:AsyncPostBackTrigger ControlID="ddlStatusesPresets" EventName="SelectedIndexChanged" />
							</Triggers>
						</asp:UpdatePanel>
					</div>
				</td>
			</tr>
			<tr>
				<td><asp:Literal ID="lPartName" runat="server" Text="Наименование детали" 
						meta:resourcekey="lPartNameResource1"></asp:Literal></td>
				<td>
					<asp:TextBox ID="_filterPartName" runat="server" Width="400px" 
						meta:resourcekey="_filterPartNameResource1"></asp:TextBox>

                    <asp:Label ID="Label2" CssClass="info" ToolTip="<%$ Resources:Hints, descriptionPartiallyHint %>" runat="server"></asp:Label>
				</td>
				<td colspan="2"></td>
			</tr>
			<tr>
				<td><asp:Literal ID="lOrderIDs" runat="server" Text="Номер заказа" 
						meta:resourcekey="lOrderIDsResource1"></asp:Literal></td>
				<td>
					<asp:TextBox ID="_filterOrderIDs" runat="server" Width="400px" 
						meta:resourcekey="_filterOrderIDsResource1"></asp:TextBox>
                    <asp:Label ID="Label3" CssClass="info" ToolTip="<%$ Resources:Hints, severalValuesCommaSeparatedHint %>" runat="server"></asp:Label>
					<asp:CustomValidator ID="_OrderIDsCustomValidator" runat="server" Display="Dynamic"
						ControlToValidate="_filterOrderIDs" 
						ErrorMessage="Неверный формат данных" 
						onservervalidate="ValidateOrderIDs" meta:resourcekey="_OrderIDsCustomValidatorResource1"></asp:CustomValidator>
				</td>
				<td colspan="2"></td>
			</tr>
			<tr>
			    <td><asp:Literal ID="lCustOrderNums" runat="server" Text="Номер заказа клиента" 
						meta:resourcekey="lCustOrderNumsResource1"></asp:Literal></td>
			    <td>
					<asp:TextBox ID="_filterCustOrderNums" runat="server" Width="400px" 
						meta:resourcekey="_filterCustOrderNumsResource1"></asp:TextBox>
                    <asp:Label ID="Label4" CssClass="info" ToolTip="<%$ Resources:Hints, severalValuesCommaSeparatedHint %>" runat="server"></asp:Label>
				</td>
			</tr>
			<tr>
				<td><asp:Literal ID="lPartNumbers" runat="server" Text="Номер детали" 
						meta:resourcekey="lPartNumbersResource1"></asp:Literal></td>
				<td>
					<asp:TextBox ID="_filterPartNumbers" runat="server" Width="400px" 
						meta:resourcekey="_filterPartNumbersResource1"></asp:TextBox>
                    <asp:Label ID="Label5" CssClass="info" ToolTip="<%$ Resources:Hints, severalValuesCommaSeparatedHint %>" runat="server"></asp:Label>
				</td>
				<td colspan="2"></td>
			</tr>
			<tr>
				<td><asp:Literal ID="lReferenceIDs" runat="server" Text="Код заказа" 
						meta:resourcekey="lReferenceIDsResource1"></asp:Literal></td>
				<td>
					<asp:TextBox ID="_filterReferenceIDs" runat="server" Width="400px" 
						meta:resourcekey="_filterReferenceIDsResource1"></asp:TextBox>
                    <asp:Label ID="Label6" CssClass="info" ToolTip="<%$ Resources:Hints, severalValuesCommaSeparatedHint %>" runat="server"></asp:Label>
				</td>
			</tr>
			<tr>
				<td><asp:Literal ID="lManufacturers" runat="server" Text="Производитель" 
						meta:resourcekey="lManufacturersResource1"></asp:Literal></td>
				<td>
					<asp:TextBox ID="_filterManufacturers" runat="server" Width="400px" 
						meta:resourcekey="_filterManufacturersResource1"></asp:TextBox>
                    <asp:Label ID="Label8" CssClass="info" ToolTip="<%$ Resources:Hints, severalValuesCommaSeparatedHint %>" runat="server"></asp:Label>
				</td>
				<td colspan="2"></td>
			</tr>
		</table>
		<asp:Button ID="_searchButton" runat="server" 
				Text="Фильтровать" OnClick="_searchButton_Click" 
				meta:resourcekey="_searchButtonResource1" CssClass="button" />
		<asp:Button ID="_clearFilterButton" runat="server" 
				Text="Очистить фильтр" CausesValidation="False" 
				OnClick="_clearFilterButton_Click" 
				meta:resourcekey="_clearFilterButtonResource1" CssClass="button" />
	</div>
</div>

<table class="table-big">
	<tr>
		<th><asp:Literal ID="lHOrderNumber" runat="server" Text="номер заказа" 
				meta:resourcekey="lHOrderNumberResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHCustOrderNumber" runat="server" Text="номер заказа клиента" 
				meta:resourcekey="lHCustOrderNumberResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHOrderDate" runat="server" Text="дата заказа" 
				meta:resourcekey="lHOrderDateResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHReferenceID" runat="server" Text="код заказа" 
				meta:resourcekey="lHReferenceIDResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHManufacturer" runat="server" Text="производитель" 
				meta:resourcekey="lHManufacturerResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHPartNumber" runat="server" Text="номер детали" 
				meta:resourcekey="lHPartNumberResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHPartName" runat="server" Text="наименование" 
				meta:resourcekey="lHPartNameResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHQty" runat="server" Text="к-во" 
				meta:resourcekey="lHQtyResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHPrice" runat="server" Text="цена" 
				meta:resourcekey="lHPriceResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHSum" runat="server" Text="сумма" 
				meta:resourcekey="lHSumResource1"></asp:Literal></th>
		<th><asp:Literal ID="lHEstDate" runat="server" 
				Text="дата поступления на склад  " meta:resourcekey="lHEstDateResource1"></asp:Literal><span style="color:red">*</span></th>
		<th><asp:Literal ID="lHStatus" runat="server" Text="статус" 
				meta:resourcekey="lHStatusResource1"></asp:Literal></th>
<%--		<th></th>
		<th></th>
		<th></th>--%>
	</tr>
	
	<asp:ListView runat="server" ID="_listView" DataSourceID="_objectDataSource" 
			ondatabound="_listView_DataBound" ondatabinding="_listView_DataBinding">
		<LayoutTemplate>
			<asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
		</LayoutTemplate>
		<ItemTemplate>
			<asp:PlaceHolder runat="server" ID="_placeHolder">
				<uc1:OrderLineTemplate ID="OrderLineTemplate1" runat="server" ShowCustOrderNum="true" />
			</asp:PlaceHolder>
		</ItemTemplate>
		<EmptyDataTemplate>
			<tr><td colspan="14"><asp:Literal ID="lEmptyList" runat="server" 
					Text="список строк пуст" meta:resourcekey="lEmptyListResource1"></asp:Literal></td></tr>
		</EmptyDataTemplate>
	</asp:ListView>
</table>
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
                    <li runat="server" id="_pagerSettingsBlock">
                        <span class="text"><%= global::Resources.Texts.DisplayOn%></span>
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
                <br />
                <%= global::Resources.Texts.TotalOnPage%> 
	                <asp:Label runat="server" ID="_pageTotalLabel" 
		                CssClass="price" /> <%= global::Resources.Texts.DollarShort%>
                <br />
                <%= global::Resources.Texts.Total%> <asp:Label runat="server" 
		                ID="_totalLabel" CssClass="price"
		                    /> <%= global::Resources.Texts.DollarShort%>
                </div>
        </div>

        </div>
</div>

<asp:ObjectDataSource runat="server" ID="_objectDataSource" EnablePaging="True" 
	StartRowIndexParameterName="startIndex" MaximumRowsParameterName="size"  
	TypeName="RmsAuto.Store.Web.Controls.OrderLineList" 
	SelectCountMethod="GetOrderLinesCount" SelectMethod="GetOrderLines" 
	onselected="_objectDataSource_Selected"  >
	<SelectParameters>
		<asp:Parameter Name="orderDateStart" Type="DateTime" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="orderDateEnd" Type="DateTime" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="partPriceStart" Type="Decimal" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="partPriceEnd" Type="Decimal" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="partName" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="orderIDs" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="custOrderNums" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="partNumbers" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="referenceIDs" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="manufacturers" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="preset" Type="String" ConvertEmptyStringToNull="true" />
		<asp:Parameter Name="statusIDs" Type="String" ConvertEmptyStringToNull="true" />
		<asp:ControlParameter Name="sort" ControlID="_sortBox" Type="Int32" DefaultValue="0" />
		<asp:Parameter Name="startIndex" Type="Int32" />
		<asp:Parameter Name="size" Type="Int32" />
	</SelectParameters>
</asp:ObjectDataSource>	