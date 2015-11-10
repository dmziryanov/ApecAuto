<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OptPaymentOrderPrint.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.OptPaymentOrderPrint" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />

<p>
    <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:
 collapse;width:528pt; height: 68px;">
        <colgroup>
            <col span="37" style="mso-width-source:userset;mso-width-alt:725;
 width:13pt" width="17" />
        </colgroup>
        <tr height="48" style="mso-height-source:userset;height:36.0pt">
            <td class="style17" colspan="37" height="48" width="629">
                <table border="0" cellpadding="0" cellspacing="0" 
                    style="border-collapse: collapse; width: 527pt">
                    <colgroup>
                        <col span="37" style="mso-width-source:userset;mso-width-alt:896;
 width:16pt" width="21" />
                    </colgroup>
                    <tr height="46" style="mso-height-source:userset;height:35.1pt">
                        <td class="style30" colspan="37" height="46" width="777">
                           Attention! Clearing this account implies the delivery terms acceptance

An order is to be processed once the receipt of funds into the bank account of the Supplier is acknowledged. The Supplier disclaimsguarantee for stock availability as of the date of invoice payment receipt.
The goods arereleased with procuracyand national identity card (passport) in hand
</td>
                    </tr>
                </table>
            </td>
                
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:
 collapse;width:481pt" width="629">
        <colgroup>
            <col span="37" width="17" />
        </colgroup>
        <tr height="17">
            <td class="style18" colspan="18" height="31" rowspan="2" width="306">
                <%=Server.HtmlEncode(_seller.BankName)%></td>
            <td class="style19" colspan="3" width="51">
                БИК</td>
            <td class="style20" colspan="16" width="272">
                <%=Server.HtmlEncode(_seller.BankCode)%></td>
        </tr>
        <tr height="14">
            <td class="style21" colspan="3" height="28" rowspan="2">
                Сч. №</td>
            <td class="style22" colspan="16" rowspan="2">
                <%=Server.HtmlEncode(_seller.BankAccount)%></td>
        </tr>
        <tr height="14">
            <td class="style23" colspan="18" height="14">
                Банк получателя</td>
        </tr>
        <tr height="17">
            <td class="style24" colspan="2" height="17">
                ИНН</td>
            <td class="style25" colspan="7"><%=Server.HtmlEncode(_seller.Inn)%></td>
            <td class="style26" colspan="2">КПП</td>
            <td class="style25" colspan="7"><%=Server.HtmlEncode(_seller.KPP)%></td>
            <td class="style27" colspan="3" rowspan="3">
                Сч. №</td>
            <td class="style27" colspan="16" rowspan="3">
                <%=Server.HtmlEncode(_seller.CorrAccount)%></td>
        </tr>
        <tr height="14">
            <td class="style28" colspan="18" height="28" width="306">
                <%=Server.HtmlEncode(_seller.CompanyName)%></td>
        </tr>
        <tr height="16">
            <td class="style29" colspan="18" height="16">Получатель <%=SiteContext.Current.CurrentClient.Profile.ClientName %></td>
            
        </tr>
    </table>
