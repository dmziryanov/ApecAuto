<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="ResetPasswordAlert">

    <html>
      <head>
        <title>«<xsl:value-of select="CompanyName"/>». Request to change account password on web-site <xsl:value-of select="SiteUrl"/>.</title>
      </head>
      <body>
        <p>
					We are glad that you joined our company!
				</p>
        <hr/>

        <p>
					Dear <xsl:value-of select="ClientFullName" />,
        </p>

        <p>
					You have requested the password change on web-site <xsl:value-of select="CompanyUrl"/></p>

        <p>Your registration info:</p>

        <p>Username: <xsl:value-of select="ClientLogin" /></p>

        <p>To change the password, follow the link:</p>

        <a href="{MaintUrl}">
          <xsl:value-of select="MaintUrl"/>
        </a>
        <hr/>

        <xsl:if test="DaysToLive[. &gt; 0]">
          <p>
            <b>
							Attention! The link is valid within <xsl:value-of select="DaysToLive" /> day.
						</b>
          </p>
        </xsl:if>

        <p>The message is generated automatically.</p>
        <br/>
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
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
