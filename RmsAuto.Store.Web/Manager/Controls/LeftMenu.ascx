<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.LeftMenu" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>

<table class="menu_off">
    <tr>
        <td class="icon"><span><img id="Img1" runat="server" src="/images/menu-off.png" width="9" height="9" /></span></td>
        <td><a href="#"><%= global::Resources.Texts.WorkWithClients %></a></td>
    </tr>
</table>
<div class="submenu">
	<ul>
		<li><asp:HyperLink ID="hlRegisterClient" runat="server" NavigateUrl="~/Manager/RegisterClient.aspx"><%= global::Resources.Texts.CreationNewClient %></asp:HyperLink></li>
		<li><asp:HyperLink ID="hlSelectClient" runat="server" NavigateUrl="~/Manager/SelectClient.aspx"></asp:HyperLink></li>
        
		<li><asp:HyperLink ID="hlAllOrders" runat="server" NavigateUrl="~/Manager/AllOrders.aspx"><%= global::Resources.Texts.AllOrders %></asp:HyperLink></li>
		<li><asp:HyperLink ID="hlUploadStatuses" runat="server" NavigateUrl="~/Manager/UploadStatuses.aspx"><%= global::Resources.Texts.UploadStatuses %></asp:HyperLink></li>
		<li><asp:HyperLink ID="hlClientLoad" runat="server" NavigateUrl="~/Manager/ClientOrdelinesLoad.aspx"><%= global::Resources.Texts.Dispatches %></asp:HyperLink></li>
		<li><asp:HyperLink ID="hlClientPayments" runat="server" NavigateUrl="~/Manager/ClientPayments.aspx"><%= global::Resources.Texts.SettlementPayments %></asp:HyperLink></li>
        <li><asp:HyperLink ID="hlUploadSpareparts" runat="server" NavigateUrl="~/Manager/UploadSpareParts.aspx">Загрузка номенклатуры</asp:HyperLink></li>
    </ul>
</div>
<table id="p_reports_header" class="menu_off" runat="server">
    <tr>
        <td class="icon"><span><img id="Img2" runat="server" src="/images/menu-off.png" width="9" height="9" /></span></td>
        <td><a href="#"><%= global::Resources.Texts.Reports %></a></td>
    </tr>
</table>
<div id="p_reports" class="submenu" runat="server">
	<ul>
		<li><asp:HyperLink ID="hlSupplyReport" runat="server" NavigateUrl="~/Manager/SupplyReportT.aspx"><%= global::Resources.Texts.GoodsReceiptReport %></asp:HyperLink></li>
		<li><asp:HyperLink ID="hlSaleReport" runat="server" NavigateUrl="~/Manager/SaleReportTable.aspx"><%= global::Resources.Texts.SalesReport %></asp:HyperLink></li>
		<li><asp:HyperLink ID="hlFinancialReport" runat="server" NavigateUrl="~/Manager/FinancialReport.aspx"><%= global::Resources.Texts.FinancialReport %></asp:HyperLink></li>
		<%--<li><asp:HyperLink ID="hlDebtReport" runat="server" NavigateUrl="~/Manager/Reports/DebtReport.aspx">Debt receivable report</asp:HyperLink></li>--%>
	</ul>
</div>