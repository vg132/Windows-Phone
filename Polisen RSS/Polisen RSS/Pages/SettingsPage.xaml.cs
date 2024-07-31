using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PolisenRSS.Pages
{
	public partial class SettingsPage : PhoneApplicationPage
	{
		private bool _loading = false;

		public SettingsPage()
		{
			InitializeComponent();
			Loaded += (sender, e) =>
			{
				_loading = true;
				ShowNotificationsToggleSwitch.IsChecked = Settings.ShowNotifications;
				_loading = false;
			};
		}

		private void ShowNotifications_Changed(object sender, RoutedEventArgs e)
		{
			if (!_loading)
			{
				if (ShowNotificationsToggleSwitch.IsChecked.Value)
				{
					Settings.ShowNotifications = true;
					Utilities.ChannelUtility.SetupChennel();
				}
				else
				{
					Settings.ShowNotifications = false;
					Utilities.ChannelUtility.SetupChennel();
				}
			}
		}

		private void SelectFeed_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/FeedPage.xaml", UriKind.Relative));
		}

		private void SelectRegions_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/RegionPage.xaml", UriKind.Relative));
		}
	}
}