<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="ClientActivationAlertRet">

    <html>
      <head>
        <title>Подтверждение регистрации нового пользователя</title>
      </head>
      <body>
				
        <p>Здравствуйте!</p>
        <p>Благодарим Вас за регистрацию на сайте apecauto.com!</p>

        <p>Вы получили это сообщение автоматически, так как Ваш адрес был использован при регистрации нового пользователя на сервере <xsl:value-of select="CompanyUrl"/>.</p>

        <p>Для активации Вашей учетной записи, пожалуйста перейдите по ссылке:</p>

        <a href="{ActivationUrl}">
          <xsl:value-of select="ActivationUrl"/>
        </a>
        <xsl:if test="DaysToLive[. &gt; 0]">
          <p>
            <b>
              Время действия ссылки в днях - <xsl:value-of select="DaysToLive" />
            </b>
          </p>
        </xsl:if>
        <p>
          В самое ближайшее время, наш менеджер свяжется с Вами,
          для уточнения Ваших данных и подтверждения регистрации.
          С информацией по работе с розничными клиентами,
          Вы можете ознакомиться на нашем сайте в разделе
          <a href="{RetailUrl}">«Розничным клиентам»</a>
        </p>
        <p>
          Будем рады видеть Вас в числе наших постоянных клиентов и надеемся на взаимовыгодное сотрудничество!
        </p>
				<table>
					<tr>
						<td style="font-size:12.0pt; font-family:Arial; color:#000;">
							Best regards,<br />
							<xsl:value-of select="CompanyName"/> Team
							<br /><br />
							<img src="http://www.apecauto.com/images/apec_logo.jpg" border="0" width="160" height="49" alt="APEC logo" />
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
