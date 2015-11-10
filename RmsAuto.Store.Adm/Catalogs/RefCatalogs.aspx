<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="RefCatalogs.aspx.cs" Inherits="RmsAuto.Store.Adm.Catalogs.RefCatalogs" ValidateRequest="false" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.2, Version=11.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.2, Version=11.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Import Namespace="RmsAuto.Store.Cms.Routing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <a href="Battery.aspx">Аккумуляторы</a><br/>
    <a href="Disc.aspx">Диски</a><br/>   
    <a href="Tires.aspx">Шины</a><br/>   
</asp:Content>
                         
    
                         
  
