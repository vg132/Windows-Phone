using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace Lybeckeffekten.Model
{
	public class LybeckRSSDataContext : DataContext
	{
		private const string ConnectionString = "Data Source=isostore:/Lybeckeffekten.sdf";

		public LybeckRSSDataContext()
			: base(LybeckRSSDataContext.ConnectionString)
		{
			if (!DatabaseExists())
			{
				SetupDatabase();
			}
		}

		private void SetupDatabase()
		{
			CreateDatabase();
			Setup.DatabaseSetup1.SetupFeeds(this);
			SubmitChanges();
		}

		public Table<Feed> Feeds;
		public Table<FeedItem> FeedItems;
	}
}
