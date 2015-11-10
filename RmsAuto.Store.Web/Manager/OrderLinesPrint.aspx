<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderLinesPrint.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.OrderLinesPrint" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Manager" />

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Товарная накладная</title>
    <style type="text/css">
	.nakladnaya {
		font-size:16px;
		font-style:normal;
		text-decoration:none;
		font-family:"Arial";
	}
	.table_text {
		font-size:11px;
		font-style:normal;
		text-decoration:none;
		font-family:"Arial";
	}
	.other_text {
		font-size:12px;
		font-style:normal;
		text-decoration:none;
		font-family:"Arial";
	}
	hr {
		border: none;
		color: black;
		height: 2px;
	}              
</style>

</head>
<body>

<%--	<table width="605" cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td class="nakladnaya">
				<b>Расходная накладная № _____ от <%= DateTime.Now.ToString("dd MMM yyyy") %></b><hr />
			</td>
		</tr>
		<tr>
			<td class="other_text" height="14"><span>Поставщик</span><span><b>_________________</b></span></td>
		</tr>
		<tr>       
			<td class="other_text" height="18"><span>Покупатель</span><span><b>_________________</b></span></td>
		</tr>
	</table>--%>
	
    <div class="nakladnaya">
		<b>Расходная накладная № <%= InvoiceNumber %> от <%= DateTime.Now.ToString("dd MMM yyyy") %></b><hr />
	</div>
	
	<div class="other_text">
		<span>Поставщик: </span><span><b><%= SupplierName %></b></span>
	</div>
	
	<div class="other_text">       
		<span>Покупатель: </span><span><b><%= ClientName %></b></span>
	</div>
	
    <asp:Repeater ID="rptMain" runat="server">
		<HeaderTemplate>
			<table width="605" cellpadding="0" cellspacing="0" border="1" class="table_text">
				<tr>
					<th width="30" height="28">№</th>
					<th width="65">Артикул</th>
					<th width="330">Товар</th>
					<th width="60">Кол-во</th>
					<th width="50">Цена</th>
					<th width="60">Сумма</th>
					<th width="60">НДС</th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td align="center"><%#Eval("Number")%></td>
				<td><%#Eval("PartNumber")%></td>
				<td><%#Eval("PartName") %></td>
				<td align="center"><%#Eval("Qty")%></td>
				<td align="right"><%#Eval("Price")%></td>
				<td align="right"><%#Eval("Total") %></td>
				<td align="right"><%# ((decimal)Eval("Total") * 18.0m / 118.0m).ToString("### ### ##0.00")%></td>
			</tr>
		</ItemTemplate>
		<FooterTemplate></table></FooterTemplate>
    </asp:Repeater>
    
	<table width="605" cellpadding="0" cellspacing="0">   
		<tr>
            <td> </td>
			<td> </td>
			<td> </td>
			<td> </td>
			<td width="540" align="right" class="other_text"><b>Итого: </b></td>
			<td colspan="2" align="right" class="other_text"><b><%= SumTotal.ToString("### ### ##0.00") %></b></td>
		</tr>     
		<tr>
		    <td> </td>
			<td> </td>
		    <td> </td>
			<td> </td>
			<td align="right" class="other_text"><b>В том числе НДС:</b></td>
			<td align="right" class="other_text"><b><%= (SumTotal * 18.0m / 118.0m).ToString("### ### ##0.00")%></b></td>
		</tr>     
		<tr>
			<td colspan="2"><hr /></td>
		</tr>
	</table>
	
	<div colspan="2" class="other_text"><span>Всего наименований <%=QtyTotal %>, на сумму <%= SumTotal.ToString("### ### ##0.00") %></span> </div>
    <br />
	<table width="605" cellpadding="0" cellspacing="0">   
		<tr>
		    <td> </td>
			<td width="54px" class="other_text"><b>Отпустил</b></td>
			<td width="160px">_________________ </td>
			<td width="52px" class="other_text"><b>Получил</b></td>
			<td>____________________</td> 
		</tr>
	</table>

</body>
</html>
