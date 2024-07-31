using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SLRealtid.Model;
using System.Net;
using SLRealtid.Extensions;

namespace SLRealtid.Parsers
{
	public class DepartureParser
	{
		private static string DpsUrlFormat = "https://api.trafiklab.se/sl/realtid/GetDpsDepartures.XML?timeWindow=60&key=PRIVATE_KEY&siteId={0}";
		private static string SubwayUrlFormat = "https://api.trafiklab.se/sl/realtid/GetDepartures.XML?timeWindow=60&key=PRIVATE_KEY&siteId={0}";
		private Dictionary<string, List<Departure>> _departures = new Dictionary<string, List<Departure>>();

		#region Departures parsed event

		private event EventHandler<DeparturesParsedEventArgs> _departuresParsed;

		public event EventHandler<DeparturesParsedEventArgs> DeparturesParsed
		{
			add { _departuresParsed += value; }
			remove { _departuresParsed -= value; }
		}

		private void OnDeparturesParsed(DeparturesParsedEventArgs e)
		{
			EventHandler<DeparturesParsedEventArgs> handler = _departuresParsed;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region LoadError event

		private event EventHandler<EventArgs> _loadError;

		public event EventHandler<EventArgs> LoadError
		{
			add { _loadError += value; }
			remove { _loadError -= value; }
		}

		private void OnLoadError(EventArgs e)
		{
			EventHandler<EventArgs> handler = _loadError;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region NoResults event

		private event EventHandler<EventArgs> _noResults;

		public event EventHandler<EventArgs> NoResults
		{
			add { _noResults += value; }
			remove { _noResults -= value; }
		}

		private void OnNoResults(EventArgs e)
		{
			EventHandler<EventArgs> handler = _noResults;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion
				

		public void DownloadDeparturesAsync(int siteId)
		{
			var client = new WebClient();
			client.DownloadStringCompleted += DpsCompleted;
			client.DownloadStringAsync(new Uri(string.Format(DepartureParser.DpsUrlFormat, siteId)));

			client = new WebClient();
			client.DownloadStringCompleted += SubwayCompleted;
			client.DownloadStringAsync(new Uri(string.Format(DepartureParser.SubwayUrlFormat, siteId)));
		}

		private void DpsCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				if (e.Error==null && !string.IsNullOrEmpty(e.Result))
				{
					var xDoc = XElement.Parse(e.Result);
					XNamespace xmlns = "http://www1.sl.se/realtidws/";

					var results = ParseDepartures(TransportationType.DpsTrain, xDoc, xmlns);
					results = ParseDepartures(TransportationType.DpsTram, xDoc, xmlns) || results;
					results = ParseDepartures(TransportationType.DpsBus, xDoc, xmlns) || results;
					results = ParseDepartures(TransportationType.DpsMetro, xDoc, xmlns) || results;
					if (!results)
					{
						OnNoResults(new EventArgs());
					}
				}
				else if (e.Error != null)
				{
					OnLoadError(new EventArgs());
				}
			}
			catch
			{
				OnLoadError(new EventArgs());
			}
		}

		private bool ParseDepartures(TransportationType type, XElement root, XNamespace xmlns)
		{
			var departures = (from item in root.Descendants(xmlns + Enum.GetName(type.GetType(), type))
												select new Departure
												{
													Destination = item.Element(xmlns + "Destination").GetStringValue(),
													DisplayTime = item.Element(xmlns + "DisplayTime").GetStringValue(),
													LineNumber = item.Element(xmlns + "LineNumber").GetStringValue(),
													TimeTabledDateTime = item.Element(xmlns + "TimeTabledDateTime").GetDateTimeValue(),
													ExpectedDateTime = item.Element(xmlns + "ExpectedDateTime").GetDateTimeValue(),
													StopAreaName = item.Element(xmlns + "StopAreaName").GetStringValue(),
													Direction = item.Element(xmlns + "JourneyDirection").GetIntValue(),
													LineName = item.Element(xmlns + "GroupOfLine").GetStringValue(),
													SiteId = item.Element(xmlns + "SiteId").GetIntValue(),
													StopAreaNumber = item.Element(xmlns + "StopAreaNumber").GetIntValue(),
													TransporationMode = item.Element(xmlns + "TransportMode").GetStringValue(),
													TransportationType = type
												})
							.Where(item => !(item.LineNumber.StartsWith("9") && item.LineNumber.Length > 3)) //Fjärrtåg
							.ToList();
			if (departures.Count > 0)
			{
				OnDeparturesParsed(new DeparturesParsedEventArgs { Departures = departures, TransporationType = type });
			}
			return departures.Count > 0;
		}

		private void SubwayCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(e.Result))
				{
					var xDoc = XElement.Parse(e.Result);

					XNamespace xmlns = "http://www1.sl.se/realtidws/";

					var q = (from item in xDoc.Descendants(xmlns + "Metro")
									 select new
									 {
										 Row1 = new Departure
										 {
											 Destination = item.Element(xmlns + "DisplayRow1").Value.Trim(),
										 },
										 Row2 = ParseRowTwo(item.Element(xmlns + "DisplayRow2").Value)
																																.ToString()
																																.Replace("min.", "min,")
																																.Split(',')
																																.Select(row => new Departure
																																{
																																	Destination = row.Trim(),
																																}),
									 }).ToList();
					var departures = new List<Departure>();
					q.ForEach(item => departures.Add(item.Row1));
					q.ForEach(item => departures.AddRange(item.Row2));

					departures = departures.Where(item => !string.IsNullOrEmpty(item.Destination))
																	.Select(item => ParseSubwayRealTime(item.Destination))
																	.Where(item => !string.IsNullOrEmpty(item.LineNumber))
																	.Where(item => item != null)
																	.OrderBy(item => item.ExpectedDateTime)
																	.ToList();
					if (departures.Count > 0)
					{
						OnDeparturesParsed(new DeparturesParsedEventArgs { TransporationType = TransportationType.DpsMetro, Departures = departures });
					}
					else
					{
						OnNoResults(new EventArgs());
					}
				}
			}
			catch
			{
				OnLoadError(new EventArgs());
			}
		}

		private string ParseRowTwo(string rowTwo)
		{
			var startIndex = rowTwo.IndexOf(':');
			if (startIndex > 0 && startIndex < (rowTwo.Length - 1))
			{
				if (IsNumber(rowTwo[startIndex - 1]) && IsNumber(rowTwo[startIndex + 1]))
				{
					startIndex = rowTwo.IndexOf(' ', startIndex);
					if (startIndex >= 0)
					{
						rowTwo = rowTwo.Insert(startIndex, "min,");
					}
				}
			}
			return rowTwo;
		}

		private bool IsNumber(char c)
		{
			int number;
			return int.TryParse(c.ToString(), out number);
		}

		private Departure ParseSubwayRealTime(string data)
		{
			var subwayData = data;
			var realTime = new Departure();
			int lineNumber;
			if (int.TryParse(subwayData.Substring(0, 2), out lineNumber) && lineNumber > 0)
			{
				realTime.LineNumber = lineNumber.ToString();
				realTime.TransportationType = TransportationType.DpsMetro;
				subwayData = subwayData.Substring(3).Trim();
				subwayData = subwayData.Replace("min", "").TrimEnd(new char[] { '.', ',', ' ' });
				if (subwayData.LastIndexOf(' ') >= 0)
				{
					var timeData = subwayData.Substring(subwayData.LastIndexOf(' ')).Trim();
					if (timeData.Contains(':'))
					{
						realTime.DisplayTime = timeData;
						if (timeData.Split(':').Length > 1)
						{
							int hour;
							int minutes;
							if (int.TryParse(timeData.Split(':')[0].Trim(), out hour) && int.TryParse(timeData.Split(':')[1].Trim(), out minutes))
							{
								if (hour >= DateTime.Now.Hour)
								{
									realTime.ExpectedDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minutes, 00);
								}
								else
								{
									var baseDate = DateTime.Now.AddDays(1);
									realTime.ExpectedDateTime = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, hour, minutes, 00);
								}
							}
						}
					}
					else
					{
						int time;
						if (int.TryParse(timeData, out time))
						{
							realTime.DisplayTime = string.Format("{0} min", time);
							realTime.ExpectedDateTime = DateTime.Now.AddMinutes(time);
						}
						else
						{
							realTime.ExpectedDateTime = DateTime.Now;
							realTime.DisplayTime = "Nu";
						}
					}
					realTime.Destination = subwayData.Substring(0, subwayData.LastIndexOf(' ')).Trim();
				}
				else
				{
					realTime.Destination = subwayData.Trim();
					realTime.ExpectedDateTime = DateTime.Now;
					realTime.DisplayTime = "Nu";
				}
			}
			return realTime;
		}
	}
}