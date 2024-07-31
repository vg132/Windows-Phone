using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Lybeckeffekten.Extensions;
using System.Windows;

namespace Lybeckeffekten.Parser
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
						Heading = GetHeading(item.Element("description").GetStringValue()),
						Content = GetContent(item.Element("description").GetStringValue()),
						FeedId = feed.Id
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

		private string GetHeading(string text)
		{
			var content = HttpUtility.HtmlDecode(text);
			if (!string.IsNullOrEmpty(content))
			{
				var start = content.IndexOf("<b>");
				var end = content.IndexOf("</b>");
				if (start != -1 && end != -1)
				{
					return content.Substring(start + 3, end - (start + 3));
				}
			}
			return text;
		}

		private string GetContent(string text)
		{
			var content = HttpUtility.HtmlDecode(text);
			if (!string.IsNullOrEmpty(content))
			{
				var start = content.IndexOf("<i>");
				var end = content.IndexOf("</i>");
				if (start != -1 && end != -1)
				{
					return content.Substring(start + 3, end - (start + 3));
				}
			}
			return text;
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