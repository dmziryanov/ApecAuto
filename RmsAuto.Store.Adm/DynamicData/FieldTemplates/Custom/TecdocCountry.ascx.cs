using System;
using System.Web.DynamicData;
using System.Web.UI;

namespace RmsAuto.Store.Adm.DynamicData.FieldTemplates
{
	public partial class TecdocCountry : FieldTemplateUserControl
	{
		public override Control DataControl
		{
			get
			{
				return Literal1;
			}
		}

		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);
			//if (FieldValue != null)
				//Literal1.Text = TechDoc.Facade.GetCountryById((int)FieldValue).Name.Text;
		}
	}
}
