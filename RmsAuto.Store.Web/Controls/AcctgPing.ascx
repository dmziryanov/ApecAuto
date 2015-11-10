<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AcctgPing.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.AcctgPing" %>
<div style="border: solid 1px gray;background-color:#FFFFCF;padding:5px;">
<asp:Literal ID="lHansa" runat="server" Text="Hansa:" /><br /> 
<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ReferencesDemo.aspx"><asp:Literal ID="lDict" runat="server" Text="справочники - демо" /></asp:HyperLink><br />
<asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/AcctgTrace.axd"><asp:Literal ID="lTrace" runat="server" Text="трассировка запросов" /></asp:HyperLink>
<iframe src="<%=ResolveUrl("~/Controls/AcctgPing.aspx")%>" width="205" height="100" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" ></iframe>
<asp:Literal runat="server" ID="ltLastModifiedDate" />
</div>
