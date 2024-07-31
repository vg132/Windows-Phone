using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid.Model
{
	public class Filter
	{
		public Filter()
		{
			Show = true;
			HiddenLines = new List<string>();
			HiddenDestinations = new List<string>();
		}

		public bool Show { get; set; }
		public List<string> HiddenLines { get; set; }
		public List<string> HiddenDestinations { get; set; }
	}
}
