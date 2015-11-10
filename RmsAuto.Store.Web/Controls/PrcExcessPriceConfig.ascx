<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrcExcessPriceConfig.ascx.cs"
    Inherits="RmsAuto.Store.Web.Controls.PrcExcessPriceConfig" %>
<%@ Register Src="~/Cms/TextItems/TextItemControl.ascx" TagName="TextItemControl"
    TagPrefix="uc1" %>
<div>
    <asp:PlaceHolder ID="phSaveOK" runat="server" Visible="False">
        <h4>
            <asp:Literal ID="lOKSave" runat="server" Text="Данные успешно сохранены" 
				meta:resourcekey="lOKSaveResource1" /></h4>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phSaveError" runat="server" Visible="False">
        <h3>
            <asp:Literal ID="lErrorSave" runat="server" Text="Данные сохранить не удалось" 
				meta:resourcekey="lErrorSaveResource1" /></h3>
    </asp:PlaceHolder>
    <asp:Literal ID="lPrcExcess" runat="server" 
		Text="Допустимое значение превышения цены (%):" 
		meta:resourcekey="lPrcExcessResource1" /> 
	<asp:TextBox ID="txtPrcExcessPriceConfig" runat="server" MaxLength="2" 
		meta:resourcekey="txtPrcExcessPriceConfigResource1"></asp:TextBox>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
		ErrorMessage="неверный формат" ControlToValidate="txtPrcExcessPriceConfig" 
		Display="Dynamic" ValidationExpression="\d{1,2}" 
		meta:resourcekey="RegularExpressionValidator1Resource1"></asp:RegularExpressionValidator>
    <br />
    <div style="margin:10px 0;">
        <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Texts, Save %>" 
			OnClick="btnSave_Click" CssClass="button"/>
    </div>
    <div>
        <uc1:TextItemControl ID="TextItemControl1" runat="server" TextItemID="UserSetting.PrcExcessPriceConfig"
                            ShowHeader="False" />
    </div>
</div>
