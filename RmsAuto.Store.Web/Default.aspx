<%@ Page Title="" Language="C#" MasterPageFile="~/PageThreeColumnsNEW.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RmsAuto.Store.Web.Default" %>

<%@ Register Src="Cms/News/NewsTopList.ascx" TagName="NewsTopList" TagPrefix="uc1" %>
<%@ Register Src="Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc2" %>
<%@ Register Src="Controls/RightMenu.ascx" TagName="RightMenu" TagPrefix="uc3" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="_mainContentPlaceHolder">
    <uc1:NewsTopList ID="NewsTopList1" runat="server"/>
    <asp:PlaceHolder runat="server" ID="_pageBodyPlaceHolder" EnableViewState="false">
		<asp:Literal runat="server" ID="_pageBodyLiteral" EnableViewState="false" />
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="_leftContentPlaceHolder">
    <uc2:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="_rightContentPlaceHolder">
    <uc3:RightMenu ID="RightMenu1" runat="server" />
</asp:Content>