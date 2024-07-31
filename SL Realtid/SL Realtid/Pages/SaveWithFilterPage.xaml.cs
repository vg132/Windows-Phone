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

namespace SLRealtid.Pages
{
	public partial class SaveWithFilterPage : PhoneApplicationPage
	{
		public SaveWithFilterPage()
		{
			InitializeComponent();
			Loaded += SaveWithFilterPage_Loaded;
		}

		private void SaveWithFilterPage_Loaded(object sender, RoutedEventArgs e)
		{
			TitleTextBlock.Text = "SL Realtid - " + App.CurrentSite.Name;
			HelpTextBlock.Text = string.Format("Sökningen har ett aktivt filter fyll i ett namn för denna sökning, t.ex. \"{0} - till jobbet\".", App.CurrentSite.Name);
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			NavigationService.GoBack();
		}

		protected void SaveButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(NameTextBox.Text))
			{
				MessageBox.Show("Ett namn måste anges.", "Namn", MessageBoxButton.OK);
			}
			else
			{
				App.ViewModel.AddFavorite(new Favorite
				{
					Name = NameTextBox.Text,
					SiteId = App.CurrentSite.SiteId,
					SiteFilter = App.CurrentFilter
				});
				NavigationService.GoBack();
			}
		}
	}
}