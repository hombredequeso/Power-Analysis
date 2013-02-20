using FluentAssertions;
using HDC.PowerAnalysis.Core;
using HDC.PowerAnalysis.Security;
using NUnit.Framework;
using PowerAnalysis.Domains.UnitTests.Controllers;
using PowerAnalysis.Domains.UnitTests.TestInfrastructure;

namespace PowerAnalysis.Domains.UnitTests.Security
{
	[TestFixture]
	public class UserPersistenceTests : AAATestInfrastructure
	{
		[Test]
		public void User_Is_Correctly_Persisted_And_Retrieved()
		{
			User user = new User(
				new RandomString().Build(),
				new RandomString().Build(),
				new[] {new RandomString().Build(), new RandomString().Build()},
				new EntityReference("company/123", "CompanyDescription"));

			Act(() =>
			    	{
			    		_session.Store(user);
			    	});

			AssertThat(() =>
			           	{
			           		var retrievedUser = _session.Load<User>(user.Id);
			           		retrievedUser.ShouldBeEquivalentTo(user);
			           	});
		}
	}
}