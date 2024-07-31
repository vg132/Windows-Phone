using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SLRealtid.Extensions;
using System.Net;

namespace SLRealtid.Parsers
{
	public class TrafficStatusParser
	{
		private static string Url = "https://api.trafiklab.se/sl/trafikenjustnu?key=PRIVATE_KEY";

		#region TrafficStatusParsed event

		private event EventHandler<TrafficStatusParsedEventArgs> _trafficStatusParsedEvent;

		public event EventHandler<TrafficStatusParsedEventArgs> TrafficStatusParsed
		{
			add { _trafficStatusParsedEvent += value; }
			remove { _trafficStatusParsedEvent -= value; }
		}

		private void OnTrafficStatusParsed(TrafficStatusParsedEventArgs e)
		{
			EventHandler<TrafficStatusParsedEventArgs> handler = _trafficStatusParsedEvent;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		public void ParseTrafficStatusAsync()
		{
			var client = new WebClient();
			client.DownloadStringCompleted += TrafficStatusCompleted;
			client.DownloadStringAsync(new Uri(TrafficStatusParser.Url));
		}

		public void TrafficStatusCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				if (e.Error==null && !string.IsNullOrEmpty(e.Result))
				{
					var xDoc = XElement.Parse(e.Result);

					XNamespace xmlns = "http://sl.se/TrafficStatus.xsd";
					var status = (from type in xDoc.Descendants(xmlns + "TrafficType")
												select new Model.TrafficStatus
												{
													Name = type.Attribute("Name").Value,
													Status = type.Attribute("TrafficStatus").Value,
													Expanded = bool.Parse(type.Attribute("Expanded").Value),
													HasPlannedEvents = bool.Parse(type.Attribute("HasPlannedEvent").Value),
													TrafficEvents = type.Descendants(xmlns + "TrafficEvent").Select(item => new Model.TrafficEvent
													{
														InfoUrl = item.Element(xmlns + "EventInfoURL").GetStringValue(),
														Line = item.Element(xmlns + "TrafficLine").GetStringValue(),
														Message = item.Element(xmlns + "Message").GetStringValue(),
														Status = item.Element(xmlns + "Status").GetStringValue(),
														Expanded = item.Element(xmlns + "Expanded").GetBooleanValue(),
														Planned = item.Element(xmlns + "Planned").GetBooleanValue(),

													}).ToList(),
												}).ToList();
					OnTrafficStatusParsed(new TrafficStatusParsedEventArgs(status));
				}
			}
			catch
			{
			}
		}
	}
}