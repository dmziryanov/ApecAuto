<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentOrderPrint.aspx.cs"
	Inherits="RmsAuto.Store.Web.PrivateOffice.PaymentOrderPrint" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="ClientOrManager" />

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=windows-1251" />
	<title>Квитанция СБ РФ (ПД-4)</title>
	<style>
	    body
	    {
	    	text-align:center;
	    }
	    .blank 
	    {
	    	width:17cm;
	    	border-top:1px solid #000000;
	    	border-left:1px solid #000000;
	    }
	    .blank, .blank tr td
	    {
			font-family: "Times New Roman" , Times, serif;
			font-size: 12px;
	    }
		.blank tr td strong
		{
			font-family: "Times New Roman" , Times, serif;
			font-size: 11px;
		}
	    .blank td.left
	    {
	    	width:5.5cm;
	    	vertical-align:top;
	    	text-align:center;
	    	border-bottom: #000000 1px solid;
			border-right: #000000 1px solid;
	    }
	    .blank td.right
	    {
	    	vertical-align:top;
	    	text-align:left;
	    	border-bottom: #000000 1px solid;
			border-right: #000000 1px solid;
	    }
		p
		{
			padding: 5px 0px 0px 5px;
		}
		ul 
		{
			padding:5px 0px 0px 10px;
			margin:0px;
		}
		li
		{
			list-style-type: none;
			padding-bottom: 5px;
			padding: 6px 0px 0px 5px;
			margin:0px;
		}
	</style>
</head>
<body>
			<table class="blank" cellpadding="0" cellspacing="0" border=0>
				<tr>
					<td class="left">
						&nbsp;<strong>Платеж</strong>
					</td>
					<td class="right">
						<ul>
						<li><strong>Получатель: </strong><font style="font-size: 90%"><%=Server.HtmlEncode(_seller.CompanyName)%><%--____________________________________________________--%></font>&nbsp;&nbsp;&nbsp;<br />
						</li>
						<li><strong>КПП:</strong><%=Server.HtmlEncode(_seller.KPP)%>&nbsp;&nbsp;&nbsp;&nbsp; <strong>ИНН:</strong> <%=Server.HtmlEncode(_seller.Inn)%><%--____________--%>&nbsp;&nbsp;<font
							style="font-size: 11px"> &nbsp;</font>
							<br />
						</li>
						<li><strong>Код ОКАТО:</strong>___________&nbsp;&nbsp;&nbsp;&nbsp;<strong>P/сч.:</strong> <%=Server.HtmlEncode(_seller.BankAccount) %><%--____________________--%>&nbsp;&nbsp; &nbsp; </li>
						<li><strong>в:</strong> <font style="font-size: 90%"><%=Server.HtmlEncode(_seller.BankName)%><%--______________________________________________________________--%></font><br />
						</li>
						<li><strong>БИК:</strong> <%=Server.HtmlEncode(_seller.BankCode)%><%--_________--%>&nbsp; <strong>К/сч.:</strong><%=Server.HtmlEncode(_seller.CorrAccount)%><%--____________________--%><br />
						</li>
						<li><strong>Код бюджетной классификации (КБК):</strong> ____________________ </li>
						<li><strong>Назначение платежа:</strong> <font style="font-size: 90%"><%=_paymentName %><%--_____________________________________________________--%></font><br />
						</li>
						<li><strong>Плательщик:</strong> <%=Server.HtmlEncode(_profile.ClientName) %><%--_________________________________________________--%><br />
						</li>
						<li><strong>Адрес плательщика:</strong> <font style="font-size: 90%">____________________________________________</font><br />
						</li>
						<li><strong>ИНН плательщика:</strong> ____________&nbsp;&nbsp;&nbsp;&nbsp; <strong>№
							л/сч. плательщика:</strong> ______________ </li>
						<li><strong>Сумма:</strong> _______ руб. __ коп. &nbsp;&nbsp;&nbsp;<strong>
						Сумма оплаты услуг банка:</strong> ____ руб. __ коп.<br />
							<br />
							Подпись:________________________ Дата: &quot;___&quot;&nbsp;_________&nbsp;______ г.<br />
							<span style="margin-left:100px;"><strong>Оплата от третьих лиц недействительна!</strong></span>
							<br />
						</li>
						</ul>
					</td>
				</tr>
				<tr>
					<td class="left">
						&nbsp;<strong>Квитанция</strong>
					</td>
					<td class="right">
						<ul>
						<li><strong>Получатель: </strong><font style="font-size: 90%"><%=Server.HtmlEncode(_seller.CompanyName)%><%--____________________________________________________--%></font>&nbsp;&nbsp;&nbsp;<br />
						</li>
						<li><strong>КПП:</strong><%=Server.HtmlEncode(_seller.KPP)%>&nbsp;&nbsp;&nbsp;&nbsp; <strong>ИНН:</strong> <%=Server.HtmlEncode(_seller.Inn)%><%--____________--%>&nbsp;&nbsp;<font
							style="font-size: 11px"> &nbsp;</font>
							<br />
						</li>
						<li><strong>Код ОКАТО:</strong>___________&nbsp;&nbsp;&nbsp;&nbsp;<strong>P/сч.:</strong> <%=Server.HtmlEncode(_seller.BankAccount) %><%--____________________--%>&nbsp;&nbsp; &nbsp; </li>
						<li><strong>в:</strong> <font style="font-size: 90%"><%=Server.HtmlEncode(_seller.BankName)%><%--______________________________________________________________--%></font><br />
						</li>
						<li><strong>БИК:</strong> <%=Server.HtmlEncode(_seller.BankCode)%><%--_________--%>&nbsp; <strong>К/сч.:</strong><%=Server.HtmlEncode(_seller.CorrAccount)%><%--____________________--%><br />
						</li>
						<li><strong>Код бюджетной классификации (КБК):</strong> ____________________ </li>
						<li><strong>Назначение платежа:</strong> <font style="font-size: 90%"><%=_paymentName %><%--_____________________________________________________--%></font><br />
						</li>
						<li><strong>Плательщик:</strong> <%=Server.HtmlEncode(_profile.ClientName) %><%--_________________________________________________--%><br />
						</li>
						<li><strong>Адрес плательщика:</strong> <font style="font-size: 90%">____________________________________________</font><br />
						</li>
						<li><strong>ИНН плательщика:</strong> ____________&nbsp;&nbsp;&nbsp;&nbsp; <strong>№
							л/сч. плательщика:</strong> ______________ </li>
						<li><strong>Сумма: _______ руб. __ коп.&nbsp;&nbsp;&nbsp;<strong>Сумма
							оплаты услуг банка:</strong> ____ руб. __ коп.<br />
							<br />
							Подпись:________________________ Дата: &quot;___&quot;&nbsp;_________&nbsp;______ г.<br />
							<span style="margin-left:100px;"><strong>Оплата от третьих лиц недействительна!</strong></span>
							<br />
						</li>
						</ul>
					</td>
				</tr>
			</table>
</body>
</html>
