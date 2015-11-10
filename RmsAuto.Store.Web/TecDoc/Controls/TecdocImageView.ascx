<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TecdocImageView.ascx.cs" Inherits="RmsAuto.Store.Web.TecDoc.Controls.TecdocImageView" %>
<%@ Import Namespace="RmsAuto.Common.Web" %>

<center>
    <asp:ObjectDataSource ID="dsImages" runat="server" SelectMethod="GetImages" TypeName="RmsAuto.TechDoc.Facade">
        <SelectParameters>
            <asp:QueryStringParameter Name="artId" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
    <asp:ListView ID="ImageView" runat="server" DataKeyNames="ID" DataSourceID="dsImages">
        <LayoutTemplate>
            <div runat="server" id="itemplaceholder" />
        </LayoutTemplate>
        <ItemTemplate>
            <div runat="server">
                <asp:Image runat="server" ID="Image1" ImageUrl='<%# GetImageUrl( (int)Eval("ID") ) %>' />
            </div>
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:Literal ID="lNoImage" runat="server" Text="Для этой детали нет изображений" />
        </EmptyDataTemplate>
    </asp:ListView>
    <br />
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ImageView" PageSize="1">
        <Fields>
            <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="False" ShowLastPageButton="False"
                FirstPageText="Первая" PreviousPageText="&laquo; Предыдущая" NextPageText="Следующая &raquo;"
                LastPageText="Последняя" RenderDisabledButtonsAsLabels="False" />
        </Fields>
    </asp:DataPager>
</center>
