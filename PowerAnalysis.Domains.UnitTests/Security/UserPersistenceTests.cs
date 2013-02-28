using System;
using FluentAssertions;
using HDC.PowerAnalysis.Core;
using HDC.PowerAnalysis.Security;
using NUnit.Framework;
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

			Act(() => { _session.Store(user); });

			AssertThat(() =>
			           	{
			           		var retrievedUser = _session.Load<User>(user.Id);
			           		retrievedUser.ShouldBeEquivalentTo(user);
			           	});
		}

		[Test]
		public void New_User_Cannot_Be_Saved_With_Same_Username_As_Another_User()
		{
			string userName = "abc123";

			// Arrange
			Arrange(() =>
			        	{
			        		var originalUser = new User(userName,
			        		                            new RandomString().Build(),
			        		                            new[] {new RandomString().Build(), new RandomString().Build()},
			        		                            new EntityReference("company/123", "CompanyDescription"));

			        		_session.Store(originalUser);
			        	});

			// Act
			ActThrows<Exception>(() =>
			                     	{
			                     		var userWithSameName = new User(userName,
			                     		                                new RandomString().Build(),
			                     		                                new[] {new RandomString().Build(), new RandomString().Build()},
			                     		                                new EntityReference("company/123", "CompanyDescription"));

			                     		_session.Store(userWithSameName);
			                     	});

			// Assert
		}
	}
}