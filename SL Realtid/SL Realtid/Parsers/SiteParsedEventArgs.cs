using SLRealtid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid.Parsers
{
	public class SiteParsedEventArgs : EventArgs
	{
		public SiteParsedEventArgs()
		{
		}

		public List<ISite> Sites { get; set; }
	}
}
