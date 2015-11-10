using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;
using RmsAuto.Store.Dac;

namespace RmsAuto.Store.SupplierSvcs
{
	public class SupplierService : ISupplierService
	{
		#region === samples ===
		//public string GetData(int value)
		//{
		//    /* OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets */

		//    string result = "FUCK";
		//    if (OperationContext.Current != null)
		//    {
		//        if (OperationContext.Current.ServiceSecurityContext != null)
		//        {
		//            if (OperationContext.Current.ServiceSecurityContext.AuthorizationContext != null)
		//            {
		//                if (OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets != null)
		//                {
		//                    result = (OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets[0]).ToString();
		//                }
		//            }
		//        }
		//    }

		//    OperationContext ctx = OperationContext.Current;
		//    try
		//    {
		//        result += Environment.NewLine + string.Format("User: {0}", ctx.ServiceSecurityContext.PrimaryIdentity.Name);
		//    }
		//    catch (Exception)
		//    {

		//        result += Environment.NewLine + "ХУЙ 1";
		//    }
		//    try
		//    {
		//        result += Environment.NewLine + string.Format("ClientCertificate: {}", ctx.Host.Credentials.ClientCertificate.ToString());
		//    }
		//    catch (Exception)
		//    {

		//        result += Environment.NewLine + "ХУЙ 2";
		//    }
			
		//    //OperationContext.Current.ServiceSecurityContext.AuthorizationContext
		//    //System.IdentityModel.Policy.AuthorizationContext ctx; //ctx.ClaimSets.Count.ToString();
		//    //ServiceSecurityContext.Current.
			
		//    //string result = OperationContext.Current.Host.Credentials.ClientCertificate.Certificate.Thumbprint;
		//    return result;

		//    //return string.Format("You entered: {0}", value);
		//}
		#endregion

		#region === sample of Complex type ===
		//public CompositeType GetDataUsingDataContract(CompositeType composite)
		//{
		//    if (composite.BoolValue)
		//    {
		//        composite.StringValue += "Suffix";
		//    }
		//    return composite;
		//}
		#endregion

		public SearchResult GetPriceAndQuantity(string PriceLogo, string DetailNum, string MakeNum)
		{
			// Сопоставление сертификата (сертификат 'замаплен' на доменную учетку) и учетки на сайте
			string acctgId = ServiceAccountDac.GetClientIDByWcfServiceAccount(CurrentIdentityName);
			SearchResult res = null;

			try
			{
				int[] permittedSupplierIds = new int[] { 1201, 1202, 1203 }; // Собственные склады наличия
				ClientProfile profile = ClientProfile.Load(acctgId);
                RmsAuto.Acctg.ClientGroup clientGroup = profile.ClientGroup;
				decimal personalMarkup = profile.PersonalMarkup;

				var parts = PricingSearch.SearchSpareParts(DetailNum, MakeNum, false).Where(p => permittedSupplierIds.Contains(p.SparePart.SupplierID));
				if (parts.Count() > 0)
				{
					res = new SearchResult();

					res.LotQuantity = parts.First().SparePart.MinOrderQty.Value; // MinOrderQty не может быть null
					res.Price = parts.First().SparePart.GetFinalSalePrice(clientGroup, personalMarkup);
					res.Quantity = parts.Sum(p => p.SparePart.QtyInStock.Value); // QtyInStock не может быть null
				}
			}
			catch (Exception)
			{
			}
			
			
			return res;
		}

		private string CurrentIdentityName
		{
			get
			{
				try
				{
					OperationContext ctx = OperationContext.Current;
					return ctx.ServiceSecurityContext.PrimaryIdentity.Name;
				}
				catch (Exception)
				{
					throw;
				}
			}
		}
	}
}

