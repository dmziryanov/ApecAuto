<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="MailBox.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.MailBox"  %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<asp:Content ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
<script>
    $(document).ready(function () {
        $(".UnreadCount").each(function (index, el)
        { el.innerHTML = '<%= UnreadMessageCount.ToString() %>' });
    });

</script>
    <div class="wrapper wrapper-content">
    <div class="row">
        <div class="col-lg-3">
            <div class="ibox float-e-margins">
                <div class="ibox-content mailbox-content">
                    <div class="file-manager">
                        <%--<a class="btn btn-block btn-primary compose-mail" href="@Url.Action("ComposeEmail", "Mailbox")">Compose Mail</a>--%>
                        <div class="space-25"></div>
                        <h5>Папки</h5>
                        <ul class="folder-list m-b-md" style="padding: 0">
                            <li><a href="@Url.Action("Inbox", "Mailbox")"> <i class="fa fa-inbox "></i>Входящие<span  id=""  class="UnreadCount label label-warning pull-right">16</span> </a></li>
<%--                            <li><a href="@Url.Action("Inbox", "Mailbox")"> <i class="fa fa-envelope-o"></i> Send Mail</a></li>
                            <li><a href="@Url.Action("Inbox", "Mailbox")"> <i class="fa fa-certificate"></i> Important</a></li>
                            <li><a href="@Url.Action("Inbox", "Mailbox")"> <i class="fa fa-file-text-o"></i> Drafts <span class="label label-danger pull-right">2</span></a></li>--%>
                            <%--<li><a href="@Url.Action("Inbox", "Mailbox")"> <i class="fa fa-trash-o"></i> Корзина</a></li>--%>
                        </ul>
                        <h5>Категории</h5>
                        <ul class="category-list" style="padding: 0">
                            <li><a href="#"> <i class="fa fa-circle text-warning"></i>Клиент </a></li>
                            <li><a href="#"> <i class="fa fa-circle text-danger"></i>Продавец</a></li>
                            <li><a href="#"> <i class="fa fa-circle text-primary"></i>Администрация Spare-Auto.Com</a></li>
                            <li><a href="#"> <i class="fa fa-circle text-info"></i>Незарегистрированный пользователь</a></li>
<%--                              <li><a href="#"> <i class="fa fa-circle text-warning"></i> Clients</a></li>--%>
                        </ul>

                      <%--  <h5 class="tag-title">Labels</h5>
                        <ul class="tag-list" style="padding: 0">
                            <li><a href=""><i class="fa fa-tag"></i> Family</a></li>
                            <li><a href=""><i class="fa fa-tag"></i> Work</a></li>
                            <li><a href=""><i class="fa fa-tag"></i> Home</a></li>
                            <li><a href=""><i class="fa fa-tag"></i> Children</a></li>
                            <li><a href=""><i class="fa fa-tag"></i> Holidays</a></li>
                            <li><a href=""><i class="fa fa-tag"></i> Music</a></li>
                            <li><a href=""><i class="fa fa-tag"></i> Photography</a></li>
                            <li><a href=""><i class="fa fa-tag"></i> Film</a></li>
                        </ul>--%>
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-9 animated fadeInRight">
            <div class="mail-box-header">

                <form method="get" action="#" class="pull-right mail-search">
                    <div class="input-group">
                        <input type="text" class="form-control input-sm" name="search" placeholder="Искать в сообщениях">
                        <div class="input-group-btn">
                            <button type="submit" class="btn btn-sm btn-primary">
                                <%= Resources.Texts.Search %>
                            </button>
                        </div>
                    </div>
                </form>
                <h2 id="InboxQty">
                  Входящие (<span class="UnreadCount"></span>)
                </h2>
                <div class="mail-tools tooltip-demo m-t-md">
                    <div class="btn-group pull-right">
                        <button class="btn btn-white btn-sm"><i class="fa fa-arrow-left"></i></button>
                        <button class="btn btn-white btn-sm"><i class="fa fa-arrow-right"></i></button>

                    </div>
                    <button class="btn btn-white btn-sm" data-toggle="tooltip" data-placement="left" title="Обновить"><i class="fa fa-refresh"></i>Обновить</button>
                    <button class="btn btn-white btn-sm" data-toggle="tooltip" data-placement="top" title="Пометить прочтенным"><i class="fa fa-eye"></i> </button>
                    <button class="btn btn-white btn-sm" data-toggle="tooltip" data-placement="top" title="Пометить важным"><i class="fa fa-exclamation"></i> </button>
                    <button class="btn btn-white btn-sm" data-toggle="tooltip" data-placement="top" title="Удалить"><i class="fa fa-trash-o"></i> </button>

                </div>
            </div>
            <div class="mail-box">

                <table class="table table-hover table-mail">
                    <tbody>
                        <asp:repeater  ID="_messageRepeater" runat="server">
                              <ItemTemplate>
                        <tr class="unread">
                            <td class="check-mail">
                                <input type="checkbox" class="i-checks">
                            </td>
                            <td class="mail-ontact"><a href="#"></a><%# ((ClientMessage)Container.DataItem).ClientName %>
                                <span  <%# ((ClientMessage)Container.DataItem).UserRole == 0 ? "" : "style='visibility:hidden'" %> class="label label-warning pull-right">Клиент</span>
                                <span  <%# ((ClientMessage)Container.DataItem).UserRole == 1 ? "" : "style='visibility:hidden'" %> class="label label-danger pull-right">Продавец</span>
                                <span  <%# ((ClientMessage)Container.DataItem).UserRole == 2 ? "" : "style='visibility:hidden'" %> class="label label-success pull-right">Spare-auto.com</span>
                                <span  <%# ((ClientMessage)Container.DataItem).UserRole == null ? "" : "style='visibility:hidden'" %> class="label label-info pull-right">Неизвестен</span>
                            </td>
                            <td class="mail-subject"><a href="#"><%# ((ClientMessage)Container.DataItem).Text %></a></td>
                            <td class="text-right mail-date"><%# ((ClientMessage)Container.DataItem).Time %></td>
                        </tr>
                                    </ItemTemplate>
                         </asp:repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

    <script type="text/javascript">
        $(document).ready(function () {

            $('.i-checks').iCheck({
                checkboxClass: 'icheckbox_square-green',
                radioClass: 'iradio_square-green',
            });

        });
    </script>
}
</asp:Content>