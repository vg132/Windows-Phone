using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SLRealtid.Converters
{
	public class VisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is bool)
			{
				return (bool)value ? Visibility.Visible : Visibility.Collapsed;
			}
			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!(value is Visibility))
			{
				return DependencyProperty.UnsetValue;
			}
			return (Visibility)value == Visibility.Visible;
		}
	}
}
