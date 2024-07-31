using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid
{
	public class Utilities
	{
		public static string GetSearchableText(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				return text.ToLower().Replace("å", "a").Replace("ä", "a").Replace("ö", "o").Trim();
			}
			return string.Empty;
		}

		public static bool ShowGroupHeading(Model.Departure departure)
		{
			return departure.TransportationType != Model.TransportationType.DpsTram;
		}

		public static string GetGroupName(Model.Departure departure)
		{
			if (departure.TransportationType == Model.TransportationType.DpsTrain)
			{
				if (departure.Direction == 1)
				{
					return "södergående tåg";
				}
				else if (departure.Direction == 2)
				{
					return "norrgående tåg";
				}
			}
			else if (departure.TransportationType == Model.TransportationType.DpsMetro)
			{
				if (departure.LineNumber == "10" || departure.LineNumber == "11")
				{
					return "blålinje";
				}
				else if (departure.LineNumber == "13" || departure.LineNumber == "14")
				{
					return "rödlinje";
				}
				else if (departure.LineNumber == "17" || departure.LineNumber == "18" || departure.LineNumber == "19")
				{
					return "grönlinje";
				}
			}
			else if (departure.TransportationType == Model.TransportationType.DpsTram)
			{
				return departure.Direction.ToString();
			}
			else if (departure.TransportationType == Model.TransportationType.DpsBus)
			{
				return "bussar";
			}
			return string.Empty;
		}
	}
}