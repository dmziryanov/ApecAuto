<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SparePartInformationExt.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.SparePartInformationExt" %>

<%@ Import Namespace="RmsAuto.Store.BL" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Store.Entities.Helpers" %>

<asp:PlaceHolder runat="server" ID="_placeHolder">

<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# this.AdditionalInfoExt.HasDefectPics %>'>
<a title="Уцененная запчасть" target="_blank"
onclick="info=window.open(this.href,'','width=780,height=720,scrollbars=yes,resizable=yes'); info.focus(); return false;"
href="<%# UrlManager.GetSparePartsDefectImagesUrl(this.AdditionalInfoKey)%>">
<img id="Img1" src="~/images/search_defect.gif" runat="server" width="14" height="15" border="0" alt="" /></a>&nbsp;&nbsp;
</asp:PlaceHolder>

</asp:PlaceHolder>