using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    public interface ICarParameters
    {
        string Brand { get; set; }
        string Model { get; set; }
        string Modification { get; set; }
        string EngineNumber { get; set; }
        EngineType EngineType { get; set; }
        short? EngineCCM { get; set; }
        short? EngineHP { get; set; }
        BodyType BodyType { get; set; }
        short Year { get; set; }
        byte? Month { get; set; }
        string VIN { get; set; }
        string Frame { get; set; }
        TransmissionType TransmissionType { get; set; }
        string TransmissionNumber { get; set; }
    }

    public static class ICarParametersExt
    {
        public static string GetMonthAndYear(this ICarParameters param)
        {
            if (param.Month.HasValue)
            {
                return String.Format("{0:00}/{1}", param.Month, param.Year);
            }
            return param.Year.ToString();
        }

        public static string GetFullName(this ICarParameters param)
        {
            if (param == null)
            {
                return null;
            }

			StringBuilder sb = new StringBuilder();

			if( !param.Model.ToUpper().Contains( param.Brand.ToUpper() ) )
				sb.Append( param.Brand );

			if( sb.Length != 0 ) sb.Append( " " );
			sb.Append( param.Model );

			//if( !string.IsNullOrEmpty( param.Modification ) )
			//	sb.AppendFormat( " ({0})", param.Modification );

			sb.AppendFormat( ", {0}", param.Year );

			if( !string.IsNullOrEmpty( param.VIN ) )
				sb.AppendFormat( ", VIN {0}", param.VIN );

			if( !string.IsNullOrEmpty( param.Frame ) )
				sb.AppendFormat( ", Frame {0}", param.Frame );

			return sb.ToString();
        }
    }
}
