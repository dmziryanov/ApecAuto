<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientProfileView.ascx.cs"
	Inherits="RmsAuto.Store.Web.Manager.Controls.ClientProfileView" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<div class="information">
<table>
    <tr>
        <th>
            Entitlement
        </th>
        <td>
            <asp:Label runat="server" ID="_ClientName" />
        </td>
    </tr>
     
    <tr>
        <th>
            E-mail
        </th>
        <td>
            <asp:Label runat="server" ID="_Email" />
        </td>
    </tr>
    <tr>
        <th>
            Purchase amount
        </th>
        <td>
            <asp:Label runat="server" ID="_TradingVolume" />
        </td>
    </tr>
    <tr>
        <th>
            Legal organizational form
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
            Region
        </th>
        <td>
            <asp:Label runat="server" ID="_Region" />
        </td>
    </tr>
    <tr>
        <th>
            Population centre (district)
        </th>
        <td>
            <asp:Label runat="server" ID="_Locality" />
        </td>
    </tr>
    <tr>
        <th>
            Your business area
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
        <th>
        </th>
        <td>
            <h5>
                Contact information</h5>
        </td>
    </tr>
    <asp:PlaceHolder ID="vreg_LegalIP_93" runat="server">
        <tr>
            <th>
                Post of executive
            </th>
            <td>
                <asp:Label runat="server" ID="_ContactPosition" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th>
            Family name
        </th>
        <td>
            <asp:Label runat="server" ID="_ContactLastName" />
        </td>
    </tr>
    <tr>
        <th>
            Name
        </th>
        <td>
            <asp:Label runat="server" ID="_ContactFirstName" />
        </td>
    </tr>
    <tr>
        <th>
            Surname
        </th>
        <td>
            <asp:Label runat="server" ID="_ContactMiddleName" />
        </td>
    </tr>
    <tr>
        <th>
            Main phone
        </th>
        <td>
            <asp:Label runat="server" ID="_ContactPhone" />
        </td>
    </tr>
    <tr>
        <th>
            <nobr>Additional phone</nobr>
        </th>
        <td>
            <asp:Label runat="server" ID="_ContactExtPhone" />
        </td>
    </tr>
    <asp:PlaceHolder ID="vregRO_Opt_143" runat="server">
        <tr>
            <th>
                Fax
            </th>
            <td>
                <asp:Label runat="server" ID="_ContactFax" />
            </td>
        </tr>
        <tr>
            <th>
                Office's schedule
            </th>
            <td>
                <asp:Label runat="server" ID="_ScheduleOfice" />
            </td>
        </tr>
        <tr>
            <th>
                Stock's schedule
            </th>
            <td>
                <asp:Label runat="server" ID="_ScheduleStock" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="vregRO_Rozn_169" runat="server">
        <tr>
            <th>
                Point of orders delivery
            </th>
            <td>
                <asp:Label runat="server" ID="_RmsStores" />
            </td>
        </tr>
        <tr>
            <th>
                Discount card number
            </th>
            <td>
                <asp:Label runat="server" ID="_DiscountCardNumber" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th>
            Shipping address
        </th>
        <td>
            <asp:Label runat="server" ID="_ShippingAddress" />
        </td>
    </tr>
    <asp:PlaceHolder ID="vreg_LegalIP_195" runat="server">
        <tr>
            <th>
            </th>
            <td>
                <h5>
                    legal information</h5>
            </td>
        </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="vreg_Legal_205" runat="server">
        <tr>
            <th>
                <nobr>Name of organization</nobr>
            </th>
            <td>
                <asp:Label runat="server" ID="_LegalName" />
            </td>
        </tr>
        <tr>
            <th>
                ИНН
            </th>
            <td>
                <asp:Label runat="server" ID="_INN_Legal" />
            </td>
        </tr>
        <tr>
            <th>
                КПП
            </th>
            <td>
                <asp:Label runat="server" ID="_KPP" />
            </td>
        </tr>
        <tr>
            <th>
                ОГРН
            </th>
            <td>
                <asp:Label runat="server" ID="_OGRN" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="vreg_IP_239" runat="server">
        <tr>
            <th>
                ИНН
            </th>
            <td>
                <asp:Label runat="server" ID="_INN_IP" />
            </td>
        </tr>
        <tr>
            <th>
                ОГРНИП
            </th>
            <td>
                <asp:Label runat="server" ID="_OGRNIP" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="vreg_LegalIP_257" runat="server">
        <tr>
            <th>
                Налогооблажение
            </th>
            <td>
                <asp:Label runat="server" ID="_NDSAggent" />
            </td>
        </tr>
        <tr>
            <th>
                Юридический адрес
            </th>
            <td>
                <asp:Label runat="server" ID="_OficialAddress" />
            </td>
        </tr>
        <tr>
            <th>
                Фактический адрес
            </th>
            <td>
                <asp:Label runat="server" ID="_RealAddress" />
            </td>
        </tr>
        <tr>
            <th>
            </th>
            <td>
                <h5>
                    Банковские реквизиты
                </h5>
            </td>
        </tr>
        <tr>
            <th>
                Расчетный счет
            </th>
            <td>
                <asp:Label runat="server" ID="_Account" />
            </td>
        </tr>
        <tr>
            <th>
                БИК банка
            </th>
            <td>
                <asp:Label runat="server" ID="_BankBIC" />
            </td>
        </tr>
        <tr>
            <th>
                Корр. счет банка
            </th>
            <td>
                <asp:Label runat="server" ID="_BankKS" />
            </td>
        </tr>
        <tr>
            <th>
                Наименование банка
            </th>
            <td>
                <asp:Label runat="server" ID="_BankName" />
            </td>
        </tr>
        <tr>
            <th>
            </th>
            <td>
                <h5>
                    Руководитель</h5>
            </td>
        </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="vreg_Legal_332" runat="server">
        <tr>
            <th>
                Должность руководителя
            </th>
            <td>
                <asp:Label runat="server" ID="_DirectorPosition" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="vreg_LegalIP_342" runat="server">
        <tr>
            <th>
                Фамилия
            </th>
            <td>
                <asp:Label runat="server" ID="_DirectorLastName" />
            </td>
        </tr>
        <tr>
            <th>
                Имя
            </th>
            <td>
                <asp:Label runat="server" ID="_DirectorFirstName" />
            </td>
        </tr>
        <tr>
            <th>
                Отчество
            </th>
            <td>
                <asp:Label runat="server" ID="_DirectorMiddleName" />
            </td>
        </tr>
        <tr>
            <th>
            </th>
            <td>
                <h5>
                    Контактное лицо для сверки взаиморасчетов</h5>
            </td>
        </tr>
        <tr>
            <th>
                Должность
            </th>
            <td>
                <asp:Label runat="server" ID="_BalanceManPosition" />
            </td>
        </tr>
        <tr>
            <th>
                Фамилия
            </th>
            <td>
                <asp:Label runat="server" ID="_BalanceManLastName" />
            </td>
        </tr>
        <tr>
            <th>
                Имя
            </th>
            <td>
                <asp:Label runat="server" ID="_BalanceManFirstName" />
            </td>
        </tr>
        <tr>
            <th>
                Отчество
            </th>
            <td>
                <asp:Label runat="server" ID="_BalanceManMiddleName" />
            </td>
        </tr>
        <tr>
            <th>
                Телефон
            </th>
            <td>
                <asp:Label runat="server" ID="_BalanceManPhone" />
            </td>
        </tr>
        <tr>
            <th>
                Электронный адрес
            </th>
            <td>
                <asp:Label runat="server" ID="_BalanceManEmail" />
            </td>
        </tr>
    </asp:PlaceHolder>
	
	
	
	<tr>
		<th>
			Персональный менеджер
		</th>
		<td>
			<asp:Literal runat="server" ID="_managerLabel" />
			<%--(<asp:Literal runat="server" ID="_managerDepartmentLabel" />)--%>
		</td>
	</tr>
	<tr runat="server" visible="true">
		<th>
			Прайс–лист №
		</th>
		<td>
			<asp:Literal runat="server" ID="_clientGroupName" />
		</td>
	</tr>
	<tr>
		<th>
			Процент предоплаты
		</th>
		<td>
			<asp:Literal runat="server" ID="_prepaymentPercentLabel" />
		</td>
	</tr>
	<tr>
		<th>
			Данные проверены
		</th>
		<td>
			<asp:Label runat="server" ID="_checkedTrueLabel">Да</asp:Label>
			<asp:Label runat="server" ID="_checkedFalseLabel">Нет</asp:Label>
		</td>
	</tr>
	<tr>
		<th>
		    Эккаунт ограничен
		</th>
		<td>
			<asp:Label runat="server" ID="_restrictedTrueLabel">Да</asp:Label>
			<asp:Label runat="server" ID="_restrictedFalseLabel">Нет</asp:Label>
		</td>
	</tr>
</table>
</div>
