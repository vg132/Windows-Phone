using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trafikläget.Models
{
	public class TrafficInformation
	{
		public TrafficInformation()
		{
			TrafficType = new List<TrafficType>();
		}

		public IList<TrafficType> TrafficTypes { get; set; }
	}
}
