<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="ShopList.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.ShopList" Title="Untitled Page" %>
<%@ Register src="Shops/ShopList.ascx" tagname="ShopList1" tagprefix="uc1" %>
<%@ Register src="Shops/FranchShopList.ascx" tagname="ShopList2" tagprefix="fsl" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" Visible="False" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">

    <style>
td a:hover
{
	text-decoration:underline;
}


    .style1
    {
        width: 141px;
    }


</style>

	
	
    <div id="h1" runat="server" style="margin-bottom:10px">
        <asp:Label ID="newHyperLink" runat="server">
        </asp:Label>&nbsp
        <a href= '<%=System.Configuration.ConfigurationManager.AppSettings["WebSiteUrl"] + "/About/Shops/Map/"+CmsContext.Current.PageParameters["Region"]+"/"+CmsContext.Current.PageParameters["ID"]+".aspx"%>'>        
        <asp:Image ID="imgId" border="0" alt="офисы продаж на карте" src='/images/ya.gif' runat="server" />
        </a>
    </div>    
    
    <div id="h2" runat="server" style="margin-bottom:10px">
	    <b class="header3Style"><asp:Literal runat="server" ID="_pageTitleLiteral" /></b></br>
	</div>
   
	
	<uc1:ShopList1 ID="ShopList1" runat="server" />
	<fsl:ShopList2 ID="ShopList2" runat="server"  visible=false/>
</asp:Content>
