using System;
using System.Linq;
using System.Collections.Generic;
using RmsAuto.Store.Acctg;
using RmsAuto.Common.Configuration;
using System.Configuration;
using RmsAuto.Acctg;

namespace RmsAuto.Store.Configuration
{
	public class DiscountGroupsConfiguration : ConfigurationSectionSingleton<DiscountGroupsConfiguration>
    {
        public override string SectionName
        {
            get { return "rmsauto.discountGroups"; }
        }

        [ConfigurationProperty("discountGroup1", IsRequired = true)]
        public RmsAuto.Acctg.ClientGroup DiscountGroup1
        {
            get
            {
				return TryGet<ClientGroup>( "discountGroup1" );
            }
        }

		[ConfigurationProperty( "discountGroup2", IsRequired = true )]
        public ClientGroup DiscountGroup2
		{
			get
			{
                return TryGet<ClientGroup>("discountGroup2");
			}
		}

		[ConfigurationProperty( "discountGroup3", IsRequired = true )]
		public ClientGroup DiscountGroup3
		{
			get
			{
				return TryGet<ClientGroup>( "discountGroup3" );
			}
		}

		[ConfigurationProperty( "discountName1", IsRequired = true )]
		public string DiscountName1
		{
			get
			{
				return TryGet<string>( "discountName1" );
			}
		}

		[ConfigurationProperty( "discountName2", IsRequired = true )]
		public string DiscountName2
		{
			get
			{
				return TryGet<string>( "discountName2" );
			}
		}

		[ConfigurationProperty( "discountName3", IsRequired = true )]
		public string DiscountName3
		{
			get
			{
				return TryGet<string>( "discountName3" );
			}
		}

	}

}
