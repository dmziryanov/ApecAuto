<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscountCard_Edit.ascx.cs" Inherits="RmsAuto.Store.Web.Controls.DiscountCard_Edit" %>
<asp:TextBox ID="_txtCardNumberPart1" runat="server" Columns="3" MaxLength="3" 
    ToolTip="первые три цифры номера карты" Width="45px" /> -
<asp:TextBox ID="_txtCardNumberPart2" runat="server" Columns="4" MaxLength="4" 
    ToolTip="следующие четыре цифры номера" Width="60px" /> -
<asp:TextBox ID="_txtCardNumberPart3" runat="server" Columns="4" MaxLength="4"
    ToolTip="последние четыре цифры номера" Width="60px" />
