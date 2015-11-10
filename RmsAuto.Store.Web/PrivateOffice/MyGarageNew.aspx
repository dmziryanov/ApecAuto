<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="MyGarageNew.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.MyGarageNew" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc2" %>
<%@ Register src="~/PrivateOffice/AuthorizationControl.ascx" tagname="AuthorizationControl" tagprefix="uc2" %>
<%@ Register Src="~/Controls/GarageCarEdit.ascx"  TagPrefix="uc3" TagName="GarageCarEdit" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
<uc2:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="Client" />

<h1><%=global::Resources.Texts.VinRequests_Garage %></h1>
<span class="right_block" style="float:left;"><a href="<%=UrlManager.GetGarageUrl() %>"><%=global::Resources.Texts.VinRequests_BackToGarageList %></a></span><br />
<br />

<div style="margin: 14px 0 0 0"
      onkeypress="javascript: if(event.keyCode==13) { <%=Page.GetPostBackClientEvent(btnAddCar, String.Empty)%>; return false; }">
    <h2><%=global::Resources.Texts.VinRequests_NewGarageCar %></h2>
    
    <uc3:GarageCarEdit runat="server" ID="_garageCarEdit" />

</div>

<br />
<asp:ImageButton ID="btnAddCar" runat="server" OnClick="btnAddCarClick" ImageUrl="<%$ Resources:ImagesURL, cart_add %>" ValidationGroup="CarEditGroup" />
</asp:Content>