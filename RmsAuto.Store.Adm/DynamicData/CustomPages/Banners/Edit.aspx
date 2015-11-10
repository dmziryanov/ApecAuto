<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Edit.aspx.cs" Inherits="RmsAuto.Store.Adm.DynamicData.CustomPages.Banners.Edit" ValidateRequest="False" %>
<%@ Register TagPrefix="rmsauto" Namespace="RmsAuto.Common.Web" Assembly="RmsAuto.Common" %>
<%@ Register src="../../../Controls/AddFile/AddFileControl.ascx" tagname="AddFileControl" tagprefix="uc1" %>
<%@ Register src="../../FieldTemplates/Custom/Html_Edit.ascx" tagname="Html_EditControl" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="_mainContentPlaceHolder" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <h2>[Баннеры].редактирование записи</h2>

    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />
    
    <table align="left"  class="detailstable" cellpadding="0" cellspacing="0">
    
        <tr>
            <th>
                <nobr>Название: <font color="red">*</font></nobr>
            </th>
            <td>
                <asp:TextBox ID="_txtName" runat="server" Width="100%" ValidationGroup="EditGroup" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtName"
                ValidationGroup="EditGroup" InitialValue="" ErrorMessage="Не введено название" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        
        <tr>
            <th>
                Вид отображения: <font color="red">*</font>
            </th>
            <td>
                <asp:DropDownList runat="server" ID="_renderTypeList"
                OnSelectedIndexChanged="renderTypeList_SelectedIndexChanged" AutoPostBack="true" Width="95%" >
                </asp:DropDownList>
                <img id="Img1" runat="server" src="~/DynamicData/Content/Images/help.gif" width="11" height="11" border="0" class="help_img"
                title="Выберите вид отображения" />
            </td>
        <asp:placeHolder ID="URLPlaceHolder" Visible="false" runat="server">
            </tr>
                    <tr>
                <th>
                    <nobr>URL:</nobr>
                </th>
                <td>
                    <asp:TextBox ID="_txtURL" runat="server" Width="100%" ValidationGroup="EditGroup" />
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:placeHolder ID="AddFilePlaceHolder" Visible="false" runat="server">
            <tr>
                <td colspan="2">
                    <nobr><strong>Добавление файла:</strong> <font color="red">*</font></nobr>
                    <uc1:AddFileControl ID="AddFileControl1" Width="100%" runat="server" />
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:placeHolder ID="HtmlPlaceHolder" Visible="false" runat="server">
            <tr >
                <td colspan="2">
                    <nobr><strong>Html: </strong>
                        <asp:placeHolder ID="HtmlRequeredPlaceHolder" Visible="false" runat="server">
                            <font color="red">*</font>
                        </asp:placeHolder>
                    </nobr>
                    <br/><br/>
                    <asp:placeHolder ID="FlashPastePlaceHolder" Visible="false" runat="server">
                        <asp:Button ID="ButtonHtmlForFlash" OnClick="ButtonHtmlForFlashClick" OnClientClick="return confirm('Будет заменено содержимое Html, продолжить?')" runat="server" Text="Вставить параметры для html обертки flash" />
                        <p>Для использования заготовки нажмите на кнопку и измените значения параметров,
                        <br/>либо самостоятельно вставьте html обертку
                        <br/><span style="color:Red;">не забудьте добавить файл!</span>
                        </p>
                        <p>Только для самостоятельной вставки:
                        <br/>Используйте для указания полного пути <strong>BANNERFULLPATH</strong> и для указания имени файла <strong>BANNERFILENAME</strong>, 
                        <br/><strong>BANNERFULLPATH</strong> и <strong>BANNERFILENAME</strong> вычисляются автоматически для добавленного swf файла</p>
                    </asp:PlaceHolder>
                    <asp:TextBox ID="_txtHtml" runat="server" Width="100%" ValidationGroup="EditGroup"></asp:TextBox><br />
                    
                    <asp:placeHolder ID="FileHtmlEditPlaceHolder" Visible="false" runat="server">
                        <asp:Button ID="_editButton" runat="server" Text="Редактировать" />
                    </asp:placeHolder>
                                        
                    <asp:placeHolder ID="HtmlRequeredValidatorPlaceHolder" Visible="false" runat="server">
                        <br/>
                        <asp:RequiredFieldValidator ID="HtmlRequiredFieldValidator" runat="server" ControlToValidate="_txtHtml" ValidationGroup="EditGroup"
                            ErrorMessage="не введен Html" Display="Dynamic"></asp:RequiredFieldValidator>
                    </asp:placeHolder>
                </td>
            </tr>
        </asp:placeHolder>
        <tr>
            <td colspan="2">
                <nobr><strong>Действия</strong>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="ButtonUpplyGoToLink" OnClick="ButtonUpplyAndGoToLinkClick" runat="server" Text="Применить и перейти к привязке" />
            </td>
            <td>
                <asp:Button ID="ButtonUpply" OnClick="ButtonUpplyClick" runat="server" Text="Применить" />
            </td>
         </tr>
         <tr>
            <td>
                <asp:Button ID="ButtonDelete" OnClientClick="return confirm('Баннер будет удален, продолжить?')" OnClick="ButtonDeleteClick" runat="server" Text="Удалить запись" />
            </td>
         </tr>
         <tr>
            <td colspan="2">
                <div class="bottomhyperlink">
                    <asp:HyperLink ID="GoToListLink" runat="server">
                        <img id="Img3" runat="server" src="~/DynamicData/Content/Images/down_arc_small.png" alt="Вернуться к списку баннеров" />
                        <font style="font-size:larger;">Вернуться к списку баннеров</font>
                    </asp:HyperLink>
                </div>
            </td>
         </tr>  
    </table>

</asp:Content>