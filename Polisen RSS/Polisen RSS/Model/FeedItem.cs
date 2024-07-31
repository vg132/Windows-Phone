using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace PolisenRSS.Model
{
	[Table]
	public class FeedItem : BaseModel
	{
		[Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public int Id { get; set; }

		[Column]
		public string Title { get; set; }

		[Column]
		public string Description { get; set; }

		[Column]
		public string Url { get; set; }

		[Column]
		public bool Read { get; set; }

		[Column]
		public DateTime Date { get; set; }

		[Column]
		public DateTime Added { get; set; }

		[Column]
		public int FeedId { get; set; }

		[Column]
		public int RegionId { get; set; }

		public bool IsActive
		{
			get { return App.ViewModel.Regions.Any(item => item.Id == RegionId); }
		}
	}
}
