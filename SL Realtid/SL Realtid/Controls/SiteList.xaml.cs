using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SLRealtid.Model;
using SLRealtid.Parsers;

namespace SLRealtid.Controls
{
	public partial class SiteList : UserControl
	{
		public SiteList()
		{
			InitializeComponent();
			DataContext = App.ViewModel.Stations;
		}

		private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(FilterTextBox.Text))
			{
				DataContext = App.ViewModel.Stations;
			}
			else
			{
				var filterText = Utilities.GetSearchableText(FilterTextBox.Text);
				DataContext = App.ViewModel.Stations.Where(item => Utilities.GetSearchableText(item.Name).StartsWith(filterText) || Utilities.GetSearchableText(item.Alias).StartsWith(filterText)).ToList();

				var siteParser = new SiteParser();
				siteParser.SiteSearchParsed += SiteParser_SiteSearchParsed;
				siteParser.SiteSearchAsync(filterText);
			}
		}

		private void SiteParser_SiteSearchParsed(object sender, SiteParsedEventArgs e)
		{
			var filterText = Utilities.GetSearchableText(FilterTextBox.Text);
			var stations = App.ViewModel.Stations.Where(item => Utilities.GetSearchableText(item.Name).StartsWith(filterText) || Utilities.GetSearchableText(item.Alias).StartsWith(filterText)).ToList();

			var sites = e.Sites.GroupBy(item => item.SiteId)
													.Select(item => item.First())
													.Where(site => stations.FirstOrDefault(item => item.SiteId == site.SiteId && !item.HasBus) == null)
													.ToList();
			sites.AddRange(stations.Cast<ISite>());
			DataContext = sites;
		}

		private void FilterTextBox_ActionIconTapped(object sender, EventArgs e)
		{
			FilterTextBox.Text = string.Empty;
		}

		private void SiteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SiteSelected();
		}

		private void SiteListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			SiteSelected();
		}

		private void SiteSelected()
		{
			var contentControl = SiteListBox.SelectedItem as ContentControl;
			if (contentControl != null && contentControl.Tag is ISite)
			{
				App.CurrentSite = (ISite)contentControl.Tag;
				App.CurrentFilter = null;
				((PhoneApplicationFrame)App.Current.RootVisual).Navigate(new Uri("/Pages/DeparturePage.xaml", UriKind.Relative));
			}
			else
			{
				SiteListBox.SelectedIndex = -1;
			}
		}
	}
}
