<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReferencesDemo.aspx.cs" Inherits="RmsAuto.Store.Web.ReferencesDemo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Ханса.Справочники</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table cellspacing="10" cellpadding="10">
        <tr>
            <td>Название</td>
            <td>Данные</td>
        </tr>
        <asp:Repeater ID="_rptReferences" runat="server" 
            onitemdatabound="_rptReferences_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td><asp:Literal ID="_ltrRefName" runat="server"></asp:Literal></td>
                    <td><asp:GridView ID="_refItems" runat="server"></asp:GridView>
                        <asp:Label ID="_lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
     </table>   
     </div>
    </form>
</body>
</html>
