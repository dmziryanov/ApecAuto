<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecDocManufacturers.ascx.cs" Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecDocManufacturers" EnableViewState="false" %>
<h3><asp:Literal ID="lCars" runat="server" Text="Легковые автомобили" /></h3>
<asp:DataList ID="_carDataList" runat="server" RepeatColumns="6" CellSpacing="2" Width="100%" ItemStyle-Width="17%" CssClass="catalog_list" ItemStyle-CssClass="item">
	<ItemTemplate>
		<asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl='<%#GetManufacturerUrl(Container.DataItem)%>'>
			<%# Server.HtmlEncode((string)Eval("Brand.Name")) %>
		</asp:HyperLink>
	</ItemTemplate>
</asp:DataList>
<br />
<h3><asp:Literal ID="lTraks" runat="server" Text="Грузовые автомобили и автобусы" /></h3>
<asp:DataList ID="_truckDataList" runat="server" RepeatColumns="6" CellSpacing="2" Width="100%" ItemStyle-Width="17%" CssClass="catalog_list" ItemStyle-CssClass="item">
	<ItemTemplate>
		<asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl='<%#GetManufacturerUrl(Container.DataItem)%>'>
			<%# Server.HtmlEncode( (string)Eval( "Brand.Name" ) )%>
		</asp:HyperLink>
	</ItemTemplate>
</asp:DataList>
