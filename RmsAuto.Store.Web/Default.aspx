<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RmsAuto.Store.Web.Default" %>

<%@ Register Src="Cms/News/NewsTopList.ascx" TagName="NewsTopList" TagPrefix="uc1" %>
<%@ Register Src="Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc2" %>
<%@ Register Src="Controls/RightMenu.ascx" TagName="RightMenu" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc3" TagName="TecDocManufacturers" Src="~/TecDoc/Controls/TecDocManufacturers.ascx" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="_mainContentPlaceHolder">
    
    <uc3:TecDocManufacturers ID="_tecDocManufacturers" runat="server" />
    <uc1:NewsTopList ID="NewsTopList1" runat="server"/>
    
    <h1>Каталоги онлайн</h1>
    <asp:PlaceHolder runat="server" ID="_pageBodyPlaceHolder" EnableViewState="false">
		<asp:Literal runat="server" ID="_pageBodyLiteral" EnableViewState="false" />
    </asp:PlaceHolder>
    
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="_leftContentPlaceHolder">
    
</asp:Content>
