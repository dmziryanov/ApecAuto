<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcctgPing.aspx.cs" Inherits="RmsAuto.Store.Web.Controls.AcctgPing1" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body style="font-size: 10px; font-family: Tahoma;background-color:#FFFFCF;">
    <form id="form1" runat="server">
    <div>
		<table width="100%">
		<tr>
		<td nowrap><asp:Literal ID="lSendReq" runat="server" Text="Запрос отправлен:" /></td>
		<td nowrap><asp:Literal ID="_ltrWebTime" runat="server" /></td>
		</tr>
		<tr>
		<td nowrap><asp:Literal ID="lHaveAns" runat="server" Text="Ответ получен:" /></td>
		<td nowrap><asp:Literal ID="_ltrRecvTime" runat="server" /></td>
		</tr>
		<tr>
		<td nowrap><asp:Literal ID="lDelay" runat="server" Text="Задержка, секунд:" /></td>
		<td nowrap><asp:Literal ID="_ltrDelayTime" runat="server" /></td>
		</tr>
		<tr>
		<td nowrap><asp:Literal ID="lReMess" runat="server" Text="Ответное сообщение:" /></td>
		<td><asp:Literal ID="_ltrAcctgTime" runat="server" /></td>
		</tr>
		<tr>
		<td colspan="2"><asp:Button runat="server" ID="PingButton" OnClick="PingButton_Click" Text="Повторить запрос" /></td>
		</tr>
		</table>
		
    </div>
    </form>
</body>
</html>
