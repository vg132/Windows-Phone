using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace SLRealtid.Model
{
	[Table]
	public class Favorite : ISite
	{
		[Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public int Id { get; set; }

		[Column]
		public int SiteId { get; set; }

		[Column]
		public string Name { get; set; }

		[Column]
		private string SerializedSiteFilter { get; set; }

		public Dictionary<TransportationType, Filter> SiteFilter
		{
			get
			{
				if (!string.IsNullOrEmpty(SerializedSiteFilter))
				{
					try
					{
						return JsonConvert.DeserializeObject<Dictionary<TransportationType, Filter>>(SerializedSiteFilter);
					}
					catch
					{
						SerializedSiteFilter = null;
					}
				}
				return null;
			}
			set 
			{
				try
				{
					SerializedSiteFilter = JsonConvert.SerializeObject(value);
				}
				catch
				{
					SerializedSiteFilter = null;
				}
			}
		}
	}
}