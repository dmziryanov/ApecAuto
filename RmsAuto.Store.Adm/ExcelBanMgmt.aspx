<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ExcelBanMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.ExcelBanMgmt" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>
<script runat="server">
	object TryEval(string fieldName)
	{
		try
		{
			return Eval(fieldName);
		}
		catch( Exception e )
		{
			return null;
		}
	}
</script>

<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <h4>Снятие БАНов на загрузку заказов EXCEL файлом</h4>
    <asp:Panel ID="_errorPanel" runat="server">
		<asp:Label id="_errorMessageLabel" runat="server" />
		<br />
		<br />
	</asp:Panel>    
    <table class="detailstable">
        <tr>
            <td>Поиск по <b>Username:</b></td>
            <td><asp:TextBox ID="_txtLogin" runat="server" MaxLength="20"></asp:TextBox></td>
            <td> + <b>Clientname:</b></td>
            <td><asp:TextBox ID="_txtLogin_2" runat="server" MaxLength="20"></asp:TextBox></td>

            <td><asp:Button ID="_btnApplyFilter" runat="server" Text="Применить" onclick="_btnApplyFilter_Click" /></td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="_loginsGridView" runat="server" AllowPaging="true" PageSize="10"
    DataSourceID="linqDataSource" CssClass="gridview" AlternatingRowStyle-CssClass="even"
    AutoGenerateColumns="false" DataKeyNames="UserID" OnRowCommand="LoginsGridView_OnRowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Username"><ItemTemplate><%# Eval("User.Username")%></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Clientname"><ItemTemplate><%# Eval("User.Clientname")%></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Email"><ItemTemplate><%# Eval("User.Email")%></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Ban" Visible="false"><ItemTemplate><%# Eval("Ban")%></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="Count" Visible="false"><ItemTemplate><%# Eval("Count")%></ItemTemplate></asp:TemplateField>
           
            <asp:TemplateField HeaderText="Снять бан" SortExpression="HIDE">
            <ItemTemplate><asp:CheckBox runat="server" Checked="false"/></ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField Visible="false">
            <ItemTemplate><asp:Label runat="server" Text=<%# Eval("UserID")%>/></ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    
     <table class="detailstable" style="float:right">
        <tr>
            <td><asp:Button runat="server" ID="resetBunsBtn" Text="Сохранить изменения" onclick="resetBuns_Click"/></td>
        </tr>
    </table>
   
    <asp:LinqDataSource ID="linqDataSource" runat="server"
    ContextTypeName="RmsAuto.Store.Entities.StoreDataContext" TableName="BanClientActions"
    AutoPage="true" EnableInsert="false" EnableUpdate="false" EnableDelete="true"
    Where="Count > 0 && Ban.Contains(@Banp) && ((@Usr == string.Empty && @Client == string.Empty) || (@Usr != string.Empty && User.Username.Contains(@Usr)) || (@Client != string.Empty && User.Clientname.Contains(@Client)))"
    OrderBy="UserID ascending">
        <WhereParameters>
            <asp:ControlParameter Name="Usr" Type = "String" ControlId = "_txtLogin"  DefaultValue = "" ConvertEmptyStringToNull = "false" />
            <asp:ControlParameter Name="Client" Type = "String" ControlId = "_txtLogin_2"  DefaultValue = "" ConvertEmptyStringToNull = "false" />
            <asp:Parameter Name="Banp" Type = "String" DefaultValue = "Files" />
        </WhereParameters>
    </asp:LinqDataSource>
</asp:Content>


