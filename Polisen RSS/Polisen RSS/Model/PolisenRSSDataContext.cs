using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace PolisenRSS.Model
{
	public class PolisenRSSDataContext : DataContext
	{
		private const string ConnectionString = "Data Source=isostore:/PolisenRSS.sdf";

		public PolisenRSSDataContext()
			: base(PolisenRSSDataContext.ConnectionString)
		{
			if (!DatabaseExists())
			{
				SetupDatabase();
			}
		}

		private void SetupDatabase()
		{
			CreateDatabase();
			Setup.DatabaseSetup1.SetupRegions(this);
			Setup.DatabaseSetup1.SetupFeeds(this);
			SubmitChanges();
		}

		public Table<Feed> Feeds;
		public Table<FeedItem> FeedItems;
		public Table<Region> Regions;
	}
}
