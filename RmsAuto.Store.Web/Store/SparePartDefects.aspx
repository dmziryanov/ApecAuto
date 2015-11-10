<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SparePartDefects.aspx.cs" Inherits="RmsAuto.Store.Web.Store.SparePartDefects" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>APEC</title>
	<link rel="StyleSheet" href="<%=ResolveUrl("~/css/css_new.css")%>" type="text/css" />
	<script src="<%= ResolveUrl("~/Scripts/jquery-1.6.2.min.js") %>" type="text/javascript"></script>
</head>

<body class="body" style="padding:15px;">

    <form id="form1" runat="server">
    
    <% 
        int heightNp = ImageHeight + 40;
        string np = "margin-top: -" + heightNp + "px;";
        string but = "margin-top: " + heightNp + "px;";
    %>

        <div style = "text-align: center">
			<asp:ObjectDataSource ID="dsImages" runat="server" SelectMethod="GetSPImageInfo" TypeName="RmsAuto.Store.Dac.SparePartImageDac">
				<SelectParameters>
					<asp:QueryStringParameter Name="sparePartImageID" Type="String" />
				</SelectParameters>
			</asp:ObjectDataSource>
			<br />
			
			<h1>В виду присутствия дефекта, цена на данную деталь снижена.<br />На фото Вы можете оценить степень повреждения.</h1>			
			
            <div>
			    <asp:ListView ID="ImageView" runat="server" DataSourceID="dsImages">
				    <LayoutTemplate>
					    <div runat="server" id="itemplaceholder" />
				    </LayoutTemplate>
				    <ItemTemplate>
				            <div id = "id_head" style = "padding-bottom: 42px;">
						    <%=global::Resources.Texts.Manufacturer%>: <asp:Label ID="_lblManufacturer" runat="server" ForeColor="#28387a"><%# Eval("Manufacturer") %></asp:Label>
						    <%=global::Resources.Texts.Article%>: <asp:Label ID="_lblArticle" runat="server" ForeColor="#28387a"><%# Eval("PartNumber") %></asp:Label>
						    <p>Фото № <%# Eval("ImageNumber") %>: <%# Eval("Description")%></p>
						    </div>

                            <div id = "id_img">
						        <asp:Image runat="server" ID="Image1" ImageUrl='<%# GetImageUrl((SparePartImage)Container.DataItem) %>' />
                            </div>
                            
				    </ItemTemplate>
				    <EmptyDataTemplate>
					    <asp:Literal ID="lNoImage" runat="server" Text="Для этой детали нет изображений" />
				    </EmptyDataTemplate>
			    </asp:ListView>
			    
			    <div id = "id_np" style = "<%= np %>">
			        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ImageView" PageSize="1">
				        <Fields>
					        <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="False" ShowLastPageButton="False"
						        FirstPageText="Первая" PreviousPageText="&lt;&nbsp;Назад&nbsp;" NextPageText="&nbsp;Далее&nbsp;&gt;"
						        LastPageText="Последняя" RenderDisabledButtonsAsLabels="False" />
				        </Fields>
			        </asp:DataPager>
			    </div>
			    
	        </div>
	        
            <div id = "id_but" style = "<%= but %>">
		        <input type="button" onclick="javascript:self.close()" value="<%=global::Resources.Texts.Close %>" />
            </div>
        </div>
    </form>
</body>
</html>
