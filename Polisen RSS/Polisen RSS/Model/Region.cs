using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace PolisenRSS.Model
{
	[Table]
	public class Region : BaseModel
	{
		private bool _isActive = false;

		public Region()
		{
		}

		public Region(int id, string name, bool isActive)
		{
			Id = id;
			Name = name;
			IsActive = isActive;
		}

		[Column(IsPrimaryKey = true, IsDbGenerated = false, CanBeNull = false)]
		public int Id { get; set; }
		[Column]
		public string Name { get; set; }
		[Column]
		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				OnPropertyChanging("IsActive");
				_isActive = value;
				OnPropertyChanged("IsActive");
			}
		}
		private bool _isEnabled;
		public bool IsEnabled
		{
			get { return Id == 0 ? true : _isEnabled; }
			set
			{
				OnPropertyChanging("IsEnabled");
				_isEnabled = value;
				OnPropertyChanged("IsEnabled");
			}
		}
	}
}
