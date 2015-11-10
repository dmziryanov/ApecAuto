<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GarageCarEdit.ascx.cs"
	Inherits="RmsAuto.Store.Web.Controls.GarageCarEdit" %>
<%@ Register Assembly="RmsAuto.Common" Namespace="RmsAuto.Common.Web.UI" TagPrefix="rms" %>
<%@ Import Namespace="RmsAuto.Store.Entities" %>
<%@ Import Namespace="RmsAuto.Common.Misc" %>

<asp:ValidationSummary ID="ValidationSummary1" runat="server"  
	ValidationGroup="CarEditGroup" DisplayMode="List"  CssClass="error" 
	meta:resourcekey="ValidationSummary1Resource1" />

<table width="100%" cellpadding="0" cellspacing="0" class="info">
	<tr>
		<th>
			<%=global::Resources.Texts.VIN%> <font color=red>**</font>
		</th>
		<td>
			<asp:TextBox ID="vrVIN" runat="server" MaxLength="17" 
				meta:resourcekey="vrVINResource1" />
			<img id="_img1" runat="server" src="~/images/help.gif" width="11" height="11" border="0" style="margin-left: 10px;" class="help_img" title="17 символов, все символы - цифры и латиница, кроме I, O и Q"/>
			<asp:RegularExpressionValidator ValidationExpression="[A-HJ-NPR-Za-hj-npr-z\d]{17}" runat="server"
			ErrorMessage="<%$ Resources:Exceptions, VinRequest_BadVin %>" 
				ValidationGroup="CarEditGroup" ControlToValidate="vrVIN" Display="Dynamic" 
				meta:resourcekey="RegularExpressionValidatorResource1" />
			
			<asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="CarEditGroup" 
                ErrorMessage="<%$ Resources:Texts, RequiredFieldsOrVIN_Frame %>"
                onservervalidate="ValidateFrameOrVIN" Display="Dynamic" 
				meta:resourcekey="CustomValidator1Resource1"></asp:CustomValidator>	
        </td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.FrameNumber%> <font color=red>**</font>
		</th>
		<td>
			<asp:TextBox ID="vrFrameNumber" MaxLength="17" runat="server" 
				meta:resourcekey="vrFrameNumberResource1" />
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_Brand%> <font color=red>*</font>
		</th>
		<td>
		    		
            <asp:DropDownList ID="_ddBrands" runat="server" 
				OnSelectedIndexChanged="_ddBrandsIndexChanged" AutoPostBack="True" 
				Width="200px" meta:resourcekey="_ddBrandsResource1">
                <asp:ListItem Value="" Text="<%$ Resources:Texts, NA_Female %>" 
					 />
            </asp:DropDownList>
            
            <img id="_img2" runat="server" src="~/images/help.gif" width="11" height="11" border="0" style="margin-left: 10px;" class="help_img" title="Выберите марку из списка или введите ее в поле"/>
            
            <asp:CustomValidator ID="_cvldBrand" runat="server" 
				ErrorMessage="Не указана марка" ValidationGroup="CarEditGroup" 
				OnServerValidate="_cvldBrandValidate"  Display="Dynamic" 
				meta:resourcekey="_cvldBrandResource1"/>
            <br /><asp:TextBox ID="_txtBrand" runat="server" 
				meta:resourcekey="_txtBrandResource1" />
            
	        <%-- asp:RequiredFieldValidator ValidationGroup="CarEditGroup" runat="server" ControlToValidate="_ddBrands"
            ErrorMessage="Не указана марка"></asp:RequiredFieldValidator --%>
            
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_Model%> <font color=red>*</font>
		</th>
		<td>
		
            <asp:DropDownList ID="_ddModels" runat="server" 
				OnSelectedIndexChanged="_ddModelsIndexChanged" AutoPostBack="True" 
				Width="200px" meta:resourcekey="_ddModelsResource1">
                <asp:ListItem Value="" Text="<%$ Resources:Texts, NA_Female %>" 
					 />
            </asp:DropDownList>
            
            <img id="_img3" runat="server" src="~/images/help.gif" width="11" height="11" border="0" style="margin-left: 10px;" class="help_img" title="Выберите модель из списка или введите ее в поле"/>
            
            <asp:CustomValidator ID="_cvldModel" runat="server" 
				ErrorMessage="Не указана модель" ValidationGroup="CarEditGroup" 
				OnServerValidate="_cvldModelValidate" Display="Dynamic" 
				meta:resourcekey="_cvldModelResource1" />

		    <br /><asp:TextBox ID="_txtModel" runat="server" 
				meta:resourcekey="_txtModelResource1" />
	        <%--asp:RequiredFieldValidator ValidationGroup="CarEditGroup" runat="server" ControlToValidate="_ddModels"
            ErrorMessage="Не указана модель"></asp:RequiredFieldValidator--%>
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_Modification%>
		</th>
		<td>
		
			<asp:DropDownList ID="_ddModifications" runat="server" 
				OnSelectedIndexChanged="_ddModificationsIndexChanged" AutoPostBack="True" 
				Width="200px" meta:resourcekey="_ddModificationsResource1">
			    <asp:ListItem Value="" Text="<%$ Resources:Texts, NA_Female %>" 
					 />
			</asp:DropDownList>
			
            <img id="_img4" runat="server" src="~/images/help.gif" width="11" height="11" border="0" style="margin-left: 10px;" class="help_img" title="Выберите модификацию из списка или введите ее в поле"/>
            
		    <br /><asp:TextBox ID="_txtModification" runat="server" 
				meta:resourcekey="_txtModificationResource1" />
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_Year%> <font color=red>*</font>
		</th>
		<td>
			<asp:TextBox ID="vrYear" runat="server" MaxLength="4" Columns="4" 
				ValidationGroup="CarEditGroup" meta:resourcekey="vrYearResource1" />
			
			<asp:RequiredFieldValidator ValidationGroup="CarEditGroup" runat="server" ControlToValidate="vrYear"
            ErrorMessage="Не указан год" ID="_rfvYearRequiredValidator" Display="Dynamic" 
				meta:resourcekey="_rfvYearRequiredValidatorResource1"></asp:RequiredFieldValidator>
            
            <asp:RangeValidator ValidationGroup="CarEditGroup" runat="server" 
				ControlToValidate="vrYear" MinimumValue="1900" MaximumValue="9999"
            ErrorMessage="Год указан неверно" Type="Integer" ID="_rvYearRangeValidator" 
				Display="Dynamic" meta:resourcekey="_rvYearRangeValidatorResource1" />
        </td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_Month%>
		</th>
		<td>
			<asp:DropDownList ID="_ddMonths" runat="server" 
				meta:resourcekey="_ddMonthsResource1">
			    <asp:ListItem Value="" Text="<%$ Resources:Texts, NA_Male %>" 
					 />
			    <asp:ListItem Value="1" Text="<%$ Resources:Texts, January %>" 
					 />
			    <asp:ListItem Value="2" Text="<%$ Resources:Texts, February %>" 
					 />
			    <asp:ListItem Value="3" Text="<%$ Resources:Texts, March %>" 
					 />
			    <asp:ListItem Value="4" Text="<%$ Resources:Texts, April %>" 
					 />
			    <asp:ListItem Value="5" Text="<%$ Resources:Texts, May %>" 
					 />
			    <asp:ListItem Value="6" Text="<%$ Resources:Texts, June %>" 
					 />
			    <asp:ListItem Value="7" Text="<%$ Resources:Texts, July %>" 
					 />
			    <asp:ListItem Value="8" Text="<%$ Resources:Texts, August %>" 
					 />
			    <asp:ListItem Value="9" Text="<%$ Resources:Texts, September %>" 
					 />
			    <asp:ListItem Value="10" Text="<%$ Resources:Texts, October %>" 
					 />
			    <asp:ListItem Value="11" Text="<%$ Resources:Texts, November %>" 
					 />
			    <asp:ListItem Value="12" Text="<%$ Resources:Texts, December %>" 
					 />
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_EngineType%>
		</th>
		<td>
			<asp:DropDownList ID="ddEngineTypes" runat="server" OnInit="ddEngineTypesInit" 
				meta:resourcekey="ddEngineTypesResource1" />
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_Engine%>
		</th>
		<td>
			<asp:TextBox ID="vrEngine" runat="server" 
				meta:resourcekey="vrEngineResource1" />
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_EngineCCM%>
		</th>
		<td>
			<asp:TextBox ID="vrEngineCCM" runat="server" 
				meta:resourcekey="vrEngineCCMResource1" />
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_EngineHP%>
		</th>
		<td>
			<asp:TextBox ID="vrEngineHP" runat="server" 
				meta:resourcekey="vrEngineHPResource1" />
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_TransmissionType%> <font color=red>*</font>
		</th>
		<td>
			<asp:DropDownList ID="ddTransmissionTypes" runat="server" 
				OnInit="ddTransmissionTypesInit" 
				meta:resourcekey="ddTransmissionTypesResource1" />
            <asp:CustomValidator ID="CustomValidator2" runat="server" 
				ErrorMessage="Не задан тип коробки передач" ValidationGroup="CarEditGroup" 
				OnServerValidate="_cvldTransmissionTypeValidate" Display="Dynamic" 
				meta:resourcekey="CustomValidator2Resource1" />
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_TransmissionNumber%>
		</th>
		<td>
			<asp:TextBox ID="vrTransmissionNumber" MaxLength="17" runat="server" 
				meta:resourcekey="vrTransmissionNumberResource1" />
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_BodyType%>
		</th>
		<td>
			<asp:DropDownList ID="ddBodyTypes" runat="server" OnInit="ddBodyTypesInit" 
				meta:resourcekey="ddBodyTypesResource1" />
		</td>
	</tr>
</table>
<%--<font color=red>*</font> <%=global::Resources.Texts.RequiredField%><br />--%>
<font color=red>**</font> <%=global::Resources.Texts.RequiredFieldsOrVIN_Frame%>
<br />
<br />

