using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using RmsAuto.Common.Configuration;

namespace RmsAuto.Store.Configuration
{
    public class LoggerConfiguration : ConfigurationSectionSingleton<LoggerConfiguration>
    {
        public override string SectionName
        {
            get { return "rmsauto.logger"; }
        }
        
        [ConfigurationProperty( "work", IsRequired = true )]
        public string LogWork
        {
            get
            {
                return TryGet<string>( "work" );
            }
        }

        [ConfigurationProperty( "logName", IsRequired = true )]
        public string LogName
        {
            get
            {
                return TryGet<string>( "logName" );
            }
        }

        [ConfigurationProperty( "applicationName", IsRequired = true )]
        public string ApplicationName
        {
            get
            {
                return TryGet<string>( "applicationName" );
            }
        }

        [ConfigurationProperty( "machineName", IsRequired = false )]
        public string MachineName
        {
            get
            {
                return string.IsNullOrEmpty( TryGet<string>( "machineName" ) ) ? "." : TryGet<string>( "machineName" );
            }
        }

        [ConfigurationProperty( "resourceFile", IsRequired = false )]
        public string ResourceFile
        {
            get
            {
                return string.IsNullOrEmpty( TryGet<string>( "resourceFile" ) ) ? "" : TryGet<string>( "resourceFile" );
            }
        }

        [ConfigurationProperty( "categoryCount", IsRequired = false )]
        public int CategoryCount
        {
            get
            {
                return TryGet<int>( "categoryCount" );
            }
        }

    }
}
