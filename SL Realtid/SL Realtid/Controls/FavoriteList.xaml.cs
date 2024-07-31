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
using System.Globalization;
using System.Windows.Media;

namespace SLRealtid.Controls
{
	public partial class FavoriteList : UserControl
	{
		public FavoriteList()
		{
			InitializeComponent();
			Loaded += FavoriteList_Loaded;
		}

		public void FavoriteList_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateList();
		}

		public void UpdateList()
		{
			FavoriteListBox.ItemsSource = App.ViewModel.Favorites;
		}

		private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			var favorite = FavoriteListBox.SelectedItem as Model.Favorite;
			if (favorite != null)
			{
				App.CurrentSite = favorite;
				App.CurrentFilter = favorite.SiteFilter;
				((PhoneApplicationFrame)App.Current.RootVisual).Navigate(new Uri("/Pages/DeparturePage.xaml", UriKind.Relative));
			}
		}

		private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var parentElement = (FrameworkElement)VisualTreeHelper.GetParent((MenuItem)sender);
			if (parentElement != null && parentElement.DataContext != null && parentElement.DataContext is Model.Favorite)
			{
				DeleteTile((Model.Favorite)parentElement.DataContext);
				App.ViewModel.DeleteFavorite((Model.Favorite)parentElement.DataContext);
				OnFavoriteDeleted(new EventArgs());
			}
		}

		private void DeleteTile(Model.Favorite favorite)
		{
			var queryString = string.Format("{0}={1}", App.FavoriteIdQS, favorite.Id);
			var tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(queryString));
			if (tile != null)
			{
				tile.Delete();
			}
		}

		private void RemoteTileMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var parentElement = (FrameworkElement)VisualTreeHelper.GetParent((MenuItem)sender);
			if (parentElement != null && parentElement.DataContext != null && parentElement.DataContext is Model.Favorite)
			{
				DeleteTile(parentElement.DataContext as Model.Favorite);
			}
		}

		private void AddTileMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var parentElement = (FrameworkElement)VisualTreeHelper.GetParent((MenuItem)sender);
			if (parentElement != null && parentElement.DataContext != null && parentElement.DataContext is Model.Favorite)
			{
				var favorite = parentElement.DataContext as Model.Favorite;
				var queryString = string.Format("{0}={1}", App.FavoriteIdQS, favorite.Id);
				var tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(queryString));
				if (tile == null)
				{
					var tileData = new StandardTileData
					{
						Title = favorite.Name,
						BackgroundImage = new Uri("/Assets/Images/LargeIcon.png", UriKind.Relative)
					};
					ShellTile.Create(new Uri(string.Format("/Pages/DeparturePage.xaml?{0}", queryString), UriKind.Relative), tileData);
				}
			}
		}
		
		#region FavoriteDeleted event

		private event EventHandler<EventArgs> _favoriteDeleted;

		public event EventHandler<EventArgs> FavoriteDeleted
		{
			add { _favoriteDeleted += value; }
			remove { _favoriteDeleted -= value; }
		}

		private void OnFavoriteDeleted(EventArgs e)
		{
			EventHandler<EventArgs> handler = _favoriteDeleted;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion				

		private void ContextMenu_Opened(object sender, RoutedEventArgs e)
		{
			var contextMenu = sender as ContextMenu;
			if (contextMenu != null)
			{
				var addTileMenuItem = contextMenu.Items.Cast<MenuItem>().FirstOrDefault(item => item.Name == "AddTileMenuItem");
				var remoteTileMenuItem = contextMenu.Items.Cast<MenuItem>().FirstOrDefault(item => item.Name == "RemoveTileMenuItem");
				if (addTileMenuItem != null && remoteTileMenuItem != null)
				{
					var parentElement = (FrameworkElement)VisualTreeHelper.GetParent(addTileMenuItem);
					if (parentElement != null && parentElement.DataContext != null && parentElement.DataContext is Model.Favorite)
					{
						var favorite = parentElement.DataContext as Favorite;
						var queryString = string.Format("{0}={1}", App.FavoriteIdQS, favorite.Id);
						var tile = ShellTile.ActiveTiles.FirstOrDefault(item => item.NavigationUri.ToString().Contains(queryString));
						addTileMenuItem.Visibility = tile == null ? Visibility.Visible : Visibility.Collapsed;
						remoteTileMenuItem.Visibility = tile != null ? Visibility.Visible : Visibility.Collapsed;
					}
				}
			}
		}
	}
}
