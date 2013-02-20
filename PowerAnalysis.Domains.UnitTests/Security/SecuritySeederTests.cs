using System.Linq;
using FluentAssertions;
using HDC.PowerAnalysis.Core;
using HDC.PowerAnalysis.Security;
using NUnit.Framework;
using PowerAnalysis.Domains.UnitTests.Controllers;
using PowerAnalysis.Domains.UnitTests.TestInfrastructure;

namespace PowerAnalysis.Domains.UnitTests.Security
{
	[TestFixture]
	public class SecuritySeederTests : AAATestInfrastructure
	{
		[Test]
		public void If_There_Are_No_Users_A_Site_Administrator_Is_Created()
		{
			var siteAdministratorPassword = "ThisIsATestPassword";
			// Act
				Act(() => {
					SecuritySeeder.Run(_store, siteAdministratorPassword);
				});
				// Assert
			AssertThat(() => {
					var masterAdministrator = _session.Query<User>().SingleOrDefault();

					Assert.NotNull(masterAdministrator);

					masterAdministrator.ShouldBeEquivalentTo(new
					{
						Id = "users/1",
						Username = "mcheeseman",
						Roles = new[]{"siteadministrator", "administrator"},
						Company = (EntityReference)null
					},
					options => options.Excluding(x => x.Password));
			});
		}
	}
}