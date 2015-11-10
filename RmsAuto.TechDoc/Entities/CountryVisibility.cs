using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Caching;
using RmsAuto.Common.DataAnnotations;

namespace RmsAuto.TechDoc.Entities
{
	[ScaffoldTable(true)]
	[MetadataType(typeof(CountryVisibilityMetadata))]
	public partial class CountryVisibility
	{
		partial void OnValidate(ChangeAction action)
		{
			if (action == ChangeAction.Insert)
			{
				using (var dc = new TecdocStoreDataContext())
				{
					if (dc.CountryVisibilities.SingleOrDefault(
						 c => c.CountryID == CountryID) != null)
						throw new ValidationException("эта страна уже есть в списке видимых");
				}
			}
			if (HttpContext.Current.Cache["visibleCountries"] != null)
				HttpContext.Current.Cache.Remove("visibleCountries");
		}
	}

	[DisplayName("TecDocs - видимость стран/регионов")]
    [Crud(CrudActions.Create | CrudActions.Delete)]
	public partial class CountryVisibilityMetadata
	{
		[DisplayName("Название")]
		[Required(ErrorMessage = "не задана страна или регион")]
		[UIHint( "Custom/TecdocCountry" )]
		public object CountryID { get; set; }
	}
}
