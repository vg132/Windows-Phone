using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trafikläget.Models
{
	public class TrafficEvent
	{
		public string Message { get; set; }
		public string TrafficLine { get; set; }
		public bool Expanded { get; set; }
	}
}
