<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.LeftMenu" %>
<%@ Register TagPrefix="culture" TagName="Culture" Src="~/Controls/Culture.ascx" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<script>
    $(document).ready(function () {
        var s = window.location.href;
        $("ul li").removeClass("active");
        $("ul li a[href*='" + s.split('/')[4] + "']").parent().addClass("active");
        $("ul li a[href*='" + s.split('/')[4] + "']").parent().parent().parent().addClass("active");
    });
</script>
<nav class="navbar-default navbar-static-side" role="navigation">

    <div class="sidebar-collapse">
        <ul class="nav" id="side-menu">
            <li class="nav-header">
                <div class="dropdown profile-element">
                    <span>
                        <img alt="image" class="img-circle" src="../Images/persona.png" />
                    </span>
                    <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                        <span class="clear">
                            <span class="">
                                <strong class="font-bold"><%= global::Resources.Texts.Manager %>:</strong>
                            </span>
                            <span class="">
                                <asp:Label runat="server" ID="_managerNameLabel" Font-Bold="true"><%= SiteContext.Current.UserDisplayName %></asp:Label><b class="caret"></b></span>
                            </span>
                    </a>
                    <ul class="dropdown-menu animated fadeInRight m-t-xs">
                        <li><a href="#">Профиль</a></li>
                        <li><a href="#">Клиенты</a></li>
                        <li><a href="#">Сообщения</a></li>
                        <li class="divider"></li>
                        <li>
                            <asp:LinkButton runat="server" ID="_logoffButton" OnClick="_logoffButton_Click" CausesValidation="false"><%= global::Resources.Texts.Logoff %></asp:LinkButton></li>
                    </ul>
                </div>
            </li>
            <li class="active">
                <ul class="nav nav-second-level">
                    <a href="#"><i class="fa fa-th-large"></i><span class="nav-label"><%= Resources.Texts.WorkWithClients %></span> <span class="fa arrow"></span></a>
                    <li class="active">>
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Manager/RegisterClient.aspx"><%= global::Resources.Texts.CreationNewClient %></asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Manager/SelectClient.aspx"><%= global::Resources.Texts.ClientsList %></asp:HyperLink></li>

                    <li>
                        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Manager/AllOrders.aspx"><%= global::Resources.Texts.AllOrders %></asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Manager/UploadStatuses.aspx"><%= global::Resources.Texts.UploadStatuses %></asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/Manager/ClientOrdelinesLoad.aspx"><%= global::Resources.Texts.Dispatches %></asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/Manager/ClientPayments.aspx"><%= global::Resources.Texts.SettlementPayments %></asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/Manager/UploadSpareParts.aspx"><%= global::Resources.Texts.UploadSpareparts %></asp:HyperLink></li>
                </ul>
            </li>
            <li>
                <ul class="nav nav-second-level">
                    <a href="#"><i class="fa fa-th-large"></i><span class="nav-label"><%= Resources.Texts.Reports %></span> <span class="fa arrow"></span></a>

                    <li>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Manager/SupplyReportT.aspx"><%= global::Resources.Texts.GoodsReceiptReport %></asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/Manager/SaleReportTable.aspx"><%= global::Resources.Texts.SalesReport %></asp:HyperLink></li>
                    <li>
                        <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/Manager/FinancialReport.aspx"><%= global::Resources.Texts.FinancialReport %></asp:HyperLink></li>
                </ul>
            </li>
        </ul>
    </div>
</nav>
