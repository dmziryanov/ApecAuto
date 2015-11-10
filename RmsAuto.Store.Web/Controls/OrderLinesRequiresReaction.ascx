<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLinesRequiresReaction.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderLinesRequiresReaction" %>
<%@ Register src="OrderLineTemplate.ascx" tagname="OrderLineTemplate" tagprefix="uc1" %>

<div class="tab-text">
    <div class="t-hold">
<asp:ListView runat="server" ID="_listView" DataSourceID="_objectDataSource" 
	ondatabinding="_listView_DataBinding">
<LayoutTemplate>
	<table>
		<tr>
			<th><asp:Literal ID="lOrderNumber" runat="server" Text="номер заказа" 
					meta:resourcekey="lOrderNumberResource1" /></th>
			<th><asp:Literal ID="lOrderDate" runat="server" Text="дата заказа" 
					meta:resourcekey="lOrderDateResource1" /></th>
			<th><asp:Literal ID="lOrderCode" runat="server" Text="код заказа" 
					meta:resourcekey="lOrderCodeResource1" /></th>
			<th><asp:Literal ID="lManufact" runat="server" Text="производитель" 
					meta:resourcekey="lManufactResource1" /></th>
			<th><asp:Literal ID="lNumberDetail" runat="server" Text="номер детали" 
					meta:resourcekey="lNumberDetailResource1" /></th>
			<th><asp:Literal ID="lName" runat="server" Text="наименование" 
					meta:resourcekey="lNameResource1" /></th>
			<th><asp:Literal ID="lQty" runat="server" Text="к-во" 
					meta:resourcekey="lQtyResource1" /></th>
			<th><asp:Literal ID="lPrice" runat="server" Text="цена" 
					meta:resourcekey="lPriceResource1" /></th>
			<th><asp:Literal ID="lSumm" runat="server" Text="сумма" 
					meta:resourcekey="lSummResource1" /></th>
			<th><asp:Literal ID="lShippmentDate" runat="server" Text="дата поставки" 
					meta:resourcekey="lShippmentDateResource1" /></th>
			<th><asp:Literal ID="lStatus" runat="server" Text="статус" 
					meta:resourcekey="lStatusResource1" /></th>
			<th></th>
			<th></th>
			<th></th>
		</tr>
		<asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
	</table>
</LayoutTemplate>
<ItemTemplate>
	<asp:PlaceHolder runat="server" ID="_placeHolder">
		<uc1:OrderLineTemplate ID="OrderLineTemplate1" runat="server" 
			OnChangeReaction="OnClientChangeReaction" />
	</asp:PlaceHolder>
</ItemTemplate>
<EmptyDataTemplate>
    <div class="clearfloat"></div>
	<table width="100%">
	<tr><td>
	<asp:Literal ID="lNotConfirmRow" runat="server" 
			Text="нет строк, требующих подтверждения" 
			meta:resourcekey="lNotConfirmRowResource1" />
	</td></tr>
	</table>
</EmptyDataTemplate>
</asp:ListView>
        </div>
    <div class="bottom">
        <div class="left">
            <div class="pages">
                <ul>
                    <li runat="server" id="_sortBlock">
	                    <asp:Literal ID="lOrderBy" runat="server" Text="<%$ Resources:Texts, OrderBy %>" />
	                    <asp:DropDownList runat="server" id="_sortBox" AutoPostBack="True" onselectedindexchanged="_sortBox_SelectedIndexChanged"></asp:DropDownList>
                    </li>
                </ul>
            </div>
        </div>
        <div class="right">
            <asp:PlaceHolder ID="PartNumberTransitionHintPlaceHolder" runat="server" Visible="False">
                <span style="font-size: xx-small;">
                    <span style="color: #f09399;">**</span>
                    <asp:Literal ID="lChangeNumber" runat="server" Text="Переход номера" 
		            meta:resourcekey="lChangeNumberResource1" />
                </span>
            </asp:PlaceHolder>
        </div>
    </div>
    <asp:ObjectDataSource runat="server" ID="_objectDataSource" 
	    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="size"  
	    TypeName="RmsAuto.Store.Web.Controls.OrderLinesRequiresReaction" 
	    SelectMethod="GetOrderLines" onselected="_objectDataSource_Selected" >
	    <SelectParameters>
		    <asp:ControlParameter Name="sort" ControlID="_sortBox" Type="Int32" DefaultValue="0" />
	    </SelectParameters>
    </asp:ObjectDataSource>
</div>