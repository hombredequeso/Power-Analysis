using System;
using System.Collections.Generic;
using System.Linq;
using HDC.PowerAnalysis.Core;
using HDC.PowerAnalysis.Security;

namespace HDC.PowerAnalysis.Mac.Queries
{
	public static class ChartQueryExtensions
	{
		public static IQueryable<Chart> SecurityFilter(this IQueryable<Chart> charts, EntityReference company, IEnumerable<string> roles)
		{
			if (charts == null) throw new ArgumentNullException("charts");

			if (roles != null && roles.Contains(Roles.SiteAdministrator))
				return charts;
			if (company == null)
				throw new ArgumentNullException("company");
			return charts.Where(x => x.Company.Id == company.Id);
		}
	}
}