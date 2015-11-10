<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="ReclamationTrackingAlert">

			<html>
				<head>
					<title>«<xsl:value-of select="CompanyName"/>» Changing of the reclamation status</title>
					<style>
						body,div,table,tr,td,p,ul,li,span, option, select,form, input, option {
						font-family: 'Trebuchet MS';
						font-size: 13px;
						color:#000000;
						}

						.list2 {
						margin: 0px 0px; border-collapse: collapse;
						border-top:2px solid #c9e2f1;
						clear:both;
						}
						.list2 th {
						border-right: #e6f5ff 2px solid;
						padding-right: 6px; padding-left: 6px; font-weight: normal; padding-bottom: 3px; color: #78868d; padding-top: 3px;
						background-color: #e9f4ff;
						text-align: left;
						border-top:2px solid #c9e2f1;
						border-left: #ffffff 1px dashed;
						border-right: #e6f5ff 0px solid;
						}
						.list2 td {
						padding-right: 6px; padding-left: 6px; font-weight: normal; padding-bottom: 6px;
						vertical-align:middle;
						padding-top: 6px;
						border-bottom: #cecece 1px dashed; background-color: #ffffff; text-align: left;
						border-left: #e9f4ff 1px dashed;
						}
						.list2 td img.str
						{
						margin-top:-7px;
						}
						.list2 tr.row0 {
						background-color:#F0F9FF;
						}
						.list2 tr.row0 td {
						padding-right: 6px; padding-left: 6px; font-weight: normal; padding-bottom: 2px; vertical-align:middle; padding-top: 2px;
						border-bottom: #cecece 1px dashed;
						text-align: left;
						opacity: 0.4;
						background-color:#F0F9FF;
						}

						td.price {
						color: #a42735;
						/*text-align: right*/
						}
						/*tr.old td.price {
						color: #CCA3AE;
						text-align: right
						}*/

						span.parent_info {
						color:red;
						cursor:pointer;
						}
					</style>
				</head>
				<body>
					<div>
						Dear Customer!<br />
						There are following statuses changes in your reclamations:<br /><br/>

						<table class="list2" cellspacing="0" cellpadding="0" width="100%" style="border-collapse:collapse;clear:both;">
							<tr>
								<th>Claim №</th>
								<th>Type of claim</th>
								<th>Date of claim</th>
								<th>Manufacturer</th>
								<th>Part number</th>
								<th>Description</th>
								<th>Quantity</th>
								<th>Request status</th>
								<th>Status date</th>
							</tr>
							<xsl:for-each select="Reclamations/Reclamation">
								<tr class="{concat('row',(position() mod 2))}">
									<td>
										<xsl:value-of select="ReclamationNumber"/>
									</td>
									<td>
										<xsl:value-of select="ReclamationType"/>
									</td>
									<td>
										<xsl:value-of select="ReclamationDate"/>
									</td>
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
									<td>
										<xsl:value-of select="CurrentStatus"/>
									</td>
									<td>
										<xsl:value-of select="CurrentStatusDate"/>
									</td>
								</tr>
							</xsl:for-each>
						</table>

						<div id="BlockMailFooter" >
							<xsl:value-of select="BlockMailFooter" disable-output-escaping="yes"/>
						</div>
						<br />

						<table>
							<tr>
								<td style="font-size:12.0pt; font-family:Arial; color:#000;">
									Best regards,<br />
									<xsl:value-of select="CompanyName"/> Team
									<br /><br />
									<img src="http://www.apecauto.com/images/apec_logo.jpg" border="0" width="160" height="49" alt="APEC logo" />
									<br /><br />
									<span style="font-size:14.0pt; font-family:Arial; color:#2a5ba0;">
										Tel. <xsl:value-of select="Phone"/><br />
										<a href="{CompanyUrl}" style="color:#2a5ba0; font-size:14.0pt; text-decoration:none;">
											<xsl:value-of select="CompanyUrl"/>
										</a>
									</span>
								</td>
							</tr>
						</table>
						
					</div>
				</body>
			</html>
        
    </xsl:template>
</xsl:stylesheet>
