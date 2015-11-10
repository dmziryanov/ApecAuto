<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.LeftMenu" %>
<%@ Register src="QuickSearch.ascx" tagname="QuickSearch" tagprefix="uc1" %>
<%@ Register src="~/Cms/Catalog/MainMenu.ascx" tagname="MainMenu" tagprefix="uc2" %>
<%@ Register src="~/Cms/Catalog/HelpMenu.ascx" tagname="HelpMenu" tagprefix="uc3" %>

<uc1:QuickSearch ID="QuickSearch1" runat="server" />
<uc2:MainMenu ID="MainMenu1" runat="server" />
<% if(Page.Request.ServerVariables["SCRIPT_NAME"].IndexOf("/Default.aspx") < 0 && Page.Request.ServerVariables["SCRIPT_NAME"].IndexOf("/default.aspx") < 0) {%>
<br />

<asp:PlaceHolder ID="phVisa" runat="server" Visible="false">
	<div style="margin:4px 0px; text-align:center;">
		<a href="/Help/HowToPay.aspx"><img style="border:0px;" src="../images/visa.jpg" alt="" /></a>
	</div>
</asp:PlaceHolder>
<% if(Page.Request.ServerVariables["SCRIPT_NAME"].IndexOf("/Default.aspx") < 0 && Page.Request.ServerVariables["SCRIPT_NAME"].IndexOf("/default.aspx") < 0) {%>

<uc3:HelpMenu ID="HelpMenu1" runat="server" />
<div class="counter_def">
<% } else  {%>
<div class="counter">
<% } %>

<noindex> 
<!--LiveInternet counter-->

<%=liveInternetLabel %>

<!--/LiveInternet-->
</noindex>

</div>

<noindex>

<%=googleScript %>
    
    <!-- Yandex.Metrika counter -->
<%=yandexScript %>
    <!-- /Yandex.Metrika counter -->
</noindex>

    <% } %>
<%-- тестовый контрол--%>
<br />
<asp:PlaceHolder ID="_pingControlPlaceHolder" runat="server"></asp:PlaceHolder>