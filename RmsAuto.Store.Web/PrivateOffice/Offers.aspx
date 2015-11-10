<%@ Page Title="" Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Offers.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.Offers" %>
<%@ Register Src="~/PrivateOffice/AuthorizationControl.ascx" TagName="AuthorizationControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Captcha.ascx" TagName="Captcha" TagPrefix="uc2" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl" TagPrefix="uc3" %>


<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc2:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	
	<uc1:AuthorizationControl ID="AuthorizationControl1" runat="server" Role="ClientOrAnonymous" />
	
	<h1><asp:Literal ID="_pageTitleLiteral" runat="server"></asp:Literal></h1>
	
	<asp:Panel ID="_offerPanel" runat="server">
		<uc3:TextItemControl ID="TextItemControl1" runat="server" TextItemID="Offers.Text" />
		
		<asp:Label ID="_errorLabel" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
		
		<table width="100%" class="info" cellpadding="0" cellspacing="0">
			<tr>
				<th>Файл</th>
				<td>
					<asp:FileUpload ID="_fileUploader" runat="server" />
					<img id="Img1" runat="server" src="~/images/help.gif" width="11" height="11" border="0" class="help_img"
						title="Возможна загрузка только *.zip и *.rar файлов, не превышающих 10 мб" alt="" />
					<asp:CustomValidator ID="cvFileUploader" runat="server" ControlToValidate="_fileUploader" Display="Dynamic"
						ErrorMessage="Неверный формат/размер файла" OnServerValidate="ValidateFile" />
				</td>
			</tr>
			<tr>
				<th>Наименование:<font color="red">*</font></th>
				<td>
					<asp:TextBox ID="_txtName" runat="server" MaxLength="100"></asp:TextBox>
					<asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="_txtName" Display="Dynamic"
						ErrorMessage="Поле является обязательным к заполнению" />
				</td>
			</tr>
			<tr>
				<th>Тема:<font color="red">*</font></th>
				<td>
					<asp:TextBox ID="_txtSubject" runat="server" MaxLength="100"></asp:TextBox>
					<asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="_txtSubject" Display="Dynamic"
						ErrorMessage="Поле является обязательным к заполнению" />
				</td>
			</tr>
			<tr>
				<th>Предложение:<font color="red">*</font></th>
				<td>
					<asp:TextBox ID="_txtDescription" runat="server" TextMode="MultiLine" MaxLength="500"
						Columns="50" Rows="10" Width="450px" Height="140px" CssClass="fdb"></asp:TextBox>
					<asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="_txtDescription" Display="Dynamic"
						ErrorMessage="Поле является обязательным к заполнению" />
				</td>
			</tr>
			<tr>
				<th>Код подтверждения:</th>
				<td><uc2:Captcha runat="server" ID="_captcha" /></td>
			</tr>
			<tr>
				<th></th>
				<td><asp:ImageButton ID="_ibSend" runat="server" ImageUrl="~/images/send_btn.gif" 
						onclick="_ibSend_Click" /></td>
			</tr>
		</table>
	</asp:Panel>
	<asp:Panel ID="_sendOkPanel" runat="server" Visible="false">
		Ваше предложение успешно отправлено.
	</asp:Panel>
	
</asp:Content>
