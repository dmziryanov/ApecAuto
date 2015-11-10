<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="StockSupplierParts.aspx.cs" Inherits="RmsAuto.Store.Web.Store.StockSupplierParts" Title="Untitled Page" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>
	
	<span style="float:right"><%=global::Resources.Texts.Prices %>
	    <asp:DropDownList ID="_ddCurrency" runat="server" disabled="disabled">
	        <asp:ListItem Text="<%$ Resources:Texts, RussianRouble %>" Value="RUB" />
        </asp:DropDownList>
    </span>
    
	<asp:Repeater ID="rptParts" runat="server">
	    <HeaderTemplate>
	    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="list">
	        <tr>
	            <th>
	            <%=global::Resources.Texts.Manufacturer %>
	            </th>
	            <th>
	            <%=global::Resources.Texts.Article %>
	            </th>
	            <th>
	            <%=global::Resources.Texts.Name %>
	            </th>
	            <th>
	            <%=global::Resources.Texts.Price_s %>
	            </th>
	            <th class="last">
	            <%=global::Resources.Texts.Search %>
	            </th>
	        </tr>
	    </HeaderTemplate>
	    <ItemTemplate>
	        <tr>
	            <td>
	                <%# Eval("Manufacturer")%>
	            </td>
	            <td>
	                <%# Eval("PartNumber")%>
	            </td>
	            <td>
                    <a target="_blank" href="<%# GetSparePartDetailsUrl((RmsAuto.Store.Entities.SparePart)Container.DataItem)%>" 
            	    onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;">
	                <%# Eval("PartDescription")%>
	                </a>
	            </td>
	            <td>
	                <%# Eval("InitialPrice") %>
	            </td>
	            <td>
	                <asp:HyperLink runat="server" NavigateUrl='<%# UrlManager.GetSearchSparePartsUrl(Eval("Manufacturer").ToString(), Eval("PartNumber").ToString(), true) %>'>
	                <%=global::Resources.Texts.LookupPrices %></a>
	                </asp:HyperLink>
	            </td>
	        </tr>
	    </ItemTemplate>
	    <FooterTemplate>
	    </table>
	    </FooterTemplate>
	</asp:Repeater>
	
	<asp:Repeater ID="rptPaging" runat="server">
	    <HeaderTemplate>
	    <b><%=global::Resources.Texts.Pages %>:</b>
	    </HeaderTemplate>
	    <ItemTemplate>
	    <asp:PlaceHolder ID="currentPageTemplate" runat="server" Visible="<%# this.PageNum == (int)Container.DataItem %>">
	    <b><%# Container.DataItem %></b>
	    </asp:PlaceHolder>
	    <asp:PlaceHolder ID="pageTemplate" runat="server" Visible="<%# this.PageNum != (int)Container.DataItem %>">
	    <asp:HyperLink runat="server" NavigateUrl="<%# UrlManager.GetStockSupplierPartsPage(this.Mfr, (int)Container.DataItem) %>"><%# Container.DataItem %></asp:HyperLink>
	    </asp:PlaceHolder>
	    </ItemTemplate>
	    <SeparatorTemplate> </SeparatorTemplate>
	</asp:Repeater>
	
</asp:Content>
