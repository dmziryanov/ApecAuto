<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="FeedbackAlert">
    <html>
      <head>
        <title>«<xsl:value-of select="CompanyName"/>» The feedback</title>
      </head>
      <body style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">
				The Customer: <xsl:value-of select="ClientName" />
        <br/>
        <br/>
				The message: <xsl:value-of select="Message" />
        <table>
          <tr>
            <td style="font-family: Arial, Helvetica, sans-serif;font-size: 12pt;color:#000000;">
              Best regards,<br />
              <xsl:value-of select="CompanyName"/> Team
              <br /><br />
              <img src="http://www.spare-auto.com/images/apec_logo.jpg" border="0" width="160" height="49" alt="  logo" />
              <br /><br />
              <span style="font-size:14.0pt; font-family:Arial; color:#2a5ba0;">
                Tel. <xsl:value-of select="Phone"/><br />
                <a href="{CompanyUrl}" style="color:#2a5ba0 !important; font-size:14.0pt; text-decoration:none;">
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
