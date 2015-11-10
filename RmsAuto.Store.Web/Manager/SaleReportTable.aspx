<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="SaleReportTable.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.SaleReportTable" %>
<%@ Register src="~/Manager/Controls/SaleReportTable.ascx" tagname="SaleReportTable" tagprefix="uc3" %>
<%@ Register src="~/Manager/Controls/ClientSearchList.ascx" tagname="ClientSearchList" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">

<script type="text/javascript">
    $(function() {
        $('.date').datepicker();
    });

    //Задекорили строчку с итого
    $(function() {
        $("table .list tr td:contains('Total:')").parent().children().addClass("VipTextStyle");
    });

    function DoBootStrap() {
        $('.ui-widget-header').hide();
        $(this).dialog("close");
        $('#AjaxLoadDialogDialog').dialog('open'); //открываем модальное окно
        var tmpStr = '<%=ResolveUrl("~/OnlineCatalogs/Battery.aspx") %>';
        tmpStr = tmpStr + '?mfr=' + escape($('select[id$="companies"]').val());
        tmpStr = tmpStr + '&Capacity=' + escape($('#Capacity').val());
        tmpStr = tmpStr + '&Polarity=' + escape($('#Polarity').val());
        tmpStr = tmpStr + '&Cleat=' + escape($('#Cleat').val()) + '&PageSize=' + GetRadioValue();
        location.href = tmpStr;   //Перенаправление

    }
</script>

</asp:Content>
 
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
    <h1>Sales Report</h1>
    <table>
        <tr>
            <td style="vertical-align:top;">
                <uc2:ClientSearchList ID="ClientSearchList" runat="server" />
            </td>
            <td style="width:100%;vertical-align:top;text-align:left;line-height:24px;padding-left:10px;">
                Date from: <asp:TextBox class="date" ID="RegDateMin" runat="server"></asp:TextBox> 
	            Date till: <asp:TextBox class="date" ID="RegDateMax" runat="server"></asp:TextBox> 
	            Order #: <asp:TextBox ID="OrderId" runat="server"></asp:TextBox> 
	            <asp:Button ID="_btnFillReport" runat="server" Text="Display" CssClass="button" onclick="_btnFillReport_Click" />
                &nbsp;   <asp:Button ID="_btnUnloadReport" runat="server" visible="false" Text="Download" CssClass="button"  />
		        <uc3:SaleReportTable ID="srt" runat="server" />
		        <div id="AjaxLoadDialog" style="width: 100%; vertical-align: middle; text-align: center; height: 100%;">
                    <div style="width: 100%; visibility:hidden; vertical-align: middle; text-align: center; height: 100%;">
                        <table style="width: 100%; height: 150px;">
                            <tr style="width: 100%; height: 100%;">
                                <td style="width: 50%; text-align: right;">Wait please, content is loading...</td>
                                <td style="width: 50%;"><img style="text-align: center; float: left;" src="/images/ajax-loader-small.gif" alt="" width="30" height="30" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
