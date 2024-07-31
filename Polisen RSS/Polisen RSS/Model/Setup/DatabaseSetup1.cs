using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolisenRSS.Model.Setup
{
	public class DatabaseSetup1
	{
		public static void SetupRegions(PolisenRSSDataContext dataContext)
		{
			dataContext.Regions.InsertOnSubmit(new Region(0, "Alla län", true));
			dataContext.Regions.InsertOnSubmit(new Region(1, "Blekinge", true));
			dataContext.Regions.InsertOnSubmit(new Region(2, "Dalarna", true));
			dataContext.Regions.InsertOnSubmit(new Region(3, "Gotlands län", true));
			dataContext.Regions.InsertOnSubmit(new Region(4, "Gävleborg", true));
			dataContext.Regions.InsertOnSubmit(new Region(5, "Halland", true));
			dataContext.Regions.InsertOnSubmit(new Region(6, "Jämtland", true));
			dataContext.Regions.InsertOnSubmit(new Region(7, "Jönköpings län", true));
			dataContext.Regions.InsertOnSubmit(new Region(8, "Kalmar län", true));
			dataContext.Regions.InsertOnSubmit(new Region(9, "Kronoberg", true));
			dataContext.Regions.InsertOnSubmit(new Region(10, "Norrbotten", true));
			dataContext.Regions.InsertOnSubmit(new Region(11, "Skåne", true));
			dataContext.Regions.InsertOnSubmit(new Region(12, "Stockholms län", true));
			dataContext.Regions.InsertOnSubmit(new Region(13, "Södermanland", true));
			dataContext.Regions.InsertOnSubmit(new Region(14, "Uppsala län", true));
			dataContext.Regions.InsertOnSubmit(new Region(15, "Värmland", true));
			dataContext.Regions.InsertOnSubmit(new Region(16, "Västerbotten", true));
			dataContext.Regions.InsertOnSubmit(new Region(17, "Västernorrland", true));
			dataContext.Regions.InsertOnSubmit(new Region(18, "Västmanland", true));
			dataContext.Regions.InsertOnSubmit(new Region(19, "Västra Götaland", true));
			dataContext.Regions.InsertOnSubmit(new Region(20, "Örebro län", true));
			dataContext.Regions.InsertOnSubmit(new Region(21, "Östergötland", true));
		}

		public static void SetupFeeds(PolisenRSSDataContext dataContext)
		{
			dataContext.Feeds.InsertOnSubmit(new Feed { Name = "nyheter", Url = "http://www.polisen.se/rss-nyheter", IsActive = true, Code = "news" });
			dataContext.Feeds.InsertOnSubmit(new Feed { Name = "händelser", Url = "http://www.polisen.se/rss-handelser", IsActive = true, Code = "event" });
			dataContext.Feeds.InsertOnSubmit(new Feed { Name = "trafik", Url = "http://www.polisen.se/rss-trafikovervakning", IsActive = true, Code = "traffic" });
			dataContext.Feeds.InsertOnSubmit(new Feed { Name = "press", Url = "http://www.polisen.se/Aktuellt/RSS/Pressmeddelande-RSS-lank/", IsActive = true, Code = "press" });
			dataContext.Feeds.InsertOnSubmit(new Feed { Name = "publikationer", Url = "http://www.polisen.se/rss-publikationer", IsActive = true, Code = "publications" });
		}
	}
}
