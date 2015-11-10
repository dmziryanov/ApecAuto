<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="WebServiceUsers.aspx.cs" Inherits="RmsAuto.Store.Adm.WebServiceUsers" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.2, Version=11.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.2, Version=11.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v11.2, Version=11.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

    <div>
    
    </div>
<h3>Справочник пользователей</h3>    
<dx:ASPxGridView ID="ASPxGridView1" runat="server" 
        CssFilePath="~/App_Themes/DevEx/{0}/styles.css" CssPostfix="DevEx" 
        DataSourceID="ServiceAccounts" AutoGenerateColumns="False" 
        KeyFieldName="ClientID" >
        <Styles CssFilePath="~/App_Themes/DevEx/{0}/styles.css" 
            CssPostfix="DevEx">
            <LoadingPanel ImageSpacing="5px">
            </LoadingPanel>
            <Header ImageSpacing="5px" SortingImageSpacing="5px">
            </Header>
        </Styles>
        <ImagesFilterControl>
            <LoadingPanel Url="~/App_Themes/DevEx/GridView/Loading.gif">
            </LoadingPanel>
        </ImagesFilterControl>
        <Images SpriteCssFilePath="~/App_Themes/DevEx/{0}/sprite.css">
            <LoadingPanelOnStatusBar Url="~/App_Themes/DevEx/GridView/StatusBarLoading.gif">
            </LoadingPanelOnStatusBar>
            <LoadingPanel Url="~/App_Themes/DevEx/GridView/Loading.gif">
            </LoadingPanel>
        </Images>
        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0">
                <EditButton Visible="True">
                </EditButton>
                <NewButton Visible="True">
                </NewButton>
                <DeleteButton Visible="True">
                </DeleteButton>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="ClientID" 
                VisibleIndex="1"     ReadOnly="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="WcfServiceAccount" 
                VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Permissions" 
                VisibleIndex="3">
            </dx:GridViewDataTextColumn>
        </Columns>
         <Templates>
            <PreviewRow>
                <asp:Image Height="40" Width="20" ID="Image1" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' />
            </PreviewRow>
        </Templates>
        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
        <StylesEditors ButtonEditCellSpacing="0">
            <ProgressBar Height="21px">
            </ProgressBar>
        </StylesEditors>
    </dx:ASPxGridView>
    <asp:LinqDataSource ID="ServiceAccounts" runat="server" 
        ContextTypeName="RmsAuto.Store.Entities.StoreDataContext" 
        TableName="ServiceAccounts" EnableDelete="True" EnableInsert="True" 
        EnableUpdate="True">
    </asp:LinqDataSource>
<h3>Справочник методов</h3>
<dx:ASPxGridView ID="ASPxGridView2" runat="server" 
        CssFilePath="~/App_Themes/DevEx/{0}/styles.css" CssPostfix="DevEx" 
        DataSourceID="LinqDataSource1" AutoGenerateColumns="False" 
        KeyFieldName="MethodId" >
        <Styles CssFilePath="~/App_Themes/DevEx/{0}/styles.css" 
            CssPostfix="DevEx">
            <LoadingPanel ImageSpacing="5px">
            </LoadingPanel>
            <Header ImageSpacing="5px" SortingImageSpacing="5px">
            </Header>
        </Styles>
        <ImagesFilterControl>
            <LoadingPanel Url="~/App_Themes/DevEx/GridView/Loading.gif">
            </LoadingPanel>
        </ImagesFilterControl>
        <Images SpriteCssFilePath="~/App_Themes/DevEx/{0}/sprite.css">
            <LoadingPanelOnStatusBar Url="~/App_Themes/DevEx/GridView/StatusBarLoading.gif">
            </LoadingPanelOnStatusBar>
            <LoadingPanel Url="~/App_Themes/DevEx/GridView/Loading.gif">
            </LoadingPanel>
        </Images>
         <Templates>
            <PreviewRow>
                <asp:Image Height="40" Width="20" ID="Image2" runat="server" 
                    ImageUrl='<%# Eval("ImageUrl") %>' />
            </PreviewRow>
        </Templates>
        <Columns>
            <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0">
                <EditButton Visible="True">
                </EditButton>
                <NewButton Visible="True">
                </NewButton>
                <DeleteButton Visible="True">
                </DeleteButton>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="MethodId" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MethodDescription" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MethodName" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
        <StylesEditors ButtonEditCellSpacing="0">
            <ProgressBar Height="21px">
            </ProgressBar>
        </StylesEditors>
    </dx:ASPxGridView>
    <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
        ContextTypeName="RmsAuto.Store.Entities.StoreDataContext" 
        TableName="WebServiceMethods" EnableDelete="True" EnableInsert="True" 
        EnableUpdate="True">
    </asp:LinqDataSource>
</asp:Content>