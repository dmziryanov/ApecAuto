<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VinRequestDetails.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.VinRequestDetails" %>
<%@ Register src="~/Controls/GarageCarDetails.ascx" tagname="GarageCarDetails" tagprefix="uc3" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

<h3><%= this.VinRequest.GetFullName() %></h3>

<uc3:GarageCarDetails runat="server" ID="_garageCarDetails" />

<asp:Repeater ID="_rptRequestItems" runat="server">
<HeaderTemplate>
	<br />
	<h3><%=global::Resources.Texts.Requested_NA %>:</h3>
    <table width="100%" cellpadding="0" cellspacing="0" class="list">
        <tr>
            <th>
                <%=global::Resources.Texts.Name %>
            </th>
            <th>
                <%=global::Resources.Texts.Qty %>
            </th>
            <th class="empty">
                <%=global::Resources.Texts.ClientCommentary %>
            </th>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Server.HtmlEncode( Eval("Name") as string ) %></td>
            <td><%# Server.HtmlEncode( Eval("Quantity").ToString() ) %></td>
            <td><%# Server.HtmlEncode( Eval("Description") as string ) %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
    </table>
    </FooterTemplate>
</asp:Repeater>

<asp:Repeater ID="_rptAnsweredRequestItems" runat="server">
<HeaderTemplate>
	<br />
	<h3><%=global::Resources.Texts.Answered_Male %>:</h3>
    <table width="100%" cellpadding="0" cellspacing="0" class="list">
        <tr>
            <th>
                <%=global::Resources.Texts.Name %>
            </th>
            <th>
                <%=global::Resources.Texts.Qty %>
            </th>
            <th>
                <%=global::Resources.Texts.ClientCommentary %>
            </th>
            <th>
                <%=global::Resources.Texts.Article %>
            </th>
            <th>
                <%=global::Resources.Texts.Manufacturer %>
            </th>
            <th>
                <%=global::Resources.Texts.OriginalPartNumber %>
            </th>
            <th>
                <%=global::Resources.Texts.DeliveryPeriod %>
            </th>
            <th>
                <%=global::Resources.Texts.Price %>
            </th>
            <th class="empty">
                <%=global::Resources.Texts.ManagerComment %>
            </th>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <%# Server.HtmlEncode( Eval("Name") as string ) %>
            </td>
            <td>
                <%# Server.HtmlEncode( Eval("Quantity").ToString() ) %>
            </td>
            <td>
                <%# Server.HtmlEncode( Eval("Description") as string) %>
            </td>
            <td>
                <a href='<%# UrlManager.GetSearchSparePartsUrl(Server.UrlEncode( Eval("Manufacturer") as string ), Server.UrlEncode( Eval("PartNumber") as string ), false) %>'>
                <%# Server.HtmlEncode( Eval("PartNumber") as string ) %>
                </a>
            </td>
            <td>
                <%# Server.HtmlEncode( Eval("Manufacturer") as string) %>
            </td>
            <td>
                <%# Server.HtmlEncode( Eval("PartNumberOriginal") as string) %>
            </td>
            <td>
                <%# Server.HtmlEncode( Eval("DeliveryDays") as string) %>
            </td>
            <td>
                <%# Eval("PricePerUnit") %>
            </td>
            <td>
                <%# Server.HtmlEncode( Eval("ManagerComment") as string) %>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
    </table><br />
    <%=global::Resources.Texts.VinTotalSum %>&nbsp;<%# this.TotalSum.ToString("C2") %>
    </FooterTemplate>
</asp:Repeater>
