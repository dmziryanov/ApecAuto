﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagerInfo.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.ManagerInfo" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Register TagPrefix="uc3" TagName="QuickSearch" Src="~/Controls/QuickSearch.ascx" %>
<%@ Register TagPrefix="culture" TagName="Culture" Src="~/Controls/Culture.ascx" %>
<script>
    $(document).ready(function() {
        $("#UnreadCount").each(function (index, el)
            { el.innerHTML = '<%= UnreadMessageCount.ToString() %>' });
    });

</script>
<div class="row border-bottom">
    <nav class="navbar navbar-fixed-top" role="navigation" style="margin-bottom: 0">
        <div class="navbar-header">
            <a class="navbar-minimalize minimalize-styl-2 btn btn-primary " href="#"><i class="fa fa-bars"></i></a>
            <uc3:QuickSearch ID="QuickSearch1" runat="server" />
        </div>

        <ul class="nav navbar-top-links navbar-right">
            <li></li>
            <li>
                <span class="m-r-sm text-muted welcome-message"><%# Resources.Texts.ManagerWorkingPlace %></span>
            </li>
            <li class="dropdown">
                <a class="dropdown-toggle count-info" data-toggle="dropdown" href="#">
                    <i class="fa fa-envelope"></i><span id="UnreadCount" class="label label-warning">16</span>
                </a>
                <ul class="dropdown-menu dropdown-messages">
                    <asp:Repeater ID="_messageRepeater" runat="server">
                        <ItemTemplate>
                            <li>
                                <div class="dropdown-messages-box">
                                    <a href="#" class="pull-left">
                                        <img alt="image" class="img-circle" src="../Images/persona.png">
                                    </a>
                                    <div class="media-body">


                                        <%--<small class="pull-right">46h ago</small>--%>
                                        <strong><%# ((ClientMessage)Container.DataItem).ClientName %></strong>: <%# ((ClientMessage)Container.DataItem).Text %>.
                                <br>
                                        <small class="text-muted"><%# ((ClientMessage)Container.DataItem).Time %></small>
                                    </div>


                                </div>
                            </li>
                            <li class="divider"></li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <li>
                        <div class="text-center link-block">
                            <a href="/Manager/MailBox.aspx">
                                <i class="fa fa-envelope"></i><strong>Все сообщения</strong>
                            </a>
                        </div>
                    </li>
                </ul>
            </li>
        <%--    <li class="dropdown">
                <a class="dropdown-toggle count-info" data-toggle="dropdown" href="#">
                    <i class="fa fa-bell"></i><span class="label label-primary">8</span>
                </a>
                <ul class="dropdown-menu dropdown-alerts">
                    <li>
                        <a href="#">
                            <div>
                                <i class="fa fa-envelope fa-fw"></i>You have 16 messages
                                <span class="pull-right text-muted small">4 minutes ago</span>
                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <a href="#">
                            <div>
                                <i class="fa fa-twitter fa-fw"></i>3 New Followers
                                <span class="pull-right text-muted small">12 minutes ago</span>
                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <a href="#">
                            <div>
                                <i class="fa fa-upload fa-fw"></i>Server Rebooted
                                <span class="pull-right text-muted small">4 minutes ago</span>
                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <div class="text-center link-block">
                            <a href="#">
                                <strong>See All Alerts</strong>
                                <i class="fa fa-angle-right"></i>
                            </a>
                        </div>
                    </li>
                </ul>
            </li>--%>
            <li>
                <culture:Culture runat="server" />
            </li>
            <li>
                <a class="right-sidebar-toggle">
                    <i class="fa fa-tasks"></i>
                </a>
            </li>
        </ul>
    </nav>
</div>
