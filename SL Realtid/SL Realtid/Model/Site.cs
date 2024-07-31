using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace SLRealtid.Model
{
	public class Site : ISite
	{
		public Site()
		{
		}

		public Site(String name, int siteId)
		{
			Name = name;
			SiteId = siteId;
		}
	
		public int SiteId { get; set; }
		public string Name { get; set; }
	}
}
