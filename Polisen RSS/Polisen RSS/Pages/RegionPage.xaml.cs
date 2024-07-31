using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PolisenRSS.Extensions;

namespace PolisenRSS.Pages
{
	public partial class RegionPage : PhoneApplicationPage
	{
		public RegionPage()
		{
			InitializeComponent();
			Loaded += (sender, e) =>
			{
				DataContext = App.ViewModel.Regions;
			};
		}

		private void RegionCheckBox_Changed(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBox;
			if (checkBox != null)
			{
				var region = checkBox.Tag as Model.Region;
				if (region.IsActive != checkBox.IsChecked.Value)
				{
					region.IsActive = checkBox.IsChecked.Value;
					App.ViewModel.SaveChanges();
					App.ReloadFeeds = true;
				}
				if (region.Id == 0)
				{
					App.ViewModel.Regions.Where(item => item.Id != 0).ToList().ForEach(item => item.IsEnabled = !checkBox.IsChecked.Value);
				}
			}
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			Services.PolisenRSSService.Update();
		}
	}
}