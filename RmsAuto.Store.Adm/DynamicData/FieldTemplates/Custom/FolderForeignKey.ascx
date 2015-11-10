<%@ Control Language="C#" CodeBehind="FolderForeignKey.ascx.cs" Inherits="RmsAuto.Store.Adm.FolderForeignKeyField" %>

<asp:HyperLink ID="HyperLink1" runat="server"
    Text="<%# GetDisplayString() %>"
    NavigateUrl="<%# GetNavigateUrl() %>"  />