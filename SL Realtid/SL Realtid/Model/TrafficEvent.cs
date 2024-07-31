using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid.Model
{
	public class TrafficEvent
	{
		public string Message { get; set; }
		public string InfoUrl { get; set; }
		public string Status { get; set; }
		public string Line { get; set; }
		public bool Expanded { get; set; }
		public bool Planned { get; set; }

		public string DisplayMessage
		{
			get
			{
				if (!string.IsNullOrEmpty(Line))
				{
					return string.Format("{0} - {1}", Line, Message);
				}
				return Message;
			}
		}
	}
}
