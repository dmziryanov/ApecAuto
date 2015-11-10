<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true"
    CodeBehind="RegisterOnline.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.RegisterOnline" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/FillClientProfileExt.ascx" TagName="FillClientProfileExt"
    TagPrefix="uc2" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    
    <script type="text/javascript">
        $(document).ready(function() {
            function getCookie(name) {
                var prefix = name + "=";
                var start = document.cookie.indexOf(prefix);
                if (start == -1)
                    return "";
                var end = document.cookie.indexOf(";", start + prefix.length);
                if (end == -1)
                    end = document.cookie.length;
                var value = document.cookie.substring(start + prefix.length, end);
                return unescape(value);
            }

//            prepareDialog2 = function() {
//                $('#RegionList').dialog({ title: 'Ваш город',
//                    position: [260, 210],
//                    autoOpen: false,
//                    resizable: false,
//                    height: 500,
//                    width: '678px',
//                    modal: true
//                });
//                $('.ui-dialog').addClass('dialogWithDropShadow');
//                $('.ui-dialog').addClass('overlay');
//                $('.ui-widget-header').removeClass('ui-widget-header');
//            };

//            if (getCookie('InternalFranchName') == '') {
//                prepareDialog2(); fillDialog(false); $('#RegionList').dialog('open');
//            }
        });
    </script>
    
    <uc2:FillClientProfileExt ID="_fillClientProfileExt" runat="server" Title="<%$ Resources:Texts, OnlineRegistration %>"
        OnCompleted="FillClientProfileCompletedExt" />
    <div id="_messagePane" runat="server" visible="false">

        <%--<script type="text/javascript">
            var pageTracker = _gat._getTracker("UA-22238226-1");
            pageTracker._trackPageview("/registration/end.html");
        </script>--%>

        <uc3:TextItemControl ID="saveOKTIC" runat="server" TextItemID="Registration.SaveOK"
        ShowHeader="false" />
        
        <asp:HyperLink ID="_btnDefault" runat="server" NavigateUrl="~/Default.aspx" Text="<%$ Resources:Texts, RedirectToDefault %>"></asp:HyperLink>
    </div>
    <% if ( Page.User.Identity.IsAuthenticated )
       { %>
    <h2>
        <%=global::Resources.Texts.YouLoggedOn %></h2>
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx" Text="<%$ Resources:Texts, RedirectToDefault %>"></asp:HyperLink>
    <% } %>
</asp:Content>
