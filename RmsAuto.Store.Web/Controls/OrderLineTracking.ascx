<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineTracking.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderLineTracking" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<%@ Import Namespace="RmsAuto.Store" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>
<%@ Register TagPrefix="rms" Namespace="RmsAuto.Store.Web.Controls" Assembly="RmsAuto.Store.Web" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>

<h3><asp:Literal ID="lHeaderHistory" runat="server" Text="История по строке заказа" 
		meta:resourcekey="lHeaderHistoryResource1" /></h3>
<div class="info_history">
<asp:Literal ID="lOrderNumber" runat="server" Text="Номер заказа:" 
		meta:resourcekey="lOrderNumberResource1" /> <b><asp:Literal runat="server" 
		ID="_orderNumberLabel" meta:resourcekey="_orderNumberLabelResource1" /></b><br />
<asp:Literal ID="lOrderDate" runat="server" Text="Дата заказа:" 
		meta:resourcekey="lOrderDateResource1" /> <b><asp:Literal runat="server" 
		ID="_orderDateLabel" meta:resourcekey="_orderDateLabelResource1" /></b><br />
<asp:Literal ID="lManufacturer" runat="server" Text="Производитель:" 
		meta:resourcekey="lManufacturerResource1" /> <b><asp:Literal runat="server" 
		ID="_manufacturerLabel" meta:resourcekey="_manufacturerLabelResource1" /></b><br />
</div>
<br />

<asp:Repeater ID="_statusChangesRepeater" runat="server" >
    <HeaderTemplate>
		<table cellpadding="0" cellspacing="0" width="100%" class="list" style="background-color:#FFFFFF;">
        <tr>
          <th><asp:Literal ID="lNumber" runat="server" Text="Номер" 
				  meta:resourcekey="lNumberResource1" /></th>
          <th><asp:Literal ID="lName" runat="server" Text="Наименование" 
				  meta:resourcekey="lNameResource1" /></th>
          <th><asp:Literal ID="lPrice" runat="server" Text="Цена" 
				  meta:resourcekey="lPriceResource1" /></th>
          <th style="white-space: nowrap"><asp:Literal ID="lQty" runat="server" Text="Кол-во" 
				  meta:resourcekey="lQtyResource1" /></th>
          <th><asp:Literal ID="lSumm" runat="server" Text="Сумма" 
				  meta:resourcekey="lSummResource1" /></th>
          <th><asp:Literal ID="lDateOnStock" runat="server" 
				  Text="Дата поступления на склад  " 
				  meta:resourcekey="lDateOnStockResource1" /><span style="color:red">*</span></th>
          <th><asp:Literal ID="lOrderCode" runat="server" Text="Код заказа" 
				  meta:resourcekey="lOrderCodeResource1" /></th>
          <th><asp:Literal ID="lStatus" runat="server" Text="Статус" 
				  meta:resourcekey="lStatusResource1" /></th>
          <th><asp:Literal ID="lStatusDate" runat="server" Text="Дата статуса" 
				  meta:resourcekey="lStatusDateResource1" /></th>
        </tr>
    </HeaderTemplate>
	<ItemTemplate>
		<tr>
			<td class='<%# (bool)Eval("IsSameSparePart") ? "disable" : "" %>'><%#Server.HtmlEncode( (string)Eval( "Data.OrderLine.PartNumber" ) )%></td>
			<td class='<%# (bool)Eval("IsSameSparePart") ? "disable" : "" %>'>
				<a target="_blank" href='<%# GetSparePartDetailsUrl((RmsAuto.Store.Entities.OrderLine)Eval("Data.OrderLine")) %>'
					
					onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
					<%#Server.HtmlEncode( (string)Eval( "Data.OrderLine.PartName" ) )%>
				</a>
			</td>
			<td style="white-space:nowrap" class='<%# (bool)Eval("IsSamePrice") ? "price disable" : "price" %>'><%#Eval("Data.OrderLine.UnitPrice", "{0:### ### ##0.00}")%></td>
			<td style="text-align:center" class='<%# (bool)Eval("IsSameQty") ? "disable" : "" %>' align="center"><%#Eval( "Data.OrderLine.Qty" )%></td>
			<td style="white-space:nowrap" class='<%# (bool)Eval("IsSameTotal") ? "price disable" : "price" %>'><%#Eval("Data.OrderLine.Total", "{0:### ### ##0.00}")%></td>
			<td class='<%# (bool)Eval("IsSameEstSupplyDate") ? "disable" : "" %>'><%#Eval( "Data.OrderLine.EstSupplyDate", "{0:dd.MM.yyyy}" )%></td>
			<td><%#Server.HtmlEncode((string)Eval("Data.OrderLine.ReferenceID"))%></td>
			<td title='<%# Server.HtmlEncode(Convert.ToString(Eval("Data.StatusChangeInfo"))) %>'><%#GetStatusName( (byte) Eval( "Data.OrderLineStatus" ) )%></td>
			<td class='<%# Container.ItemIndex!=0 && Container.ItemIndex!=_list.Count-1 ? "disable" : "" %>'><%#Eval( "Data.StatusChangeTime", "{0:dd.MM.yyyy HH:mm}" )%></td>
		</tr>
	</ItemTemplate>
	<FooterTemplate>
		</table>
	</FooterTemplate>
</asp:Repeater>


<uc1:TextItemControl ID="_estSupplyDateHint_TextItemControl" runat="server" 
	TextItemID="Orders.EstSupplyDateHint"
	ShowHeader="false" />
