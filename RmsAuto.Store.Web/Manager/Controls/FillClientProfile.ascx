<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FillClientProfile.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.FillClientProfile" %>
<%@ Register src="~/Controls/Phone_Edit.ascx" tagname="Phone_Edit" tagprefix="uc1" %>
<%@ Register src="~/Controls/DiscountCard_Edit.ascx" tagname="DiscountCard_Edit" tagprefix="uc1" %>
<%@ Register src="~/Cms/TextItems/TextItemControl.ascx" tagname="TextItemControl" tagprefix="uc1" %>
<script type="text/javascript">

function checkClientCategory()
{
 	var arr = $('input[name$=_rblClientCategory][value=0]');
	if( arr.length!=0 )
	{
		var isLegal = arr.get(0).checked;
		
		if( isLegal ) 
		{
			$('input[name$=_rblTradingVolume][value=0]' ).attr('disabled','disabled');
			$('label[for$=_rblTradingVolume_0]').attr('disabled','disabled');
			$('input[name$=_rblTradingVolume][value=1]' ).attr('checked','checked');

			$('#_compNameRow').show();
		}
		else
		{
			$('input[name$=_rblTradingVolume][value=0]' ).removeAttr('disabled');
			$('label[for$=_rblTradingVolume_0]').removeAttr('disabled');

			$('#_compNameRow').hide();
		}
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
	checkClientCategory();
});
</script>
<div class="information">
    <table>
        <%--<tr>
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
                    RepeatDirection="Vertical"> 
                    <asp:ListItem Value="0" Selected="True">розница</asp:ListItem>
                    <asp:ListItem Value="1">опт</asp:ListItem>
                </asp:RadioButtonList>
             </td>
         </tr>--%>
         <tr id="_compNameRow">
                <th class="nowrap">Название организации *</th>
                <td>
                   <asp:TextBox ID="_txtCompanyName" runat="server" Width="228px"></asp:TextBox>
               
                        <asp:CustomValidator ID="_companyNameValidator" runat="server" 
						    ErrorMessage="не задано название организации"
						    onservervalidate="ValidateCompanyName">
					    </asp:CustomValidator>
                </td>    
        </tr>
        <tr>
             <th class="nowrap">Сфера деятельности</th>
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
            <th class="nowrap">Дисконтная карта</th>
            <td>
                <uc1:DiscountCard_Edit ID="_discountCardNumber" runat="server" />
                (заполняется при наличии)
                <asp:RegularExpressionValidator ID="_regexValidatorDiscountCardNumber" Display="Dynamic" runat="server" ControlToValidate="_discountCardNumber"
                    ErrorMessage="неверно указан номер карты (номер должен быть вида AAA-NNNN-NNNN, где A - латинские буквы, а N - цифры)" ValidationExpression="[A-Za-z]{3}\d{8}" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="empty">
			    &nbsp;
            </td>
        </tr>
        <tr>
           <th>Фамилия*</th>
           <td><asp:TextBox ID="_txtLastName" runat="server" Width="228px"></asp:TextBox>
            <img src="<%=ResolveUrl("~/images/help.gif")%>"  class="help_img" title="Заполняется только русскими буквами" /> 
            <asp:RequiredFieldValidator ID="_lastNameReqValidator" runat="server" 
               ControlToValidate="_txtLastName" ErrorMessage="не задана фамилия клиента" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" Display="Dynamic" ErrorMessage="<%$ Resources:Exceptions, LastNameShouldBeCyrillic %>"
            ControlToValidate="_txtLastName" ValidationExpression="^[А-Яа-я,-,\s]+$" />
           </td>
       </tr>
       <tr>
           <th>Имя <font color=red>*</font></th>
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
                <th>Отчество <font color="red">*</font></th>
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
                    <td colspan="2" class="empty">
            
                        <asp:CustomValidator ID="_phoneCustomValidator" runat="server" 
                            ErrorMessage="должен быть задан основной контактный телефон"
                            onservervalidate="ValidateMainPhone"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <th>Пункт получения заказов</th>
                    <td><asp:DropDownList ID="_ddlRmsStores" runat="server" Width="228px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Адрес доставки</th>
                    <td><asp:TextBox ID="_txtShippingAddress" runat="server" TextMode="MultiLine" Width="228px" Height="75px" MaxLength="500"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2" class="empty">
                        <br />
                    </td>
                </tr>
                <tr>
                    <th>Электронный адрес <font color=red>**</font></th>
                    <td><asp:TextBox ID="_txtEmail" runat="server" Width="228px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="_txtEmail" ErrorMessage="Неверный формат email адреса" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="_emailCustomValidator" runat="server" 
                        Display="Dynamic"
                        ValidateEmptyText="true"
                        ErrorMessage="электронный адрес уже используется другим клиентом"
                        onservervalidate="ValidateEmail"></asp:CustomValidator>
                    <uc1:TextItemControl
                        ID="_emailHintTextItem"
                        runat="server"
                        TextItemID="Registration.EmailHint"
                        ShowHeader="False" />
                    </td>
                </tr>
                <tr>
                    <td class="empty"></td>
                    <td class="empty">
                       <font color=red>**</font> адрес используется только для отправки кода активации и сохраняется<br /> 
                       в учетных данных клиента только в момент активации на сайте     
                    </td>
                </tr>
                <tr>
				    <th></th>
                    <td>
                        <asp:CheckBox ID="_chkRestrict" runat="server" Text="ограниченный эккаунт" />
                    </td>
                </tr>
      </table>
</div>


