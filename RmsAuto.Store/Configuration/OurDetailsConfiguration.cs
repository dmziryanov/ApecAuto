using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Common.Configuration;
using System.Configuration;

namespace RmsAuto.Store.Configuration
{
	public class OurDetailsConfiguration : ConfigurationSectionSingleton<OurDetailsConfiguration>
	{
		public override string SectionName
		{
			get { return "rmsauto.ourDetails"; }
		}

		[ConfigurationProperty( "CompanyName", IsRequired = true )]
		public string CompanyName
		{
			get { return TryGet<string>( "CompanyName" ); }
		}

		[ConfigurationProperty("INN", IsRequired = true )]
		public string INN
		{
			get { return TryGet<string>("INN"); }
		}

		[ConfigurationProperty( "BankAccount", IsRequired = true )]
		public string BankAccount
		{
			get { return TryGet<string>( "BankAccount" ); }
		}

		[ConfigurationProperty("BankName", IsRequired = true )]
		public string BankName
		{
			get { return TryGet<string>( "BankName" ); }
		}

		[ConfigurationProperty( "BIK", IsRequired = true )]
		public string BIK
		{
			get { return TryGet<string>( "BIK" ); }
		}

		[ConfigurationProperty( "CorrAccount", IsRequired = true )]
		public string CorrAccount
		{
			get { return TryGet<string>( "CorrAccount" ); }
		}
	}
}
