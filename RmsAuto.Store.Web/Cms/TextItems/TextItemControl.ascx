<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextItemControl.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.TextItems.TextItemControl" %>

<asp:PlaceHolder runat="server" ID="_headerPlaceHolder">
<h1><asp:Literal runat="server" ID="_headerLabel" /></h1>
</asp:PlaceHolder>
<div><asp:Literal runat="server" ID="_bodyLabel" /></div>