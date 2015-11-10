<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FillClientProfile.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.FillClientProfile" %>
<%@ Register src="~/Controls/WizardTopSideBar.ascx" tagname="WizardTopSideBar" tagprefix="uc1" %>
<%@ Register src="~/Controls/Phone_Edit.ascx" tagname="Phone_Edit" tagprefix="uc1" %>
<%@ Register src="~/Controls/DiscountCard_Edit.ascx" tagname="DiscountCard_Edit" tagprefix="uc1" %>
<%@ Register src="~/Controls/Captcha.ascx" tagname="Captcha" tagprefix="uc1" %>
<%@ Register Src="~/Controls/EditUser.ascx" TagName="EditUser" TagPrefix="uc1" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>
<script language="javascript">

function checkClientCategory(cat)
{
    var vol0 = document.getElementById('<%=_rblTradingVolume.ClientID %>_0');
    var vol1 = document.getElementById('<%=_rblTradingVolume.ClientID %>_1');
    var reqmsg = document.getElementById('<%=_generalInfoReqMsgLabel.ClientID %>');
    var cname = document.getElementById('_compNameRow');
    
    if (cat == 0)
    {
        vol1.checked = true;
        vol0.checked = false;
        vol0.disabled = true;
 		$('label[for$=_rblTradingVolume_0]').attr('disabled','disabled');
        cname.style.display = '';
        reqmsg.style.display = '';
    }
    else
    {
        vol0.disabled = false;
        vol0.checked = true;
 		$('label[for$=_rblTradingVolume_0]').removeAttr('disabled');
        vol1.checked = false;
        cname.style.display = 'none';
        reqmsg.style.display = 'none';
    }
}

function nextStep()
{
	var els = $('input[name$=NextImageButton]');
	if( els.length!=0 ) els.get(0).click();
}
function finishWizard()
{
	var els = $('input[name$=FinishImageButton]');
	if( els.length!=0 ) els.get(0).click();
}

$(function(){
	var arr = $('input[name$=_rblClientCategory][value=0]');
	if( arr.length!=0 )
	{
		var isLegal = arr.get(0).checked;
		$('#<%=_generalInfoReqMsgLabel.ClientID%>').css( 'display', isLegal ? '' : 'none' );
		
		if( isLegal ) 
		{
			$('input[name$=_rblTradingVolume][value=0]' ).attr('disabled','disabled');
			$('label[for$=_rblTradingVolume_0]').attr('disabled','disabled');
			$('input[name$=_rblTradingVolume][value=1]' ).attr('checked','checked');
		}
	}
});
</script>

<div onkeydown="javascript: if(event.keyCode==13 && (document.activeElement==null || document.activeElement.tagName!='TEXTAREA') ) { nextStep(); return false; }">

