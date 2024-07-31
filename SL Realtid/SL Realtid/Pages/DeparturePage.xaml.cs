using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SLRealtid.Parsers;
using SLRealtid.Controls;
using SLRealtid.Model;
using System.Threading;

namespace SLRealtid.Pages
{
	public partial class DeparturePage : PhoneApplicationPage
	{
		private int _loadErrors = 0;
		private int _noResults = 0;
		private bool _loaded = false;

		private ApplicationBarIconButton _removeFavoriteButton = null;
		private ApplicationBarIconButton _addFavoriteButton = null;
		private ApplicationBarIconButton _siteFilterButton = null;
		private ApplicationBarIconButton _siteRefreshButton = null;

		public DeparturePage()
		{
			InitializeComponent();
			SetupButtons();
			Loaded += DeparturePage_Loaded;
		}

		private void DeparturePage_Loaded(object sender, RoutedEventArgs e)
		{
			if (!_loaded)
			{
				TitleTextBlock.Text = "SL Realtid - " + App.CurrentSite.Name;
				LoadDepartures();
			}
		}

		private void SetupButtons()
		{
			_siteFilterButton = new ApplicationBarIconButton();
			_siteFilterButton.Click += FilterButton_Click;
			_siteFilterButton.IconUri = new Uri("/Assets/Images/appbar.search.refine.rest.png", UriKind.Relative);
			_siteFilterButton.Text = "filtrera";
			ApplicationBar.Buttons.Add(_siteFilterButton);

			_siteRefreshButton = new ApplicationBarIconButton();
			_siteRefreshButton.Click += RefreshButton_Click;
			_siteRefreshButton.IconUri = new Uri("/Assets/Images/appbar.refresh.rest.png", UriKind.Relative);
			_siteRefreshButton.Text = "ladda om";
			ApplicationBar.Buttons.Add(_siteRefreshButton);

			_addFavoriteButton = new ApplicationBarIconButton();
			_addFavoriteButton.Click += AddFavoriteButton_Click;
			_addFavoriteButton.IconUri = new Uri("/Assets/Images/appbar.favs.addto.rest.png", UriKind.Relative);
			_addFavoriteButton.Text = "spara";

			_removeFavoriteButton = new ApplicationBarIconButton();
			_removeFavoriteButton.Click += RemoveFavoriteButton_Click;
			_removeFavoriteButton.IconUri = new Uri("/Assets/Images/appbar.favs.remove.rest.png", UriKind.Relative);
			_removeFavoriteButton.Text = "ta bort";
		}

		private void ToggleFavoriteToolbarButton()
		{
			if (App.ViewModel.Favorites.FirstOrDefault(item => item.SiteId == App.CurrentSite.SiteId) != null && App.CurrentFilter == null)
			{
				if (!ApplicationBar.Buttons.Contains(_removeFavoriteButton))
				{
					ApplicationBar.Buttons.Remove(_addFavoriteButton);
					ApplicationBar.Buttons.Insert(0, _removeFavoriteButton);
				}
			}
			else
			{
				if (!ApplicationBar.Buttons.Contains(_addFavoriteButton))
				{
					ApplicationBar.Buttons.Remove(_removeFavoriteButton);
					ApplicationBar.Buttons.Insert(0, _addFavoriteButton);
				}
			}
		}

		private void LoadDepartures()
		{
			ToggleFavoriteToolbarButton();
			_siteFilterButton.IsEnabled = false;
			_siteRefreshButton.IsEnabled = false;

			App.CurrentDepartures = new Dictionary<TransportationType, List<Departure>>();

			MainPanorama.Items.Clear();
			LoadingProgressBar.IsIndeterminate = true;
			LoadingProgressBar.Visibility = System.Windows.Visibility.Visible;
			LoadErrorTextBlock.Visibility = System.Windows.Visibility.Collapsed;
			NoResultTextBlock.Visibility = System.Windows.Visibility.Collapsed;

			_loadErrors = 0;
			_noResults = 0;
			var departureParser = new DepartureParser();
			departureParser.DeparturesParsed += DepartureParser_DeparturesParsed;
			departureParser.LoadError += DepartureParser_LoadError;
			departureParser.NoResults += DepartureParser_NoResults;
			departureParser.DownloadDeparturesAsync(App.CurrentSite.SiteId);
		}

		private void DepartureParser_NoResults(object sender, EventArgs e)
		{
			_noResults++;
			if (_noResults == 2 && LoadingProgressBar.Visibility == System.Windows.Visibility.Visible)
			{
				LoadingProgressBar.Visibility = System.Windows.Visibility.Collapsed;
				NoResultTextBlock.Visibility = System.Windows.Visibility.Visible;
				_siteFilterButton.IsEnabled = false;
				_siteRefreshButton.IsEnabled = true;
			}
		}

