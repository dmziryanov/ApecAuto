<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/ManagerNEW.Master" AutoEventWireup="true" CodeBehind="UploadSpareParts.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.UploadSpareParts" %>
<%@ Import Namespace="RmsAuto.Store.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
        <p>
        1. Номер строки - целое число<br>
        2. Идетификатор размера SizeID - целое число (пока для всех 1)<br>
        3. Производитель - тип строка длина до 50<br>
        4. Код детали ОЕМ - тип строка длина до 50 (Внимание по этому номеру производится поиск основной и аналогов!)<br>
        5. Внутренний код детали ОЕМ - тип строка длина до 50 (Это дополнительный, может быть любым на ваше усмотрение)<br>
        6. Название, тип строка длина до 255<br>
        7. Описание, строка до 255<br>
        8. Вес.<br>
        9. Объем.<br>
        10. Цена. Числовой формат. Разделитель дробной части: '.'<br>
        11. Доступное количество на вашем складе<br>
        12. Минимальный срок поставки клиенту (в вашем регионе)<br>
        13. Максимальный срок поставки клиенту (в вашем регионе)<br>
        14. Номер поставщика (число, указываете ваш любой номер, идентифицируюший поставщика) ВАЖНО! файл может содержать номеклатуру одного поставщика, для всех строк этот параметр должен быть одинаковый. Старые строки этого поставщика удаляются<br>
        15. Минимальное количество для заказа (целое число)<br>
        16. Дополнительный код, по умочанию равен 1<br>
        17. Постоянная ценобразующая (добавляется к основной цене и равна 0). Разделитель дробной части: '.'<br>
        18. Номер группы деталей (заполняется 0)<br>
		19. Ваш личный код, для ваc он равен <h1><%=  SiteContext.Current.InternalFranchName %></h1> в случае несовпадения, детали не загрузятся<br>
            </p>
    <h3>Загрузка номенклатуры</h3>
	<p>
	<a href='<%= this.GetTemplateUrl() %>' class="button">File upload example</a>
	<br/>
	<br/>
	<asp:FileUpload ID="fuMain" runat="server" />&nbsp; 
	<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" CssClass="button"/></p>
	<p><span style="color:Green"><asp:Literal ID="lPreloadInfo" runat="server"></asp:Literal>&nbsp;</span></p>
    <div class="tab-text">
        <div class="t-hold">
         
        </div>
    </div>
	
	<p runat="server" id="pButtons">
	</p>
	<p><asp:Literal ID="lSummaryInfo" runat="server"></asp:Literal></p>
</asp:Content>
