<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AcctgHansaMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.AcctgHansaMgmt" %>
<%@ Register src="Controls/MetaModelTables.ascx" tagname="MetaModelTables" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <uc1:MetaModelTables ID="MetaModelTables1" runat="server" 
    DataContextType="RmsAuto.Store.Acctg.Entities.AcctgDataContext, RmsAuto.Acctg" HeaderText="Управление Ханса" />
</asp:Content>