</p>

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
        .style1
        {
            height: 12.95pt;
            width: 65pt;
            color: windowtext;
            font-size: 9.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: middle;
            white-space: nowrap;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style2
        {
            width: 13pt;
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style3
        {
            width: 117pt;
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: center;
            vertical-align: bottom;
            white-space: nowrap;
            border-left-style: none;
            border-left-color: inherit;
            border-left-width: medium;
            border-right-style: none;
            border-right-color: inherit;
            border-right-width: medium;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style4
        {
            width: 13pt;
            color: windowtext;
            font-size: 9.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-left-style: none;
            border-left-color: inherit;
            border-left-width: medium;
            border-right-style: none;
            border-right-color: inherit;
            border-right-width: medium;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style5
        {
            width: 13pt;
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-left-style: none;
            border-left-color: inherit;
            border-left-width: medium;
            border-right-style: none;
            border-right-color: inherit;
            border-right-width: medium;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style6
        {
            width: 130pt;
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: center;
            vertical-align: bottom;
            white-space: nowrap;
            border-left-style: none;
            border-left-color: inherit;
            border-left-width: medium;
            border-right-style: none;
            border-right-color: inherit;
            border-right-width: medium;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style7
        {
            height: 11.1pt;
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style8
        {
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style9
        {
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial;
            text-align: center;
            vertical-align: top;
            white-space: nowrap;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style10
        {
            height: 12.95pt;
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style11
        {
            color: windowtext;
            font-size: 9.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style12
        {
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: center;
            vertical-align: bottom;
            white-space: nowrap;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style13
        {
            height: 12.95pt;
            color: windowtext;
            font-size: 9.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: middle;
            white-space: nowrap;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style14
        {
            color: windowtext;
            font-size: 9.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-left-style: none;
            border-left-color: inherit;
            border-left-width: medium;
            border-right-style: none;
            border-right-color: inherit;
            border-right-width: medium;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style15
        {
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-left-style: none;
            border-left-color: inherit;
            border-left-width: medium;
            border-right-style: none;
            border-right-color: inherit;
            border-right-width: medium;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style16
        {
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: center;
            vertical-align: bottom;
            white-space: nowrap;
            border-left-style: none;
            border-left-color: inherit;
            border-left-width: medium;
            border-right-style: none;
            border-right-color: inherit;
            border-right-width: medium;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style17
        {
            height: 36.0pt;
            width: 481pt;
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial;
            text-align: center;
            vertical-align: middle;
            white-space: normal;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style18
        {
            height: 24.05pt;
            width: 234pt;
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: top;
            white-space: normal;
            border-left: .5pt solid black;
            border-right: .5pt solid black;
            border-top: .5pt solid black;
            border-bottom-style: none;
            border-bottom-color: inherit;
            border-bottom-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style19
        {
            width: 39pt;
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: middle;
            white-space: nowrap;
            border: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style20
        {
            width: 208pt;
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: middle;
            white-space: nowrap;
            border-left: .5pt solid black;
            border-right: .5pt solid black;
            border-top: .5pt solid black;
            border-bottom-style: none;
            border-bottom-color: inherit;
            border-bottom-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style21
        {
            height: 22.2pt;
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: top;
            white-space: nowrap;
            border: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style22
        {
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: top;
            white-space: nowrap;
            border-left: .5pt solid black;
            border-right: .5pt solid black;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style23
        {
            height: 11.1pt;
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-left: .5pt solid black;
            border-right: .5pt solid black;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style24
        {
            height: 12.95pt;
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: middle;
            white-space: nowrap;
            border-left: .5pt solid black;
            border-right-style: none;
            border-right-color: inherit;
            border-right-width: medium;
            border-top: .5pt solid black;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style25
        {
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: middle;
            white-space: nowrap;
            border-left-style: none;
            border-left-color: inherit;
            border-left-width: medium;
            border-right: .5pt solid black;
            border-top: .5pt solid black;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style26
        {
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: middle;
            white-space: nowrap;
            border-left: .5pt solid black;
            border-right-style: none;
            border-right-color: inherit;
            border-right-width: medium;
            border-top: .5pt solid black;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style27
        {
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: top;
            white-space: nowrap;
            border: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style28
        {
            height: 22.2pt;
            width: 234pt;
            color: windowtext;
            font-size: 10.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: top;
            white-space: normal;
            border-left: .5pt solid black;
            border-right: .5pt solid black;
            border-top: .5pt solid black;
            border-bottom-style: none;
            border-bottom-color: inherit;
            border-bottom-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style29
        {
            height: 12.0pt;
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: left;
            vertical-align: bottom;
            white-space: nowrap;
            border-left: .5pt solid black;
            border-right: .5pt solid black;
            border-top-style: none;
            border-top-color: inherit;
            border-top-width: medium;
            border-bottom: .5pt solid black;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
        }
        .style30
        {
            height: 35.1pt;
            width: 592pt;
            color: windowtext;
            font-size: 8.0pt;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            text-align: center;
            vertical-align: middle;
            white-space: normal;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
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
				<b>
                <br />
                Счет на оплату № _____ от <%= DateTime.Now.ToString("dd MMM yyyy") %></b><hr />
			</div>
		
		<div class="other_text">
			<span>Поставщик: </span><span><b><%= SupplierName %></b></span>
		</div>
		<div class="other_text">       
			<span>Покупатель: </span><span><b><%= ClientName %></b></span>
		</div>
		
		<div class="other_text">       
			<span>Грузополучатель: </span><span><b><%= ClientName %></b></span>
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
		    <td> </td>
			<td> </td>
		    <td> </td>
			<td> </td>
			<td align="right" class="other_text"><b>Всего:</b></td>
			<td align="right" class="other_text"><b><%= SumTotal.ToString("### ### ##0.00")%></b></td>
		</tr>		
		
		
		<tr>
			<td colspan="2"><hr /></td>
		</tr>
	</table>
	
	<div colspan="2" class="other_text"><span>Всего наименований <%=QtyTotal %>, на сумму <%= SumTotal.ToString("### ### ##0.00") %></span> </div>
    <br />
	<table width="605" cellpadding="0" cellspacing="0">   
		<tr>
		    <td> 
                <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:
 collapse;width:481pt" width="629">
                    <colgroup>
                        <col span="37" style="mso-width-source:userset;mso-width-alt:725;
 width:13pt" width="17" />
                    </colgroup>
                    <tr height="17">
                        <td class="style1" colspan="5" height="17" width="85">
                            Руководитель</td>
                        <td class="style2" width="17">
                        </td>
                        <td class="style3" colspan="9" width="153">
                            &nbsp;</td>
                        <td class="style2" width="17">
                        </td>
                        <td class="style4" width="17">
                            &nbsp;</td>
                        <td class="style4" width="17">
                            &nbsp;</td>
                        <td class="style4" width="17">
                            &nbsp;</td>
                        <td class="style4" width="17">
                            &nbsp;</td>
                        <td class="style4" width="17">
                            &nbsp;</td>
                        <td class="style4" width="17">
                            &nbsp;</td>
                        <td class="style4" width="17">
                            &nbsp;</td>
                        <td class="style4" width="17">
                            &nbsp;</td>
                        <td class="style4" width="17">
                            &nbsp;</td>
                        <td class="style5" width="17">
                            &nbsp;</td>
                        <td class="style2" width="17">
                        </td>
                        <td class="style6" colspan="10" width="170">
                            &nbsp;</td>
                    </tr>
                    <tr height="14">
                        <td class="style7" height="14">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style9" colspan="9">
                            должность</td>
                        <td class="style8">
                        </td>
                        <td class="style9" colspan="10">
                            подпись</td>
                        <td class="style8">
                        </td>
                        <td class="style9" colspan="10">
                            расшифровка подписи</td>
                    </tr>
                    <tr height="17">
                        <td class="style10" height="17">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style12">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                    </tr>
                    <tr height="17">
                        <td class="style13" colspan="11" height="17" style="mso-ignore: colspan">
                            Главный (старший) бухгалтер</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                            &nbsp;</td>
                        <td class="style8">
                        </td>
                        <td class="style14">
                            &nbsp;</td>
                        <td class="style14">
                            &nbsp;</td>
                        <td class="style14">
                            &nbsp;</td>
                        <td class="style14">
                            &nbsp;</td>
                        <td class="style14">
                            &nbsp;</td>
                        <td class="style14">
                            &nbsp;</td>
                        <td class="style14">
                            &nbsp;</td>
                        <td class="style14">
                            &nbsp;</td>
                        <td class="style14">
                            &nbsp;</td>
                        <td class="style15">
                            &nbsp;</td>
                        <td class="style8">
                        </td>
                        <td class="style16" colspan="10">
                            &nbsp;</td>
                    </tr>
                    <tr height="14">
                        <td class="style7" height="14">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style8">
                        </td>
                        <td class="style9" colspan="10">
                            подпись</td>
                        <td class="style8">
                        </td>
                        <td class="style9" colspan="10">
                            расшифровка подписи</td>
                    </tr>
                </table>
            </td>
			<td width="54px" class="other_text">&nbsp;</td>
			<td width="160px">&nbsp;</td>
			<td width="52px" class="other_text">&nbsp;</td>
			<td>&nbsp;</td> 
		</tr>
	</table>

</body>
</html>
