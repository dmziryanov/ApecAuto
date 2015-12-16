<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientProfileViewLite.ascx.cs" Inherits="RmsAuto.Store.Web.Manager.Controls.ClientProfileViewLite" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<div class="information">
<table>
    
    
    <%--<asp:PlaceHolder ID="vreg_LegalPhy_15" runat="server">
    <tr>
        <th>
            Наименование
        </th>
        <td>
            <asp:Label runat="server" ID="_ClientName" />
        </td>
    </tr>
    </asp:PlaceHolder>
    
     <asp:PlaceHolder ID="vreg_IP_13" runat="server">
         <tr>
            <th>
                <nobr>Наименование</nobr>
            </th>
            <td>
                <asp:TextBox runat="server" ID="_ClientNameIP" Width="317px" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="_ClientNameIP"
                                ErrorMessage="не задано наименование" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
              
        </tr>
    </asp:PlaceHolder>--%>
    <tr>
		<th>Entitlement</th>
		<td>
			<asp:TextBox runat="server" ID="txtClientName" Width="317px" />
			<asp:RequiredFieldValidator ID="rfv_ClientName" runat="server" ControlToValidate="txtClientName"
				ErrorMessage="Entitlement is not defined" Display="Dynamic" />
		</td>
    </tr>
    
    <tr>
        <th>
            e-mail
        </th>
        <td>
            <asp:TextBox runat="server" ID="_Email" Width="317px" />
           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_Email"
                                ErrorMessage="не задан почтовый адрес" Display="Dynamic"></asp:RequiredFieldValidator> Пока не убирать, вдруг понадобится--%>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="_Email" ErrorMessage="Wrong email address" 
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                Display="Dynamic">
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>
            Client category
        </th>
        <td>
            <asp:Label runat="server" ID="_TradingVolume" />
        </td>
    </tr>
    <tr>
        <th>
            Legal type
        </th>
        <td>
            <asp:Label runat="server" ID="_ClientCategory" />
        </td>
    </tr>
    <tr>
        <th>
            Country
        </th>
        <td>
            <asp:Label runat="server" ID="_Country" />
        </td>
    </tr>
    <tr>
        <th>
            Locality (district)
        </th>
        <td>
            <asp:Label runat="server" ID="_Locality" />
        </td>
    </tr>
    <tr>
        <th>
            Scope type
        </th>
        <td>
            <asp:Label runat="server" ID="_ScopeType" />
        </td>
    </tr>
    <tr>
        <th>
            Information source
        </th>
        <td>
            <asp:Label runat="server" ID="_HowKnow" />
        </td>
    </tr>
    <tr>
        <th></th>
			<td>
				<span class="blue"><asp:Literal ID="lContactInfo" runat="server"  Text="<%$ Resources:RegistrationTexts, ContactInformation %>" /> </span>
			</td>
    </tr>
    <tr>
        <th>
            additional e-mail
        </th>
        <td>
            <asp:TextBox Width="317px" runat="server" ID="email2" />
                        <asp:CompareValidator ID="CompareValidator1" runat="server"  ControlToValidate="email2" 
                        ControlToCompare="_Email" Operator="NotEqual"
                        ErrorMessage="Main and additional e-mail are the same"></asp:CompareValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="email2" ErrorMessage="wrong e-mail format" 
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                Display="Dynamic">
            </asp:RegularExpressionValidator>                        
        </td>
    </tr>
    
    <asp:PlaceHolder ID="vreg_LegalIP_93" runat="server">
        <tr>
            <th>
                Job position
            </th>
            <td>
                <asp:TextBox Width="317px"  runat="server" ID="_ContactPosition" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th>
            Surname
        </th>
        <td>
            <asp:TextBox Width="317px" runat="server" ID="_ContactLastName" ></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>
            Name
        </th>
        <td>
            <asp:TextBox Width="317px" runat="server" ID="_ContactFirstName" />
        </td>
    </tr>
      <tr>
        <th>
            Main phone
        </th>
        <td>
            <asp:TextBox Width="317px" runat="server" ID="_ContactPhone" />
        </td>
    </tr>
    <tr>
        <th>
            <nobr>Additional phone</nobr>
        </th>
        <td>
            <asp:TextBox Width="317px" runat="server" ID="_ContactExtPhone" />
        </td>
    </tr>
    <asp:PlaceHolder ID="vregRO_Opt_143" runat="server">
        <tr>
            <th>
                Fax
            </th>
            <td>
                <asp:TextBox Width="317px" runat="server" ID="_ContactFax" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="vregRO_Rozn_169" runat="server">
        <tr>
            <th>
                Point of delivery
            </th>
            <td>
                <asp:TextBox Width="317px" runat="server" ID="_RmsStores" />
            </td>
        </tr>
        <tr>
            <th>
                Discount card number
            </th>
            <td>
                <asp:TextBox Width="317px" runat="server" ID="_DiscountCardNumber" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th>
            Delivery address
        </th>
        <td>
            <asp:TextBox runat="server" ID="_ShippingAddress" Width="317px" />
        </td>
    </tr>
    <asp:PlaceHolder ID="vreg_LegalIP_195" runat="server">
        <tr>
			<th></th>
			<td>
				<span class="blue"><asp:Literal ID="lLegalInfo" runat="server" Text="<%$ Resources:RegistrationTexts, LegalInformation %>" /></span>
			</td>
		</tr>
		<tr>
			<th>
				<nobr><asp:Literal ID="lLegalName" runat="server" 
					Text="Company name" meta:resourcekey="lLegalNameResource1" /></nobr>
			</th>
			<td><asp:Label ID="_CompanyName" runat="server" /></td>
		</tr>
		<tr>
			<th><asp:Literal ID="lCompanyRegistrationID" runat="server" Text="Company registration ID" /></th>
			<td><asp:Label ID="_CompanyRegistrationID" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<th><asp:Literal ID="lCompanyAddress" runat="server" Text="Company address" /></th>
			<td><asp:Label ID="_CompanyAddress" runat="server"></asp:Label></td>
		</tr>
    </asp:PlaceHolder>


    
   
    <asp:PlaceHolder ID="vreg_LegalIP_257" runat="server">
        <tr>
