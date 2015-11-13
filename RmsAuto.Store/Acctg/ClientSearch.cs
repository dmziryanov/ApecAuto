using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using RmsAuto.Store.Acctg.Entities;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;
using RmsAuto.Store.Web;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Acctg
{
    public enum ClientSearchMatching
    {
        Exact = 0,
        Fuzzy = 1
    }

    public static class ClientSearch
    {
        public static IEnumerable<BriefClientInfo> Search(
            string clientName, 
            string mainPhone,
            ClientSearchMatching matching)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {

                List<BriefClientInfo> ret = new List<BriefClientInfo>();
                if (!LightBO.IsLight())
                {
                    var rezSearch = dc.DataContext.spSelUsersByFields(clientName, "", mainPhone);
                    
                    foreach (var oneRez in rezSearch)
                    {
                        ret.Add(new BriefClientInfo()
                        {
                            ClientID = oneRez.AcctgID,
                            ClientName = oneRez.ClientName,
                          //  MainPhone = oneRez.ContactPhone
                        });
                    }
                }
				else
                {
                    dc.DataContext.Log = new DebugTextWriter();
                    string query = @"select a.UserID as UserID,  AcctgID as ClientID,  ManagerId , ClientName as ClientName, ContactPhone as MainPhone, TradingVolume as TradingVolume, CreationTime, ISNULL(b.isAutoOrder, 0) as isAutoOrder,  isChecked from dbo.Users a LEFT JOIN dbo.UserSettings b ON a.UserID = b.UserID where ClientName like {0} and ContactPhone like {1} and InternalFranchName = {2} AND UserRole = 0";
					
                    return dc.DataContext.ExecuteQuery<BriefClientInfo>(query, "%"+clientName+"%", "%"+mainPhone+"%", SiteContext.Current.InternalFranchName).ToList();
                }
                return ret;
            }
        }


        public static IEnumerable<BriefClientInfo> LiteClientSearch(
          string clientName,
          string mainPhone,
          ClientSearchMatching matching, string isAutoOrder, string isChecked, string TradingVolume, string RegDateMin, string RegDateMax, string ManagerID)
        {
            var signs = new string[3] { ">=", "=", "=" };

            using (var dc = new DCFactory<StoreDataContext>(false))
            {
                dc.SetUnCommit();
                IEnumerable<BriefClientInfo> rezSearch;

                dc.DataContext.Log = new DebugTextWriter(); //включить логгер

                
                    rezSearch = dc.DataContext.ExecuteQuery<BriefClientInfo>(@"select a.AcctgID as ClientID, a.ManagerId , a.ClientName, a.ContactPhone as MainPhone, isChecked, TradingVolume, ISNULL(b.isAutoOrder, 0) as isAutoOrder, a.CreationTime as CreationTime  from dbo.users a LEFT JOIN dbo.UserSettings b ON a.UserID = b.UserID WHERE clientname like '%" + clientName + "%' and ContactPhone like '%" + mainPhone + "%' and InternalFranchName = '" + SiteContext.Current.InternalFranchName + @"' and
                    ISNULL(b.isAutoOrder, 0) " + signs[int.Parse(isAutoOrder)] + (int.Parse(isAutoOrder) - 1) + " and CAST(a.isChecked as int) " + signs[int.Parse(isChecked)+1] + " {0} and a.TradingVolume " + signs[int.Parse(TradingVolume)] + " {1} AND UserRole = 0 " + RegDateMin + RegDateMax + ManagerID, isChecked, (int.Parse(TradingVolume) - 1));

                return rezSearch.ToList();
            }
        }

        public static IEnumerable<BriefClientInfo> Search(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("email cannot be empty", "email");
            
			using (var dc = new DCFactory<StoreDataContext>())
            {
                var rezSearch = dc.DataContext.spSelUsersByFields("", email, "");
                List<BriefClientInfo> ret = new List<BriefClientInfo>();
                foreach (var oneRez in rezSearch)
                {
                    ret.Add(new BriefClientInfo()
                    {
                        ClientID = oneRez.AcctgID,
                        ClientName = oneRez.ClientName,
                        //MainPhone = oneRez.ContactPhone
                    });
                }
                return ret;
            }
        }
    }
}
