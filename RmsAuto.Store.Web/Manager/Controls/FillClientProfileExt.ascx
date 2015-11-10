<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FillClientProfileExt.ascx.cs"
    Inherits="RmsAuto.Store.Web.Manager.Controls.FillClientProfileExt" %>
<%@ Register Src="~/Controls/DiscountCard_Edit.ascx" TagName="DiscountCard_Edit"
    TagPrefix="uc1" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Phone_Edit.ascx" TagName="Phone_Edit" TagPrefix="uc1" %>
<%@ Import Namespace="RmsAuto.Store.Acctg" %>

<script type="text/javascript">
    function _cvDdlIsSelect_ClientValidate(source, arguments) {
        if (arguments.Value == 0) {
            arguments.IsValid = false;
        } else {
            arguments.IsValid = true;
        }
    }
</script>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div class="information">
<table>
    <tr>
        <th>
            <%= global::Resources.Texts.Email %>
        </th>
        <td>
            <asp:TextBox ID="_txtEmail" runat="server" Width="222px"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="_txtEmail"
                ErrorMessage="<%$ Resources:ValidatorsMessages, WrongEmailFormat %>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                Display="Dynamic"></asp:RegularExpressionValidator>
            <br />
            <span style="font-size:xx-small;"><%= global::Resources.Texts.EmailUsed %></span>
        </td>
    </tr>
    <%--<tr>
        <th>
            Категория покупателя<font color="red">*</font>
        </th>
        <td>
            <asp:RadioButtonList ID="_rblTradingVolume" runat="server" RepeatDirection="Vertical"
                AutoPostBack="true">
                <asp:ListItem Value="0" Selected="True">розница</asp:ListItem>
                <asp:ListItem Value="1">опт</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th>
            Организационно-правовая форма<font color="red">*</font>
        </th>
        <td>
            <asp:RadioButtonList ID="_rblClientCategory" runat="server" RepeatDirection="Vertical"
                AutoPostBack="true">
                <asp:ListItem Value="0">юридическое лицо (ООО, ОАО, ЗАО)</asp:ListItem>
                <asp:ListItem Value="2">индивидуальный предприниматель (ИП)</asp:ListItem>
                <asp:ListItem Value="1" Selected="True">физическое лицо</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>--%>
    <tr>
        <th class="nowrap">
            <%= global::Resources.RegistrationTexts.Country %> *
        </th>
        <td>
            <asp:DropDownList ID="_ddlCountry" runat="server" Width="230px" AutoPostBack="true"
                OnSelectedIndexChanged="_ddlCountry_SelectedIndexChanged">
                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SelectDdl %>" Value="0"></asp:ListItem>
            </asp:DropDownList>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Select country from list"
                ControlToValidate="_ddlCountry" Display="Dynamic" OnServerValidate="_cvDdlIsSelect_ServerValidate" />
            <br />
        </td>
    </tr>
    <tr>
        <th class="nowrap">
            <%= global::Resources.RegistrationTexts.City %> *
        </th>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="_ddlCountry" EventName="SelectedIndexChanged" />
                </Triggers>
                <ContentTemplate>
                    <asp:TextBox ID="_ddlRegion" runat="server" Width="222px">
                    </asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    
    
    <tr>
        <th class="nowrap">
            <%= global::Resources.RegistrationTexts.Scope %>: *
        </th>
        <td>
           
            <asp:TextBox ID="_txtScopeType" runat="server" Width="222px" TextMode="MultiLine"
                ToolTip="<%$ Resources:RegistrationTexts, CommentHint %>"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th class="nowrap">
            <%= global::Resources.RegistrationTexts.SourceOfInformation %> *
        </th>
        <td>
            <asp:DropDownList ID="_ddlHKT" runat="server" Width="230px">
                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SelectDdl %>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_InternetSearch %>" Value="1"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_OnlineAdvertising %>" Value="2"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_PreviouslyWorked %>" Value="3"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_Recommended %>" Value="4"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:RegistrationTexts, SI_Other %>" Value="5"></asp:ListItem>
            </asp:DropDownList>
            <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="<%$ Resources:ValidatorsMessages, SelectSourceInformation %>"
                ControlToValidate="_ddlHKT" Display="Dynamic" OnServerValidate="_cvDdlIsSelect_ServerValidate"
                ClientValidationFunction="_cvDdlIsSelect_ClientValidate" />
            <br />
            <asp:TextBox ID="_txtHKT" runat="server" Width="222px" TextMode="MultiLine" ToolTip="comment"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>
        </th>
        <td>
            <span class="blue"><%= global::Resources.RegistrationTexts.ContactInformation %></span>
        </td>
    </tr>
    <asp:PlaceHolder ID="vreg_LegalIP_202" runat="server">
        <tr>
            <th>
                <%= global::Resources.RegistrationTexts.ContactPosition %>
            </th>
            <td>
                <asp:TextBox ID="_txtContactPosition" runat="server" Width="222px"></asp:TextBox> 
                <span class="info" title="<%$ Resources:ValidatorsMessages, LettersAllowed %>" runat="server"></span>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator21" runat="server"
                    Display="Dynamic" ErrorMessage="<%$ Resources:Exceptions, PositionShouldBeCyrillic %>"
                    ControlToValidate="_txtContactPosition" ValidationExpression="^[А-Яа-я,A-za-z,-,\s]+$" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th class="nowrap">
            <%= global::Resources.RegistrationTexts.Surname %> *
        </th>
        <td>
            <asp:TextBox ID="_txtContactLastName" runat="server" Width="222px"></asp:TextBox>
            <span class="info" title="<%$ Resources:ValidatorsMessages, LettersAllowed %>" runat="server"></span>
            <asp:RequiredFieldValidator ID="_lastNameReqValidator" runat="server" ControlToValidate="_txtContactLastName"
                ErrorMessage="<%$ Resources:ValidatorsMessages, FamilyName %>" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                Display="Dynamic" ErrorMessage="<%$ Resources:Exceptions, LastNameShouldBeCyrillic %>"
                ControlToValidate="_txtContactLastName" ValidationExpression="^[А-Яа-я,A-za-z,-,\s]+$" />
        </td>
    </tr>
    <tr>
        <th class="nowrap">
            <%= global::Resources.RegistrationTexts.Name %> *
        </th>
        <td>
            <asp:TextBox ID="_txtContactFirstName" runat="server" Width="222px"></asp:TextBox>
            <span class="info" title="<%$ Resources:ValidatorsMessages, LettersAllowed %>" runat="server"></span>
            <asp:RequiredFieldValidator ID="_firstNameReqValidator" runat="server" ControlToValidate="_txtContactFirstName"
                ErrorMessage="<%$ Resources:ValidatorsMessages, NameNotFilled %>" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                ErrorMessage="<%$ Resources:Exceptions, FirstNameShouldBeCyrillic %>" ControlToValidate="_txtContactFirstName"
                ValidationExpression="^[А-Яа-я,A-za-z,-,\s]+$" />
        </td>
    </tr>
   
    <tr>
        <th class="nowrap">
            <%= global::Resources.RegistrationTexts.MainPhone %> *
        </th>
        <td>
            <uc1:Phone_Edit ID="_ContactPhone" runat="server" Required="true" CheckSummCount="True" />
        </td>
    </tr>
    <tr>
        <th class="nowrap">
            <%= global::Resources.RegistrationTexts.AdditionalPhone %>
        </th>
        <td>
            <uc1:Phone_Edit ID="_ContactExtPhone" runat="server" Required="false" />
        </td>
    </tr>
    <asp:PlaceHolder ID="vregRO_Opt_218" runat="server">
        <tr>
            <th>
                <%= global::Resources.RegistrationTexts.Fax %>
            </th>
            <td>
                <uc1:Phone_Edit ID="_ContactFax" runat="server" Required="false" CheckSummCount="True" />
            </td>
        </tr>
        
    </asp:PlaceHolder>

    <tr>
        <th class="nowrap">
            <%= global::Resources.RegistrationTexts.DeliveryAddress %>
        </th>
        <td>
            <asp:TextBox ID="_txtShippingAddress" runat="server" Height="100px" MaxLength="1024"
                TextMode="MultiLine" Width="222px"></asp:TextBox>
        </td>
    </tr>
    
    
    
        <asp:PlaceHolder ID="vreg_Legal_808" runat="server">
                        <tr>
                            <th><asp:Literal ID="lLegalName" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyName %>" />: *</th>
                            <td><asp:TextBox ID="_txtLegalName" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtLegalName"
                                ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyCompanyName %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lCompanyRegistrationID" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyRegistrationID %>" />: *</th>
                            <td><asp:TextBox ID="_txtCompanyRegistrationID" runat="server" Width="222px" MaxLength="200"></asp:TextBox>
							<span class="info" title="Trade License No / VAT ID / TAX ID"></span>&nbsp;<asp:RequiredFieldValidator ID="rfvCompanyRegistrationID" runat="server" ControlToValidate="_txtCompanyRegistrationID"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </tr>
                       
                    </asp:PlaceHolder>

    <asp:PlaceHolder ID="vreg_LegalIP_392" runat="server">
          
                        <tr>
                            <th><asp:Literal ID="lLegalAddress" runat="server" Text="<%$ Resources:RegistrationTexts, CompanyAddress %>" />: *</th>
                            <td><asp:TextBox ID="_txtLegalAddress" runat="server" Height="100px" MaxLength="1024"
                                            TextMode="MultiLine" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator CssClass="validator" ID="RequiredFieldValidator6" runat="server" ControlToValidate="_txtLegalAddress"
                                            Display="Dynamic" ErrorMessage="<%$ Resources:ValidatorsMessages, EmptyCompanyAddress %>"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <th></th>
                            <td><span class="blue"><asp:Literal ID="lBankInfo" runat="server" Text="<%$ Resources:RegistrationTexts, BankDetails %>" /></span></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lBankName" runat="server" Text="<%$ Resources:RegistrationTexts, BankName %>"></asp:Literal>: *</th>
                            <td><asp:TextBox ID="_txtBankName" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvBankName" runat="server" ControlToValidate="_txtBankName"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic" /></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lIBAN" runat="server" Text="<%$ Resources:RegistrationTexts, IBAN %>" />: *</th>
                            <td><asp:TextBox ID="_txtIBAN" runat="server" Width="222px" MaxLength="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvIBAN" runat="server" ControlToValidate="_txtIBAN"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lSWIFT" runat="server" Text="<%$ Resources:RegistrationTexts, SWIFT %>" /> *</th>
                            <td><asp:TextBox ID="_txtSWIFT" runat="server" Width="222" MaxLength="200"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvSWIFT" runat="server" ControlToValidate="_txtSWIFT"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </tr>
                        <ltr>
                            <th><asp:Literal ID="lBankAddress" runat="server" Text="<%$ Resources:RegistrationTexts, BankAddress %>" />: *</th>
                            <td><asp:TextBox ID="_txtBankAddress" runat="server" MaxLength="200" TextMode="MultiLine" Rows="2"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvBankAddress" CssClass="validator" runat="server" ControlToValidate="_txtBankAddress"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </ltr>
                        <tr>
                            <th></th>
                            <td><span class="blue"><asp:Literal ID="lDirector" runat="server" Text="<%$ Resources:RegistrationTexts, GeneralManager %>" /></span></td>
                        </tr>

                  
                        <tr>
                            <th><asp:Literal ID="lDirectorFirstName" runat="server" Text="<%$ Resources:RegistrationTexts, Name %>" />: *</th>
                            <td><asp:TextBox ID="_txtDirectorFirstName" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="_txtDirectorFirstName"
                                ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" Display="Dynamic"
                                ErrorMessage="<%$ Resources:Exceptions, InvalidCharacter %>" ControlToValidate="_txtDirectorFirstName"
                                ValidationExpression="^[А-ЯЁа-яёA-Za-z,-,\s]+$" /></td>
                        </tr>
                        <tr>
                            <th><asp:Literal ID="lDirectorLastName" runat="server" Text="<%$ Resources:RegistrationTexts, Surname %>" />: *</th>
                            <td><asp:TextBox ID="_txtDirectorLastName" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="_txtDirectorLastName"
                                ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" Display="Dynamic"
                                ErrorMessage="<%$ Resources:Exceptions, InvalidCharacter %>" ControlToValidate="_txtDirectorLastName"
                                ValidationExpression="^[А-ЯЁа-яёA-Za-z,-,\s]+$" /></td>
                        </tr>
                    
                    </asp:PlaceHolder>

      

       <tr>
                        <th></th>
                        <td><span class="blue"><asp:Literal ID="lCorrespondentBank" runat="server" Text="<%$ Resources:RegistrationTexts, CorrespondentBank %>" /></span></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCorrespondentBankName" runat="server" Text="<%$ Resources:RegistrationTexts, BankName %>"></asp:Literal></th>
                        <td><asp:TextBox ID="_txtCorrespondentBankName" runat="server" Width="222px" MaxLength="200"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCorrespondentIBAN" runat="server" Text="<%$ Resources:RegistrationTexts, IBAN %>" /></th>
                        <td><asp:TextBox ID="_txtCorrespondentIBAN" runat="server" Width="222px" MaxLength="200"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCorrespondentSWIFT" runat="server" Text="<%$ Resources:RegistrationTexts, SWIFT %>" /></th>
                        <td><asp:TextBox ID="_txtCorrespondentSWIFT" runat="server" Width="222px" MaxLength="200"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="lCorrespondentBankAddress" runat="server" Text="<%$ Resources:RegistrationTexts, BankAddress %>" /></th>
                        <td><asp:TextBox ID="_txtCorrespondentBankAddrss" runat="server" MaxLength="200" TextMode="MultiLine" Rows="2"></asp:TextBox></td>
                    </tr>
    <tr>
        <th>
        </th>
        <td>
            <asp:CheckBox ID="_chkRestrict" runat="server" Text="<%$ Resources:RegistrationTexts, LimitedAccount %>" />
        </td>
    </tr>
    <tr>
        <th></th>
        <td class="empty">
            * <%= global::Resources.RegistrationTexts.ObligatoryFields %>
        </td>
    </tr>
</table>
</div>