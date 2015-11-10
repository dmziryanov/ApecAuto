using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using RmsAuto.Common.Misc;

namespace RmsAuto.Store.Entities
{
    public partial class UserMaintEntry
    {
        private ClientRegistrationDataExt _regData;

        public ClientRegistrationDataExt RegistrationData
        {
            get
            {
                if (RegDataBytes == null)
                    return null;
                else
                {
                    if (_regData == null)
                    {
                            if (SecUtil.FromSecureByteArray(RegDataBytes.ToArray(), string.Empty) is ClientRegistrationDataExt){
                                _regData = (ClientRegistrationDataExt)SecUtil.FromSecureByteArray(RegDataBytes.ToArray(), string.Empty);
                            }
                            
                            else
                            {
                                throw new Exception("Тип хранимых данных неверен");                             
                            }

                    }
                    return _regData;
                }
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _regData = value;
                RegDataBytes = SecUtil.ToSecureByteArray(value, string.Empty);
            }
        }

        //deas 29.03.2011 task3563
        // добавлена обработка расширенного профиля
        private ClientRegistrationDataExt _regDataExt;

        public ClientRegistrationDataExt RegistrationDataExt
        {
            get
            {
                if ( RegDataBytes == null )
                    return null;
                else
                {
                    if ( _regDataExt == null )
                        _regDataExt = (ClientRegistrationDataExt)SecUtil
                            .FromSecureByteArray( RegDataBytes.ToArray(), string.Empty );
                    return _regDataExt;
                }
            }
            set
            {
                if ( value == null )
                    throw new ArgumentNullException( "value" );
                _regDataExt = value;
                RegDataBytes = SecUtil.ToSecureByteArray( value, string.Empty );
            }
        }

        partial void OnCreated()
        {
            EntryTime = DateTime.Now;
        }
    }
}
