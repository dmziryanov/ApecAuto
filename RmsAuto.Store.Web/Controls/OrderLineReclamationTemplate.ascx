<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLineReclamationTemplate.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.OrderLineReclamationTemplate" %>

		<tr>
			<td class="nowrap"><%#GetOrderDisplayNumber( (RmsAuto.Store.Entities.Order)Eval( "Order" ) )%></td>
			<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%#ShowCustOrderNum%>'>
				<td><%#Server.HtmlEncode(Convert.ToString(Eval( "Order.CustOrderNum" )))%></td>
			</asp:PlaceHolder>
			<td><%#Eval("Order.OrderDate","{0:dd.MM.yyyy}") %></td>
			
			<td><%#Server.HtmlEncode((string)Eval( "ReferenceID" ))%></td>
			
			<td><%#Server.HtmlEncode((string)Eval( "Manufacturer" ))%></td>
			<td><%#Server.HtmlEncode((string)Eval( "PartNumber" ))%></td>
			<td>
				<a target="_blank" href="<%#GetSparePartDetailsUrl((RmsAuto.Store.Entities.OrderLine)Page.GetDataItem())%>"
					onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
					<%#Server.HtmlEncode((string)Eval( "PartName" ))%>
				</a>
			</td>
			<td align="center"><%#Eval( "Qty" )%></td>
			<td class="price"><span class="blue"><%#Eval("UnitPrice", "{0:### ### ##0.00}")%></span></td>
			<td class="price"><span class="blue"><%#Eval("Total", "{0:### ### ##0.00}")%></span></td>
			<td><%#Eval( "EstSupplyDate", "{0:dd.MM.yyyy}" )%></td>
			<td><%#GetStatusName( (byte)Eval( "CurrentStatus" ) )%></td>
			<td>
				<%#InitRedirectButton( (byte)Eval("CurrentStatus"), (DateTime)Eval("CurrentStatusDate"), (int) (Eval("AcctgOrderLineID") ?? 0) )%>
				<asp:Button ID="btnToRequest" runat="server" Text="Сформировать заявку" OnClick="btnToRequest_Click" CommandArgument='<%#Eval("OrderLineID")%>' />
			</td>
		</tr>