<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientProfile.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.ClientProfile" %>

<div class="information">
	<table>
    
    <tr id="Tr1" runat="server" visible="false">
        <th>
            <asp:Literal ID="lCroupName" runat="server" Text="Прайс–лист №"
				meta:resourcekey="lCroupNameResource1" />
        </th>
        <td>
            <asp:Label runat="server" ID="_clientGroupName"
				meta:resourcekey="_clientGroupNameResource1" />
        </td>
    </tr>
		<%--<tr runat="server" visible="false">
			<th>
				<asp:Literal ID="lCroupName" runat="server" Text="Прайс–лист №"
					meta:resourcekey="lCroupNameResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_clientGroupName"
					meta:resourcekey="_clientGroupNameResource1" />
			</td>
		</tr>--%>
		<tr class="bg">
			<th>
				<asp:Literal ID="lClientName" runat="server" Text="Наименование" 
					meta:resourcekey="lClientNameResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_ClientName" 
					meta:resourcekey="_ClientNameResource1" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lEmail" runat="server" Text="Электронный адрес" 
					meta:resourcekey="lEmailResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_Email" meta:resourcekey="_EmailResource1" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lTradingVolume" runat="server" Text="Объем закупок" 
					meta:resourcekey="lTradingVolumeResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_TradingVolume" 
					meta:resourcekey="_TradingVolumeResource1" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lClientCategory" runat="server" 
					Text="Организационно-правовая форма" 
					meta:resourcekey="lClientCategoryResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_ClientCategory" 
					meta:resourcekey="_ClientCategoryResource1" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lCountry" runat="server" Text="Страна" 
					meta:resourcekey="lCountryResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_Country" meta:resourcekey="_CountryResource1" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lLocality" runat="server" Text="Населенный пункт (округ)" 
					meta:resourcekey="lLocalityResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_Locality" 
					meta:resourcekey="_LocalityResource1" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lScopeType" runat="server" Text="Сфера вашей деятельности" 
					meta:resourcekey="lScopeTypeResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_ScopeType" 
					meta:resourcekey="_ScopeTypeResource1" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lHowKnow" runat="server" Text="Источник информации" 
					meta:resourcekey="lHowKnowResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_HowKnow" />
			</td>
		</tr>
		<!-- ============== Contact information =================== -->
		<tr>
			<th></th>
			<td>
				<span class="blue"><asp:Literal ID="lContactInfo" runat="server" Text="Контактная информация" meta:resourcekey="lContactInfoResource1" /></span>
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lContactPosition" runat="server" Text="Должность исполнителя" 
					meta:resourcekey="lContactPositionResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_ContactPersonPosition" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lContactFirstName" runat="server" Text="Имя" 
					meta:resourcekey="lContactFirstNameResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_ContactPersonName" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lContactLastName" runat="server" Text="Фамилия" 
					meta:resourcekey="lContactLastNameResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_ContactPersonSurname" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lContactPhone" runat="server" Text="Основной телефон" 
					meta:resourcekey="lContactPhoneResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_ContactPersonPhone" />
			</td>
		</tr>
		<tr>
			<th>
				<nobr><asp:Literal ID="lContactExtPhone" runat="server" 
					Text="Дополнительный телефон" meta:resourcekey="lContactExtPhoneResource1" /></nobr>
			</th>
			<td>
				<asp:Label runat="server" ID="_ContactPersonExtPhone" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lContactFax" runat="server" Text="Факс" 
					meta:resourcekey="lContactFaxResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_ContactPersonFax" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lContactPersonEmail" runat="server" Text="Email" />
			</th>
			<td>
				<asp:Label ID="_ContactPersonEmail" runat="server"></asp:Label>
			</td>
		</tr>
		<%--<asp:PlaceHolder ID="vregRO_Rozn_169" runat="server">
			<tr>
				<th>
					<asp:Literal ID="lRmsStores" runat="server" Text="Пункт получения заказов" 
						meta:resourcekey="lRmsStoresResource1" />
				</th>
				<td>
					<asp:Label runat="server" ID="_RmsStores" 
						meta:resourcekey="_RmsStoresResource1" />
				</td>
			</tr>
			<tr>
				<th>
					<asp:Literal ID="lDiscountCardNumber" runat="server" Text="Дисконтная карта" 
						meta:resourcekey="lDiscountCardNumberResource1" />
				</th>
				<td>
					<asp:Label runat="server" ID="_DiscountCardNumber" 
						meta:resourcekey="_DiscountCardNumberResource1" />
				</td>
			</tr>
		</asp:PlaceHolder>--%>
		<tr>
			<th>
				<asp:Literal ID="lShoppingAddress" runat="server" Text="Адрес доставки" 
					meta:resourcekey="lShoppingAddressResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_DeliveryAddress" />
			</td>
		</tr>
		<!-- ============== Legal information =================== -->
		<tr>
			<th></th>
			<td>
				<span class="blue"><asp:Literal ID="lLegalInfo" runat="server" Text="Юридическая информация" 
						meta:resourcekey="lLegalInfoResource1" /></span>
			</td>
		</tr>
		<tr>
			<th>
				<nobr><asp:Literal ID="lLegalName" runat="server" 
					Text="Наименование организации" meta:resourcekey="lLegalNameResource1" /></nobr>
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
		<!-- ============== Banking information =================== -->
		<tr>
			<th></th>
			<td>
				<span class="blue">
					<asp:Literal ID="lBankInfo" runat="server" Text="Банковские реквизиты" 
						meta:resourcekey="lBankInfoResource1" />
				</span>
			</td>
		</tr>
		<tr>
			<th><asp:Literal ID="lBankName" runat="server" Text="Bank name" /></th>
			<td><asp:Label ID="_BankName" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<th><asp:Literal ID="lIBAN" runat="server" Text="IBAN" /></th>
			<td><asp:Label ID="_IBAN" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<th><asp:Literal ID="lSWIFT" runat="server" Text="SWIFT" /></th>
			<td><asp:Label ID="_SWIFT" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<th><asp:Literal ID="lBankAddress" runat="server" Text="Bank address" /></th>
			<td><asp:Label ID="_BankAddress" runat="server"></asp:Label></td>
		</tr>
		<!-- ============== General manager =================== -->
		<tr>
			<th></th>
			<td>
				<span class="blue">
					<asp:Literal ID="lDirector" runat="server" Text="Руководитель" 
						meta:resourcekey="lDirectorResource1" /></span>
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lDirectorFirstName" runat="server" Text="Имя" 
					meta:resourcekey="lDirectorFirstNameResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_DirectorName" />
			</td>
		</tr>
		<tr>
			<th>
				<asp:Literal ID="lDirectorLastName" runat="server" Text="Фамилия" 
					meta:resourcekey="lDirectorLastNameResource1" />
			</th>
			<td>
				<asp:Label runat="server" ID="_DirectorSurname" />
			</td>
		</tr>
		<!-- ============== Correspondent bank =================== -->
		<tr>
			<th></th>
			<td>
				<span class="blue"><asp:Literal ID="lCorrespondingBank" runat="server" Text="Correspondent bank" /></span>
			</td>
		</tr>
		<tr>
			<th><asp:Literal ID="lCorrespondingBankName" runat="server" Text="Bank name" /></th>
			<td><asp:Label ID="_CorrespondingBankName" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<th><asp:Literal ID="lCorrespondingIBAN" runat="server" Text="IBAN" /></th>
			<td><asp:Label ID="_CorrespondingIBAN" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<th><asp:Literal ID="lCorrespondingSWIFT" runat="server" Text="SWIFT" /></th>
			<td><asp:Label ID="_CorrespondingSWIFT" runat="server"></asp:Label></td>
		</tr>
		<tr>
			<th><asp:Literal ID="lCorrespondingBankAddress" runat="server" Text="Bank address" /></th>
			<td><asp:Label ID="_CorrespondingBankAddress" runat="server"></asp:Label></td>
		</tr>
	</table>
</div>

