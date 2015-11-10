<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoFileLoader.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.PhotoFileLoader" %>

<asp:FileUpload id="fileUploadId" runat="server" Width="70%" />

<!--img src="<%=ResolveUrl("~/images/help.gif")%>"  class="help_img" title="Фото не более 2Мб, в формате jpg, tif, png" /-->

<asp:CustomValidator ID="fileSelection" runat="server"
Display="Dynamic"
ErrorMessage="Не выбран файл"
OnServerValidate="fileSelectionValidator"/>

<asp:CustomValidator ID="fileSize" runat="server"
Display="Dynamic"
ErrorMessage="Слишком большой размер файла"
OnServerValidate="fileSizeValidator"/>

<asp:CustomValidator ID="fileExtension" runat="server"
Display="Dynamic"
ErrorMessage="Выберете файл формата png, jpg, tif"
OnServerValidate="fileExtensionValidator"/>
