using SLRealtid.Controls;
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
	public class DepartureGroupConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var departures = value as IEnumerable<Departure>;
			var controls = new List<ContentControl>();
			if (departures != null)
			{
				var q = departures.Select(item => new DepartureViewItem
														{
															Departure = item,
															Destination = !string.IsNullOrEmpty(item.LineNumber) ? string.Format("{0}. {1}", item.LineNumber, item.Destination) : item.Destination,
															TimeTableTime = item.TimeTabledDateTime != DateTime.MinValue ? item.TimeTabledDateTime.ToString("HH:mm") : item.DisplayTime,
															ExpectedTime = item.ExpectedDateTime.ToString("HH:mm:ss"),
															ShowExpected = item.TransportationType == TransportationType.DpsMetro || Math.Abs((item.TimeTabledDateTime - item.ExpectedDateTime).TotalSeconds) < 60 ? Visibility.Collapsed : Visibility.Visible,
															Direction = item.Direction,
															GroupName = Utilities.GetGroupName(item)
														})
													.GroupBy(item => item.GroupName);
				bool hasGroups = q.Count() > 1;
				foreach (var group in q)
				{
					if (Utilities.ShowGroupHeading(group.First().Departure))
					{
						controls.Add(new ContentControl() { Content = group.Key + " mot:", ContentTemplate = App.Current.Resources["GroupHeadingTemplate"] as DataTemplate });
					}
					else
					{
						controls.Add(new ContentControl() { Content = "mot:", ContentTemplate = App.Current.Resources["GroupHeadingTemplate"] as DataTemplate });
					}
					foreach (var departure in group)
					{
						controls.Add(new ContentControl { Content = departure, ContentTemplate = App.Current.Resources["DepartureTemplate"] as DataTemplate });
					}
				}
			}
			return controls;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
