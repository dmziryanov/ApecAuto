<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true"
    CodeBehind="Cart.aspx.cs" Inherits="RmsAuto.Store.Web.Cart1" %>

<%@ Register Src="~/Controls/ShoppingCartItems.ascx" TagName="ShoppingCartItems"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/PageHeader.ascx" TagName="PageHeader" TagPrefix="uc3" %>
<%@ Register Src="~/PrivateOffice/AuthorizationControl.ascx" TagName="AuthorizationControl"
    TagPrefix="uc2" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
    TagPrefix="uc4" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Store.Web" Assembly="RmsAuto.Store" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Store.BL" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
     <script type="text/javascript">
         $(document).ready(function() {
             $("img").each(function() {
                 if (this.src == undefined || this.src == "") {
                     $(this).hide();
                 }
             });


             $("input[name*='chkAddToOrder']").click(function() {
             if (this.checked) {
                         //this.checked = false; 
                    }
                    else {
                         //this.checked = true; 
                    }
             });

             function GetChkButtonsCount() {
                 
                 var cnt = 0;
                 
                 //Выбрали все кнопки и посчитали отмеченные
                 $("input[name*='chkAddToOrder']").each(function() { 
                    if (this.checked)  
                        {  cnt++; } 
                  });
                     
                    if (cnt > 0)
                    { 
                        return true; 
                    }
                 else {
                     alert('Не выбрано ни одной позиции!');
                     return false
                 }
             }

             $("input[name*='_btnCheckout']").click(GetChkButtonsCount);

         });
    </script>
    <uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="ClientOrAnonymous" />
    <h1>
        <%=global::Resources.Texts.YourCart %></h1>
    <rmsauto:ShoppingCartVersionValidator ID="_cartVersionValidator" runat="server" Mode="ChangeCartContent"
        ErrorMessage="<%$ Resources:ValidatorsMessages, InvalidCartVersion %>" />
    <uc1:ShoppingCartItems ID="ShoppingCartItems1" runat="server" />
    <uc4:TextItemControl ID="_guestPriceHint_TextItemControl" runat="server" TextItemID="ShoppingCart.GuestPriceHint"
        ShowHeader="false" />
    <span style="float:right;margin-right:5px;">
        <asp:HyperLink runat="server" ID="_printLink" Target="_blank"><%=global::Resources.Texts.PrintVersion %></asp:HyperLink></span>
    <asp:Button ID="_btnCheckout" CssClass="button" OnClick="_btnCheckout_Click" Text="<%$Resources:Texts, PlaceAnOrder %>" runat="server" />
    <% if ( CurrentClient.IsGuest )
       { %>
    <h3>
        <%=global::Resources.Texts.NeedAutorization %></h3>
    <asp:Button ID="_login" runat="server" OnClick="_btnLogon_Click" CssClass="button" Text="Autorization" /> 
    <a class="button" href="<%=UrlManager.GetRegistrationUrl() %>"><%=global::Resources.Texts.Registration %></a>
    <% }
      
       else if ( !( CurrentClient.Profile.IsChecked || ( CurrentClient.Profile.TradingVolume == TradingVolume.Retail && CurrentClient.Profile.Category == ClientCategory.Physical ) ) )
       { %>
		<h3>
		<%= ClientNotChecked %>
        </h3>
    <% }
       else if ( CurrentClient.Profile.IsRestricted )
       { %>
    <h3>
        <%=global::Resources.Texts.ClientIsRestricted %></h3>
    <% } %>
</asp:Content>
