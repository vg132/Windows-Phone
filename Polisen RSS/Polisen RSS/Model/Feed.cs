using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace PolisenRSS.Model
{
	[Table]
	public class Feed : BaseModel
	{
		private bool _isActive;
	
		[Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public int Id { get; set; }

		[Column]
		public string Name { get; set; }

		[Column]
		public string Url { get; set; }

		[Column]
		public string Code { get; set; }

		[Column]
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				OnPropertyChanging("IsActive");
				_isActive = value;
				OnPropertyChanged("IsActive");
			}
		}
	}
}