<asp:Wizard ID="_fillProfileWizard" runat="server" 
         OnActiveStepChanged="_fillProfileWizard_ActiveStepChanged"
         onfinishbuttonclick="_fillProfileWizard_FinishButtonClick"
         DisplaySideBar="false" Width="100%" 
         StartNextButtonImageUrl="<%$ Resources:ImagesURL, nextstep %>" StartNextButtonType="Image" 
         StepNextButtonImageUrl="<%$ Resources:ImagesURL, nextstep %>" StepNextButtonType="Image" 
         StepPreviousButtonImageUrl="<%$ Resources:ImagesURL, prevstep %>" StepPreviousButtonType="Image" 
         FinishPreviousButtonImageUrl="<%$ Resources:ImagesURL, prevstep %>" FinishPreviousButtonType="Image" 
         FinishCompleteButtonImageUrl="<%$ Resources:ImagesURL, btn_reg %>" FinishCompleteButtonType="Image">
        <NavigationStyle CssClass="wizard_btn" HorizontalAlign="Left" />
        <HeaderTemplate>
            <h1><% = Title %></h1>
            
            <%--    Этот контрол можно использовать "внутри" хидера или сайд-бара визарда, а так же отдельно от визарда, но тогда
                    надо задать параметр WizardToNavigate этого контрола с ID-шником необходимого визарда --%>
            <table cellpadding=0 cellspacing=0 class="steps" Width=100%>
            <tr>
            <uc1:WizardTopSideBar runat="server">
                <StepElement>
                    <td width=20%><%# Container.StepTitle %></td>
                </StepElement>
                <StepElementSelected>
                    <td class="on" width=20%>
                   <%# Container.StepTitle %>
                    </td>
                </StepElementSelected>
                <SeparatorTemplate>
                <td class=empty><img id="Img1" runat="server" src="~/images/1pix.gif" width="2" height="2" border="0"></td>
                </SeparatorTemplate>
            </uc1:WizardTopSideBar>
            </tr>
            </table>
        </HeaderTemplate>
        <WizardSteps>
            <asp:WizardStep ID="_generalInfoStep" runat="server" Title="общие сведения" StepType="Start">
                <table cellpadding=0 cellspacing=0 class="info" width=100%>
                     <tr>
                        <th>Категория клиента</th>
                        <td>
                            <asp:RadioButtonList ID="_rblClientCategory" runat="server"  
                                RepeatDirection="Vertical">
                                <asp:ListItem Value="1" Selected="True">физическое лицо</asp:ListItem>
                                <asp:ListItem Value="0">юридическое лицо</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>    
                    </tr>
                    <tr>
                        <th>Объем торговли</th>
                        <td>
                            <asp:RadioButtonList ID="_rblTradingVolume" runat="server" 
                                RepeatDirection="Vertical"
                                OnSelectedIndexChanged="_rblTradingVolume_SelectedIndexChanged"> 
                                <asp:ListItem Value="0" Selected="True">розница</asp:ListItem>
                                <asp:ListItem Value="1">опт</asp:ListItem>
                            </asp:RadioButtonList>
							<asp:CustomValidator ID="_categoryVolumeValidator" runat="server" 
                                ErrorMessage="недопустимое значение объёма торговли для выбранного типа клиента"
                                onservervalidate="_categoryVolumeValidator_ServerValidate" Display="Dynamic"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr id="_compNameRow" style='display: <% =(ClientCategory ==  RmsAuto.Store.Acctg.ClientCategory.Legal) ? "" : "none" %>'>
                        <th><nobr>Название организации <font color=red>*</font></nobr></th>
                        <td>
                            <asp:TextBox ID="_txtCompanyName" runat="server" Width="228px"></asp:TextBox>
                            <asp:CustomValidator ID="_companyNameValidator" runat="server" 
                                ErrorMessage="не задано название организации"
                                onservervalidate="_companyNameCustomValidator_ServerValidate"></asp:CustomValidator>
                        </td>    
                    </tr>
                    <tr>
                        <th>Сфера деятельности</th>
                        <td>
                            <asp:DropDownList ID="_ddlFieldsOfAction" runat="server" Width="230px">
                                <asp:ListItem Text="частное лицо"></asp:ListItem>
                                <asp:ListItem Text="автосервис"></asp:ListItem>
                                <asp:ListItem Text="автомагазин"></asp:ListItem>
                                <asp:ListItem Text="эксперт-оценщик"></asp:ListItem>
                                <asp:ListItem Text="иная"></asp:ListItem>
                            </asp:DropDownList><br />
                            <asp:TextBox ID="_txtFieldOfAction" runat="server" Width="228px" TextMode="MultiLine" ToolTip="Комментарий к сфере деятельности"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Дисконтная карта</th>
                        <td>
                            <uc1:DiscountCard_Edit ID="_discountCardNumber" runat="server" />
                            (заполняется при наличии)
                            <asp:RegularExpressionValidator ID="_regexValidatorDiscountCardNumber" Display="Dynamic" runat="server" ControlToValidate="_discountCardNumber"
                                ErrorMessage="неверно указан номер карты (номер должен быть вида AAA-NNNN-NNNN, где A - латинские буквы, а N - цифры)" ValidationExpression="[A-Za-z]{3}\d{8}" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="empty">
                        <span runat="server" ID="_generalInfoReqMsgLabel"><font color="red">*</font> обязательные поля для заполнения</span>

                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep ID="_contactInfoStep" runat="server" Title="контакты" StepType="Step">
                <table cellpadding=0 cellspacing=0 class="info" width=100%>
                    <tr>
                        <th>Фамилия <font color=red>*</font></th>
                        <td><asp:TextBox ID="_txtLastName" runat="server" Width="228px"></asp:TextBox>
                        <img src="<%=ResolveUrl("~/images/help.gif")%>"  class="help_img" title="Заполняется только русскими буквами" /> 
                            <asp:RequiredFieldValidator ID="_lastNameReqValidator" runat="server" 
                                ControlToValidate="_txtLastName" ErrorMessage="не задана фамилия клиента" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" Display="Dynamic"
                                ErrorMessage="<%$ Resources:Exceptions, LastNameShouldBeCyrillic %>"
                                ControlToValidate="_txtLastName" ValidationExpression="^[А-Яа-я,-,\s]+$" />
                        </td>
                    </tr>
                    <tr>
                        <th>Имя<font color="red">*</font></th>
                        <td><asp:TextBox ID="_txtFirstName" runat="server" Width="228px"></asp:TextBox>
                        <img src="<%=ResolveUrl("~/images/help.gif")%>"  class="help_img" title="Заполняется только русскими буквами" /> 
                            <asp:RequiredFieldValidator ID="_firstNameReqValidator" runat="server" 
                                ControlToValidate="_txtFirstName" ErrorMessage="не задано имя клиента" Display="Dynamic"></asp:RequiredFieldValidator>
                                
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                                ErrorMessage="<%$ Resources:Exceptions, FirstNameShouldBeCyrillic %>"
                                ControlToValidate="_txtFirstName" ValidationExpression="^[А-Яа-я,-,\s]+$" />
                        </td>
                    </tr>
                    <tr>
                        <th>Отчество<font color="red">*</font></th>
                        <td><asp:TextBox ID="_txtMiddleName" runat="server" Width="228px"></asp:TextBox>
                        <img src="<%=ResolveUrl("~/images/help.gif")%>"  class="help_img" title="Заполняется только русскими буквами" /> 
                        <asp:RequiredFieldValidator ID="_MiddleNameReqValidator" runat="server" ControlToValidate="_txtMiddleName"
                            ErrorMessage="не задано отчество клиента" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic"
                                ErrorMessage="<%$ Resources:Exceptions, PatronimicNameShouldBeCyrillic %>"
                                ControlToValidate="_txtMiddleName" ValidationExpression="^[А-Яа-я,-,\s]+$" />
                        </td>
                    </tr>
                    <tr>
                        <th>Основной телефон <font color=red>*</font></th>
                        <td><uc1:Phone_Edit ID="_mainPhone" runat="server" Required="true" />                         
                        </td>
                    </tr>
                    <tr>
                        <th><nobr>Дополнительный телефон 1</nobr></th>
                        <td><uc1:Phone_Edit ID="_auxPhone1" runat="server" Required="false" />
                        </td>
                    </tr>
                    <tr>
                        <th><nobr>Дополнительный телефон 2</nobr></th>
                        <td><uc1:Phone_Edit ID="_auxPhone2" runat="server" Required="false"/>
                        </td>
                    </tr>
                    <tr>
                        <th>Электронный адрес<font color=red>*</font></th>
                        <td>
                            <asp:TextBox ID="_txtEmail" runat="server" Width="165px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_emailRequiredValidator" runat="server"
                                ControlToValidate="_txtEmail" Display="Dynamic" 
                                ErrorMessage="Не указан электронный адрес"></asp:RequiredFieldValidator> 
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="_txtEmail" ErrorMessage="Неверный формат email адреса" 
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="_emailCustomValidator" runat="server" 
                                Display="Dynamic"
                                ControlToValidate="_txtEmail" 
                                ErrorMessage="электронный адрес уже используется другим пользователем интернет-магазина"
                                onservervalidate="_ValidateEmail"></asp:CustomValidator>
                            <uc1:TextItemControl
                                ID="_emailHintTextItem"
                                runat="server"
                                TextItemID="Registration.EmailHint"
                                ShowHeader="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="empty">
							<font color="red">*</font> обязательные поля для заполнения
							<br />
                            <asp:CustomValidator ID="_phoneCustomValidator" runat="server" 
                                ErrorMessage="должен быть задан основной контактный телефон"
                                onservervalidate="_phoneCustomValidator_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                </table>
                
            </asp:WizardStep>
            <asp:WizardStep ID="_shippingInfoStep" runat="server" Title="параметры получения заказов" StepType="Step">
            <table cellpadding=0 cellspacing=0 class="info" width=100%>
                    <tr>
                        <th>Пункт получения заказов <font color=red>*</font></th>
                        <td><asp:DropDownList ID="_ddlRmsStores" runat="server" Width="228px">
                            </asp:DropDownList>
							<asp:RequiredFieldValidator 
							ID="_rmsStoreValidator" runat="server" ControlToValidate="_ddlRmsStores"
							ErrorMessage="не выбран пункт получения заказов"></asp:RequiredFieldValidator> 
							<br />
							Пожалуйста, <span style="color:#295d89;font-weight:bold">выберите удобный для Вас офис продаж</span>, где Вы будете <br />
							оплачивать и получать заказы и при необходимости решать вопросы <br />
							по подбору запчастей с помощью Вашего персонального менеджера.
                        </td>
                    </tr>
                    <tr id="_shippingAddressRow1" runat="server">
                        <th>Адрес доставки <font color=red>**</font></th>
                        <td><asp:TextBox ID="_txtShippingAddress" runat="server" TextMode="MultiLine" Width="228px" Height="150px" MaxLength="500"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="empty">
							<font color="red">*</font> обязательные поля для заполнения
							<span id="_shippingAddressRow2" runat="server">
							<br /><font color="red">**</font> в случае курьерской или почтовой доставки
							</span>
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep ID="_onlineInfoStep" runat="server" Title="Логин/пароль в интернет-магазине" StepType="Step">
                <uc1:EditUser ID="_editUser" runat="server" />
            </asp:WizardStep>
            <asp:WizardStep ID="_reviewClientInfo" runat="server" Title="подтверждение регистрации" StepType="Finish">
            <table cellpadding=0 cellspacing=0 class="info" width=100%>
                    <tr>
                        <th>Клиент</th>
                        <td><% =ClientName %></td>
                    </tr>
                    <tr>
                        <th>Сфера деятельности</th>
                        <td><% =FieldOfAction %></td>
                    </tr>
                    <tr>
                        <th>Категория клиента</th>
                        <td><% = _rblClientCategory.SelectedItem.Text %></td>
                    </tr>
                    <tr>
                        <th>Дисконтная карта</th>
                        <td><% =DiscountCardNumberDisplay %></td>
                    </tr>
                    <tr>
                        <th>Объем торговли</th>
                        <td><% = _rblTradingVolume.SelectedItem.Text %></td>
                    </tr>
                    <tr>
                        <th>Контактное лицо</th>
                        <td><% =ContactPerson %></td>
                    </tr>
                    <tr>
                        <th>Основной телефон</th>
                        <td><% =MainPhone %></td>
                    </tr>
                    <tr>
                        <th>Дополнительный телефон 1</th>
                        <td><% =AuxPhone1 %></td>
                    </tr>
                    <tr>
                        <th>Дополнительный телефон 2</th>
                        <td><% =AuxPhone2 %></td>
                    </tr>
                    <tr>
                        <th>Пункт получения заказов</th>
                        <td><% =!string.IsNullOrEmpty(RmsStoreId) ? AcctgRefCatalog.RmsStores[RmsStoreId].FullInfo : "не выбран" %></td>
                    </tr>
                    <tr>
                        <th>Адрес доставки</th>
                        <td><% =ShippingAddress %></td>
                    </tr>
                    <tr>
                        <th>Код подтверждения <font color=red>*</font></th>
                        <td>
							<div onkeydown="javascript: if(event.keyCode==13) { finishWizard(); return false; }">
							<uc1:captcha runat="server" id="_captcha" />
							</div>							
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="empty">
							<font color="red">*</font> введите код в поле с учетом регистра
						</td>
					</tr>
             </table>
           </asp:WizardStep>
         </WizardSteps>
       <SideBarStyle CssClass="wizard_left" />
     </asp:Wizard>

</div>