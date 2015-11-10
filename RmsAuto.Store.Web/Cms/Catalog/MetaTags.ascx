<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetaTags.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Catalog.MetaTags" %>
<meta name="description" content="<%= Server.HtmlEncode( _description ?? "" ) %>" />
<meta name="keywords" content="<%= Server.HtmlEncode( _keywords ?? "" ) %>" />
