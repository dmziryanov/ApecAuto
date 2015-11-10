<?xml version="1.0" encoding="utf-8"?>
  
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    
	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="OrderLineTrackingAlert">

		<html>
			<head>
				<title><xsl:value-of select="CompanyName"/>. Order statuses have been changed!</title>
			</head>
			<body style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">
        <div style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">
          Dear Customer!<br />
          Your order statuses have been changed as follow:<br /><br/>
        </div>
				<table border="0" cellspacing="0" cellpadding="0" width="100%" style="border-collapse:collapse;">
					<tr>
						<th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Order No</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Customer’s order No</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Order date</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Brand</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Part No</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Description</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Qnt</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Reference</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Price</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Amount</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">The date goods to be at <xsl:value-of select="CompanyName"/> WH<span style="color:red">*</span></th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Status</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Remarks</th>
            <th style="font-family: Arial, Helvetica, sans-serif;background-color:#c7c8c8;color:#7e7f7f;padding:5px 10px;font-size:12px;">Status date</th>
					</tr>
						<xsl:for-each select="OrderLines/OrderLine">
							<!--
							<xsl:if test="count(ParentOrderLine)!=0">
								<tr class="{concat('row',(position() mod 2))}">
									<td nowrap="nowrap" rowspan="2">
										<nobr>
											<xsl:value-of select="OrderDisplayNumber"/>
										</nobr>
									</td>
									<td rowspan="2">
										<xsl:value-of select="OrderDate"/>
									</td>
									<td style="border-bottom:none">
										<xsl:value-of select="ParentOrderLine/Manufacturer"/>
									</td>
									<td style="border-bottom:none">
										<xsl:value-of select="ParentOrderLine/PartNumber"/>
									</td>
									<td style="border-bottom:none">
										<xsl:value-of select="ParentOrderLine/PartName"/>
									</td>
									<td  style="border-bottom:none" align="center">
										<xsl:value-of select="ParentOrderLine/Qty"/>
									</td>
									<td class="price"  style="border-bottom:none" nowrap="nowrap">
										<nobr>
											<xsl:value-of select="ParentOrderLine/UnitPrice"/>
										</nobr>
									</td>
									<td class="price" style="border-bottom:none" nowrap="nowrap">
										<nobr>
											<xsl:value-of select="ParentOrderLine/Total"/>
										</nobr>
									</td>
									<td style="border-bottom:none">
										<xsl:value-of select="ParentOrderLine/EstSupplyDate"/>
									</td>
									<td rowspan="2">
										<xsl:value-of select="CurrentStatusName"/>
									</td>
									<td rowspan="2">
										<xsl:value-of select="CurrentStatusDescription"/>
									</td>
									<td rowspan="2">
										<xsl:value-of select="CurrentStatusDate"/>
									</td>							</tr>
							</xsl:if>
							<tr class="{concat('row',(position() mod 2))}">
								<xsl:if test="count(ParentOrderLine)=0">
									<td nowrap="nowrap">
										<nobr>
												<xsl:value-of select="OrderDisplayNumber"/>
										</nobr>
									</td>
									<td>
										<xsl:value-of select="OrderDate"/>
									</td>
								</xsl:if>
								<td>
									<xsl:value-of select="Manufacturer"/>
								</td>
								<td>
									<xsl:value-of select="PartNumber"/>
								</td>
								<td>
									<xsl:value-of select="PartName"/>
								</td>
								<td align="center">
									<xsl:value-of select="Qty"/>
								</td>
								<td class="price" nowrap="nowrap">
									<nobr>
										<xsl:value-of select="UnitPrice"/>
									</nobr>
								</td>
								<td class="price" nowrap="nowrap">
									<nobr>
										<xsl:value-of select="Total"/>
									</nobr>
								</td>
								<td>
									<xsl:value-of select="EstSupplyDate"/>
								</td>
								<xsl:if test="count(ParentOrderLine)=0">
									<td>
										<xsl:value-of select="CurrentStatusName"/>
									</td>
									<td>
										<xsl:value-of select="CurrentStatusDescription"/>
									</td>
									<td>
										<xsl:value-of select="CurrentStatusDate"/>
									</td>
								</xsl:if>
							</tr>
								-->
							<tr>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;white-space:nowrap;font-size:12px;">
									<xsl:value-of select="OrderDisplayNumber"/>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;font-size:12px;">
                  <xsl:value-of select="CustOrderNum"/>
                </td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;font-size:12px;">
									<xsl:value-of select="OrderDate"/>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;font-size:12px;">
									<xsl:value-of select="Manufacturer"/>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;font-size:12px;">
									<xsl:value-of select="PartNumber"/>
									<xsl:if test="count(ParentOrderLine)!=0">
										<span style="color:red;cursor:pointer;" title="Взамен детали: {ParentOrderLine/Manufacturer} {ParentOrderLine/PartNumber} {ParentOrderLine/PartName}">**</span>
									</xsl:if>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;font-size:12px;">
									<xsl:value-of select="PartName"/>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;text-align:center;font-size:12px;">
									<xsl:value-of select="Qty"/>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;text-align:center;font-size:12px;">
                  <xsl:value-of select="ReferenceID"/>
                </td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;text-align:center;white-space:nowrap;text-align:right;font-size:12px;">
                  <strong style="color:#2a5ba0;">
										<xsl:value-of select="UnitPrice"/>
                  </strong>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;text-align:center;white-space:nowrap;text-align:right;font-size:12px;">
                  <strong style="color:#2a5ba0;">
										<xsl:value-of select="Total"/>
									</strong>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;font-size:12px;">
									<xsl:value-of select="EstSupplyDate"/>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;font-size:12px;">
									<xsl:value-of select="CurrentStatusName"/>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;font-size:12px;">
									<xsl:value-of select="CurrentStatusDescription"/>
								</td>
                <td style="font-family: Arial, Helvetica, sans-serif;background-color:#e7e8e8;color:#7e7f7f;border-top:1px solid #717171;padding:5px 10px;font-size:12px;">
									<xsl:value-of select="CurrentStatusDate"/>
								</td>
							</tr>
						</xsl:for-each>
				</table>

        <p style="font-family: Arial, Helvetica, sans-serif;color:#000000;">
            <span style="color: red;">*</span> <span style="font-size:7pt;">
							  You may receive the cargo (or it is to be delivered) on the following working day after the date the cargo is received at <xsl:value-of select="CompanyName"/> warehouse
						</span>
            <!--<xsl:variable name="numChange" select="NumChange"/>
            <xsl:choose>
              <xsl:when test="$numChange">
              <br/>
              <span style="color: red;">**</span>
              <span style="font-size: xx-small;">Переход номера</span>
              </xsl:when>
            </xsl:choose>-->
            <xsl:if test="NumChange=0">
              <br/>
              <span style="color: red;">**</span>
              <span style="font-size: 7pt;">Superceded part</span>
            </xsl:if>
          </p>

          <div id="BlockMailFooter" style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">
            <xsl:value-of select="BlockMailFooter" disable-output-escaping="yes"/>
          </div>

          <br/>
					<table>
						<tr>
              <td style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">
								Best regards,<br />
								<xsl:value-of select="CompanyName"/> Team
								<br /><br />
								<img src="http://www.apecauto.com/images/apec_logo.jpg" border="0" width="160" height="49" alt="APEC logo" />
								<br /><br />
								<span style="font-size:13px; font-family:Arial;">
									Tel. <xsl:value-of select="Phone"/><br />
									<a href="{CompanyUrl}" style="color:#2a5ba0 !important; font-size:13px; text-decoration:underline;">
                    <span style="color:#2a5ba0">
										  <xsl:value-of select="CompanyUrl"/>
                    </span>
									</a>
								</span>
							</td>
						</tr>
					</table>
			</body>
		</html>

	</xsl:template>
</xsl:stylesheet>