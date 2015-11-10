<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetaTags.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Controls.MetaTags" EnableViewState="false" %>
<meta name="description" content="<%= Server.HtmlEncode( _description ?? "" ) %>" />
<meta name="keywords" content="<%= Server.HtmlEncode( _keywords ?? "" ) %>" />
