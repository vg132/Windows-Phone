using SLRealtid.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SLRealtid.ViewModel
{
	public class RealTimeViewModel
	{
		private RealTimeDataContext _dataContext = null;

		public RealTimeViewModel()
		{
			_dataContext = new RealTimeDataContext();
		}

		public List<Station> Stations { get; set; }

		public List<Favorite> Favorites { get; set; }

		public void AddFavorite(Favorite favorite)
		{
			_dataContext.Favorites.InsertOnSubmit(favorite);
			_dataContext.SubmitChanges();
			LoadCollectionsFromDatabase();
		}

		public void DeleteFavorite(Favorite favorite)
		{
			_dataContext.Favorites.DeleteOnSubmit(favorite);
			_dataContext.SubmitChanges();
			LoadCollectionsFromDatabase();
		}

		public void LoadCollectionsFromDatabase()
		{
			Stations = _dataContext.Stations.ToList().OrderBy(item => item.Name, StringComparer.Create(new CultureInfo("sv-SE"), true)).ToList();
			Favorites = _dataContext.Favorites.ToList().OrderBy(item => item.Name, StringComparer.Create(new CultureInfo("sv-SE"), true)).ToList();
		}
	}
}
