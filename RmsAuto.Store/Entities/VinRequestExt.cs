using System;

namespace RmsAuto.Store.Entities
{
    [Serializable]
    public partial class VinRequest : ICarParameters
    {
		partial void OnVINChanged()
		{
			if( !string.IsNullOrEmpty( _VIN ) )
				_VIN = _VIN.ToUpper();
		}
    }
}
