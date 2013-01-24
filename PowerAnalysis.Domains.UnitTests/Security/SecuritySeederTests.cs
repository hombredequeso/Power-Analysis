using FluentAssertions;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Utility;
using NUnit.Framework;
using Raven.Client.Embedded;

namespace PowerAnalysis.Domains.UnitTests.Security
{
	[TestFixture]
	public class SecuritySeederTests
	{
		[Test]
		public void If_Site_Administrator_Does_Not_Exist_It_Is_Created()
		{
			// Arrange
			using (var dataStore = new EmbeddableDocumentStore {RunInMemory = true}.Initialize())
			{
				// Act
				var siteAdministratorPassword = "ThisIsATestPassword";
				SecuritySeeder.Run(dataStore, siteAdministratorPassword);

				// Assert
				using (var session = dataStore.OpenSession())
				{
					var masterAdministrator = session.Load<User>("users/mcheeseman");

					Assert.NotNull(masterAdministrator);
					masterAdministrator.Roles.Should().Contain("siteadministrator","administrator");
					Assert.IsTrue(Encryption.Verify(siteAdministratorPassword, masterAdministrator.Password));
				}
			}
		}
	}
}