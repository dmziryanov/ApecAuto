<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="ClientActivationAlertWhSl">

		<html>
			<head>
				<title>New user confirmation of registration</title>
			</head>
      <body style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">

        <p style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">We are glad that you joined <xsl:value-of select="CompanyName"/>!</p>
        <hr/>

        <p style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">Hello!</p>

        <p style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">You received this message automatically as your e-mail was mentioned at the registration of a new user at the server <xsl:value-of select="CompanyUrl"/>.</p>

        <p style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">To confirm the registration follow the link below:</p>

        <a href="{ActivationUrl}" style="color:#2a5ba0 !important;font-family: Arial, Helvetica, sans-serif; font-size:13px; text-decoration:none;">
          <span style="color:#2a5ba0">
            <xsl:value-of select="ActivationUrl"/>
          </span>
        </a>

        <p style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">Attention! Your account won’t be available until the registration is completed.</p>

        <xsl:if test="DaysToLive[. &gt; 0]">
          <p style="font-family: Arial, Helvetica, sans-serif;font-size: 13px;color:#000000;">
            <b>The link is valid within  <xsl:value-of select="DaysToLive" /> days.</b>
          </p>
        </xsl:if>
        <hr/>

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
