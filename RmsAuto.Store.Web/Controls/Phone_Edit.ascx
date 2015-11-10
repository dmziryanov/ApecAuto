<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Phone_Edit.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.Phone_Edit" %>
<%--<asp:TextBox ID="_txtCode" runat="server" Columns="5" MaxLength="5" 
    ToolTip="только цифры без скобок и пробелов, например 495" Width="45px" 
	meta:resourcekey="_txtCodeResource1" />--%>
<asp:TextBox ID="_txtNumber" 
	runat="server" Columns="25" 
    ToolTip="только цифры без дефисов и пробелов, например 1234567"  
	CssClass="phone_edit" meta:resourcekey="_txtNumberResource1" Width="165px">
</asp:TextBox>
<span class="info" title="<%$ Resources:Hints, phoneHint %>" runat="server"></span>
<asp:RequiredFieldValidator ID="_numberReqValidator" runat="server" ControlToValidate="_txtNumber"
Display="Dynamic" ErrorMessage="не задан номер" meta:resourcekey="_numberReqValidatorResource1"></asp:RequiredFieldValidator>
<%--<asp:RegularExpressionValidator ID="_codeRegexValidator" Display="Dynamic" 
	runat="server" ControlToValidate="_txtCode"
ErrorMessage="код должен содержать только цифры (от 3 до 5)" 
	ValidationExpression="\d{3,5}" meta:resourcekey="_codeRegexValidatorResource1"></asp:RegularExpressionValidator>  --%>  
<asp:RegularExpressionValidator ID="_numberRegexValidator" Display="Dynamic" runat="server" ControlToValidate="_txtNumber"
	ErrorMessage="номер должен содержать только цифры (от 5 до 12)" 
	ValidationExpression="^[0-9\-\+ ()]{0,20}$" 
	meta:resourcekey="_numberRegexValidatorResource1">
</asp:RegularExpressionValidator>
<%--<asp:CustomValidator ID="_cvPhoneCount" runat="server" Display="Dynamic" 
	ErrorMessage=" полный телефонный номер должен состоять из 11 цифр" 
	OnServerValidate="_cvPhoneCount_ServerValidate" 
	meta:resourcekey="_cvPhoneCountResource1"></asp:CustomValidator>--%>
<%--<script type="text/javascript">
    $(function () {
        $("#<%=_txtNumber.ClientID%>").mask('<%=Mask%>', { autoclear: false });
    });
</script>--%>