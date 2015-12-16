<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="RegisterClient.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.RegisterClient" Title="Manager working place" %>
<%@ Register src="Controls/FillClientProfile.ascx" tagname="FillClientProfile" tagprefix="uc1" %>
<%@ Register src="Controls/FillClientProfileExt.ascx" tagname="FillClientProfileExt" tagprefix="uc1" %>

<asp:Content ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <h1><%= global::Resources.Texts.RegisterNewClient %></h1>
    <div id="_profilePane" runat="server">
        <% if (ClientAlreadyExists)
            { %>
            <span class="error">
                Ошибка сохранения профиля клиента.<br />
                Система уже содержит профиль с указанным названием клиента и телефоном.
            </span>
        <% } %>   
        <uc1:FillClientProfile ID="_fillClientProfile" runat="server" />
        <uc1:FillClientProfileExt ID="_fillClientProfileExt" runat="server" />
        <br />
        <asp:Button ID="_btnRegister" Class="btn btn-primary btn-sm" runat="server" Text="<%$ Resources:RegistrationTexts, Register %>" onclick="_btnRegister_Click" /> 
    </div>    
    <div id="_msgPane" runat="server" visible="false">
        <% if (ProfileCreated)
           { %>
            Profile is successfully saved. After loading profile into accounting system you can find customer using the search.
        <% } %>
        <% if (OnineAccessOfferSubmitted)
           { %>
        <br />The letter with activation reference is send on the email pointed.
        <% } %>
    </div>
</asp:Content>
