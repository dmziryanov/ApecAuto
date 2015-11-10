<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManufacturerDetails.ascx.cs"
	Inherits="RmsAuto.Store.Web.Cms.Manufacturers.ManufacturerDetails" %>
<h1>
    <asp:Literal ID="lHeader" runat="server" Text="Производители автозапчастей" 
		meta:resourcekey="lHeaderResource1" />
	</h1>
<h2>
	<asp:Literal runat="server" ID="_manufacturerLabel" 
		meta:resourcekey="_manufacturerLabelResource1" /></h2>
<table class="info" width="100%">
	<tr runat="server" id="_logoRow">
		<th>
		<asp:Literal ID="lLogo" runat="server" Text="Логотип" 
				meta:resourcekey="lLogoResource1" />
		</th>
		<td>
			<asp:Image runat="server" ID="_logoImage" 
				meta:resourcekey="_logoImageResource1" />
		</td>
	</tr>
	<tr>
		<th>
			<asp:Literal ID="lInfo" runat="server" Text="Описание" 
				meta:resourcekey="lInfoResource1" />
		</th>
		<td>
			<asp:Literal runat="server" ID="_infoLabel" 
				meta:resourcekey="_infoLabelResource1" />
		</td>
	</tr>
	<tr runat="server" id="_siteRow">
		<th>
			<asp:Literal ID="lSite" runat="server" Text="Сайт" 
				meta:resourcekey="lSiteResource1" />
		</th>
		<td>
			<asp:HyperLink runat="server" ID="_siteLink" Target="_blank" 
				meta:resourcekey="_siteLinkResource1">[_siteLink]</asp:HyperLink>
		</td>
	</tr>
	<tr runat="server" id="_filesRow">
		<th>
			<asp:Literal ID="lFile" runat="server" Text="Файлы" 
				meta:resourcekey="lFileResource1" />
		</th>
		<td>
			<asp:Repeater runat="server" ID="_filesRepeater">
			<HeaderTemplate><ul></HeaderTemplate>
			<ItemTemplate>
				<li>
					<asp:HyperLink runat="server" ID="Link1"
						NavigateUrl='<%# GetFileUrl((int)Eval("FileID")) %>' 
						Text='<%# Server.HtmlEncode((string)Eval("FileName")) %>' meta:resourcekey="Link1Resource1"/>
				</li>
			</ItemTemplate>
			<FooterTemplate></ul></FooterTemplate>
			</asp:Repeater>
		</td>
	</tr>
</table>
