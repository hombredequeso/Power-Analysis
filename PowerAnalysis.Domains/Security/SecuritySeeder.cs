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
				 if (session.Load<User>("users/" + siteadministratorUsername) == null)
				 {
					 var masterAdministrator = new User(siteadministratorUsername,
														Encryption.HashPassword(siteAdministratorPassword),
														new[]
					                                   	{
					                                   		Roles.SiteAdministrator,
															Roles.Administrator
					                                   	});
					 session.Store(masterAdministrator);
					 session.SaveChanges();
				 }
			 }
		 }
	}
}