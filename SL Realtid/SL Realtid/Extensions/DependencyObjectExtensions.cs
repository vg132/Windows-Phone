using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SLRealtid.Extensions
{
	public static class DependencyObjectExtensions
	{
		public static T FindFirstElementInVisualTree<T>(this DependencyObject parentElement) where T : DependencyObject
		{
			var count = VisualTreeHelper.GetChildrenCount(parentElement);
			if (count == 0)
			{
				return null;
			}
			for (int i = 0; i < count; i++)
			{
				var child = VisualTreeHelper.GetChild(parentElement, i);
				if (child != null && child is T)
				{
					return (T)child;
				}
				else
				{
					var result = FindFirstElementInVisualTree<T>(child);
					if (result != null)
					{
						return result;
					}
				}
			}
			return null;
		}
	}
}
