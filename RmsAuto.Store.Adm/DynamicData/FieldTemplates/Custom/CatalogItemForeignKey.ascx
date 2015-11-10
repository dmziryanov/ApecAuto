<%@ Control Language="C#" CodeBehind="CatalogItemForeignKey.ascx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.FieldTemplates.Custom.ForeignKeyField" %>

<asp:HyperLink ID="HyperLink1" runat="server"
    Text="<%# GetDisplayString() %>"
    NavigateUrl="<%# GetNavigateUrl() %>"  />