using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RmsAuto.Store.Adm.Catalogs
{
	/*
	 * Служебные ID-шники для картинок "nophoto", которые назначаются в каталогах
	 * в случае если нет картинки. Файлы с данными ID обязаны быть в файлах
	 * т.е. должны существовать ~/Files/1.ashx, ~/Files/2.ashx, ~/Files/3.ashx и т.д.
	 */
	public enum NoPhotoID : int
	{
		Tire = 1,
		Disc = 2,
		Battery = 3
	}
}
