<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="OfferAlert">
			<html>
				<head>
					<title>Предложение поставщика</title>
				</head>
        <body style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">
					<p>Поступило следующее предложение от поставщика:</p>
					  Наименование: <xsl:value-of select="OfferTitle"/>
					<br />
					  Тема: <xsl:value-of select="OfferSubject"/>
					<br />
					  Предложение: <xsl:value-of select="OfferBody"/>
          <hr/>
					<table>
						<tr>
							<td style="font-size:12.0pt; font-family:Arial; color:#000;">
								Best regards,<br />
								<xsl:value-of select="CompanyName"/> Team
								<br /><br />
								<img src="http://www.spare-auto.com/images/apec_logo.jpg" border="0" width="160" height="49" alt="logo" />
								<br /><br />
								<span style="font-size:14.0pt; font-family:Arial; color:#002060;">
									Tel. <xsl:value-of select="Phone"/><br />
									<a href="{CompanyUrl}" style="color:#002060; font-size:14.0pt; text-decoration:none;">
										<xsl:value-of select="CompanyUrl"/>
									</a>
								</span>
							</td>
						</tr>
					</table>
				</body>
			</html>
    </xsl:template>
</xsl:stylesheet>
