using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trafikläget.Models;

namespace Trafikläget.ViewModels
{
	public class TrafficInformationViewModel : INotifyPropertyChanged
	{
		private TrafficInformation _trafficInformation = null;
		private IList<TrafficType> _trafficTypes = null;

		public TrafficInformationViewModel(TrafficInformation trafficInformation)
		{
			_trafficInformation = trafficInformation;
		}

		public IList<TrafficType> TrafficInformation
		{
			get
			{
				return _trafficTypes;
			}
			set
			{
				_trafficTypes = value;
				OnPropertyChanged("TrafficInformation");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if(PropertyChanged!=null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
