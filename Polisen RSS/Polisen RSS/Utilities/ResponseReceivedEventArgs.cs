using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolisenRSS.Utilities
{
	public class ResponseReceivedEventArgs : EventArgs
	{
		public string Response { get; set; }
	}
}