		public void DepartureParser_LoadError(object sender, EventArgs e)
		{
			_loadErrors++;
			if (_loadErrors == 2 && LoadingProgressBar.Visibility == System.Windows.Visibility.Visible)
			{
				LoadingProgressBar.Visibility = System.Windows.Visibility.Collapsed;
				LoadErrorTextBlock.Visibility = System.Windows.Visibility.Visible;
				_siteFilterButton.IsEnabled = false;
				_siteRefreshButton.IsEnabled = true;
			}
		}

		public void DepartureParser_DeparturesParsed(object sender, DeparturesParsedEventArgs e)
		{
			Model.Filter filter = (App.CurrentFilter!=null && App.CurrentFilter.ContainsKey(e.TransporationType)) ? App.CurrentFilter[e.TransporationType] : new Model.Filter();
			if (filter.Show)
			{
				var filteredDepartures = e.Departures.Where(item => !filter.HiddenLines.Select(line => line.ToLower().Trim()).Contains(item.LineNumber.ToLower().Trim()))
																							.Where(destination => filter.HiddenDestinations.All(item => !destination.Destination.ToLower().Contains(item.ToLower())))
																							.ToList();
				MainPanorama.Items.Add(new PanoramaItem
				{
					Header = GetDepartureTypeName(e.TransporationType).ToLower(),
					Content = new DepartureList(filteredDepartures, e.TransporationType)
				});

				if (App.CurrentDepartures.ContainsKey(e.TransporationType))
				{
					App.CurrentDepartures[e.TransporationType] = e.Departures;
				}
				else
				{
					App.CurrentDepartures.Add(e.TransporationType, e.Departures);
				}
			}
			LoadingProgressBar.IsIndeterminate = false;
			LoadingProgressBar.Visibility = System.Windows.Visibility.Collapsed;
			MainPanorama.Visibility = System.Windows.Visibility.Visible;
			_siteFilterButton.IsEnabled = true;
			_siteRefreshButton.IsEnabled = true;

			if (Settings.ShowDepartureHelp)
			{
				Settings.ShowDepartureHelp = false;
				ThreadPool.QueueUserWorkItem((o) =>
				{
					Thread.Sleep(750);
					Dispatcher.BeginInvoke(() => MessageBox.Show("Här ser du avgångarna för den valda stationen. Om du vill begränsa resultatet kan du trycka på filter ikonen, där kan du sedan ställa in exakt vad du vill se för resultat. Du kan även spara en sökning som en favorit och sedan lägga in den som en tile på telefonens startskärm.", "Tips", MessageBoxButton.OK));
				});
			}
		}

		public string GetDepartureTypeName(TransportationType departureType)
		{
			switch (departureType)
			{
				case TransportationType.DpsMetro:
					return "Tunnelbana";
				case TransportationType.DpsBus:
					return "Buss";
				case TransportationType.DpsTrain:
					return "Pendeltåg";
				case TransportationType.DpsTram:
					return "Lokalbana";
			}
			return string.Empty;
		}

		protected void RefreshButton_Click(object sender, EventArgs e)
		{
			LoadDepartures();
		}

		public void RemoveFavoriteButton_Click(object sender, EventArgs e)
		{
			if (App.ViewModel.Favorites.FirstOrDefault(item => item.SiteId == App.CurrentSite.SiteId) != null)
			{
				App.ViewModel.DeleteFavorite(App.ViewModel.Favorites.FirstOrDefault(item => item.SiteId == App.CurrentSite.SiteId));
				ToggleFavoriteToolbarButton();
			}
		}

		private void AddFavoriteButton_Click(object sender, EventArgs e)
		{
			if (App.CurrentFilter != null)
			{
				((PhoneApplicationFrame)App.Current.RootVisual).Navigate(new Uri("/Pages/SaveWithFilterPage.xaml", UriKind.Relative));
			}
			else
			{
				App.ViewModel.AddFavorite(new Favorite
				{
					Name = App.CurrentSite.Name,
					SiteId = App.CurrentSite.SiteId
				});
				ToggleFavoriteToolbarButton();
			}
		}

		protected void FilterButton_Click(object sender, EventArgs e)
		{
			var transporationType = ((Controls.DepartureList)((PanoramaItem)MainPanorama.SelectedItem).Content).TransportationType;
			((PhoneApplicationFrame)App.Current.RootVisual).Navigate(new Uri(string.Format("/Pages/FilterSettingsPage.xaml?{0}={1}", App.TransportationTypeQS, transporationType.ToString()), UriKind.Relative));
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (NavigationContext.QueryString.ContainsKey(App.RemoveFromBackStackQS))
			{
				NavigationService.RemoveBackEntry();
			}
			else if (NavigationContext.QueryString.ContainsKey(App.FavoriteIdQS))
			{
				int id;
				if (int.TryParse(NavigationContext.QueryString[App.FavoriteIdQS], out id))
				{
					var favorite = App.ViewModel.Favorites.FirstOrDefault(item => item.Id == id);
					if (favorite != null)
					{
						App.CurrentSite = favorite;
						App.CurrentFilter = favorite.SiteFilter;
					}
				}
			}
		}
	}
}