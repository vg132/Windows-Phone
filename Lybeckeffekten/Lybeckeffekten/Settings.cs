using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Lybeckeffekten
{
	public class Settings
	{
		private static IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

		private const string AppVersionKey = "AppVersion";
		private const string IsRegisterdKey = "IsRegisterdKey";
		private const string ChannelUriKey = "ChannelUriKey";
		private const string ShowNotificationsKey = "ShowNotificationsKey";
		private const string IsFirstStartKey = "IsFirstStartKey";

		public static bool IsFirstStart
		{
			get { return GetSetting<bool>(Settings.IsFirstStartKey, true); }
			set { SetSetting<bool>(Settings.IsFirstStartKey, value); }
		}

		public static decimal AppVersion
		{
			get { return GetSetting<decimal>(Settings.AppVersionKey, -1); }
			set { SetSetting<decimal>(Settings.AppVersionKey, value); }
		}

		public static bool IsRegisterd
		{
			get { return GetSetting<bool>(Settings.IsRegisterdKey, false); }
			set { SetSetting<bool>(Settings.IsRegisterdKey, value); }
		}

		public static string ChannelUri
		{
			get { return GetSetting<string>(Settings.ChannelUriKey, string.Empty); }
			set { SetSetting<string>(Settings.ChannelUriKey, value); }
		}

		public static bool ShowNotifications
		{
			get { return GetSetting<bool>(Settings.ShowNotificationsKey, true); }
			set { SetSetting<bool>(Settings.ShowNotificationsKey, value); }
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