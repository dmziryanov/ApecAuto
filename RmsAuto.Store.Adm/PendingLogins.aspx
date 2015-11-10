<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="PendingLogins.aspx.cs" Inherits="RmsAuto.Store.Adm.PendingLogins" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<script runat="server">
	object TryEval( string fieldName )
	{
		try
		{
			return Eval( fieldName );
		}
		catch( Exception ex )
		{
			return null;
		}
	}
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <h4>Регистрационные данные пользователей сайта, ожидающие активации</h4>
    <asp:Panel ID="_errorPanel" runat="server">
		<asp:Label id="_errorMessageLabel" runat="server" />
		<br />
		<br />
	</asp:Panel>    
    <table class="detailstable">
        <tr>
            <td>Показать логины начинающиеся с</td>
            <td><asp:TextBox ID="_txtLogin" runat="server" MaxLength="20"></asp:TextBox></td>
            <td><asp:Button ID="_btnApplyFilter" runat="server" Text="Применить" 
                    onclick="_btnApplyFilter_Click" /></td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="_loginsGridView" runat="server" AllowPaging="true" PageSize="10"
    DataSourceID="_loginsDataSource" CssClass="gridview" AlternatingRowStyle-CssClass="even"
    AutoGenerateColumns="false" DataKeyNames="EntryUid" OnRowCommand="LoginsGridView_OnRowCommand">
        <Columns>
            <asp:TemplateField ItemStyle-Width="20">
                        <ItemTemplate>
					        <nobr>
					        <asp:LinkButton ID="SendActivationEmailLinkButton" runat="server" 
					        CommandName="SendActivationEmail" CommandArgument='<%# Eval("EntryUid") %>'
                                CausesValidation="false" ToolTip="Отправить активационную ссылку" 
                                OnClientClick='return confirm("Письмо с активационной будет отправлено. Вы уверены?");'
                            ><asp:Image runat="server" ID="Image1" ImageUrl="~/DynamicData/Content/Images/mail.gif" /></asp:LinkButton>
                            <asp:LinkButton ID="FlashActivationLinkButton" runat="server"
                            CommandName="FlashActivation" CommandArgument='<%# Eval("EntryUid") %>'
                            CausesValidation="false" ToolTip="Мгновенная активация"
                            OnClientClick='return confirm("Аккаунт будет мгновенно активирован. Вы уверены?");'>
                                <asp:Image runat="server" ID="Image3" ImageUrl="~/DynamicData/Content/Images/achtung.png" />
                            </asp:LinkButton>
                            <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete"
                                CausesValidation="false" ToolTip="Удалить" 
                                OnClientClick='return confirm("Запись будет удалена. Вы уверены?");'
                            ><asp:Image runat="server" ID="Image2" ImageUrl="~/DynamicData/Content/Images/cross.png" /></asp:LinkButton>
         			        </nobr>
                        </ItemTemplate>
                    </asp:TemplateField>
            <asp:TemplateField HeaderText="Дата/Время">
                <ItemTemplate><%# Eval("EntryTime", "{0:dd.MM.yyyy HH.mm.ss}") %></ItemTemplate>    
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Логин">
                <ItemTemplate><%# Eval("Username") %></ItemTemplate>    
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Клиент">
                <ItemTemplate><%# TryEval( "RegistrationData" ) != null ? TryEval( "RegistrationData.ClientName" ) : TryEval( "RegistrationDataExt.ClientName" )%></ItemTemplate>    
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Категория">
                <ItemTemplate><%# TryEval( "RegistrationData" ) != null ? ( (Enum)Eval( "RegistrationData.ClientCategory" ) ).ToTextOrName() : TryEval( "RegistrationDataExt" ) != null ? ( (Enum)Eval( "RegistrationDataExt.ClientCategory" ) ).ToTextOrName() : ""%></ItemTemplate>    
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Тип">
                <ItemTemplate><%#  TryEval( "RegistrationData" ) != null ? ( (Enum)Eval( "RegistrationData.TradingVolume" ) ).ToTextOrName() : TryEval( "RegistrationDataExt" ) != null ? ( (Enum)Eval( "RegistrationDataExt.TradingVolume" ) ).ToTextOrName() : ""%></ItemTemplate>    
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Телефон">
                <ItemTemplate><%# TryEval( "RegistrationData" ) != null ? TryEval( "RegistrationData.MainPhone" ) : TryEval( "RegistrationDataExt.ContactPhone" )%></ItemTemplate>    
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email">
                <ItemTemplate><%# TryEval( "RegistrationData" ) != null ? TryEval( "RegistrationData.Email" ) : TryEval( "RegistrationDataExt.Email" )%></ItemTemplate>    
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Адрес доставки">
                <ItemTemplate><%# TryEval( "RegistrationData" ) != null ? TryEval( "RegistrationData.ShippingAddress" ) : TryEval( "RegistrationDataExt.ShippingAddress" )%></ItemTemplate>    
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:LinqDataSource ID="_loginsDataSource" runat="server"
    ContextTypeName="RmsAuto.Store.Entities.StoreDataContext" TableName="UserMaintEntries"
    AutoPage="true" EnableInsert="false" EnableUpdate="false" EnableDelete="true"
    Where="Username != null && ( Username.StartsWith(@Login) || @Login == string.Empty)"
    OrderBy="EntryTime descending"
    >
        <WhereParameters>
            <asp:ControlParameter Name="Login" Type = "String" ControlId = "_txtLogin"  DefaultValue = "" ConvertEmptyStringToNull = "false" />     
        </WhereParameters>
    </asp:LinqDataSource>
</asp:Content>

