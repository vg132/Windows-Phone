using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Windows;

namespace Lybeckeffekten.Model
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
		public DateTime Date { get; set; }

		[Column]
		public DateTime Added { get; set; }

		[Column]
		public int FeedId { get; set; }

		[Column]
		public string Heading { get; set; }

		public Visibility ShowHeading { get { return string.IsNullOrEmpty(Heading) ? Visibility.Collapsed : Visibility.Visible; } }

		[Column]
		public string Content { get; set; }
	}
}
