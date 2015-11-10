using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    public partial class UserGarageCar : ICarParameters
    {
		partial void OnVINChanged()
		{
			if( !string.IsNullOrEmpty( _VIN ) )
				_VIN = _VIN.ToUpper();			
		}
    }
}
