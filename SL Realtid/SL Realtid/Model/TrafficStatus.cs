using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid.Model
{
	public class TrafficStatus
	{
		public TrafficStatus()
		{
			TrafficEvents = new List<TrafficEvent>();
		}

		public string Name { get; set; }
		public string Status { get; set; }
		public bool Expanded { get; set; }
		public bool HasPlannedEvents { get; set; }
		public bool HasEvents { get { return TrafficEvents.Count() > 0; } }
		public List<TrafficEvent> TrafficEvents { get; set; }
	}
}