<th></th>
			<td>
				<span class="blue">
					<asp:Literal ID="lBankInfo" runat="server"  Text="<%$ Resources:RegistrationTexts, BankDetails %>"  />
				</span>
			</td>
        </tr>
              <tr>
       <th><asp:Literal ID="lBankName" runat="server" Text="<%$ Resources:RegistrationTexts, BankName %>"></asp:Literal>: *</th>
       <td><asp:TextBox ID="_txtBankName" runat="server" Width="222px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvBankName" runat="server" ControlToValidate="_txtBankName"
	    ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic" /></td>
    </tr>
         <tr>
                            <th><asp:Literal ID="lBankAddress" runat="server" Text="<%$ Resources:RegistrationTexts, BankAddress %>" />: *</th>
                            <td><asp:TextBox ID="_txtBankAddress" runat="server" MaxLength="200" TextMode="MultiLine" Rows="2"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvBankAddress" CssClass="validator" runat="server" ControlToValidate="_txtBankAddress"
								ErrorMessage="<%$ Resources:ValidatorsMessages, FieldIsEmpty %>" Display="Dynamic"></asp:RequiredFieldValidator></td>
        </tr>

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
        <tr>
            <th>
                IBAN
            </th>
            <td>
                <asp:TextBox runat="server" ID="_txtIBAN" />
            </td>
        </tr>
        <tr>
            <th>
                SWIFT
            </th>
            <td>
                <asp:TextBox runat="server" ID="_txtSWIFT" />
            </td>
        </tr>
             <th></th>
                    <td><span class="blue"><asp:Literal ID="lCorrespondentBank" runat="server" Text="<%$ Resources:RegistrationTexts, CorrespondentBank %>" /></span>

                    </td>
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
  
    </asp:PlaceHolder>
   
   
	<tr>
		<th>
			Personal manager
		</th>
		<td>
			<asp:DropDownList runat="server" ID="_managerList" AutoPostBack="False" 
                DataTextField="FullName" DataValueField="EmployeeID" Height="25px" 
                Width="320px">
			</asp:DropDownList>
			<%--(<asp:Literal runat="server" ID="_managerDepartmentLabel" />)--%>
		</td>
	</tr>
	<tr id="Tr1" runat="server" visible="true">
		<th>
			Price #
		</th>
		<td>
			<%--onselectedindexchanged="_clientGroupName_SelectedIndexChanged"--%>
			<asp:DropDownList runat="server" ID="_clientGroupName" AutoPostBack="False" 
                Height="25px" Width="320px">
			    <asp:ListItem Value="1" Text="Retail basic"></asp:ListItem>
			    <asp:ListItem Value="2" Text="Retail -3%"></asp:ListItem>
			    <asp:ListItem Value="3" Text="Retail -5%"></asp:ListItem>
			    <asp:ListItem Value="4" Text="Retail -7%"></asp:ListItem>
			    <asp:ListItem Value="20" Text="F-Wholesale1"></asp:ListItem>
			    <asp:ListItem Value="21" Text="F-Wholesale2"></asp:ListItem>
			    <asp:ListItem Value="8"  Text="F-Wholesale3"></asp:ListItem>
			    <asp:ListItem Value="10" Text="F-Wholesale4"></asp:ListItem>
			    <asp:ListItem Value="13" Text="F-Wholesale5"></asp:ListItem>
			    <asp:ListItem Value="15" Text="F-Wholesale6"></asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<%--<tr>
		<th>Автозаказ</th>
		<td>
			<asp:CheckBox ID="chbIsAutoOrder" runat="server" />
		</td>
	</tr>--%>
	<tr>
		<th>is Checked</th>
		<td>
			<asp:CheckBox ID="chbIsChecked" runat="server" />
		</td>
	</tr>
	<tr>
		<th>
			Prepayment percent
		</th>
		<td>
			<asp:TextBox ID="_prepaymentPercentLabel" runat="server" Width="57px"></asp:TextBox>&nbsp;%
		</td>
	</tr>
    <tr>
		<th>
			Possible payment delay
		</th>
		<td>
			<asp:TextBox ID="_DelayDays" runat="server" Width="55px"></asp:TextBox>&nbsp;days
		</td>
	</tr>	
	
    <tr>
		<th>
			Limit on orders
		</th>
		<td>
			<asp:TextBox ID="_paymentLimit" runat="server" Width="55px"></asp:TextBox>&nbsp;<%=global::Resources.Texts.RoubleShort %>
			&nbsp; <asp:RangeValidator ID="_paymentLimitRangeValidator" runat="server"
						ControlToValidate="_paymentLimit"
						MinimumValue="0"
						MaximumValue="100000"
						Type="Double"
						Text="Value must rank from 0 to 100 000 usd."></asp:RangeValidator>
		</td>
	</tr>	
		
	<tr>
		<%--<th>
		    Аккаунт ограничен
		</th>
		<td>
			<asp:Label runat="server" ID="_restrictedTrueLabel">Да</asp:Label>
			<asp:Label runat="server" ID="_restrictedFalseLabel">Нет</asp:Label>
		</td>--%>
	</tr>
	<tr>
		<th></th>
		<td><asp:Button ID="btnSave" runat="server" Text="Save" Class="btn btn-primary btn-sm" OnClick="btnSave_Click" /></td>
	</tr>
</table>
</div>

