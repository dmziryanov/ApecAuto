<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLinesWholesale.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderLinesWholesale" %>
<%@ Register src="OrderLineTemplate.ascx" tagname="OrderLineTemplate" tagprefix="uc1" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>

<div class="orders">
    <div class="tab-text">
        <div class="t-hold">
            <div class="br"></div>
            <div class="bl"></div>
            <table class="table-big">
	            <tbody class="list2 list3">
	            <tr>
		            <th><asp:Literal ID="lOrderNumber" runat="server" Text="номер заказа" 
				            meta:resourcekey="lOrderNumberResource1" /></th>
		            <asp:PlaceHolder runat="server" ID="_headerCustOrderNumPlaceHolder">
                    <th><asp:Literal ID="lClientOrderNumber" runat="server" Text="номер заказа клиента" 
				            meta:resourcekey="lClientOrderNumberResource1" /></th></asp:PlaceHolder>
		            <th><asp:Literal ID="lOrderDate" runat="server" Text="дата заказа" 
				            meta:resourcekey="lOrderDateResource1" /></th>
		            <th><asp:Literal ID="lOrderCode" runat="server" Text="код заказа" 
				            meta:resourcekey="lOrderCodeResource1" /></th>
		            <th><asp:Literal ID="lManufact" runat="server" Text="производитель" 
				            meta:resourcekey="lManufactResource1" /></th>
		            <th><asp:Literal ID="lDetailNumber" runat="server" Text="номер детали" 
				            meta:resourcekey="lDetailNumberResource1" /></th>
		            <th><asp:Literal ID="lName" runat="server" Text="наименование" 
				            meta:resourcekey="lNameResource1" /></th>
		            <th class="nowrap"><asp:Literal ID="lQty" runat="server" Text="к-во" 
				            meta:resourcekey="lQtyResource1" /></th>
		            <th><asp:Literal ID="lPrice" runat="server" Text="цена" 
				            meta:resourcekey="lPriceResource1" /></th>
		            <th><asp:Literal ID="lSumm" runat="server" Text="сумма" 
				            meta:resourcekey="lSummResource1" /></th>
		            <th><asp:Literal ID="lDateOnStock" runat="server" 
				            Text="дата поступления на склад  " 
				            meta:resourcekey="lDateOnStockResource1" /><span style="color:red">*</span></th>
		            <th><asp:Literal ID="lStatus" runat="server" Text="статус" 
				            meta:resourcekey="lStatusResource1" /></th>
	            </tr>
	            </tbody>
	            <tbody runat="server" id="_filters1Block" class="filters">
	            <tr>
	             <td class="td1">
		   <asp:TextBox runat="server" ID="_filterOrderIdTextBox" 
			             meta:resourcekey="_filterOrderIdTextBoxResource1" width="40px" />
		             <asp:DropDownList runat="server" ID="_filterOrderIdDropDownBox" width="50px" />
		            <asp:RegularExpressionValidator runat="server" ID="_filterOrderIdValidator" 
			             ControlToValidate="_filterOrderIdTextBox" SetFocusOnError="True" 
			             Display="Dynamic" ValidationExpression="(\d+\/)?\d+" 
			             Text="Введите номер заказа в формате 99/999" 
			             meta:resourcekey="_filterOrderIdValidatorResource1" />
	             </td>
	             <asp:PlaceHolder runat="server" ID="_filterCustOrderNumPlaceHolder"><td>
		            <asp:TextBox runat="server" ID="_filterCustOrderNumBox" 
			             meta:resourcekey="_filterCustOrderNumBoxResource1" width="65px" />
	             </td></asp:PlaceHolder>
	             <td><asp:TextBox runat="server" ID="_filterOrderDateBox" 
			             meta:resourcekey="_filterOrderDateBoxResource1" CssClass="date" width="66px" /></td>
	             <td>&nbsp;</td>
	             <td style="padding-left:5px;"><asp:DropDownList runat="server" ID="_filterManufacturerBox" /></td>
	             <td><asp:TextBox runat="server" ID="_filterPartNumberBox" 
			             meta:resourcekey="_filterPartNumberBoxResource1" width="65px" /></td>
	             <td><asp:TextBox runat="server" ID="_filterPartNameBox" 
			             meta:resourcekey="_filterPartNameBoxResource1" Width="70px" /></td>
	             
		            
                    <td></td>
                    <td></td>
                    <td></td>
	             <td>
		            <asp:TextBox runat="server" ID="_filterEstSupplyDateBox"  CssClass="date"
			             meta:resourcekey="_filterEstSupplyDateBoxResource1" />
		            <asp:RegularExpressionValidator runat="server" 
			             ID="_filterEstSupplyDateValidator" ControlToValidate="_filterEstSupplyDateBox" 
			             SetFocusOnError="True" Display="Dynamic" 
			             ValidationExpression="\d{1,2}\/\d{1,2}\/\d{4}" 
			             Text="Введите дату в формате дд.мм.гггг" 
			             meta:resourcekey="_filterEstSupplyDateValidatorResource1" />
	            </td>
	             <td >
		             <asp:DropDownList style="position: relative; width: 180px; top: 0px;" 
                         runat="server" ID="_filterStatusBox" /></td>
	            </tr>
	            </tbody>
	            <tbody runat="server" id="_filters2Block"  class="filters2">
	            <tr>
	             <td colspan="12"><div style="float:right;"><asp:LinkButton runat="server" CssClass="button"
			             ID="_clearFilterButton" onclick="_clearFilterButton_Click" CausesValidation="False" 
			             meta:resourcekey="_clearFilterButtonResource1" Text="очистить фильтр">
            </asp:LinkButton></div><asp:LinkButton CssClass="button" runat="server" ID="_searchButton" Text="Filter"
						            ImageUrl="<%$ Resources:ImagesURL, filter_btn %>" onclick="_searchButton_Click" 
			             meta:resourcekey="_searchButtonResource1" /></td>
	            </tr>
	            </tbody>
	            <asp:ListView runat="server" ID="_listView" DataSourceID="_objectDataSource" 
			            ondatabound="_listView_DataBound" ondatabinding="_listView_DataBinding">
	            <LayoutTemplate>
		            <tbody class="list2 list3">
		            <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
		            </tbody>
	            </LayoutTemplate>
	            <ItemTemplate>
		            <asp:PlaceHolder runat="server" ID="_placeHolder">
			            <uc1:OrderLineTemplate 
					            ID="OrderLineTemplate1" runat="server" ShowCustOrderNum='<%#ShowCustOrderNum%>' 
					            OnChangeReaction="OnClientChangeReaction" /></asp:PlaceHolder>					
	            </ItemTemplate>
	            <EmptyDataTemplate>
		            <tr><td colspan="12"><asp:Literal ID="lEmptyRowList" runat="server" 
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
	                        <asp:DropDownList runat="server" ID="_sortBox" AutoPostBack="True" 
		                        OnSelectedIndexChanged="_sortBox_SelectedIndexChanged" />
                        </li>
                        <li runat="server" id="_pagerSettingsBlock">
                            <asp:Literal ID="lCountView" runat="server" Text="display by:" meta:resourcekey="lCountViewResource1" />
	                        <asp:DropDownList runat="server" ID="_pageSizeBox" AutoPostBack="True" OnSelectedIndexChanged="_pageSizeBox_SelectedIndexChanged">
		                        <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
		                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
		                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
		                        <asp:ListItem Text="400" Value="400"></asp:ListItem>
	                        </asp:DropDownList>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="right" runat="server" id="_totalsBlock">
                <asp:Literal ID="lSummOnPage" runat="server" Text="Тоtal on page" meta:resourcekey="lSummOnPageResource1" /> <asp:Label runat="server" ID="_pageTotalLabel" CssClass="price" />&nbsp;<%=global::Resources.Texts.RussianRouble%>
                <br />
                <asp:Literal ID="lAllSumm" runat="server" Text="Итого" meta:resourcekey="lAllSummResource1" /> <asp:Label runat="server" ID="_totalLabel" CssClass="price" />&nbsp;<%=global::Resources.Texts.RoubleShort%>
            </div>
        </div>
    </div>
</div>

<br>

<br />


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
	TypeName="RmsAuto.Store.Web.Controls.OrderLinesWholesale" 
	SelectCountMethod="GetOrderLinesCount" SelectMethod="GetOrderLines" 
	onselected="_objectDataSource_Selected"  >
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
</asp:ObjectDataSource>
