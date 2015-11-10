<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopEmployeeList.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Shops.ShopEmployeeList" %>

<asp:GridView ID="_gridView" runat="server" DataSourceID="_linqDataSource"
	AutoGenerateColumns="False" DataKeyNames="EmpID" CssClass="e_list" 
	meta:resourcekey="_gridViewResource1">
<AlternatingRowStyle CssClass="tr" />
<Columns>
	<asp:BoundField HeaderText="Имя" DataField="EmpName" 
		meta:resourcekey="BoundFieldResource1" />
	<asp:BoundField HeaderText="Должность" DataField="EmpPosition" 
		meta:resourcekey="BoundFieldResource2" />
	<asp:TemplateField headertext="Email" meta:resourcekey="TemplateFieldResource1"><itemtemplate>
		<a href='mailto:<%# Eval("EmpEmail") %>'><%#Eval("EmpEmail")%></a></itemtemplate></asp:TemplateField>
	<asp:BoundField HeaderText="ICQ" DataField="EmpICQ" 
		meta:resourcekey="BoundFieldResource3" />
	<asp:TemplateField headertext="Телефон" 
		meta:resourcekey="TemplateFieldResource2"><itemtemplate><%#Eval("EmpPhone")%></itemtemplate></asp:TemplateField>
</Columns>
</asp:GridView>

<asp:LinqDataSource ID="_linqDataSource" runat="server"
	ContextTypeName="RmsAuto.Store.Cms.Entities.CmsDataContext" 
    TableName="Employees" 
    Where="EmpVisible && Shop.ShopID=@ShopID"
    OrderBy="EmpPriority, EmpName" >
</asp:LinqDataSource>
