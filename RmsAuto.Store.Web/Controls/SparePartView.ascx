<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SparePartView.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.SparePartView" %>
<h1><%# PartName%></h1>
<div class="information">
    <table>
        <tr>
            <td class="title"><%=global::Resources.Texts.Manufacturer %></td><td><%# Manufacturer %></td>
        </tr>
        <tr>
            <td class="title"><%=global::Resources.Texts.Article %></td><td><%# PartNumber%></td>
        </tr>
        <tr>
            <td class="title"><%=global::Resources.Texts.PartName %></td><td><%# PartName%></td>
        </tr>
        <tr>
            <td class="title"><%=global::Resources.Texts.Description %></td><td><%# PartDescription%></td>
        </tr>
        <tr>
            <td class="title"><%=global::Resources.Texts.WeightPhysical %></td><td><%# WeightPhysical.HasValue ? Convert.ToString(WeightPhysical)+ " " + Resources.Texts.Kg : "NA" %></td>
        </tr>
        <tr>
            <td class="title"><%=global::Resources.Texts.WeightVolume %></td><td><%# WeightVolume.HasValue ? Convert.ToString(WeightVolume) : "NA"%></td>
        </tr>
        <tr>
            <td class="title"><%=global::Resources.Texts.Price %></td><td class="price"><%# ActualPrice.HasValue ? Convert.ToString(ActualPrice)+ " " + Resources.Texts.DollarShort : "NA" %></td>
        </tr>
        <tr>
            <td class="title"><%=global::Resources.Texts.DeliveryPeriod %></td><td> <%=global::Resources.Texts.From %> <%# DisplayDeliveryDaysMin %> <%=global::Resources.Texts.To %> <%# DisplayDeliveryDaysMax %> <%=global::Resources.Texts.Days %></td>
        </tr>
         <tr>
            <td class="title"><%=global::Resources.Texts.AvailabilityInStock %></td><td><%# QtyInStock %></td>
        </tr>
        <tr>
            <td class="title"><%=global::Resources.Texts.MinOrderQty %></td><td><%# MinOrderQty.HasValue ? Convert.ToString(MinOrderQty) : "NA"%></td>
        </tr>
    </table>
</div>



