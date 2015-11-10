<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SparePartInformation.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.SparePartInformation" %>
<%@ Register src="SparePartView.ascx" tagname="SparePartView" tagprefix="uc1" %>
<%@ Import Namespace="RmsAuto.Store.BL" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.TechDoc.Entities" %>

<asp:PlaceHolder runat="server" ID="_placeHolder">

<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# this.AdditionalInfo.HasPics %>'>
<a title="<%= global::Resources.Texts.Photos %>" target="_blank"
onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;"
href="<%# UrlManager.GetTecDocInfoImagesUrl(this.AdditionalInfo.TecDocArticulId)%>"><img id="Img1" src="~/images/search_photo.png" runat="server" width="14" height="15" border="0" /></a>&nbsp;&nbsp;
</asp:PlaceHolder>
<asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# this.AdditionalInfo.HasDescription %>'>
<a title="<%= global::Resources.Texts.Description %>" target="_blank"
onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;"
href="<%# UrlManager.GetTecDocInfoUrl(this.AdditionalInfo.TecDocArticulId)%>"><img id="Img2" src="~/images/search_desc.png" runat="server" width="14" height="15" border="0" /></a>&nbsp;&nbsp;
</asp:PlaceHolder>
<asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# this.AdditionalInfo.HasAppliedCars %>'>
<a title="<%= global::Resources.Texts.ApplyingToCar %>"
onclick="info=window.open(this.href,'','width=600,height=500,scrollbars=yes,resizable=yes'); info.focus(); return false;"
target="_blank" href="<%# UrlManager.GetTecDocInfoCarsUrl(this.AdditionalInfo.TecDocArticulId)%>"><img id="Img3" src="~/images/search_auto.png" runat="server" width="14" height="15" border="0" /></a>&nbsp;&nbsp;
</asp:PlaceHolder>

</asp:PlaceHolder>