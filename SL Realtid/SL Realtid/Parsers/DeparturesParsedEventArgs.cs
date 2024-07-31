using SLRealtid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid.Parsers
{
	public class DeparturesParsedEventArgs : EventArgs
	{
		public DeparturesParsedEventArgs()
		{
		}

		public TransportationType TransporationType { get; set; }
		public List<Departure> Departures { get; set; }
	}
}
