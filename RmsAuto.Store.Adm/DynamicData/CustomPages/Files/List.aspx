<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.Files.List" Title="Untitled Page" %>
<%@ Register src="../../../Controls/FileManager/FileManagerControl.ascx" tagname="FileManagerControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	<h2>Файлы</h2>

	<uc1:FileManagerControl ID="FileManagerControl1" runat="server" />
</asp:Content>
