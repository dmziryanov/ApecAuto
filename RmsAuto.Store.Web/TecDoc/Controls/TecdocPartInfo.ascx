<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecdocPartInfo.ascx.cs"
	Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecdocPartInfo" %>


<h1><%=global::Resources.Texts.PartInfo %></h1>
<table>
    <tbody>
        <tr>
            <th><%=global::Resources.Texts.Manufacturer %></th>
            <th><%=global::Resources.Texts.Article %></th>

            <th><%=global::Resources.Texts.Name %></th>
            <th><%=global::Resources.Texts.ConstructiveNumbers %></th>
        </tr>
        <tr>
            <td><%# this.PartInfo.Article.Supplier.Name %></td>
            <td><%# this.PartInfo.Article.ArticleNumber %></td>
            <td><%# this.PartInfo.Article.CompleteName.Text %></td>
            <td>
                <asp:Repeater ID="_rptConsNumebrs" runat="server" DataSource="<%# this.PartInfo.OriginalNumbers %>">
                    <ItemTemplate>
                    <%# Container.DataItem %><br />
                    </ItemTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </tbody>
</table>