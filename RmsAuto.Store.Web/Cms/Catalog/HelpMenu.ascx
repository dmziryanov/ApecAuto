<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Catalog.HelpMenu" EnableViewState="false" %>
<div class="block">
	<div class="title"><span class="icon"><img src="/images/help.png" width="17" height="17" alt="" /></span><%=global::Resources.Texts.Help %></div>
	<asp:Repeater ID="helpMenuRepeater" runat="server">
		<HeaderTemplate>
			<div class="in">
			<ul class="help">
		</HeaderTemplate>
		<ItemTemplate>
			<li>
				<asp:HyperLink runat="server" ID="HyperLink1" Visible='<%# !(bool)Eval("IsSelected") %>' NavigateUrl='<%# Eval("Url") %>' Text='<%# Server.HtmlEncode( (string)Eval("Name") ) %>' />
				<asp:Literal runat="server" ID="Literal1" Visible='<%# Eval("IsSelected") %>' Text='<%# Server.HtmlEncode( (string)Eval("Name") ) %>' />
			</li>
		</ItemTemplate>
		<FooterTemplate>
			</ul>
			</div>
		</FooterTemplate>
	</asp:Repeater>
</div>