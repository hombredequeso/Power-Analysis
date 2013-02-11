using System.Linq;
using HDC.PowerAnalysis.Utility;
using Raven.Client;

namespace HDC.PowerAnalysis.Security
{
	public static class SecuritySeeder
	{
		private static string siteadministratorUsername = "mcheeseman";

		 public static void Run(IDocumentStore documentStore, string siteAdministratorPassword)
		 {
			 using (var session = documentStore.OpenSession())
			 {
				 if (!session.Query<User>().Any())
				 {
					 var masterAdministrator = new User(siteadministratorUsername,
														Encryption.HashPassword(siteAdministratorPassword),
														new[]
					                                   	{
					                                   		Roles.SiteAdministrator,
															Roles.Administrator
					                                   	},
														null);
					 session.Store(masterAdministrator);
					 session.SaveChanges();
				 }
			 }
		 }
	}
}