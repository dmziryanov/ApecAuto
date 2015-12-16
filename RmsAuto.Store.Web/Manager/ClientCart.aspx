<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="ClientCart.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientCart" Title="Manager working place" %>
<%@ Register src="Controls/ClientSubMenu.ascx" tagname="ClientSubMenu" tagprefix="uc1" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<%@ Register src="../Controls/ShoppingCartItems.ascx" tagname="ShoppingCartItems" tagprefix="uc3" %>
<%@ Register src="Controls/PlaceOrderOptions.ascx" tagname="OrderOptions" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc2:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<uc1:ClientSubMenu ID="_clientSubMenu" runat="server" />
	<uc3:ShoppingCartItems ID="ShoppingCartItems1" runat="server" />
	<div class="link_block"><asp:HyperLink runat="server" ID="_printWithPartNumbers" Text="Printable version" Target="_blank" /></div>
	<div class="link_block"><asp:HyperLink runat="server" ID="_printWithoutPartNumbers" Text="Printable version without Part Nos" Target="_blank" /></div>
	<asp:Panel ID="_placeOrderPanel" runat="server">
	    <hr />
	    <h4>Order placement parameters</h4>
	    <uc1:OrderOptions id="_orderOptions" runat="server" />
	    <asp:LinkButton ID="_btnPlaceOrder" runat="server" Class="btn btn-primary btn-sm" Text="<%$Resources:Texts, PlaceAnOrder %>" 
	    OnClientClick="javascript:return validateAndConfirm()" onclick="_btnPlaceOrder_Click" />
    </asp:Panel>
    <asp:Label runat="server" ID="_errorLabel" CssClass="error" EnableViewState="false" />    
    <script type="text/javascript">
        function validateAndConfirm() {
            Page_ClientValidate();
            if (Page_IsValid) {
                return confirm('The order will be placed. Are you sure?');
            }
            return Page_IsValid;
        }
    </script>
</asp:Content>


