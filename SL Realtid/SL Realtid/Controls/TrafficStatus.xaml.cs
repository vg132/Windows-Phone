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

namespace SLRealtid.Controls
{
	public partial class TrafficStatus : UserControl
	{
		public TrafficStatus()
		{
			InitializeComponent();
			SetupControls();
		}

		public void SetupControls()
		{
			var trafficStatusParser = new TrafficStatusParser();
			trafficStatusParser.TrafficStatusParsed += TrafficStatusParser_TrafficStatusParsed;
			trafficStatusParser.ParseTrafficStatusAsync();
		}

		private void TrafficStatusParser_TrafficStatusParsed(object sender, TrafficStatusParsedEventArgs e)
		{
			if (e.TrafficStatus != null)
			{
				TrafficStatusListBox.ItemsSource = e.TrafficStatus;
			}
		}
	}
}
