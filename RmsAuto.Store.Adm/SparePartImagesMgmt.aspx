<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="SparePartImagesMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.SparePartImagesMgmt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	<h4>Фотографии деталей</h4>
	
	<asp:ValidationSummary ID="vSummary" runat="server" ShowMessageBox="true" HeaderText="Ошибки заполнения:" ShowSummary="false" />
	<table>
		<tr>
			<td>Артикул:</td>
			<td><asp:TextBox ID="_txtPartNumber" runat="server"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator ID="rfvPartNumber" runat="server" Display="Dynamic" ControlToValidate="_txtPartNumber"
					ErrorMessage="Поле 'Артикул' не может быть пустым">*</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>Производитель:</td>
			<td><asp:TextBox ID="_txtManufacturer" runat="server"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator ID="rfvManufacturer" runat="server" Display="Dynamic" ControlToValidate="_txtManufacturer"
					ErrorMessage="Поле 'Производитель' не может быть пустым">*</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>Код поставщика:</td>
			<td>
				<asp:DropDownList ID="_ddlSupplierID" runat="server">
					<asp:ListItem Value="1212" Text="РМС Авто склад скидка -20%"></asp:ListItem>
					<asp:ListItem Value="1215" Text="РМС Авто склад скидка -50%"></asp:ListItem>
				</asp:DropDownList>
			</td>
			<td></td>
		</tr>
	</table>
	<br />
	<table>
		<tr>
			<th>Файл</th>
			<th>№</th>
			<th>Описание</th>
			<th></th>
		</tr>
		<tr>
			<td><asp:FileUpload ID="_fuImage1" runat="server" /></td>
			<td><asp:TextBox ID="_txtImageNumber1" runat="server" Width="20" Text="1"></asp:TextBox></td>
			<td><asp:TextBox ID="_txtDescription1" runat="server" Width="250" MaxLength="500"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator ID="rfvImage1" runat="server" Display="Dynamic" ControlToValidate="_fuImage1"
					ErrorMessage="1-ый uploader должен быть заполнен">*</asp:RequiredFieldValidator>
				<asp:CustomValidator ID="cvImage1" runat="server" Display="Dynamic" ControlToValidate="_fuImage1" OnServerValidate="Uploaders_ServerValidate"
					ErrorMessage="">*</asp:CustomValidator>
			</td>
		</tr>
		<tr>
			<td><asp:FileUpload ID="_fuImage2" runat="server" /></td>
			<td><asp:TextBox ID="_txtImageNumber2" runat="server" Width="20" Text="2"></asp:TextBox></td>
			<td><asp:TextBox ID="_txtDescription2" runat="server" Width="250" MaxLength="500"></asp:TextBox></td>
			<td>
				<asp:CustomValidator ID="cvImage2" runat="server" Display="Dynamic" ControlToValidate="_fuImage2" OnServerValidate="Uploaders_ServerValidate"
					ErrorMessage="">*</asp:CustomValidator>
			</td>
		</tr>
		<tr>
			<td><asp:FileUpload ID="_fuImage3" runat="server" /></td>
			<td><asp:TextBox ID="_txtImageNumber3" runat="server" Width="20" Text="3"></asp:TextBox></td>
			<td><asp:TextBox ID="_txtDescription3" runat="server" Width="250" MaxLength="500"></asp:TextBox></td>
			<td>
				<asp:CustomValidator ID="cvImage3" runat="server" Display="Dynamic" ControlToValidate="_fuImage3" OnServerValidate="Uploaders_ServerValidate"
					ErrorMessage="">*</asp:CustomValidator>
			</td>
		</tr>
		<tr>
			<td><asp:FileUpload ID="_fuImage4" runat="server" /></td>
			<td><asp:TextBox ID="_txtImageNumber4" runat="server" Width="20" Text="4"></asp:TextBox></td>
			<td><asp:TextBox ID="_txtDescription4" runat="server" Width="250" MaxLength="500"></asp:TextBox></td>
			<td>
				<asp:CustomValidator ID="cvImage4" runat="server" Display="Dynamic" ControlToValidate="_fuImage4" OnServerValidate="Uploaders_ServerValidate"
					ErrorMessage="">*</asp:CustomValidator>
			</td>
		</tr>
		<tr>
			<td><asp:FileUpload ID="_fuImage5" runat="server" /></td>
			<td><asp:TextBox ID="_txtImageNumber5" runat="server" Width="20" Text="5"></asp:TextBox></td>
			<td><asp:TextBox ID="_txtDescription5" runat="server" Width="250" MaxLength="500"></asp:TextBox></td>
			<td>
				<asp:CustomValidator ID="cvImage5" runat="server" Display="Dynamic" ControlToValidate="_fuImage5" OnServerValidate="Uploaders_ServerValidate"
					ErrorMessage="">*</asp:CustomValidator>
			</td>
		</tr>
		<tr>
			<td colspan="4"><asp:Button ID="_btnSave" runat="server" Text="Сохранить" OnClick="_btnSave_Click" /></td>
		</tr>
		<tr>
			<td colspan="4"><asp:Label ID="_lblInfo" runat="server"></asp:Label></td>
		</tr>
	</table>
</asp:Content>
