<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpMenu.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Catalog.HelpMenu" EnableViewState="false" %>
<div class="dropdown" style="padding-left: 5px; float: right; display: inline-block">
	<asp:Repeater ID="helpMenuRepeater" runat="server">
		<HeaderTemplate>
            <button class="btn btn-info btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <%=global::Resources.Texts.Help %>
            <span class="caret"></span>
                
            </button>
        <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
		</HeaderTemplate>
		<ItemTemplate>
			<li>
				<asp:HyperLink runat="server" CssClass="btn btn-info" ID="HyperLink1" Visible='<%# !(bool)Eval("IsSelected") %>' NavigateUrl='<%# Eval("Url") %>' Text='<%# Server.HtmlEncode( (string)Eval("Name") ) %>' />
				<asp:Literal runat="server" ID="Literal1" Visible='<%# Eval("IsSelected") %>' Text='<%# Server.HtmlEncode( (string)Eval("Name") ) %>' />
			</li>
		</ItemTemplate>
		<FooterTemplate>
			</ul>
			
		</FooterTemplate>
	</asp:Repeater>
</div>