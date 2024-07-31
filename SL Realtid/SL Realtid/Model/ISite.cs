using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLRealtid.Model
{
	public interface ISite
	{
		string Name { get; set; }
		int SiteId { get; set; }
	}
}
