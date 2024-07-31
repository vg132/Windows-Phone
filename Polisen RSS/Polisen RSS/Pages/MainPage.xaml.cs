using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Microsoft.Phone.Notification;
using PolisenRSS.Services;
using System.ComponentModel;

namespace PolisenRSS.Pages
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();
			if (!Settings.IsFirstStart && Settings.ShowNotifications)
			{
				Utilities.ChannelUtility.SetupChennel();
			}
			Loaded += (sender, e) =>
			{
				if (App.ReloadFeeds)
				{
					App.ReloadFeeds = false;
					App.ViewModel.LoadCollectionsFromDatabase();
					SetupFeeds();
				}
				if (Settings.IsFirstStart)
				{
					var result = MessageBox.Show("Polisen RSS använder sig av \"Push Notifications\" för att informera telefonen att något nytt har hänt. Vill du aktivera \"Push Notifications\" för Polisen RSS?", "Visa notifikationer", MessageBoxButton.OKCancel);
					if (result != MessageBoxResult.OK)
					{
						Settings.ShowNotifications = false;
					}
					else
					{
						Utilities.ChannelUtility.SetupChennel();
					}
					Settings.IsFirstStart = false;
					RefreshFeeds();
				}
			};
			DataContext = App.ViewModel.Feeds;
			App.ReloadFeeds = true;
		}

		private void SetupFeeds()
		{
			FeedsPivot.Items.Clear();
			foreach (var feed in App.ViewModel.Feeds.Where(item => item.IsActive))
			{
				FeedsPivot.Items.Add(new PivotItem
				{
					Header = feed.Name,
					Content = new Controls.FeedItemList
					{
						Feed = feed
					},
				});
			}
		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			RefreshFeeds();
		}

		private void RefreshFeeds()
		{
			var feedParser = new Parser.FeedParser();
			foreach (var feed in App.ViewModel.Feeds.Where(item => item.IsActive))
			{
				feedParser.ParseFeedAsync(feed);
			}
		}

		private void SettingsButton_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/SettingsPage.xaml", UriKind.Relative));
		}
	}
}