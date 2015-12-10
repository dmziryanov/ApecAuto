<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.LeftMenu" %>





<% if(Page.Request.ServerVariables["SCRIPT_NAME"].IndexOf("/Default.aspx") < 0 && Page.Request.ServerVariables["SCRIPT_NAME"].IndexOf("/default.aspx") < 0) {%>
<br />

<asp:PlaceHolder ID="phVisa" runat="server" Visible="false">
	<div style="margin:4px 0px; text-align:center;">
		<a href="/Help/HowToPay.aspx"><img style="border:0px;" src="../images/visa.jpg" alt="" /></a>
	</div>
</asp:PlaceHolder>
<% if(Page.Request.ServerVariables["SCRIPT_NAME"].IndexOf("/Default.aspx") < 0 && Page.Request.ServerVariables["SCRIPT_NAME"].IndexOf("/default.aspx") < 0) {%>


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