<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FillFranchProfileExt.ascx.cs"
Inherits="RmsAuto.Store.Web.Controls.FillFranchProfileExt" %>

<%@ Register Src="~/Controls/Phone_Edit.ascx" TagName="Phone_Edit" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/PhotoFileLoader.ascx" TagName="PhotoFileLoader" TagPrefix="uc2" %>

<script type="text/javascript" language="javascript">
    function _cvDdlIsSelect_ClientValidate(source, arguments)
    {
        if (arguments.Value == 0)
        {
            arguments.IsValid = false;
        }
        else
        {
            arguments.IsValid = true;
        }
    }
</script>

            <style type="text/css">
                .titleStyle
                {
                    font-size: large;
                    margin-top: 10px;
                    height: 43px;
                    vertical-align:middle;
                    width:100%;
	                color:#78868d;
	                text-align:left;
	                font-weight:bold;
	                padding:3px;
	                width:100%;
	                vertical-align:middle;
                }
                .style1
                {
                    width: 40%;
	                border-top:1px solid #c9e2f1;
	                margin-top:0px;
	                color:#78868d;
	                text-align:left;
	                font-weight:bold;
	                padding:3px;
	                vertical-align:middle;
                }
            </style>

            <table runat="server" cellpadding="0" cellspacing="0" width="100%" id="mainTable">
                <tr style="margin-bottom:10px;">
                    <th class="titleStyle" nowrap="nowrap" align="left" colspan="2">
                    <asp:Literal runat="server" Text="Анкетные данные предполагаемого франчайзи"/>
                    </th>
                </tr>
                
                <tr>
                    <th class="style1">
                        <asp:Literal ID="lLocalyti" runat="server" Text="Город" /><font color="red">*</font>
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="Заполняется только русскими буквами" />
                    </th>
                    <td>
                        <asp:TextBox ID="_txtLocality" runat="server" Width="70%"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="_txtLocality"
                            Display="Dynamic" ErrorMessage="Не указан город"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="_txtLocality"
                            Display="Dynamic" ErrorMessage="допустим ввод русских букв, цифр и следующих символов: , . ; - "
                            ValidationExpression="^[А-ЯЁа-яё]+[А-ЯЁа-яё\d\-\s\.;,]*$" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                            runat="SERVER" ControlToValidate="_txtLocality" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,30})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal runat="server" Text="Юридическое название компании" /><font color="red">*</font>
                    </th>
                    <td>
                        <asp:TextBox ID="_jurAddress" runat="server" Width="70%"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="_jurAddress"
                            Display="Dynamic" ErrorMessage="Не указано название компании"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" 
                            runat="SERVER" ControlToValidate="_jurAddress" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,50})$">
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal runat="server" Text="Фактический адрес торговой площади" /><font color="red">*</font>
                    </th>
                    <td>
                        <asp:TextBox ID="_geoAddress" runat="server" Width="70%"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="_geoAddress"
                            Display="Dynamic" ErrorMessage="Не указан фактический адрес"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="_geoAddress"
                            Display="Dynamic" ErrorMessage="допустим ввод русских букв, цифр и следующих символов: , . ; - "
                            ValidationExpression="^[А-ЯЁа-яё]+[А-ЯЁа-яё\d\-\s\.;,]*$" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" 
                            runat="SERVER" ControlToValidate="_geoAddress" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="lScopeType" runat="server" Text="Сфера деятельности" /><font
                            color="red">*</font>
                    </th>
                    <td>
                        <asp:DropDownList ID="_ddlScopeType" runat="server" Width="70%">
                            <asp:ListItem Text="<выберите>" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CustomValidator runat="server" ErrorMessage="выберите значение из списка"
                            ControlToValidate="_ddlScopeType" Display="Dynamic"
                            ClientValidationFunction="_cvDdlIsSelect_ClientValidate" />
                        <br />
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal9" runat="server" Text="Контактное лицо" /><font color="red">*</font>
                    </th>
                    <td>
                        <asp:TextBox ID="_cntName" runat="server" Width="70%"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="_cntName"
                            Display="Dynamic" ErrorMessage="Не указано контактное лицо"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="_cntName"
                            Display="Dynamic" ErrorMessage="допустим ввод русских букв, цифр и следующих символов: , . ; - "
                            ValidationExpression="^[А-ЯЁа-яё]+[А-ЯЁа-яё\d\-\s\.;,]*$" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" 
                            runat="SERVER" ControlToValidate="_cntName" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal runat="server" Text="Должность" /><font color="red">*</font>
                    </th>
                    <td>
                        <asp:TextBox ID="_appoint" runat="server" Width="70%"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="_appoint"
                            Display="Dynamic" ErrorMessage="Не указана должность"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" 
                            runat="SERVER" ControlToValidate="_appoint" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                 <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal11" runat="server" Text="Контактный телефон" /><font color="red">*</font>
                    </th>
                    <td Width="70%">
                        <uc1:Phone_Edit runat="server" Required="true" ID="_ContactPhone" runat="server" 
                            CheckSummCount="True" />
                    </td>
                </tr>

                 <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal2" runat="server" Text="Контактный 2-й телефон" />
                    </th>
                    <td Width="70%">
                        <uc1:Phone_Edit ID="_ContactPhone_2" runat="server" Required="false" CheckSummCount="True" />
                    </td>
                </tr>

                 <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal1" runat="server" Text="Электронный адрес" /><font color="red">*</font>
                    </th>
                    <td>
                        <asp:TextBox ID="_txtEmail" runat="server" Width="70%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_emailRequiredValidator" runat="server" ControlToValidate="_txtEmail"
                            Display="Dynamic" ErrorMessage="Не указан электронный адрес"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="_txtEmail"
                            ErrorMessage="Неверный формат email адреса" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            Display="Dynamic"></asp:RegularExpressionValidator>
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal8" runat="server" Text="Web-адрес сайта компании" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox8" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator 
                            runat="SERVER" ControlToValidate="TextBox8" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                 <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal12" runat="server" Text="Основные товарные позиции" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox12" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" 
                            runat="SERVER" ControlToValidate="TextBox12" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
               
                   <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal14" runat="server" Text="Наличие своего автопарка" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="Укажите кол-во а/м и для каких целей" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox14" runat="server" Width="70%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal15" runat="server" Text="Наличие бизнеса по системе Франчайзинга" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="Укажите торговую марку в случае положительного ответа" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox15" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" 
                            runat="SERVER" ControlToValidate="TextBox15" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal16" runat="server" Text="Планирование рекламной деятельности" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox16" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" 
                            runat="SERVER" ControlToValidate="TextBox16" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                
                   <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal17" runat="server" Text="Присутствие конкурирующих торговых сетей в Вашем  городе" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="Укажите основных конкурентов" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox17" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" 
                            runat="SERVER" ControlToValidate="TextBox17" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal18" runat="server" Text="Товарооборот по компании за последние 12 месяцев" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="В рублях" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox18" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator11" 
                            runat="SERVER" ControlToValidate="TextBox18" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal3" runat="server" Text="Процент своего объема продаж в Вашем регионе" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="Примерная оценка" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator12" 
                            runat="SERVER" ControlToValidate="TextBox1" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal19" runat="server" Text="Количество сотрудников в компании" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox19" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" 
                            runat="SERVER" ControlToValidate="TextBox19" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal21" runat="server" Text="Площадь склада" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox21" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator14" 
                            runat="SERVER" ControlToValidate="TextBox21" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal22" runat="server" Text="Площадь магазина" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox22" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator15" 
                            runat="SERVER" ControlToValidate="TextBox22" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal23" runat="server" Text="Площадь офиса" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox23" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator16" 
                            runat="SERVER" ControlToValidate="TextBox23" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal24" runat="server" Text="Кол-во наименований товара в наличии" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="На текущий момент" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox24" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator17" 
                            runat="SERVER" ControlToValidate="TextBox24" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal25" runat="server" Text="Кол-во наименований заказных позиций" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="В месяц" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox25" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator18" 
                            runat="SERVER" ControlToValidate="TextBox25" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal26" runat="server" Text="Кол-во поставщиков" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox26" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator19" 
                            runat="SERVER" ControlToValidate="TextBox26" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                
                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal27" runat="server" Text="Кол-во оптовых клиентов" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox27" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator20" 
                            runat="SERVER" ControlToValidate="TextBox27" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal28" runat="server" Text="Кол-во розничных клиентов" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox28" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator21" 
                            runat="SERVER" ControlToValidate="TextBox28" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal29" runat="server" Text="Кол-во приходных документов в месяц" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox29" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator22" 
                            runat="SERVER" ControlToValidate="TextBox29" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal runat="server" Text="Кол-во расходных документов в месяц" Mode="PassThrough" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox30" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator23" 
                            runat="SERVER" ControlToValidate="TextBox30" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal31" runat="server" Text="Кол-во оплат поставщикам в месяц" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox31" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator24" 
                            runat="SERVER" ControlToValidate="TextBox31" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal32" runat="server" Text="Кол-во оплат от покупателей в месяц" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox32" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator25" 
                            runat="SERVER" ControlToValidate="TextBox32" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal4" runat="server" Text="Способы развития клиентской базы" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator26" 
                            runat="SERVER" ControlToValidate="TextBox2" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal33" runat="server" Text="Модель принтера для этикеток" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="Модель, характеристики и кол-во принтеров" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox33" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator27" 
                            runat="SERVER" ControlToValidate="TextBox33" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal34" runat="server" Text="Модель кассового оборудования" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox34" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator28" 
                            runat="SERVER" ControlToValidate="TextBox34" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal35" runat="server" Text="Модель ККМ" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="Контрольно-кассовая машина" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox35" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator29" 
                            runat="SERVER" ControlToValidate="TextBox35" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal36" runat="server" Text="Модель сканера штрихода" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="Модель, характеристики и кол-во" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox36" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator30" 
                            runat="SERVER" ControlToValidate="TextBox36" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal37" runat="server" Text="Название учетно-торговой системы" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox37" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator31" 
                            runat="SERVER" ControlToValidate="TextBox37" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal38" runat="server" Text="Кол-во пользователей в учетно-торговой системе" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox38" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator32" 
                            runat="SERVER" ControlToValidate="TextBox38" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal5" runat="server" Text="Скорость и качество внешнего канала связи, Mb/сек"/>
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox39" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator33" 
                            runat="SERVER" ControlToValidate="TextBox39" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal40" runat="server" Text="Скорость и качество локального канала связи, Mb/сек"/>
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox40" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator34" 
                            runat="SERVER" ControlToValidate="TextBox40" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal41" runat="server" Text="Наличие автоматизации клиент-банка" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox41" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator35" 
                            runat="SERVER" ControlToValidate="TextBox41" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal ID="Literal42" runat="server" Text="Наличие СУС" />
                        <img src="<%=ResolveUrl("~/images/help.gif")%>" class="help_img" title="Система управления складом" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox42" runat="server" Width="70%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator36" 
                            runat="SERVER" ControlToValidate="TextBox42" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,100})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>

                <tr>
                    <th class="style1">
                        <asp:Literal runat="server" Text="Дополнительная информация, комментарии, предложения и вопросы" />
                    </th>
                    <td>
                        <asp:TextBox ID="TextBox421" runat="server" Width="70%" Height="100px" TextMode="MultiLine"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator37" 
                            runat="SERVER" ControlToValidate="TextBox421" 
                            ErrorMessage="Ограничение на длину строки" ValidationExpression="^(.{0,1000})$">
                        </asp:RegularExpressionValidator>                    
                    </td>
                </tr>
                
                
            <tr>
                <th class="style1">
                        <asp:Literal runat="server" Text="Фотография, вид с улицы" />
                </th>
                <td Width="70%">
                    <uc2:PhotoFileLoader runat="server" ID="externalPhoto" CheckFileIsSelected ="false"/>
                </td>
            </tr>
                
            <tr>
                <th class="style1">
                        <asp:Literal runat="server" Text="Фотография, вид внутри помещения" />
                </th>
                <td Width="70%">
                    <uc2:PhotoFileLoader runat="server" ID="internalPhoto" CheckFileIsSelected ="false"/>
                </td>
            </tr>
                
            <tr>
                <th align="left" style="height:40px;"/>
            </tr>
            
            </table>
