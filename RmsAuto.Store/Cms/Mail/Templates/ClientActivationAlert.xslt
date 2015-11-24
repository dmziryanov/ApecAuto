<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="ClientActivationAlert">

		<html>
			<head>
				<title>Подтверждение регистрации нового пользователя</title>
			</head>
			<body>
        <p>Мы рады приветствовать Вас на сайте компании apecauto.com !</p>
        <hr/>
        <p>Здравствуйте!</p>
        <p>Вы получили это сообщение автоматически, так как Ваш адрес был использован при регистрации нового пользователя на сервере <xsl:value-of select="CompanyUrl"/>.</p>
        <p>Для подтверждения регистрации перейдите по следующей ссылке:</p>

        <a href="{ActivationUrl}">
          <xsl:value-of select="ActivationUrl"/>
        </a>

        <p>Внимание! Ваша учётная запись не будет активной, пока Вы не подтвердите свою регистрацию.</p>

        <xsl:if test="DaysToLive[. &gt; 0]">
        <p>
          <b>Время действия ссылки в днях - <xsl:value-of select="DaysToLive" /></b>
        </p>
        </xsl:if>
        <hr/>
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
