<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VinRequestsList.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.VinRequestsList" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

<asp:Repeater ID="rptRequests" runat="server" EnableViewState="false">
    <HeaderTemplate>
        <table width="100%" cellpadding="0" cellspacing="0" class="list">
        <tr>
            <th><%=global::Resources.Texts.Date %></th>
            <th><%=global::Resources.Texts.Auto %></th>
            <th><%=global::Resources.Texts.Status %></th>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td>
					<%# Eval("RequestDate","{0:dd.MM.yyyy HH:mm}") %>
                </td>
                <td>
					<a href="<%# UrlManager.GetVinRequestDetailsUrl((int)Eval("Id"))  %>"><%# (Container.DataItem as VinRequest).GetFullName() %></a>
                </td>
                <td>
                <asp:Literal ID="lHaveReq" runat="server" Text="<%$ Resources:Texts, ReceivedResponce %>" Visible='<%# (bool)Eval("Proceeded") %>' />
                <asp:Literal ID="lReqSend" runat="server" Text="<%$ Resources:Texts, SentRequest %>" Visible='<%# !(bool)Eval("Proceeded") %>' />
				</td>
            </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
