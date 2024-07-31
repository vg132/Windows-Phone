using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PolisenRSS.Extensions
{
	public static class XElementExtensions
	{
		public static bool GetBooleanValue(this XElement element)
		{
			if (element != null)
			{
				bool value;
				if (bool.TryParse(element.Value, out value))
				{
					return value;
				}
			}
			return false;
		}

		public static string GetStringValue(this XElement element)
		{
			if (element != null)
			{
				return element.Value.Trim();
			}
			return string.Empty;
		}

		public static int GetIntValue(this XElement element)
		{
			if (element != null)
			{
				int value;
				if (int.TryParse(element.Value.Trim(), out value))
				{
					return value;
				}
			}
			return 0;
		}

		public static DateTime GetDateTimeValue(this XElement element)
		{
			if (element != null)
			{
				return DateTime.Parse(element.Value.Trim());
			}
			return DateTime.MinValue;
		}
	}

}
