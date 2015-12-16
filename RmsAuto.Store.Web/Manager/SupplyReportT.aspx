<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="SupplyReportT.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.SupplyReportT" Title="Manager working place" %>
<%@ Register src="~/Manager/Controls/SupplyReportTable.ascx" tagname="SupplyReportTable" tagprefix="uc3" %>
<%@ Register src="~/Manager/Controls/ClientSearchList.ascx" tagname="ClientSearchList" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
 <script type="text/javascript">
    $(function() {
        $('.date').datepicker();
    });

    $(function() {
        $("table .list tr td:contains('Summary:')").parent().children().addClass("VipTextStyle");
    });
</script>

</asp:Content>

 
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
<h1>Goods Receipt Report</h1>
    <table>
        <tr>
            <td style="vertical-align:top;">
                <uc2:ClientSearchList ID="ClientSearchList" runat="server" />
            </td>
            <td style="width:100%;vertical-align:top;text-align:left;line-height:24px;padding-left:10px;">
                Date from: <asp:TextBox class="date" ID="RegDateMin" runat="server" Width="100"></asp:TextBox> 
	            till: <asp:TextBox class="date" ID="RegDateMax" runat="server" Width="100"></asp:TextBox> 
	            order number: <asp:TextBox ID="OrderId" runat="server"></asp:TextBox> 
	            <asp:Button ID="_btnFillReport" runat="server" Text="Display" Width="74px" Class="btn btn-primary btn-sm" onclick="_btnFillReport_Click" />
                &nbsp;<asp:Button ID="_btnUnloadReport" runat="server" Text="Download" visible="false" Width="74px" Class="btn btn-primary btn-sm" />
		        <uc3:SupplyReportTable ID="srt" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
