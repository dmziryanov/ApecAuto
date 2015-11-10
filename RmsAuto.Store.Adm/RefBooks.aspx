<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="DeskBooks.aspx.cs" Inherits="RmsAuto.Store.Adm.DeskBooks" %>
<%@ Register src="Controls/MetaModelTables.ascx" tagname="MetaModelTables" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc1:metamodeltables ID="MetaModelTables1" runat="server" 
        DataContextType="RmsAuto.Store.Entities.dcCommonDataContext, RmsAuto.Store" 
        HeaderText="Общие справочники" />
</asp:Content>
