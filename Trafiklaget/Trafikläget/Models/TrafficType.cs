using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trafikläget.Models
{
	public class TrafficType
	{
		public TrafficType()
		{
			Events = new List<TrafficEvent>();
		}

		public string Name { get; set; }
		public string Status { get; set; }
		public IList<TrafficEvent> Events { get; set; }
	}
}
