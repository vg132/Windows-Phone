using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Data.Linq;

namespace SLRealtid.Model
{
	[Table]
	public class Station : INotifyPropertyChanged, INotifyPropertyChanging, ISite
	{
		public Station()
		{
		}

		public Station(string name, int siteId, double latitude, double longitude, string alias, bool hasTrain, bool hasSubway, bool hasTram, bool hasBus)
		{
			Name = name;
			SiteId = siteId;
			Latitude = latitude;
			Longitude = longitude;
			Alias = alias;
			HasTrain = hasTrain;
			HasSubway = hasSubway;
			HasTram = hasTram;
			HasBus = hasBus;			
		}

		[Column(IsPrimaryKey=true, IsDbGenerated=true, CanBeNull=false)]
		public int Id { get; set; }

		[Column]
		public int SiteId { get; set; }

		[Column]
		public string Name { get; set; }

		[Column]
		public string Alias { get; set; }

		[Column]
		public double Latitude { get; set; }

		[Column]
		public double Longitude { get; set; }

		[Column(CanBeNull=true)]
		public DateTime? LastSearch { get; set; }

		[Column]
		public int Searches { get; set; }

		[Column]
		public bool HasBus { get; set; }

		[Column]
		public bool HasTrain { get; set; }

		[Column]
		public bool HasTram { get; set; }

		[Column]
		public bool HasSubway { get; set; }

		#region Events

		public event PropertyChangingEventHandler PropertyChanging;

		private void NotifyPropertyChanging(string propertyName)
		{
			if (PropertyChanging != null)
			{
				PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
	}
}