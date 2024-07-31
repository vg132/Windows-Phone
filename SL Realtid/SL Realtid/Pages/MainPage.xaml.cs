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
using System.Globalization;
using System.Threading;

namespace SLRealtid.Pages
{
	public partial class MainPage : PhoneApplicationPage
	{
		private PanoramaItem _favoritesItem = null;

		// Constructor
		public MainPage()
		{
			InitializeComponent();
			this.DataContext = App.ViewModel;
			SetupPanorama();
			Loaded += MainPage_Loaded;
		}

		public void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			SetupFavorites();
			if (Settings.ShowSearchHelp)
			{
				Settings.ShowSearchHelp = false;
				ThreadPool.QueueUserWorkItem((o) =>
				{
					Thread.Sleep(750);
					Dispatcher.BeginInvoke(() => MessageBox.Show("Här hittar du alla stationer. Från början visas en lista med alla spårbundna stationer men genom att söka kan du även hitta busstationer.", "Tips", MessageBoxButton.OK));
				});
			}
		}

		private void SetupFavorites()
		{
			var favoriteListItem = MainPanorama.Items.Cast<PanoramaItem>().FirstOrDefault(item => item.Content is Controls.FavoriteList);
			if (App.ViewModel.Favorites.Count() > 0 && favoriteListItem == null)
			{
				MainPanorama.Items.Insert(0, _favoritesItem);
			}
			else if (App.ViewModel.Favorites.Count() == 0 && favoriteListItem != null)
			{
				MainPanorama.Items.Remove(favoriteListItem);
			}
			((Controls.FavoriteList)_favoritesItem.Content).UpdateList();
		}

		private void SetupPanorama()
		{
			var favoriteList = new Controls.FavoriteList();
			favoriteList.FavoriteDeleted += FavoriteList_FavoriteDeleted;
			_favoritesItem = new PanoramaItem { Header = "favoriter", Content = favoriteList };
			if (App.ViewModel.Favorites.Count() > 0)
			{
				MainPanorama.Items.Add(_favoritesItem);
			}
			MainPanorama.Items.Add(new PanoramaItem
			{
				Header = "stationer",
				Content = new Controls.SiteList()
			});
			MainPanorama.Items.Add(new PanoramaItem
			{
				Header = "trafikläget",
				Content=new Controls.TrafficStatus()
			});
		}

		public void FavoriteList_FavoriteDeleted(object sender, EventArgs e)
		{
			SetupFavorites();
		}

		private void SettingsButton_Click(object sender, EventArgs e)
		{
		}
	}
}