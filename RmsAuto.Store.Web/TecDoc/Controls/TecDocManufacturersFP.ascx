<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecDocManufacturersFP.ascx.cs" Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecDocManufacturersFP" EnableViewState="false" %>
<asp:DataList ID="_carDataList" runat="server" RepeatColumns="4" CellSpacing="2" Width="100%" ItemStyle-Width="25%" ItemStyle-CssClass="item">
	<ItemTemplate>
		<asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl='<%#GetManufacturerUrl(Container.DataItem)%>'>
			<%# Server.HtmlEncode((string)Eval("Brand.Name")) %>
		</asp:HyperLink>
	</ItemTemplate>
</asp:DataList>