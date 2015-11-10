<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ContentMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.ContentMgmt" %>
<%@ Register src="Controls/MetaModelTables.ascx" tagname="MetaModelTables" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
<uc1:MetaModelTables ID="MetaModelTables1" runat="server" DataContextType="RmsAuto.Store.Cms.Entities.CmsDataContext, RmsAuto.Store" HeaderText="Разделы контента" />

</asp:Content>
