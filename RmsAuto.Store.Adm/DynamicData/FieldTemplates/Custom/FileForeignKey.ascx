<%@ Control Language="C#" CodeBehind="FileForeignKey.ascx.cs" Inherits="RmsAuto.Store.Adm.FileForeignKeyField" %>

<asp:HyperLink ID="HyperLink1" runat="server"
	Target="_blank"
    Text="<%# GetDisplayString() %>"
    NavigateUrl="<%# GetNavigateUrl() %>"  />