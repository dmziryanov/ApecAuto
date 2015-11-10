<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientPicker.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.ClientPicker" %>
<h1>Client’s search in accounting system</h1>
<div onkeypress="if(event.keyCode==13) { <%=Page.GetPostBackClientEvent(_btnSearchClient,string.Empty)%>; return false; }">
	Client’s name: <asp:TextBox id="_txtClientName" runat="server"></asp:TextBox> 
	Main phone number: <asp:TextBox ID="_txtPhone" runat="server"></asp:TextBox> 

	<asp:Button ID="_btnSearchClient" runat="server" Text="<%$ Resources:Texts, Search %>" 
		onclick="_btnSearchClient_Click" CssClass="button" />
</div>

<br />    
<asp:Repeater ID="_rptSearchResults" runat="server" 
    onitemcommand="_rptSearchResults_ItemCommand"
    OnItemDataBound="_rptSearchResults_ItemDataBound">
    <HeaderTemplate>
        <table class="list" cellpadding="0" cellspacing="0">
        <tr>
            <th>Client’s name</th>
            <th>Contact phone</th>
            <th>Options</th>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("ClientName") %></td>
            <td><%# Eval("MainPhone") %></td>
            <td>
                <asp:LinkButton ID="_lbAddToHandySet" runat="server" CommandName="AddToHandySet" CommandArgument='<%# Eval("ClientId") %>' Text="Add to operational list"></asp:LinkButton>
                <asp:Label ID="_lblWarning" CssClass="error" runat = "server" />
                <asp:Label ID="_lblNotActivated" Visible = "false" CssClass="SimpleGrayTextStyle" runat = "server" meta:resourcekey="NotAсtivatedAlert" Text="Client has not been activated" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table
    </FooterTemplate>
</asp:Repeater>
<% if (NoClientsFound) { %>
   No client is found upon your request. <br />
   Try to change search criterion
<% } %>
<% if (TooManyClientsFound) { %>
    Search criterions are too indecipherable<br />
    Please specificate search terms.
<% } %>




