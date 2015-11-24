<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="ShopDetails.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.ShopDetails" Title="Untitled Page" meta:resourcekey="PageResource1" %>
<%@ Register src="Shops/ShopDetails.ascx" tagname="ShopDetails" tagprefix="uc1" %>
<%@ Register src="Shops/ShopEmployeeList.ascx" tagname="ShopEmployeeList" tagprefix="uc2" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

    <script type="text/javascript">
    hs.align = 'center';
    hs.transitions = ['expand', 'crossfade'];
    hs.fadeInOut = true;
    hs.dimmingOpacity = 0.8;
    hs.outlineType = 'rounded-white';
    hs.captionEval = 'this.thumb.alt';
    hs.marginBottom = 105; // make room for the thumbstrip and the controls
    hs.numberPosition = 'caption';

    // Add the slideshow providing the controlbar and the thumbstrip
    
</script>
	<uc1:ShopDetails ID="_shopDetails" runat="server" />
</asp:Content>