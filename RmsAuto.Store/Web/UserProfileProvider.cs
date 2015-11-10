using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Web.Hosting;

using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    public class UserProfileProvider : ProfileProvider
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(name))
                name = "UserProfileProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom_RmsAuto_User_Profile_provider");
            }
                        
            base.Initialize(name, config);

            ApplicationName = !string.IsNullOrEmpty(config["applicationName"]) ?
                config["applicationName"] : HostingEnvironment.ApplicationVirtualPath;
        }

        public override string ApplicationName
        {
            get; set;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            if (!(bool)context["IsAuthenticated"])
                throw new BL.BLException("UserProfileProvider doesn't allow anonymous profiles");

            var user = UserDac.GetUserByUsername((string)context["UserName"]);
            if (user == null)
                throw new InvalidOperationException(string.Format("User '{0}' doesn't exist", context["UserName"]));

            UserProfileDac.SaveProfile(user.UserID, ToNameValueMap(collection), DateTime.Now);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            if (!(bool)context["IsAuthenticated"])
                throw new BL.BLException("UserProfileProvider doesn't allow anonymous profiles");
            
            
            SettingsPropertyValueCollection svc = new SettingsPropertyValueCollection();
            var entry = UserProfileDac.GetProfileByUsername((string)context["UserName"]);

            foreach (SettingsProperty prop in collection)
            {
                var pv = new SettingsPropertyValue(prop);
                if (entry != null)
                    pv.PropertyValue = entry.NameObjectMap[prop.Name];
                svc.Add(pv);
            }
            if (entry != null)
            {
                UserProfileDac.UpdateActivityTime(entry.UserID, DateTime.Now);
            }
            return svc;
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new NotImplementedException();
        }

        private IDictionary<string, object> ToNameValueMap(SettingsPropertyValueCollection collection)
        {
            var map = new Dictionary<string, object>();
            foreach (SettingsPropertyValue pv in collection)
                 map.Add(pv.Property.Name, pv.PropertyValue);
            return map;
        }
    }
}
