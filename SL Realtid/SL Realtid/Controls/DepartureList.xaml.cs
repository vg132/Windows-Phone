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

namespace SLRealtid.Controls
{
	public partial class DepartureList : UserControl
	{
		public DepartureList(List<Departure> departures, Model.TransportationType transporationType)
		{
			InitializeComponent();
			if (departures.Count() > 0)
			{
				DataContext = departures;
				DepartureListBox.Visibility = System.Windows.Visibility.Visible;
				NoResultTextBlock.Visibility = System.Windows.Visibility.Collapsed;
			}
			else
			{
				NoResultTextBlock.Visibility = System.Windows.Visibility.Visible;
				DepartureListBox.Visibility = System.Windows.Visibility.Collapsed;
			}
			TransportationType = transporationType;
		}

		public Model.TransportationType TransportationType { get; set; }
	}
}
