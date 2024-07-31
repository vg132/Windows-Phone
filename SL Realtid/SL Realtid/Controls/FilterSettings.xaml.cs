using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SLRealtid.Extensions;

namespace SLRealtid.Controls
{
	public partial class FilterSettings : UserControl
	{
		public FilterSettings()
		{
			InitializeComponent();
			Loaded += FilterSettings_Loaded;
		}

		private void FilterSettings_Loaded(object sender, RoutedEventArgs e)
		{
			if (Filter == null)
			{
				Filter = new Model.Filter();
			}
			ShowCheckBox.IsChecked = Filter.Show;
			ExcludeFilterTextBox.Text = string.Join(",", Filter.HiddenDestinations);
			var lines = Departures.Select(item => item.LineNumber).Distinct().Select(item => item.ToString());
			ExcludedLinesTextBox.Text = string.Join(",", Filter.HiddenLines.Where(item => !lines.Contains(item)));
			LineListBox.ItemsSource = lines.Select(item => new LineViewItem
																			{
																				Name = item,
																				IsSelected = Filter.HiddenLines.Contains(item.ToString())
																			})
																			.ToList();
		}

		protected void ShowCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			ContentStackPanel.Visibility = Visibility.Visible;
		}

		protected void ShowCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			ContentStackPanel.Visibility = Visibility.Collapsed;
		}

		public Model.TransportationType FilterType { get; set; }
		public List<Model.Departure> Departures { get; set; }
		public Model.Filter Filter { get; set; }

		public Model.Filter NewFilter
		{
			get
			{
				var hiddenLines = new List<string>();
				for (int i = 0; i < LineListBox.Items.Count; i++)
				{
					var listBoxItem = (ListBoxItem)LineListBox.ItemContainerGenerator.ContainerFromIndex(i);
					var checkBox = listBoxItem.FindFirstElementInVisualTree<CheckBox>();
					if (checkBox.IsChecked.Value)
					{
						hiddenLines.Add(checkBox.Content.ToString());
					}
				}
				hiddenLines.AddRange(ExcludedLinesTextBox.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()));
				return new Model.Filter
										{
											Show = ShowCheckBox.IsChecked.Value,
											HiddenLines = hiddenLines,
											HiddenDestinations = ExcludeFilterTextBox.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).ToList()
										};
			}
		}
	}

	public class LineViewItem
	{
		public string Name { get; set; }
		public bool IsSelected { get; set; }
	}
}