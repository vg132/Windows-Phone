using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lybeckeffekten.Model.Setup
{
	public class DatabaseSetup1
	{
		public static void SetupFeeds(LybeckRSSDataContext dataContext)
		{
			dataContext.Feeds.InsertOnSubmit(new Feed { Name = "nyheter", Url = "http://www.lybeckeffekten.se/Quotes/Feed", IsActive = true, Code = "lybeck" });
		}
	}
}
