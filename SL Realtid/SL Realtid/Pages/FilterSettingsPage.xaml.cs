using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;

namespace SLRealtid.Pages
{
	public partial class FilterSettingsPage : PhoneApplicationPage
	{
		public FilterSettingsPage()
		{
			InitializeComponent();
			Loaded += FilterSettingsPage_Loaded;
		}

		private void FilterSettingsPage_Loaded(object sender, RoutedEventArgs e)
		{
			AddPivotItem(Model.TransportationType.DpsTrain, "pendeltåg");
			AddPivotItem(Model.TransportationType.DpsMetro, "tunnelbana");
			AddPivotItem(Model.TransportationType.DpsTram, "lokalbana");
			AddPivotItem(Model.TransportationType.DpsBus, "buss");

			if (Settings.ShowFilterHelp)
			{
				Settings.ShowFilterHelp = false;
				ThreadPool.QueueUserWorkItem((o) =>
				{
					Thread.Sleep(750);
					Dispatcher.BeginInvoke(() => MessageBox.Show("Med hjälp av ett filter kan du dölja vissa linjer från resultatet av en sökning så att du bara ser den information som är viktig för dig. Du kan sedan välja att spara sökningen som en favorit och då kommer filtret att sparas med den sökningen och aktiveras varje gång favoriten laddas.", "Tips", MessageBoxButton.OK));
				});
			}
		}

		private void AddPivotItem(Model.TransportationType type, string header)
		{
			if (App.CurrentDepartures.ContainsKey(type))
			{
				var item = new PivotItem
				{
					Header = header,
					Content = new Controls.FilterSettings
					{
						Departures = App.CurrentDepartures[type],
						Filter = App.CurrentFilter!=null && App.CurrentFilter.ContainsKey(type) ? App.CurrentFilter[type] : null,
						FilterType = type
					}
				};
				FilterSettingsPivot.Items.Add(item);
				if (type == SelectedTransportationType)
				{
					FilterSettingsPivot.SelectedItem = item;
				}
			}
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			App.CurrentFilter = new Dictionary<Model.TransportationType, Model.Filter>();
			foreach (var item in FilterSettingsPivot.Items.Cast<PivotItem>())
			{
				var filterSettings = item.Content as Controls.FilterSettings;
				App.CurrentFilter.Add(filterSettings.FilterType, filterSettings.NewFilter);
			}
			NavigationService.RemoveBackEntry();
			((PhoneApplicationFrame)App.Current.RootVisual).Navigate(new Uri(string.Format("/Pages/DeparturePage.xaml?{0}=true", App.RemoveFromBackStackQS), UriKind.Relative));
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			NavigationService.GoBack();
		}

		private Model.TransportationType SelectedTransportationType { get; set; }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (NavigationContext.QueryString.ContainsKey(App.TransportationTypeQS))
			{
				SelectedTransportationType = (Model.TransportationType)Enum.Parse(typeof(Model.TransportationType), NavigationContext.QueryString[App.TransportationTypeQS], true);
			}
		}
	}
}