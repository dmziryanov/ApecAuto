<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="RmsAuto.Store.Adm.Import" %>
<%@ Register src="Controls/ImportReportView.ascx" tagname="ImportReportView" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <h4>������</h4>
    <asp:Wizard ID="_importWizard" runat="server" 
    DisplaySideBar="false"
    onfinishbuttonclick="_importWizard_FinishButtonClick"
    OnActiveStepChanged="_importWizard_ActiveStepChanged">
        <WizardSteps>
            <asp:WizardStep ID="_selectFormatStep" runat="server" title="������ ����������� ������" StepType="Start">
                �������� ������ ����������� ������<br />
                <asp:DropDownList ID="_ddlCsvFormat" runat="server">
                    <asp:ListItem Value="" Text="�� ������" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="prices" Text="������� �����������"></asp:ListItem>
                    <asp:ListItem Value="crosses" Text="�������������� ������"></asp:ListItem>
                </asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="�� ������ ������ ������" ControlToValidate="_ddlCsvFormat" />
                <br />
            </asp:WizardStep>
            <asp:WizardStep runat="server" title="����� ��������" ID="_selectModeStep" StepType= "Step">
                �������� ������ �������� ������<br />
                <asp:RadioButtonList ID="_rblImportMode" runat="server">
                    <asp:ListItem Value="Bulk" Text="�������������" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Smart" Text="�������/����������"></asp:ListItem>
                </asp:RadioButtonList><br />
            </asp:WizardStep>
            <asp:WizardStep runat="server" ID="_selectFileStep" title="���� ��� ��������" StepType="Finish">
                �������� ���� ��� ��������<br />
                <asp:FileUpload ID="_fileUpload" runat="server" />
                <asp:RequiredFieldValidator ID="_uploadValidator" runat="server" 
                ErrorMessage="�������� ���� ��� �������" ControlToValidate="_fileUpload" />
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="��������" StepType="Complete">
                <h4><% = _ddlCsvFormat.SelectedItem.Text %> - �������� �������</h4>
                <uc1:ImportReportView ID="_importReportView" runat="server" />           
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
