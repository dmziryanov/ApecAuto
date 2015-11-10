<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="SearchSparePartsLogExport.aspx.cs" Inherits="RmsAuto.Store.Adm.SearchSparePartsLogExport" Title="Выгрузка лога поисковых запросов" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
    <style type="text/css">@import "scripts/datepick/redmond.datepick.css";</style> 
	<script type="text/javascript" src="scripts/datepick/jquery.datepick.min.js"></script>
	<script type="text/javascript" src="scripts/datepick/jquery.datepick-ru.js"></script>
	<script type="text/javascript">
		$(function(){
		$('.date').datepick({firstDay: 1,dateFormat: 'dd.mm.yy'}); 
		});
	</script>

    <h4>Экспорт лога поисковых запросов</h4>
    
    Период выгрузки: с <asp:TextBox runat="server" ID="_date1Box" CssClass="date" Columns="10" /> по <asp:TextBox runat="server" ID="_date2Box" CssClass="date" Columns="10" />
    
    <a href="" target="_blank" onclick="this.href='SearchSparePartsLog.ashx?date1='+$('input[name$=_date1Box]').val()+'&date2='+$('input[name$=_date2Box]').val()">Выгрузить</a>
</asp:Content>
