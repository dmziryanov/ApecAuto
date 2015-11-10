extern alias DataStreams1; //to make sure it is DataStreams1.dll, reference alias is set to DataStreams1

using DataStreams1.DataStreams.Csv;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
	class PricesImporter : ImporterBase
	{
		private int? _supplierId = null;
		private PricesCleaner _priceCleaner;
       

		public PricesImporter(ImportSettings settings, ImportMode mode) : base(settings, mode)
		{
			_priceCleaner = new PricesCleaner(settings);
		}

      

		protected override void OnValidRecord( ValidateRecordEventArgs e )
		{
			if( _Activity.Mode != ImportMode.Smart )
			{
				int supplierId = (int)e.Record[ "SupplierID" ];
				string partName = (string)e.Record[ "PartName" ];
				string partNumber = (string)e.Record[ "PartNumber" ];

				if ( !_supplierId.HasValue )
				{

                    //Прайсклинер тоже ставит лок
                    _priceCleaner.ClearData(supplierId, _CurrentReport.Counters);
					_supplierId = supplierId;

                    //Проверка в списке собственных складов
                    var cnt = LockerManager.CheckOwnStore(supplierId, _Connection);

                    //Если поставщик находится в собственных складах наличия то пытаемся получить лок на базу данных, если не получаем то вываливаемся с исключением
                    if (cnt > 0 && LockerManager.AquireCheckLock(_Connection) < 0)
                    {
                        //Если здесь вываливаемся, то попадаем 
                        throw new Exception();
                    }
				}

				if (partName == "\u0000")
					e.AddValidationErrorDetail(new CustomValidationErrorDetail( "PartName - некорректное значение" ));

				if( partNumber == "\u0000" )
					e.AddValidationErrorDetail(new CustomValidationErrorDetail("PartNumber - некорректное значение"));

				//импорт прайс-листов: ид поставщика должен быть у всех одинаковый
				if( _supplierId != supplierId )
				{
					e.AddValidationErrorDetail(new CustomValidationErrorDetail( "SupplierID - не совпадает с первой строкой файла"));
				}
			}
		}
	}
}
