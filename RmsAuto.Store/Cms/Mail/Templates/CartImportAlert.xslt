<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="CartImportAlert">
    <html>
      <head>
        <title><xsl:value-of select="ClientToHeader" /> customer: uploaded results via <xsl:value-of select="CompanyUrl"/></title>
      </head>
      <body>
        <p>
          Dear <xsl:value-of select="CompanyName"/> customer!
        </p>
        <p>
					You have received this e-mail because you pointed out that you would like to receive all the results of uploaded orders at the web-site  <xsl:value-of select="CompanyUrl"/>.
          <br />
					Some of uploaded lines are incorrect or need to be confirmed.
				</p>
        <p>
					Please find attached the file with indicated errors. You may revise it and try again to upload.
				</p>
				<table>
					<tr>
						<td style="font-size:12.0pt; font-family:Arial; color:#000;">
							Best regards,<br />
							<xsl:value-of select="CompanyName"/> Team
							<br /><br />
							<img src="http://www.spare-auto.com/images/apec_logo.jpg" border="0" width="160" height="49" alt="  logo" />
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
