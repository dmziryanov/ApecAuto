<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="VinRequestAnswerAlert">

		<html>
			<head>
				<title>
          «<xsl:value-of select="CompanyName"/>». Ответ на запрос по VIN</title>
			</head>
			<body>
				<table>
					<tr>
						<th>
							Наименование
						</th>
						<th>
							Количество
						</th>
            <th>
              Комментарий
            </th>
            <th>
							Производитель /<br />Артикул
						</th>
						<th>
							Оригинальный номер
						</th>
            <th>
              Срок поставки
            </th>
            <th>
              Цена за шт.
            </th>
            <th>
							Комментарий менеджера
						</th>
					</tr>
					<xsl:for-each select="Items/Item">
						<tr>
              <td>
                <xsl:value-of select="Name" />
              </td>
              <td>
                <xsl:value-of select="Qty" />
              </td>
              <td>
								<xsl:value-of select="Description" />
							</td>
							<td>
								<xsl:value-of select="Manufacturer" /> /<br />
                <xsl:value-of select="PartNumber" />
							</td>
							<td>
                <a href="{SearchUrl}">
                  <xsl:value-of select="PartNumberOriginal" />
                </a>
							</td>
              <td>
                <xsl:value-of select="DeliveryPeriod" />
              </td>
              <td>
								<xsl:value-of select="PricePerUnit" />
							</td>
							<td>
								<xsl:value-of select="ManagerComment" />
							</td>
						</tr>
					</xsl:for-each>
				</table>

				Сумма по Вашему запросу составила <xsl:value-of select="TotalPrice"/> руб.<br /><br />
				Для просмотра ответа на запрос по VIN пройдите по ссылке:

				<a href="{VinRequestUrl}">
					<xsl:value-of select="VinRequestUrl"/>
				</a>
        <br />
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
