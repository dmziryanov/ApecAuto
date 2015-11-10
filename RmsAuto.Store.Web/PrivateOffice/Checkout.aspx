<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="RmsAuto.Store.Web.Checkout" meta:resourcekey="PageResource1" %>
<%@ Register src="~/Controls/PlaceOrder.ascx" tagname="PlaceOrder" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlaceOrderWhSl.ascx" tagname="PlaceOrderWhSl" tagprefix="uc4" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc3" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Store.Web" Assembly="RmsAuto.Store" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">

    <uc2:LeftMenu ID="_leftMenu" runat="server" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />
	<asp:PlaceHolder runat="server" ID="_checkoutPlaceHolder">

		<rmsauto:ShoppingCartVersionValidator ID="_cartVersionValidator" runat="server" Mode="Checkout" 
			ErrorMessage="<%$ Resources:ValidatorsMessages, InvalidCartVersion2 %>" />
		<uc1:PlaceOrder ID="_placeOrder" runat="server" Visible="false" />
		<uc4:PlaceOrderWhSl ID="_placeOrderWhSl" runat="server" Visible="false" />
	    
    </asp:PlaceHolder>
    
    <asp:PlaceHolder runat="server" ID="_retailRefusalPlaceHolder">
    
		<h1><asp:Literal ID="lWelcome" runat="server" Text="Уважаемые Дамы и Господа!" 
				meta:resourcekey="lWelcomeResource1" /></h1>
		
        <asp:Literal ID="lNeedPhone" runat="server" 
			Text="Для возможности оформления заказа на нашем сайте, просьба связаться с нашим менеджером по телефону +7(495) 585-5-585." 
			meta:resourcekey="lNeedPhoneResource1" />
    
    </asp:PlaceHolder>
</asp:Content>

