<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ListSparePartImagesMgmt.aspx.cs" Inherits="RmsAuto.Store.Adm.ListSparePartImagesMgmt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

    <script type ="text/javascript">

        function confirmDelete() {
            if (confirm("Вы подтверждаете удаление?")) {
	            return true;
	        } else {
	            //window.location.href = "ListSparePartImagesMgmt.aspx";
	            return false;
	        }
        }
        
    </script>

	<h4>Список Фотографий деталей</h4>
	
	<%-- фильтр --%>
	<div style = "font-weight: bold">
	    Фильтр:
	</div>
	<br />
	Производитель:
	<asp:TextBox ID = "TBManufacturer" runat = "server"></asp:TextBox>
	Артикул:
	<asp:TextBox ID = "TBPartNumber" runat = "server"></asp:TextBox>
	Склад уценки:
	<asp:DropDownList ID = "DDLSupplierID" runat = "server"></asp:DropDownList>
	<asp:Button ID = "BSubmit" Text = "Выполнить" PostBackUrl="ListSparePartImagesMgmt.aspx" runat = "server" />
	<br />
    <br />
	<asp:Label runat = "server" ID = "LblMessage" Visible = "false" style = "color: Red"></asp:Label>
	<br />
	<br />
	
    <div class="bottomhyperlink">
	    <a href = "SparePartImagesMgmt.aspx" ><img src = "/DynamicData/Content/Images/plus.gif" />Добавить фотографию</a>
    </div>
	
	<br />
	
	<%-- вывод таблицы с фотографиями --%>
	<table id = "PhotoTable">
	
	<tr>
	    <th>
	    </th>
	    <th>
	    </th>
	    <th>
	        Производитель
	    </th>
	    <th>
	        Артикул
	    </th>
	    <th>
	        Склад уценки
	    </th>
	    <th>
	        Номер фотографии
	    </th>
	    <th>
	        Информация о фотографии
	    </th>
	</tr>
	
	
	<%
        string storage = "";    
	    foreach (var spare in displayListSparePartImages)
        {
            if (spare.SupplierID == 1212)
            {
                storage = "Уценка 20%";
            }
            else
            {
                storage = "Уценка 50%";
            }
    %>
    
    <tr>
        <td>
            <a href = "SparePartImagesMgmt.aspx?action=edit&manufacturer=<%= spare.Manufacturer %>&partnumber=<%= spare.PartNumber %>&supplierid=<%= spare.SupplierID %>&imagenumber=<%= spare.ImageNumber %>">
                <img src = "/DynamicData/Content/Images/page_edit.png" width = "16px" height = "16px" title = "Редактировать фотографию" />
            </a>
        </td>
        <td>
            <a href = "ListSparePartImagesMgmt.aspx?action=del&manufacturer=<%= spare.Manufacturer %>&partnumber=<%= spare.PartNumber %>&supplierid=<%= spare.SupplierID %>&imagenumber=<%= spare.ImageNumber %>" onclick = "return confirmDelete()">
                <img src = "/DynamicData/Content/Images/cross.png" width = "16px" height = "16px" title = "Удалить фотографию" />
            </a>
        </td>
	    <td>
	        <%= spare.Manufacturer %>
	    </td>
	    <td>
	        <%= spare.PartNumber %>
	    </td>
	    <td>
	        <%= storage %>
	    </td>
	    <td>
	        <%= spare.ImageNumber %>
	    </td>
	    <td>
	        <%= spare.Description %>
	    </td>
	</tr>
    
    <%
        }
    %>
	
	<tr>
        <td colspan = "5" style = "border-bottom: solid 0px #000000;">
        
            <%-- пейджинг --%>
            <table id = "PhotoTable2">
	            <tr>
	            <% if (currentNumPossitoin > minPossition)
                   {
                       int previousPage = (currentNumPossitoin - 1) * numPageInLine;
                %>
                    <td>
                        <a href = "ListSparePartImagesMgmt.aspx?action=page&id=<%= previousPage %>">
                            <
                        </a>
                    </td>        
                <%
                    }        
                %>    
                
	            <% 
                    for (int i = ((currentNumPossitoin - 1) * numPageInLine + 1); i <= currentNumPossitoin * numPageInLine; i++)
                    {
                        string colorPage;
                        if (i == id)
                        {
                            colorPage = "#7ffb91";
                        }
                        else
                        {
                            colorPage = "#b4f9fa";    
                        }

                        if (i <= numAllPage)
                        {
                %>
                
               
                    <td>
                    
    <asp:Label ID="Label1" runat="server" Text="Страница "></asp:Label>                
  	<asp:TextBox ID = "PageNumberID" runat = "server" style = "text-align:center;" Width="48px"></asp:TextBox>
    <asp:Label ID="FromToPs" runat="server"></asp:Label>                
                       <%--div style = "width: 20px; background-color: <%= colorPage %>; height: 13px;">
                            <a href = "ListSparePartImagesMgmt.aspx?action=page&id=<%= i %>">
                                <%= i%>
                            </a>
                        </div--%>
                    </td>    
	            <%
                        }
                    }
	            %>
        	    
	            <% if (currentNumPossitoin < maxPossition)
                   {
                       int nextPage = currentNumPossitoin * numPageInLine + 1;
                %>
                    <td>
                        <a href = "ListSparePartImagesMgmt.aspx?action=page&id=<%= nextPage %>">
                            >
                        </a>
                    </td>         
                <%
                    }        
                %>   
	            </tr>
	        </table>
        
        </td>
	    <td colspan = "2" style = "text-align: right; border-bottom: solid 0px #000000;">
	        Строк на странице:
	        <asp:DropDownList ID="DDLNumLineInPage" runat="server" AutoPostBack = "true" >
	            <asp:ListItem Value="10" Text="10"></asp:ListItem>
		        <asp:ListItem Value="50" Text="50"></asp:ListItem>
		        <asp:ListItem Value="100" Text="100"></asp:ListItem>
	        </asp:DropDownList>
	    </td>
	</tr>
	
	</table>
	
</asp:Content>


