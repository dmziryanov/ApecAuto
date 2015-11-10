<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StoreMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.StoreMgmt" %>
<%@ Register src="Controls/MetaModelTables.ascx" tagname="MetaModelTables" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc1:MetaModelTables ID="MetaModelTables1" DataContextType="RmsAuto.Store.Entities.StoreDataContext, RmsAuto.Store" HeaderText="Справочники" runat="server" />
</asp:Content>
