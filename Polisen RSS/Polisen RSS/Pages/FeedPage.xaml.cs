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
	public partial class FeedPage : PhoneApplicationPage
	{
		public FeedPage()
		{
			InitializeComponent();
			Loaded += (sender, e) =>
			{
				DataContext = App.ViewModel.Feeds;
			};
		}

		private void RegionCheckBox_Changed(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBox;
			if (checkBox != null)
			{
				var feed = checkBox.Tag as Model.Feed;
				if (feed.IsActive != checkBox.IsChecked.Value)
				{
					feed.IsActive = checkBox.IsChecked.Value;
					App.ViewModel.SaveChanges();
					App.ReloadFeeds = true;
				}
			}
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			Services.PolisenRSSService.Update();
		}
	}
}