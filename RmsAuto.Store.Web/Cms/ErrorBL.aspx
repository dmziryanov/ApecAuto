<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true"
    CodeBehind="ErrorBL.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.ErrorBL" Title="Untitled Page" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/Controls/PageHeader.ascx" TagName="PageHeader" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <h1>
        <asp:Literal ID="lErrorHead" runat="server" Text="Ошибка" 
			meta:resourcekey="lErrorHeadResource1" /></h1>
    <asp:MultiView ID="_mvDetailError" runat="server" ActiveViewIndex="0">
        <asp:View ID="_viewStandartError" runat="server">
            <b><%=global::Resources.Exceptions.CommonException%></b>
            <br />
            <p>
                <asp:HyperLink ID="_btnDefault" runat="server" NavigateUrl="~/Default.aspx" 
					Text="Перейти на главную страницу" meta:resourcekey="_btnDefaultResource1"></asp:HyperLink>
            </p>
        </asp:View>
        <asp:View ID="_vDetailError" runat="server">
            <asp:Label runat="server" ID="_errorLabel" ForeColor="Red" Font-Bold="True" 
				meta:resourcekey="_errorLabelResource1"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
