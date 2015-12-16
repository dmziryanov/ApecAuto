<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true"
	CodeBehind="ActivateUserAccount.aspx.cs" Inherits="RmsAuto.Store.Web.ActivateUserAccount" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Register Src="~/Controls/SetPassword.ascx" TagName="SetPassword" TagPrefix="uc" %>
<%@ Register Src="~/Controls/EditUser.ascx" TagName="EditUser" TagPrefix="uc" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>


<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	<asp:Panel ID="_maintTaskPanel" runat="server">
		<h1><% =MaintEntryPurpose.ToTextOrName() %></h1>
		<div onkeypress="javascript: if(event.keyCode==13) { <%=GetPostBackClientEvent(_btnApply, String.Empty)%>; return false; }">
			<uc:EditUser ID="_editUser" runat="server" />
			<uc:SetPassword ID="_setPassword" runat="server" />
			<asp:Button ID="_btnApply" runat="server" OnClick="_btnApply_Click" Text="Send" Class="btn btn-primary btn-sm" />
		</div>
	</asp:Panel>
	<asp:Panel ID="_messagePanel" runat="server">

			<% switch( MaintEntryPurpose ) %>
			<% { %>
			<% case UserMaintEntryPurpose.NewClientRegistration: %>
				
				<uc1:TextItemControl ID="_newClientRegistration_TextItemControl" runat="server" TextItemID="Registration.OK"
                            ShowHeader="True" />
			
			<% break; %>
			<% case UserMaintEntryPurpose.ActivateClient: case UserMaintEntryPurpose.ExistingClientWebAccess: %>

				<uc1:TextItemControl ID="_existingClientWebAccess_TextItemControl" runat="server" 
					DefaultHeader="Регистрация успешно завершена"
					DefaultBody="Учетная запись пользователя интернет-магазина успешно создана и активирована" />
				
			<% break; %>
			<% case UserMaintEntryPurpose.PasswordRecovery: %>
				<h1><%=global::Resources.Texts.PasswordChangeOK %></h1>
				<asp:HyperLink ID="_btnDefault" runat="server" NavigateUrl="~/Default.aspx" Text="<%$ Resources:Texts, RedirectToDefault %>"></asp:HyperLink>
			<% break; %>
			<% } %>

	</asp:Panel>
	<asp:Panel ID="_errorPanel" runat="server">
		<div id="_errorMessage" runat="server" class="error" />
		<br />
		<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx" Text="<%$ Resources:Texts, RedirectToDefault %>"></asp:HyperLink>
	</asp:Panel>
</asp:Content>
