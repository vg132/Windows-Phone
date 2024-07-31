using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using PolisenRSS.Extensions;
using System.Windows;

namespace PolisenRSS.Parser
{
	public class FeedParser
	{
		public void ParseFeedAsync(Model.Feed feed)
		{
			var client = new WebClient();
			client.DownloadStringCompleted += FeedParser_DownloadStringCompleted;
			client.DownloadStringAsync(new Uri(feed.Url), feed);
		}

		private void FeedParser_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			if (e.Error == null && !string.IsNullOrEmpty(e.Result))
			{
				var feed = e.UserState as Model.Feed;
				if (feed != null)
				{
					var feedItems = XElement.Parse(e.Result).Descendants("item").Select(item => new Model.FeedItem
					{
						Added = DateTime.Now,
						Description = item.Element("description").GetStringValue(),
						Title = FixTitle(item.Element("title").GetStringValue()),
						Url = item.Element("link").GetStringValue().Trim(),
						Date = item.Element("pubDate").GetDateTimeValue(),
						Read = false,
						FeedId = feed.Id,
						RegionId = GetRegionId(item.Element("link").GetStringValue().Trim().ToLower())
					});
					Deployment.Current.Dispatcher.BeginInvoke(() =>
					{
						App.ViewModel.AddFeedItems(feedItems);
					});
					OnFeedParsed(new FeedParsedEventArgs { Feed = feed, FeedItems = feedItems.ToList() });
				}
			}
		}

		private string FixTitle(string title)
		{
			if (string.IsNullOrEmpty(title) || !title.Contains(","))
			{
				return title;
			}
			try
			{
				DateTime titleDate;
				if (DateTime.TryParse(title.Substring(0, title.IndexOf(',')).Trim(), out titleDate))
				{
					return title.Substring(title.IndexOf(',') + 1).Trim();
				}
			}
			catch { }
			return title;
		}

		private int GetRegionId(string url)
		{
			if (url.Contains("blekinge"))
			{
				return 1;
			}
			else if (url.Contains("dalarna"))
			{
				return 2;
			}
			else if (url.Contains("gotland"))
			{
				return 3;
			}
			else if (url.Contains("gavleborg"))
			{
				return 4;
			}
			else if (url.Contains("halland"))
			{
				return 5;
			}
			else if (url.Contains("jamtland"))
			{
				return 6;
			}
			else if (url.Contains("jonkoping"))
			{
				return 7;
			}
			else if (url.Contains("kalmar"))
			{
				return 8;
			}
			else if (url.Contains("kronoberg"))
			{
				return 9;
			}
			else if (url.Contains("norrbotten"))
			{
				return 10;
			}
			else if (url.Contains("skane"))
			{
				return 11;
			}
			else if (url.Contains("stockholm"))
			{
				return 12;
			}
			else if (url.Contains("sodermanland"))
			{
				return 13;
			}
			else if (url.Contains("uppsala"))
			{
				return 14;
			}
			else if (url.Contains("varmland"))
			{
				return 15;
			}
			else if (url.Contains("vasterbotten"))
			{
				return 16;
			}
			else if (url.Contains("vasternorrland"))
			{
				return 17;
			}
			else if (url.Contains("vastmanland"))
			{
				return 18;
			}
			else if (url.Contains("vastra-gotaland"))
			{
				return 19;
			}
			else if (url.Contains("orebro"))
			{
				return 20;
			}
			else if (url.Contains("ostergotland"))
			{
				return 21;
			}
			return 0;
		}

		#region FeedParsed event

		private event EventHandler<FeedParsedEventArgs> _feedParsed;

		public event EventHandler<FeedParsedEventArgs> FeedParsed
		{
			add { _feedParsed += value; }
			remove { _feedParsed -= value; }
		}

		private void OnFeedParsed(FeedParsedEventArgs e)
		{
			EventHandler<FeedParsedEventArgs> handler = _feedParsed;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion
	}
}