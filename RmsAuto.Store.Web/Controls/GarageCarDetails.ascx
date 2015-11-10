<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GarageCarDetails.ascx.cs"
	Inherits="RmsAuto.Store.Web.Controls.GarageCarDetails" %>

<table runat="server" id="_table" width="100%" cellpadding="0" cellspacing="0" class="info">
	<tr runat="server" id="_trVinPlace">
		<th>
			<%=global::Resources.Texts.VIN %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_vinLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trFrameNumberPlace">
		<th>
			<%=global::Resources.Texts.FrameNumber %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_frameLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trBrandPlace">
		<th>
			<%=global::Resources.Texts.VinRequests_Brand %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_brandLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trModelPlace">
		<th>
			<%=global::Resources.Texts.VinRequests_Model %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_modelLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trModificationPlace">
		<th>
			<%=global::Resources.Texts.VinRequests_Modification %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_modificationLabel" />
		</td>
	</tr>
	<tr>
		<th>
			<%=global::Resources.Texts.VinRequests_Year %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_yearLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trMonthPlace">
		<th>
			<%=global::Resources.Texts.VinRequests_Month %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_monthLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trEngineTypePlace">
		<th>
			<%=global::Resources.Texts.VinRequests_EngineType %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_engineTypeLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trEngineNumberPlace">
		<th>
			<%=global::Resources.Texts.VinRequests_Engine %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_engineNumberLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trEngineCCMPlace">
		<th>
			<%=global::Resources.Texts.VinRequests_EngineCCM %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_engineCCMLabel" />
			<%# this.CarParameters.EngineCCM %>
		</td>
	</tr>
	<tr runat="server" id="_trEngineHPPlace">
		<th>
			<%=global::Resources.Texts.VinRequests_EngineHP %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_engineHPLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trTransmissionTypePlace">
		<th>
			<%=global::Resources.Texts.VinRequests_TransmissionType %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_transmissionTypeLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trTransmissionNumberPlace">
		<th>
			<%=global::Resources.Texts.VinRequests_TransmissionNumber %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_transmissionNumberLabel" />
		</td>
	</tr>
	<tr runat="server" id="_trBodyTypePlace">
		<th>
			<%=global::Resources.Texts.VinRequests_BodyType %>
		</th>
		<td>
			<asp:Literal runat="server" ID="_bodyTypeLabel" />
		</td>
	</tr>
</table>
