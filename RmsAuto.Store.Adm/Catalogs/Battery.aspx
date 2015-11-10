<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Battery.aspx.cs" Inherits="RmsAuto.Store.Adm.Battery" ValidateRequest="false" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.2, Version=11.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.2, Version=11.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v11.2, Version=11.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Import Namespace="RmsAuto.Store.Cms.Routing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

<script type="text/jscript">
        function file_save(file) {
            $("input[id$='FileIDHidden']").val(file.fileID);
            $("input[id$='ImageUrlText']").val(file.fileID);
        }
 </script>
    <h3>Файл для загрузки</h3>
       <asp:FileUpload Id = "FileUpload1" runat = "server">
       </asp:FileUpload>&nbsp;<asp:Button id="UploadButton" 
           Text="Загрузить файл"
           OnClick="Button1_Click"
           runat="server">
       </asp:Button>&nbsp;
       <!-- dbg: a href='http://webtest.rmsauto.ru/Files/1474.ashx'>Шаблон файла</a-->
       <a href='http://rmsauto.ru/Files/1876.ashx'>Шаблон файла</a>
      <h3>Загруженные модели аккумуляторов</h3>
<dx:ASPxGridView ID="ASPxGridView1" runat="server" 
        CssFilePath="~/App_Themes/DevEx/{0}/styles.css" CssPostfix="DevEx" 
        DataSourceID="LinqDataSource1" AutoGenerateColumns="False" 
        KeyFieldName="PartNumber" >
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
            <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="10">
                <EditButton Visible="True">
                </EditButton>
                <NewButton Visible="True">
                </NewButton>
                <DeleteButton Visible="True">
                </DeleteButton>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="ImageUrl" Caption="Изображение" 
                VisibleIndex="0"     ReadOnly="True">
                           <DataItemTemplate>
                    <asp:Image  height="100" width="100" ID="TireImage" runat="server" ImageUrl=' <%# UrlManager.GetFileUrl(Convert.ToInt32(Eval("ImageUrl")))%>' />
                </DataItemTemplate>
                <EditItemTemplate>
                    
<asp:Image ID="TireImageEdit" runat="server" ImageUrl=' <%# UrlManager.GetFileUrl(Convert.ToInt32(Eval("ImageUrl")))%>' />
<asp:HiddenField runat="server" ID="FileIDHidden" Value='<%# Bind("ImageUrl") %>' />

<span runat="server" id="ImageDiv"></span>
<asp:TextBox ID="ImageUrlText" runat="server" Text='<%# Bind("ImageUrl") %>'></asp:TextBox>

<input type="button" value="Выбрать" onclick="mso.imageManagerPopup.select( 'image',file_save,null)" />

                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Manufacturer" VisibleIndex="2"  ReadOnly="false" Caption="Производитель">
            <EditItemTemplate>
             <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" 
                        CssFilePath="~/App_Themes/DevEx/{0}/styles.css" CssPostfix="DevEx" 
                        DataSourceID="BatteryManufacturers" Spacing="0" 
                        SpriteCssFilePath="~/App_Themes/DevEx/{0}/sprite.css" TextField="Name"
                        Value='<%# Bind("ManufaturerId") %>' ValueField="Id" ValueType="System.Int32">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <LoadingPanelStyle ImageSpacing="5px">
                        </LoadingPanelStyle>
                        <LoadingPanelImage Url="~/App_Themes/DevEx/Editors/Loading.gif">
                        </LoadingPanelImage>
                    </dx:ASPxComboBox>
                    <asp:LinqDataSource ID="BatteryManufacturers" runat="server" 
                        ContextTypeName="RmsAuto.Store.Cms.Entities.CmsDataContext" OrderBy="Name" 
                        Select="new (Name, Id)" TableName="BatteryBrands">
                    </asp:LinqDataSource>
                    <p><a href="BatteryBrands/List.aspx" target="_blank">Справочник брендов</a></p>
                    </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ModelName" Caption="Название модели" VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Capacity" Caption="Ёмкость" VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="polarity" Caption="Полярность" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="cleat" Caption="Клеммы" VisibleIndex="7">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="Size" Caption="Габариты" VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PartNumber" Caption="Артикул" 
                VisibleIndex="1">
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
    <asp:LinqDataSource ID="LinqDataSource1" runat="server"
        ContextTypeName="RmsAuto.Store.Cms.Entities.CmsDataContext" 
        TableName="Batteries" EnableDelete="True" EnableInsert="True" 
        EnableUpdate="True" 
        OrderBy="Manufacturer, ModelName, PartNumber">
    </asp:LinqDataSource>
</asp:Content>
                         
    
                         
  
