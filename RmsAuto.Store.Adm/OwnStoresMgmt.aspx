<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OwnStoresMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.OwnStocksMgmt" %>
<%@ Register src="Controls/MetaModelTables.ascx" tagname="MetaModelTables" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc1:metamodeltables ID="MetaModelTables1" runat="server" DataContextType="RmsAuto.Store.Entities.dcCommonDataContext, RmsAuto.Store" HeaderText="" />
</asp:Content>
