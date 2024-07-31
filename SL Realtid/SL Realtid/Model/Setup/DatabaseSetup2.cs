using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid.Model.Setup
{
	public class DatabaseSetup2
	{
		public static void UpdateStations(RealTimeDataContext dataContext)
		{
			dataContext.Stations.InsertOnSubmit(new Station("Uppsala centralstation", 6086, 59.858577, 17.646167, string.Empty, true, false, false, false));
			dataContext.Stations.InsertOnSubmit(new Station("Knivsta", 6091, 59.725709, 17.786736, string.Empty, true, false, false, false));
			dataContext.Stations.InsertOnSubmit(new Station("Arlanda C", 9511, 59.649758, 17.929194, string.Empty, true, false, false, true));
		}
	}
}
