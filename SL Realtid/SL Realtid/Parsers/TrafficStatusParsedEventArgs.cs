using SLRealtid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid.Parsers
{
	public class TrafficStatusParsedEventArgs : EventArgs
	{
		public TrafficStatusParsedEventArgs()
		{
		}

		public TrafficStatusParsedEventArgs(List<TrafficStatus> trafficStatus)
		{
			TrafficStatus = trafficStatus;
		}

		public List<TrafficStatus> TrafficStatus { get; set; }
	}
}
