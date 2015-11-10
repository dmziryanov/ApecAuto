<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SparePartsManufacturers.ascx.cs"
	Inherits="RmsAuto.Store.Web.Controls.SparePartsManufacturers" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>

<div class="orders">
    <asp:Label runat="server" ID="_titleLabel" Font-Bold="true"></asp:Label>
    <asp:Label runat="server" ID="_errorLabel" Font-Bold="true" ForeColor="Red"></asp:Label>
    <div class="tab-text">
        <div class="t-hold">
            <asp:Repeater ID="_manufacturerRepeater" runat="server">
	            <HeaderTemplate>
		            <table>
			            <tr>
				            <th><%=global::Resources.Texts.Number %></th>
				            <th><%=global::Resources.Texts.Manufacturer %></th>
				            <th><%=global::Resources.Texts.Description %></th>
				            <th>&nbsp;</th>
			            </tr>
	            </HeaderTemplate>
	            <ItemTemplate>
		            <tr>
			            <td>
				            <%# Server.HtmlEncode( (string)Eval("PartNumber") ) %>
			            </td>
			            <td>
				            <%# Server.HtmlEncode((string)Eval("Manufacturer"))%>
			            </td>
			            <td>
				            <%# Server.HtmlEncode( Convert.ToString(Eval("PartDescription")) )%>
			            </td>
			            <td style="text-align:center;">
				            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# GetSearchSparePartsUrl((string)Eval("Manufacturer"), (string)Eval("PartNumber")) %>'
					            Text='<%$ Resources:Texts, Find %>' />
			            </td>
		            </tr>
	            </ItemTemplate>
	            <FooterTemplate>
		            </table>
	            </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>

<div style="margin-top: 20px">
	<uc1:TextItemControl ID="_callToManagerHint_TextItemControl" runat="server" 
	TextItemID="SearchSpareParts.CallToManagerHint"
	ShowHeader="false" />
</div>