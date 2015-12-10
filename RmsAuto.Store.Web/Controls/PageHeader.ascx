<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageHeader.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.PageHeader" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>
<%@ Register src="~/Cms/Catalog/BreadCrumbs.ascx" tagname="BreadCrumbs" tagprefix="uc1" %>
<table width="100%" cellpadding=0 cellspacing=0 border=0>
<tr>
    <td class="basket_top" valign=top>
        
    </td>
    
</tr>
</table>
<div class="context2">
<uc1:BreadCrumbs ID="_breadCrumbs" runat="server" />
</div>
