using System.Web;
using System.Web.Mvc;
using HDC.PowerAnalysis.Web.Attributes;

namespace PowerAnalysis
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new CustomAuthorizeAttribute());
			filters.Add(new AppHarbor.Web.RequireHttpsAttribute());
		}
	}
}