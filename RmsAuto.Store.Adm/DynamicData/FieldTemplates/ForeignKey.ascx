<%@ Control Language="C#" CodeBehind="ForeignKey.ascx.cs" Inherits="RmsAuto.Store.Adm.ForeignKeyField" %>

<asp:HyperLink ID="HyperLink1" runat="server"
    Text="<%# GetDisplayString() %>"
    NavigateUrl="<%# GetNavigateUrl() %>"  />