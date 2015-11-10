<%@ Page Language="C#" MasterPageFile="~/Manager/Manager.Master" AutoEventWireup="true" CodeBehind="VinRequestList.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.VinRequestList" Title="Manager working place" %>
<%@ Register src="Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc2" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc2:HandyClientSetBar ID="HandyClientSetBar1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<h1>Запросы по VIN</h1>

	Показать: <asp:DropDownList runat="server" ID="_filterBox" AutoPostBack="true">
		<asp:ListItem Text="Все запросы" Value="0"></asp:ListItem>
		<asp:ListItem Text="Запросы моих клиентов" Value="1" Selected></asp:ListItem>
		<asp:ListItem Text="Запросы клиентов моего магазина" Value="2"></asp:ListItem>
	</asp:DropDownList>
	
	&nbsp;
	Статус: <asp:DropDownList runat="server" ID="_statusBox" AutoPostBack="true" >
		<asp:ListItem Text="Все" Value="0"></asp:ListItem>
		<asp:ListItem Text="Новые" Value="1" Selected></asp:ListItem>
		<asp:ListItem Text="Обработанные" Value="2"></asp:ListItem>
	</asp:DropDownList>

	<asp:Label runat="server" ID="_errorLabel" CssClass="error" EnableViewState="false" />

	<asp:ListView ID="_listView" runat="server" DataSourceID="_vinRequestDataSource" 
		onitemcommand="_listView_ItemCommand">
	<LayoutTemplate>
		<table width="100%" cellpadding="0" cellspacing="0" class="list" style="margin-top:0px;">
		<tr>
			<th>Дата запроса</th>
			<th>Запрос</th>
			<th>Обработан</th>
			<th></th>
		</tr>
		<tr runat="server" id="itemPlaceholder" />
		</table>
		<asp:DataPager ID="_dataPager" runat="server" PageSize="20">
			<Fields>
				<asp:NextPreviousPagerField ShowFirstPageButton="False" 
					ShowNextPageButton="False" ShowPreviousPageButton="False" PreviousPageText="&lt;" />
				<asp:NumericPagerField />
				<asp:NextPreviousPagerField ShowLastPageButton="False" 
					ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText="&gt;" />
			</Fields>
		</asp:DataPager>
	</LayoutTemplate>
	<ItemTemplate>
        <tr runat="server">
            <td><%# Eval("RequestDate","{0:dd.MM.yyyy HH:mm:ss}") %></td>
            <td>
				<b><%#(Container.DataItem as VinRequest).GetFullName()%></b>
            </td>
            <td>
				<%#(bool)Eval("Proceeded")?Eval("AnswerDate","{0:dd.MM.yyyy HH:mm:ss}"):"нет"%>
            </td>
            <td><asp:LinkButton runat="server" ID="_openLink" CommandName="Open" CommandArgument='<%#Eval("Id")%>'>Открыть</asp:LinkButton></td>
        </tr>
	</ItemTemplate>
	<EmptyDataTemplate>
		<br />
		<span class="error">Нет данных</span>
	</EmptyDataTemplate>
	</asp:ListView>
	
	<asp:LinqDataSource ID="_vinRequestDataSource" runat="server" 
		ContextTypeName="RmsAuto.Store.Entities.StoreDataContext" 
		OrderBy="RequestDate desc" TableName="VinRequests" EnableViewState="true">
	</asp:LinqDataSource>
	
</asp:Content>
