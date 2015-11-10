using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using System.Security.Principal;

using RmsAuto.Common.Misc;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web
{
    public static class LogonService
    {
        public static User Logon(
            string login,
            string password,
            bool isPersistent,
            out string errMessage)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentException("Login cannot be empty", "login");
            if (password == null)
                throw new ArgumentNullException("password");

            var user = UserDac.GetUser(login, password.GetMD5Hash());

            if (user != null)
            {
                if ( string.IsNullOrEmpty( user.AcctgID ) )
                {
					errMessage = "LogonUserIsNotActivated";//"Пользователь не активирован";
                    return null;
                }
                else
                {
                    CheckUserProfile( user );

                    SetAuthCookie( login, user.UserID, user.AcctgID, user.Role, user.InternalFranchName );
                    errMessage = "";

                    return user;
                }
            }
			errMessage = "LogonWrongLoginOrPassword";//"Неверный логин или пароль";
            return null;
        }

		static void CheckUserProfile( User user )
		{
			if( user.Role == SecurityRole.Client )
			{
				//проверить, что профиль подгружается
				var profile = ClientProfile.Load( user.AcctgID );
			}
			else if( user.Role == SecurityRole.Manager )
			{
				//проверить, что в справочнике сотрудников есть такой сотрудник
				//var empInfo = AcctgRefCatalog.RmsEmployees[ user.AcctgID ];
				//if( empInfo == null )
				//	throw new BLException( "EmployeesRef doesn't contain record for EmployeeID: " + user.AcctgID );
			}
			else
			{
				throw new InvalidOperationException("Unknown user role");
			}
		}

		public static void RemoveAuthCookie()
		{
			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
			cookie.Expires = DateTime.Now.AddDays(-1);
			HttpContext.Current.Response.Cookies.Set(cookie);
			return;
		}

        public static void Logoff()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
			RemoveAuthCookie();
			HttpContext.Current.Response.Redirect( "~/Default.aspx", true );
        }

        public static void FormsAuthentication_OnAuthenticate(
            object sender,
            FormsAuthenticationEventArgs args)
        {
            if (FormsAuthentication.CookiesSupported)
            {
                HttpRequest request = HttpContext.Current.Request;
                if (request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(
                       request.Cookies[FormsAuthentication.FormsCookieName].Value);
                    
                    string[] parts = ticket.UserData.Split(';');

                    int userId = int.Parse(parts[0]);
                    string acctgId = parts[1];
                    byte bRole = byte.Parse(parts[2]);
                    string  internalFranchName = parts[3];
                    
                    if (!Enum.IsDefined(typeof(SecurityRole), bRole))
                        throw new Exception("Incorrect Users.UserRole value");


                    //
                    if (HttpContext.Current.Request.Cookies["InternalFranchName"] == null || HttpContext.Current.Request.Cookies["InternalFranchName"].Value != internalFranchName)
                    {
                        //SiteContext._internalFranchName = internalFranchName;
                        HttpCookie coockie = new HttpCookie("InternalFranchName");
                        coockie.Domain = "rmsauto.ru";
                        coockie.Path = "/";
                        coockie.Value = HttpUtility.HtmlEncode(internalFranchName);
                        HttpContext.Current.Request.Cookies.Add(coockie);
                        HttpContext.Current.Response.Cookies.Add(coockie);


                        HttpCookie CityNamecoockie = new HttpCookie("cityName");
                        CityNamecoockie.Domain = "rmsauto.ru";
                        CityNamecoockie.Path = "/";
                        IEnumerable<City> cities;
                        using (var dcCommon = new RmsAuto.Store.Entities.dcCommonDataContext())
                        {
                                //Извлекаем наборы данных в списки, так как LINQ to SQL не дает выполнять запросы к различным контекстам
                                //TODO: сделать AcctgRefCatalog.Cities, вынести в справочник, чтобы не лезть в базу каждый раз
                                cities = dcCommon.Cities.Select(x => x).ToList();
                        }
                        //var regionId = AcctgRefCatalog.RmsFranches[(string)context.Request.QueryString[UrlKeys.Activation.FranchCode]].RegionID;
                        var regionId = AcctgRefCatalog.RmsFranches[internalFranchName].RegionID;
                        CityNamecoockie.Value = HttpUtility.UrlEncodeUnicode(cities.Where(x => x.CityID == regionId).Select(x => x.Name).FirstOrDefault());
                        HttpContext.Current.Request.Cookies.Add(CityNamecoockie);
                        HttpContext.Current.Response.Cookies.Add(CityNamecoockie);
                    }
                    
                    
                    

                    SecurityRole role = (SecurityRole)bRole;
                    var user = new CustomPrincipal(
                          new FormsIdentity(ticket),
                          new string[] { role.ToString() },
                          userId, 
                          acctgId,
                          role, internalFranchName);
                    args.User = user;
                }
            }
            else
            {
				//DO NOTHING
				//throw new HttpException( "Cookieless Forms Authentication is not " +
				//                        "supported for this application." );
            }
        }

        private static void SetAuthCookie(
            string username,
            int userId,
            string acctgId,
            SecurityRole role,
            string InternalFranchName)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
               1,
               username,
               DateTime.Now,
               DateTime.Now.AddMinutes(AuthenticationConfig.Forms.Timeout.TotalMinutes),
               false,
               string.Format("{0};{1};{2};{3}", userId, acctgId, ((byte)role).ToString(), InternalFranchName));

			HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
			HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);

            HttpContext.Current.Response.Cookies.Add(
                new HttpCookie(
                    FormsAuthentication.FormsCookieName,
                    FormsAuthentication.Encrypt(ticket)));
        }
    }
}
