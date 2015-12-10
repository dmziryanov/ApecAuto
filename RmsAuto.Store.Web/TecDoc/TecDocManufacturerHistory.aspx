<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TecDocManufacturerHistory.aspx.cs"
	Inherits="RmsAuto.Store.Web.TecDoc.TecDocManufacturerHistory" MasterPageFile="~/PageTwoColumnsNEW.Master" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/PageHeader.ascx" TagName="PageHeader" TagPrefix="uc2" %>
<%@ Register Src="Controls/TecDocModels.ascx" TagName="TecDocModels" TagPrefix="uc3" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc4" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="_leftContentPlaceHolder">
	<uc1:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="_mainContentPlaceHolder">
	
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="_textContentPlaceHolder">
	<h1><asp:Literal runat="server" ID="_titleLabel"></asp:Literal></h1>
	<h2><asp:Literal runat="server" ID="_subTitleLabel"></asp:Literal></h2>
	
	<uc4:TextItemControl ID="TextItemControl1" runat="server" TextItemID="OnlineCatalogs.HelpPhone" ShowHeader="false" />

	<div>
		<asp:PlaceHolder runat="server" ID="_autoXPPlaceHolder">
			<asp:ImageButton runat="server" ID="_autoXPLink" OnCommand="XPButton_Command" style="border-style:none; margin: 4px 8px;" ImageUrl="<%$ Resources:ImagesURL, catalog_originals_img %>"/>
			<span style="font-size: 16px"><sup>(<%=global::Resources.Texts.SearchOriginalParts %>)</sup><br /></span>
		</asp:PlaceHolder>
		<asp:HyperLink runat="server" ID="_tecdocLink"><img runat="server" ID="Img1" style="border-style:none; margin: 4px 8px;" height="22" width="210" alt="" src="<%$ Resources:ImagesURL, catalog_nonoriginals_img %>" /></asp:HyperLink>
		<span style="font-size: 16px"><sup>(<%=global::Resources.Texts.SearchNotoriginalParts %>)</sup><br /></span>
	</div>
	
	<asp:PlaceHolder runat="server" ID="_logoImagePlaceHolder">
		<div><asp:Image runat="server" ID="_logoImage" /></div>
    </asp:PlaceHolder>
    
    <div>
	<asp:Literal runat="server" ID="_infoLabel"></asp:Literal>
	</div>
</asp:Content>
