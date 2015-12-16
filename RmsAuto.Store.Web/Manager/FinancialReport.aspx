<%@ Page Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="FinancialReport.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.FinancialReport" %>

<%@ Register src="~/Manager/Controls/FinancialReport.ascx" tagname="finreporttable" tagprefix="uc3" %>
<%@ Register src="~/Manager/Controls/ClientSearchList.ascx" tagname="ClientSearchList" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">

<script type="text/javascript">
    $(function () {
        $('.date').datepicker();
    });

    $(function() {
        $("table .table-big tr td:contains('Total:')").parent().children().addClass("VipTextStyle");
    });
</script>

</asp:Content>
 
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
<h1>Financial report</h1>
    <table>
        <tr>
            <td style="vertical-align:top;">
                <uc2:ClientSearchList ID="ClientSearchList" runat="server" />
            </td>
            <td style="width:100%;vertical-align:top;text-align:left;line-height:24px;padding-left:10px;">
                Manager:
	                <asp:DropDownList runat="server" ID="_managerList" AutoPostBack="True" 
                        DataTextField="FullName" DataValueField="EmployeeID" 
                        Width="190px">
			        </asp:DropDownList>
  
                Date from: <asp:TextBox class="date" ID="RegDateMin" runat="server" Width="100"></asp:TextBox> 
	            till: <asp:TextBox class="date" ID="RegDateMax" runat="server" Width="100"></asp:TextBox> 
	            &nbsp;<asp:Button ID="_btnFillReport" runat="server" Text="Display" Class="btn btn-primary btn-sm"  onclick="_btnFillReport_Click" />
	            &nbsp;<asp:Button ID="_btnUnloadReport" runat="server" Text="Download" Class="btn btn-primary btn-sm" visible="false" OnClientClick="window.open('FinancialReport.ashx')" />
		        <uc3:finreporttable ID="srt" runat="server" />
            </td>
        </tr>
    </table>

	        
</asp:Content>
