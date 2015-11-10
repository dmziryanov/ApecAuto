<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GarageCars.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.GarageCars" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

	<asp:Repeater ID="rptGarageCars" runat="server" OnItemCommand="rptGarageCarsCommand">
	<HeaderTemplate>
	<table width="100%" cellpadding="0" cellspacing="0" border="0" class="list" style="margin-top:0px;">
		<tr>
			<th width="50%">
				<%=global::Resources.Texts.Car %>
			</th>
			<th class="last">
				<%=global::Resources.Texts.Actions %>
			</th>
		</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr>
			<td>
			    <asp:HyperLink runat="server" ID="_editLinkFromCarName" NavigateUrl='<%# GetViewUrl( (int)Eval("Id") ) %>'>
				<%# (Container.DataItem as ICarParameters).GetFullName() %>
				</asp:HyperLink>
			</td>
			<td>
				<asp:HyperLink runat="server" ID="_editLink" NavigateUrl='<%# GetEditUrl( (int)Eval("Id") ) %>'><%# global::Resources.Texts.Edit %></asp:HyperLink>
			
				<asp:LinkButton runat="server" ID="lbtnDelete" CommandName='<%# UrlKeys.VinRequests.Operations.Delete %>' CommandArgument='<%# Eval("Id") %>'
					OnClientClick='<%#string.Format("return confirm(\x27{0}\x27);", global::Resources.Texts.DeleteConfirmation)%>'><%# global::Resources.Texts.Delete %></asp:LinkButton>&nbsp;
				
				<asp:HyperLink Visible="<%# !IsManagerMode %>" runat="server" ID="_vinCreateLink" NavigateUrl='<%# GetCreateVinUrl( (int)Eval("Id") ) %>'><%# global::Resources.Texts.VinReqest_CreateVinRequest%></asp:HyperLink>	
					
			</td>
		</tr>
	</ItemTemplate>
	<FooterTemplate>
	</table>
	</FooterTemplate>
	</asp:Repeater>
