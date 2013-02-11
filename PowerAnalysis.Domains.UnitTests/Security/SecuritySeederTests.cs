using System.Linq;
using FluentAssertions;
using HDC.PowerAnalysis.Core;
using HDC.PowerAnalysis.Security;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace PowerAnalysis.Domains.UnitTests.Security
{
	[TestFixture]
	public class SecuritySeederTests
	{
		[Test]
		public void If_There_Are_No_Users_A_Site_Administrator_Is_Created()
		{
			// Arrange
			using (var dataStore = new EmbeddableDocumentStore {RunInMemory = true}.Initialize())
			{
				dataStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
				// Act
				var siteAdministratorPassword = "ThisIsATestPassword";
				SecuritySeeder.Run(dataStore, siteAdministratorPassword);

				// Assert
				using (var session = dataStore.OpenSession())
				{
					var masterAdministrator = session.Query<User>().SingleOrDefault();

					Assert.NotNull(masterAdministrator);

					masterAdministrator.ShouldBeEquivalentTo(new
					{
						Id = "users/1",
						Username = "mcheeseman",
						Roles = new[]{"siteadministrator", "administrator"},
						Company = (EntityReference)null
					},
					options => options.Excluding(x => x.Password));
				}
			}
		}
	}
}