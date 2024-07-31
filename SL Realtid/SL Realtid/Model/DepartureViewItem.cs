using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SLRealtid.Model
{
	public class DepartureViewItem
	{
		public Departure Departure { get; set; }
		public string Destination { get; set; }
		public string TimeTableTime { get; set; }
		public string ExpectedTime { get; set; }
		public Visibility ShowExpected { get; set; }
		public int Direction { get; set; }
		public string GroupName { get; set; }
	}
}
