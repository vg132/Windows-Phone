using PolisenRSS.Model;
using PolisenRSS.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PolisenRSS.ViewModel
{
	public class PolisenRSSViewModel : INotifyPropertyChanged
	{
		private PolisenRSSDataContext _dataContext = null;

		public PolisenRSSViewModel()
		{
			_dataContext = new PolisenRSSDataContext();
			LoadCollectionsFromDatabase();
		}

		private ObservableCollection<Region> _regions = null;
		public ObservableCollection<Region> Regions
		{
			get { return _regions; }
			set { _regions = value; }
		}

		private ObservableCollection<Feed> _feeds = null;
		public ObservableCollection<Feed> Feeds
		{
			get { return _feeds; }
			set
			{
				_feeds = value;
				OnPropertyChanged("Feeds");
			}
		}

		public Dictionary<int, ObservableCollection<FeedItem>> _feedItems = new Dictionary<int, ObservableCollection<FeedItem>>();
		public ObservableCollection<FeedItem> GetFeedItems(int feedId)
		{
			if (!_feedItems.ContainsKey(feedId))
			{
				_feedItems[feedId] = new ObservableCollection<FeedItem>();
			}
			return _feedItems[feedId];
		}

		public void AddFeedItems(IEnumerable<FeedItem> feedItems)
		{
			var currentFeedItems = _dataContext.FeedItems;
			_dataContext.FeedItems.InsertAllOnSubmit(feedItems.Where(item => !currentFeedItems.Any(i => string.Compare(i.Url, item.Url, StringComparison.CurrentCultureIgnoreCase) == 0)));
			_dataContext.SubmitChanges();
			LoadCollectionsFromDatabase();
		}

		public void SaveChanges()
		{
			_dataContext.SubmitChanges();
		}

		public void LoadCollectionsFromDatabase()
		{
			Regions = new ObservableCollection<Region>(_dataContext.Regions.ToList().OrderBy(item => item.Name, StringComparer.Create(new CultureInfo("sv-SE"), true)));
			Feeds = new ObservableCollection<Feed>(_dataContext.Feeds);
			foreach (var feed in Feeds)
			{
				if (!_feedItems.ContainsKey(feed.Id))
				{
					_feedItems.Add(feed.Id, new ObservableCollection<FeedItem>());
				}

				var feedItems = _dataContext.FeedItems.ToList().Where(item => item.FeedId == feed.Id && Regions.Any(region => region.IsActive && region.Id == item.RegionId)).OrderByDescending(item => item.Date);
				_dataContext.FeedItems.DeleteAllOnSubmit(feedItems.Skip(100));
				_dataContext.SubmitChanges();

				_feedItems[feed.Id].Clear();
				foreach (var feedItem in feedItems)
				{
					_feedItems[feed.Id].Add(feedItem);
				}
				OnPropertyChanged("FeedItems");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
