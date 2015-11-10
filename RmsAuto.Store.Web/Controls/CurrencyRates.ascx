<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrencyRates.ascx.cs"
	Inherits="RmsAuto.Store.Web.Controls.CurrencyRates" %>
<div class="kurs" runat="server" id="_currencyRatesPanel">
	<img runat="server" src="<%$ Resources:ImagesURL, rurs_img %>" alt="" width="76" height="13" border="0" />
	<table cellspacing="2" cellpadding="2" border="0" width="190">
		<tr runat="server" id="_usdRow">
			<td><asp:Literal ID="lDollar" runat="server" Text="Доллар" /></td>
			<td><asp:Literal ID="lUSD" runat="server" Text="USD" /></td>
			<td width="100%"><div class="ppt">&nbsp;</div></td>
			<td nowrap><b><asp:Literal runat="server" ID="_usdRateLabel" /></b> <asp:Literal ID="lRub" runat="server" Text="руб" /></td>
		</tr>
		<tr runat="server" id="_eurRow">
			<td><asp:Literal ID="lEuro" runat="server" Text="Евро" /></td>
			<td><asp:Literal ID="lEUR" runat="server" Text="EUR" /></td>
			<td width="100%"><div class="ppt">&nbsp;</div></td>
			<td nowrap><b><asp:Literal runat="server" ID="_eurRateLabel" /></b> <asp:Literal ID="lRub2" runat="server" Text="руб" /></td>
		</tr>
	</table>
</div>
