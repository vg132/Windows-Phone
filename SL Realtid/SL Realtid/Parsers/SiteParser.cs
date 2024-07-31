using SLRealtid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using SLRealtid.Extensions;

namespace SLRealtid.Parsers
{
	public class SiteParser
	{
		private const string SiteSearchUrlFormat = "https://api.trafiklab.se/sl/realtid/GetSite?key=PRIVATE_KEY&stationSearch={0}";

		public void SiteSearchAsync(string query)
		{
			var client = new WebClient();
			client.DownloadStringCompleted += DownloadSiteSearchCompleted;
			client.DownloadStringAsync(new Uri(string.Format(SiteParser.SiteSearchUrlFormat, query)));
		}

		public void DownloadSiteSearchCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				if (e.Error==null && !string.IsNullOrEmpty(e.Result))
				{
					var xDoc = XElement.Parse(e.Result);
					XNamespace xmlns = "http://www1.sl.se/realtidws/";

					var sites = (from item in xDoc.Descendants(xmlns + "Site")
											 select new Site
											 {
												 Name = item.Element(xmlns + "Name").GetStringValue(),
												 SiteId = item.Element(xmlns + "Number").GetIntValue()
											 })
											 .Where(site => site.Name.Length > 4 || site.Name != site.Name.ToUpper())
											 .Cast<ISite>()
											.ToList();
					OnSiteSearchParsed(new SiteParsedEventArgs { Sites = sites });
				}
			}
			catch
			{
			}
		}

		#region SiteSearchParsed event

		private event EventHandler<SiteParsedEventArgs> _myEvent;

		public event EventHandler<SiteParsedEventArgs> SiteSearchParsed
		{
			add { _myEvent += value; }
			remove { _myEvent -= value; }
		}

		private void OnSiteSearchParsed(SiteParsedEventArgs e)
		{
			EventHandler<SiteParsedEventArgs> handler = _myEvent;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion
	}
}
