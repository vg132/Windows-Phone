using SLRealtid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SLRealtid.Converters
{
	public class SiteGroupConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var items = value as IEnumerable<ISite>;
			var controls = new List<ContentControl>();
			if (items != null)
			{
				var sites = new List<ISite>();
				var stations = new List<ISite>();

				foreach (var site in items)
				{
					if (site is Station)
					{
						stations.Add(site);
					}
					else
					{
						sites.Add(site);
					}
				}
				AddControls(controls, stations, "spårbunden trafik:", sites.Count() > 0);
				AddControls(controls, sites, "buss:", stations.Count() > 0);
			}
			return controls;
		}

		private void AddControls(List<ContentControl> controls, List<ISite> sites, string heading, bool showHeading)
		{
			if (sites.Count > 0)
			{
				if (showHeading)
				{
					controls.Add(new ContentControl() { Content = heading, ContentTemplate = App.Current.Resources["GroupHeadingTemplate"] as DataTemplate });
				}
				foreach (var site in sites)
				{
					controls.Add(new ContentControl { Content = site, Tag = site, ContentTemplate = App.Current.Resources["SiteTemplate"] as DataTemplate });
				}
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}