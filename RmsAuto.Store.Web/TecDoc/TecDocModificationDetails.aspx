<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TecDocModificationDetails.aspx.cs"
	Inherits="RmsAuto.Store.Web.TecDoc.TecDocModificationDetails" MasterPageFile="~/PageTwoColumns.Master" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/PageHeader.ascx" TagName="PageHeader" TagPrefix="uc2" %>
<%@ Register src="Controls/TecDocPartsTree.ascx" tagname="TecDocPartsTree" tagprefix="uc3" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc4" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="_leftContentPlaceHolder">
	<uc1:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="_headerContentPlaceHolder">
	<uc2:PageHeader ID="PageHeader1" runat="server" RenderCurrentNodeAsLink="true" />
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="_textContentPlaceHolder">
	<h1><asp:Literal runat="server" ID="_titleLabel"></asp:Literal></h1>
	<h2><asp:Literal runat="server" ID="_subTitleLabel"></asp:Literal></h2>
	
	<uc4:TextItemControl ID="TextItemControl1" runat="server" TextItemID="OnlineCatalogs.HelpPhone" ShowHeader="false" />

	<div class="column2_left"><b><%=global::Resources.Texts.CatalogOfNotOriginalParts %></b></div>
    <div class="column2_right">
	<asp:PlaceHolder runat="server" ID="_autoXPPlaceHolder">
	    <asp:HyperLink runat="server" ID="_autoXPLink" Target="_blank"><img runat="server" ID="_image1" border=0 height="22" width="210" alt="" src="<%$ Resources:ImagesURL, catalog_originals_img %>" /></asp:HyperLink>
    </asp:PlaceHolder>
    &nbsp;
	</div>
	
	<div>
		<uc3:TecDocPartsTree ID="_tecDocPartsTree" runat="server" />
	</div>
</asp:Content>
