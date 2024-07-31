using Microsoft.Phone.Notification;
using PolisenRSS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PolisenRSS.Utilities
{
	public class ChannelUtility
	{
		private const string NotificationChannelName = "PolisenRSS";

		public static void SetupChennel()
		{
			var httpChannel = HttpNotificationChannel.Find(ChannelUtility.NotificationChannelName);
			if (httpChannel == null)
			{
				httpChannel = new HttpNotificationChannel(ChannelUtility.NotificationChannelName);
				ConfigureChannel(httpChannel);
				httpChannel.Open();
			}
			else
			{
				ConfigureChannel(httpChannel);
			}
			if (!httpChannel.IsShellToastBound && Settings.ShowNotifications)
			{
				httpChannel.BindToShellToast();
			}
			else if (httpChannel.IsShellToastBound && !Settings.ShowNotifications)
			{
				httpChannel.UnbindToShellToast();
			}
		}

		private static void ConfigureChannel(HttpNotificationChannel httpChannel)
		{
			httpChannel.ChannelUriUpdated += (s, eventArg) =>
			{
				if (Settings.ChannelUri != eventArg.ChannelUri.ToString())
				{
					PolisenRSSService.Unregister(Settings.ChannelUri);
				}
				PolisenRSSService.Register(eventArg.ChannelUri.ToString());
				Settings.ChannelUri = eventArg.ChannelUri.ToString();
			};

			httpChannel.ShellToastNotificationReceived += (s, eventArg) =>
			{
				var feedParser = new Parser.FeedParser();
				foreach (var feed in App.ViewModel.Feeds)
				{
					feedParser.ParseFeedAsync(feed);
				}
			};
		}
	}
}
