using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Lybeckeffekten.Controls
{
	public partial class FeedItemList : UserControl
	{
		public FeedItemList()
		{
			InitializeComponent();
		}

		public Model.Feed Feed
		{
			set { FeedItemsListBox.ItemsSource = App.ViewModel.GetFeedItems(value.Id); }
		}

		private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			var stackPanel = sender as StackPanel;
			if (stackPanel != null && stackPanel.Tag is Model.FeedItem)
			{
				var task = new WebBrowserTask();
				task.Uri = new Uri(((Model.FeedItem)stackPanel.Tag).Url);
				task.Show();
			}
		}
	}
}
