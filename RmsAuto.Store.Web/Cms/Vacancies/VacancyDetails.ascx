<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VacancyDetails.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Vacancies.VacancyDetails" %>
<h1><asp:Literal ID="lHeader" runat="server" Text="Вакансии" 
		meta:resourcekey="lHeaderResource1" /></h1>
<asp:Repeater ID="_repeater" runat="server">
<ItemTemplate>
    <h2><%#Server.HtmlEncode( (string)Eval( "VacancyName" ) )%></h2>
	<table cellspacing="0" cellpadding="0" border="0" width="100%" class="info">
	<tr>
		<th><asp:Literal ID="lWorkPlace" runat="server" Text="Место работы" 
				meta:resourcekey="lWorkPlaceResource1" /></th>
		<td><asp:HyperLink runat="server" ID="HyperLink2" 
				NavigateUrl='<%# Eval("Shop")!=null?UrlManager.GetShopDetailsUrl(Convert.ToInt32(Eval("ShopID"))):"" %>' 
				meta:resourcekey="HyperLink2Resource1"><%# Eval("Shop")!=null ? Server.HtmlEncode( (string)Eval("Shop.ShopName")) : "" %></asp:HyperLink></td>
	</tr>
	<tr>
		<th><asp:Literal ID="lSex" runat="server" Text="Пол" 
				meta:resourcekey="lSexResource1" /></th>
		<td>
			<%#(RmsAuto.Store.Cms.Entities.Gender)Eval("VacancyGender") != RmsAuto.Store.Cms.Entities.Gender.Female ? "<img src=\""+ResolveUrl("~/images/vac_m.gif")+"\" alt=\"Мужчина\" width=9 height=16 border=0>" : "" %>
			<%#(RmsAuto.Store.Cms.Entities.Gender)Eval( "VacancyGender" ) != RmsAuto.Store.Cms.Entities.Gender.Male ? "<img src=\""+ResolveUrl("~/images/vac_w.gif")+"\" alt=\"Женщина\" width=9 height=16 border=0>" : ""%>
		</td>
	</tr>
	<tr>
		<th><asp:Literal ID="lAge" runat="server" Text="Возраст" 
				meta:resourcekey="lAgeResource1" /></th>
		<td><%#Eval( "VacancyAgeFrom", "от {0}" ) %> 
		         <%# (int?)Eval( "VacancyAgeTo")!=null ? Eval( "VacancyAgeTo", " до {0}" ) : "" %> 
		</td>
	</tr>
	<tr>
		<th><asp:Literal ID="lEducation" runat="server" Text="Образование" 
				meta:resourcekey="lEducationResource1" /></th>
		<td><%#Server.HtmlEncode( (string)Eval( "VacancyEducation" ) )%></td>
	</tr>
	<tr>
		<th><asp:Literal ID="lExper" runat="server" Text="Опыт работы" 
				meta:resourcekey="lExperResource1" /></th>
		<td><%#Eval( "VacancyExperience" )%></td>
	</tr>
	<tr>
		<th class="last"><asp:Literal ID="lRequirement" runat="server" Text="Требования" 
				meta:resourcekey="lRequirementResource1" /></th>
		<td><%#Eval( "VacancyRequirement" )%></td>
	</tr>
	<tr>
		<th class="last"><asp:Literal ID="lIncomeLevel" runat="server" 
				Text="Заработная плата" meta:resourcekey="lIncomeLevelResource1" /></th>
		<td><%#Server.HtmlEncode( (string)Eval( "VacancyIncomeLevel" ) )%>&nbsp;</td>
	</tr>
	<tr>
		<th class="last"><asp:Literal ID="lFunction" runat="server" Text="Обязанности" 
				meta:resourcekey="lFunctionResource1" /></th>
		<td><%#Eval("VacancyFunctions")%></td>
	</tr>
	<tr>
		<th class="last"><asp:Literal ID="lNote" runat="server" Text="Дополнительно" 
				meta:resourcekey="lNoteResource1" /></th>
		<td><%#Eval("VacancyNote")%></td>
	</tr>
	<tr>
		<th class="last"><asp:Literal ID="lContacts" runat="server" Text="Контакты" 
				meta:resourcekey="lContactsResource1" /></th>
		<td><%#Eval("VacancyContacts")%></td>
	</tr>
	</table>
</ItemTemplate>
</asp:Repeater>
<br />
<br />
	<span class="link_block">
<asp:HyperLink runat="server" ID="_allVacancyLink" 
	NavigateUrl="~/About/Vacancies.aspx" 
	meta:resourcekey="_allVacancyLinkResource1"><asp:Literal 
	ID="lReturnToListVacancy" runat="server" Text="Вернуться к списку вакансий" 
	meta:resourcekey="lReturnToListVacancyResource1" />
</asp:HyperLink></span>