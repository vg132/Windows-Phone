using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace SLRealtid
{
	public class Settings
	{
		private static IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

		private const string AppVersionKey = "AppVersion";
		private const string ShowDepartureHelpKey = "DepartureHelpShownKey";
		private const string ShowFilterHelpKey = "FilterHelpShownKey";
		private const string ShowSearchHelpKey = "ShowSearchHelpKey";

		public static bool ShowSearchHelp
		{
			get { return GetSetting(Settings.ShowSearchHelpKey, true); }
			set { SetSetting(Settings.ShowSearchHelpKey, value); }
		}

		public static decimal AppVersion
		{
			get { return GetSetting<decimal>(Settings.AppVersionKey, -1); }
			set { SetSetting<decimal>(Settings.AppVersionKey, value); }
		}

		public static bool ShowDepartureHelp
		{
			get { return GetSetting(Settings.ShowDepartureHelpKey, true); }
			set { SetSetting(Settings.ShowDepartureHelpKey, value); }
		}

		public static bool ShowFilterHelp
		{
			get { return GetSetting(Settings.ShowFilterHelpKey, true); }
			set { SetSetting(Settings.ShowFilterHelpKey, value); }
		}

		private static void SetSetting<T>(string key, T value)
		{
			if (appSettings.Contains(key))
			{
				appSettings[key] = value;
			}
			else
			{
				appSettings.Add(key, value);
			}
		}

		private static T GetSetting<T>(string key, T fallback)
		{
			if (appSettings.Contains(key))
			{
				return (T)appSettings[key];
			}
			return fallback;
		}
	}
}
