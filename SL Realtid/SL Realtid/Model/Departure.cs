using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid.Model
{
	public class Departure
	{
		public String Destination { get; set; }
		public DateTime TimeTabledDateTime { get; set; }
		public DateTime ExpectedDateTime { get; set; }
		public string LineNumber { get; set; }
		public String StopAreaName { get; set; }
		public String DisplayTime { get; set; }
		public int StopAreaNumber { get; set; }
		public int Direction { get; set; }
		public int SiteId { get; set; }
		public string TransporationMode { get; set; }
		public string LineName { get; set; }
		public TransportationType TransportationType { get; set; }
	}
}
