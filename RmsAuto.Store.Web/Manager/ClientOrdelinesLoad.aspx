<%@ Page Language="C#"  MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="ClientOrdelinesLoad.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.ClientOrdelinesLoad" %>
<%@ Register src="Controls/ClientOrderLines.ascx" tagname="ClientsFilter" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server" >
	<uc4:ClientsFilter ID="ClientsFilter1" runat="server"  />
</asp:Content>

