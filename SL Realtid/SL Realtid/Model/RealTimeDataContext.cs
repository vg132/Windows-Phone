using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace SLRealtid.Model
{
	public class RealTimeDataContext : DataContext
	{
		private const string ConnectionString = "Data Source=isostore:/SLRealtid.sdf";

		public RealTimeDataContext()
			: base(RealTimeDataContext.ConnectionString)
		{
			if (!DatabaseExists())
			{
				SetupDatabase();
			}
			else if (Settings.AppVersion != 2)
			{
				UpgradeDatabase();
			}
		}

		private void UpgradeDatabase()
		{
			Setup.DatabaseSetup2.UpdateStations(this);
			SubmitChanges();
			Settings.AppVersion = 2;
		}

		private void SetupDatabase()
		{
			CreateDatabase();
			Setup.DatabaseSetup1.SetupStations(this);
			SubmitChanges();
		}

		public Table<Station> Stations;
		public Table<Favorite> Favorites;
	}
}
