using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Web;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Dac
{
	public static class UserDac
	{
		private static Func<StoreDataContext, string, string, IQueryable<User>> _getUsers =
			CompiledQuery.Compile(
			( StoreDataContext dc, string login, string password ) =>
				from user in dc.Users
				where user.Username == login && user.Password == password
				select user );

        private static Func<StoreDataContext, int, IQueryable<User>> _getUsersByUserId =
                    CompiledQuery.Compile(
                    (StoreDataContext dc, int userId) =>
                        from user in dc.Users
                        where user.UserID == userId && user.Role == SecurityRole.Client
                        select user);

		private static Func<StoreDataContext, string, IQueryable<User>> _getUsersByClientId =
			CompiledQuery.Compile(
			( StoreDataContext dc, string clientId ) =>
				from user in dc.Users
				where user.AcctgID == clientId && user.Role == SecurityRole.Client
				select user );

		private static Func<StoreDataContext, string, IQueryable<User>> _getUsersByUsername =
			CompiledQuery.Compile(
			( StoreDataContext dc, string username ) =>
				from user in dc.Users
				where user.Username == username
				select user );

        private static Func<StoreDataContext, int, IQueryable<string>> _getAcctgIDByUserID =
            CompiledQuery.Compile(
            (StoreDataContext dc, int userID) =>
                from user in dc.Users
                where user.UserID == userID
                select user.AcctgID);

		public static User GetUser(string login, string password)
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
                if (LightBO.IsLight())
                {
                    return _getUsers(dc.DataContext, login, password).Where(x => x.InternalFranchName == SiteContext.Current.InternalFranchName).SingleOrDefault();
                }
                else
                {
                    return _getUsers(dc.DataContext, login, password).SingleOrDefault();
                }
			}
		}

        public static User GetUserByUserId(int userId)
        {
            using (DCFactory<StoreDataContext> dc = new DCFactory<StoreDataContext>())
            {
                return _getUsersByUserId(dc.DataContext, userId).SingleOrDefault();
            }
        }

		public static User GetUserByClientId( string clientId )
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				return _getUsersByClientId( dc.DataContext, clientId ).SingleOrDefault();
			}
		}

		public static User GetUserByUsername( string username )
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				return _getUsersByUsername( dc.DataContext, username ).SingleOrDefault();
			}
		}

        public static string GetAcctgIDByUserID(int userID)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                return _getAcctgIDByUserID(dc.DataContext, userID).SingleOrDefault();
            }
        }

		internal static void AddUser( User user, StoreDataContext context )
		{
			context.Users.InsertOnSubmit( user );
			
			//создать запись для сервиса отправки оповещений
			if( user.Role == SecurityRole.Client )
			{
				var alert = context.ClientAlertInfos.SingleOrDefault( a => a.ClientID == user.AcctgID );
				if( alert == null )
					context.ClientAlertInfos.InsertOnSubmit( new ClientAlertInfo { ClientID = user.AcctgID, OrderTrackingLastAlertDate = DateTime.Now } );
				else
					alert.OrderTrackingLastAlertDate = DateTime.Now;
			}

			context.SubmitChanges();
		}

		internal static void UpdatePassword( int userId, string password, StoreDataContext context )
		{
			context.Users
					.Single( u => u.UserID == userId )
					.Password = password;
			context.SubmitChanges();
		}
	}
}
