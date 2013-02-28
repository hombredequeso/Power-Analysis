using System.Linq;
using HDC.PowerAnalysis.Mac;

namespace HDC.PowerAnalysis.Security
{
	public static class UserSecurityAccessExtensions
	{
		 public static bool HasAccessTo(this User user, Chart chart)
		 {
		 	return user.Roles.Contains(Roles.SiteAdministrator)
		 	       || user.Company == chart.Company;
		 }
	}
}