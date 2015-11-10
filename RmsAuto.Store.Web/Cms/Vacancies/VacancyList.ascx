<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VacancyList.ascx.cs" Inherits="RmsAuto.Store.Web.Cms.Vacancies.VacancyList" %>

<asp:ListView ID="_listView" runat="server">
<LayoutTemplate>
	<table cellspacing="0" cellpadding="0" border="0" width="100%" class="list">
    <tr>
        <th><asp:Literal ID="lVacancy" runat="server" Text="Вакансия" 
				meta:resourcekey="lVacancyResource1" /></th>
        <th><asp:Literal ID="lWorkPlace" runat="server" Text="Место работы" 
				meta:resourcekey="lWorkPlaceResource1" /></th>
        <th><asp:Literal ID="lSex" runat="server" Text="Пол" 
				meta:resourcekey="lSexResource1" /></th>
        <th><asp:Literal ID="lAge" runat="server" Text="Возраст" 
				meta:resourcekey="lAgeResource1" /></th>
        <th><asp:Literal ID="lEducat" runat="server" Text="Образование" 
				meta:resourcekey="lEducatResource1" /></th>
        <th><asp:Literal ID="lExper" runat="server" Text="Опыт работы" 
				meta:resourcekey="lExperResource1" /></th>
        <th class="last"><asp:Literal ID="lIncomeLevel" runat="server" 
				Text="Заработная плата" meta:resourcekey="lIncomeLevelResource1" /></th>
    </tr>
    <tr>
    	<td colspan="7" class="empty" style=""><img runat="server" src="~/images/1pix.gif" width="1" height="3" border="0" /> </td>
    </tr>
    <tr runat="server" id="itemPlaceholder" >
    </tr>
    </table>
</LayoutTemplate>
<ItemTemplate>
	<tr id="Li1" runat="server">
		<td runat="server"><asp:HyperLink runat="server" ID="HyperLink2" 
				NavigateUrl='<%# UrlManager.GetVacancyDetailsUrl((int)Eval("VacancyID")) %>'><%#Server.HtmlEncode( (string)Eval( "VacancyName" ) )%></asp:HyperLink></td>
		<td runat="server"><asp:HyperLink runat="server" ID="HyperLink1" 
				NavigateUrl='<%# Eval("Shop")!=null?UrlManager.GetShopDetailsUrl(Convert.ToInt32(Eval("ShopID"))):"" %>'><%# Eval("Shop")!=null ? Server.HtmlEncode( (string)Eval("Shop.ShopName")) : "" %></asp:HyperLink></td>
		<td runat="server"><center>
			<%#(RmsAuto.Store.Cms.Entities.Gender)Eval("VacancyGender") != RmsAuto.Store.Cms.Entities.Gender.Female ? "<img src=\""+ResolveUrl("~/images/vac_m.gif")+"\" alt=\"Мужчина\" width=9 height=16 border=0>" : "" %>
			<%#(RmsAuto.Store.Cms.Entities.Gender)Eval( "VacancyGender" ) != RmsAuto.Store.Cms.Entities.Gender.Male ? "<img src=\""+ResolveUrl("~/images/vac_w.gif")+"\" alt=\"Женщина\" width=9 height=16 border=0>" : ""%>
		</center></td>
		<td runat="server"><nobr><%#Eval( "VacancyAgeFrom", "от {0}" ) %> 
		         <%# (int?)Eval( "VacancyAgeTo")!=null ? Eval( "VacancyAgeTo", " до {0}" ) : "" %> 
		    </nobr>
		</td>
		<td runat="server"><%#Server.HtmlEncode( (string)Eval( "VacancyEducation" ) )%></td>
		<td runat="server"><%#Eval( "VacancyExperience" )%></td>
		<td runat="server"><%#Server.HtmlEncode( (string)Eval( "VacancyIncomeLevel" ) )%></td>
	</tr>
</ItemTemplate>
<EmptyDataTemplate>
	<asp:Literal ID="lEmptyList" runat="server" Text="Список вакансий пуст" 
		meta:resourcekey="lEmptyListResource1" />
</EmptyDataTemplate>
</asp:ListView>


