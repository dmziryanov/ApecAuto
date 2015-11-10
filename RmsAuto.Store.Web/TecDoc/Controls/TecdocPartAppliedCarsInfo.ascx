<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecdocPartAppliedCarsInfo.ascx.cs"
	Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecdocPartAppliedCarsInfo" %>
<%@ Import Namespace="System.Collections.Generic" %>

<span class="blue"><%=global::Resources.Texts.ApplyingToCars %></span>
<asp:Repeater ID="_rpt" runat="server" DataSource="<%# this.Cars %>">
<HeaderTemplate>
<table>
    <tbody>
        <tr>
            <th><%=global::Resources.Texts.Model %></th>
            <th><%=global::Resources.Texts.DatesOfIssue %></th>
            <th><%=global::Resources.Texts.Body %></th>
            <th><%=global::Resources.Texts.VolumeCommaL %></th>
            <th><%=global::Resources.Texts.PowerCommaHP %></th>
            <th><%=global::Resources.Texts.Fuel %></th>
        </tr>
</HeaderTemplate>
<ItemTemplate>
        <tr>
            <td>
                <%# Eval("Name.Text") %>
            </td>
            <td>
                <%=global::Resources.Texts.AC_From %>
                <%# Eval("DateFrom") %>
                <%=global::Resources.Texts.AC_To %>
                <%# Eval("DateTo") %>
            </td>
            <td>
                <%# Eval("BodyName.Text") %>
            </td>
            <td>
                <%# GetEngineVolume(Container.DataItem as RmsAuto.TechDoc.Entities.TecdocBase.CarType) %>
            </td>
            <td>
                <%# Eval("TYP_HP_FROM")%>
            </td>
            <td>
                <%# Eval("FuelSupplyName.Text") %>
            </td>
        </tr>
</ItemTemplate>    
<FooterTemplate>
    </tbody>
</table>
</FooterTemplate>
  
</asp:Repeater>