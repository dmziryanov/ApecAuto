using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RmsAuto.Store.Acctg;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Web;

namespace RmsAuto.Store.GlobalDBSettings
{
	public class GlobalSettings
	{
		/// <summary>
		/// internal settings
		/// </summary>
		private List<GlobalSetting> settings { get; set; }
		/// <summary>
		/// flag to demote already initialized
		/// </summary>
		private bool initialized;

		/// <summary>
		/// Constructor
		/// </summary>
		public GlobalSettings()
		{
			initialized = false;
		}

		/// <summary>
		/// Initializes the class
		/// </summary>
		public void Init()
		{
			if (!initialized)
			{
				RetrieveAllSettings();
			}
			initialized = true;
		}

		/// <summary>
		/// Loads all the settings then add the Current Settings class to the Http Application State
		/// </summary>
		private void RetrieveAllSettings()
		{
            if (HttpContext.Current != null)
            {
                List<String> FranchNames = AcctgRefCatalog.RmsFranches.Items.Select(x => x.InternalFranchName).ToList();
                foreach (string s in FranchNames)
                {
                    var GS = new GlobalSettings();
					using (var dc = new DCWrappersFactory<dcCommonDataContext>(s))
					{
						GS.settings = new List<GlobalSetting>();
						GS.initialized = true;
						HttpContext.Current.Application["__globalDBSettings" + s] = GS;
					}
                }
            }
		}

		/// <summary>
		/// Gets a setting By the Name and returns the Object Setting
		/// </summary>
		/// <param name="Name">name of setting</param>
		/// <returns>setting</returns>
		public GlobalSetting GetSettingByName(string Name)
		{
			if (!initialized || settings.Count == 0) { RetrieveAllSettings(); }
			return settings.Where(x => x.Name == Name).FirstOrDefault();
		}

		/// <summary>
		/// Gets a setting by the Setting ID and returns the Setting object
		/// </summary>
		/// <param name="SettingID">id of setting</param>
		/// <returns>setting</returns>
		public GlobalSetting GetSettingByID(int SettingID)
		{
			if (!initialized || settings.Count == 0) { RetrieveAllSettings(); }
			return settings.Where(x => x.SettingID == SettingID).FirstOrDefault();
		}

		/// <summary>
		/// Creates a setting By Name & Value
		/// </summary>
		/// <param name="Name">name of setting</param>
		/// <param name="Value">value of setting</param>
		/// <remarks>
		/// Calls CreateSetting(Name,Value,Description) with an empty description
		/// </remarks>
		public void CreateSetting(string Name, string Value)
		{
			CreateSetting(Name, Value, string.Empty);
		}

		/// <summary>
		/// Creates a setting with Name, Value & Description
		/// </summary>
		/// <param name="Name">name of setting</param>
		/// <param name="Value">value of setting</param>
		/// <param name="Description">description of setting</param>
		public void CreateSetting(string Name, string Value, string Description)
		{
			if (GetSettingByName(Name) != null)
			{
				UpdateSetting(Name, Value);
				return;
			}
			using (var dc = (new DCWrappersFactory<dcCommonDataContext>()).DataContext)
			{
				var setting = new GlobalSetting();
				setting.Name = Name;
				setting.Value = Value;
				setting.Description = Description;
				dc.GlobalSettings.InsertOnSubmit(setting);
				dc.SubmitChanges();
			}
			RetrieveAllSettings();
		}

		/// <summary>
		/// Updates a setting, located the setting by name, then sets the Value to Value
		/// </summary>
		/// <param name="Name">name of setting</param>
		/// <param name="Value">value of setting</param>
		public void UpdateSetting(string Name, string Value)
		{
			if (GetSettingByName(Name) == null)
			{
				CreateSetting(Name, Value);
				return;
			}
			else
			{
				using (var dc = (new DCWrappersFactory<dcCommonDataContext>()).DataContext)
				{
					var setting = dc.GlobalSettings.SingleOrDefault(x => x.Name == Name);
					if (setting == null)
						CreateSetting(Name, Value);
					setting.Value = Value;
					dc.SubmitChanges();
				}
				RetrieveAllSettings();
			}
		}


		/// <summary>
		/// Resolves the Current Settings in the HTTP Context Application
		/// </summary>
		public static GlobalSettings Current
		{
			get
			{
				if (HttpContext.Current != null)
					return (GlobalSettings)HttpContext.Current.Application["__globalDBSettings"+SiteContext.Current.InternalFranchName];
				return null;
			}
		}
	}
}
