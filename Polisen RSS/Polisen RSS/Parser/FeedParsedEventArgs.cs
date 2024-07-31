using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolisenRSS.Parser
{
	public class FeedParsedEventArgs : EventArgs
	{
		public FeedParsedEventArgs()
		{
		}

		public Model.Feed Feed { get; set; }
		public List<Model.FeedItem> FeedItems { get; set; }
	}
}
