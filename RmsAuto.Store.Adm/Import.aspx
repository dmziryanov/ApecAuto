<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="RmsAuto.Store.Adm.Import" %>
<%@ Register src="Controls/ImportReportView.ascx" tagname="ImportReportView" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <h4>Импорт</h4>
    <asp:Wizard ID="_importWizard" runat="server" 
    DisplaySideBar="false"
    onfinishbuttonclick="_importWizard_FinishButtonClick"
    OnActiveStepChanged="_importWizard_ActiveStepChanged">
        <WizardSteps>
            <asp:WizardStep ID="_selectFormatStep" runat="server" title="Формат загружаемых данных" StepType="Start">
                Выберите формат загружаемых данных<br />
                <asp:DropDownList ID="_ddlCsvFormat" runat="server">
                    <asp:ListItem Value="" Text="не выбран" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="prices" Text="ценовые предложения"></asp:ListItem>
                    <asp:ListItem Value="crosses" Text="дополнительные кроссы"></asp:ListItem>
                </asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="не выбран формат данных" ControlToValidate="_ddlCsvFormat" />
                <br />
            </asp:WizardStep>
            <asp:WizardStep runat="server" title="Режим загрузки" ID="_selectModeStep" StepType= "Step">
                Выберите способ загрузки данных<br />
                <asp:RadioButtonList ID="_rblImportMode" runat="server">
                    <asp:ListItem Value="Bulk" Text="массированный" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Smart" Text="вставка/обновление"></asp:ListItem>
                </asp:RadioButtonList><br />
            </asp:WizardStep>
            <asp:WizardStep runat="server" ID="_selectFileStep" title="Файл для загрузки" StepType="Finish">
                Выберите файл для загрузки<br />
                <asp:FileUpload ID="_fileUpload" runat="server" />
                <asp:RequiredFieldValidator ID="_uploadValidator" runat="server" 
                ErrorMessage="выберите файл для импорта" ControlToValidate="_fileUpload" />
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Протокол" StepType="Complete">
                <h4><% = _ddlCsvFormat.SelectedItem.Text %> - протокол импорта</h4>
                <uc1:ImportReportView ID="_importReportView" runat="server" />           
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
