using FluentAssertions;
using HDC.PowerAnalysis.Security;
using NUnit.Framework;
using Raven.Client.Embedded;

namespace PowerAnalysis.Domains.UnitTests.Security
{
	[TestFixture]
	public class UserPersistenceTests
	{
		[Test]
		public void User_Is_Correctly_Persisted_And_Retrieved()
		{
			using (var dataStore = new EmbeddableDocumentStore { RunInMemory = true }.Initialize())
			{
				// Arrange
				User user = new User(
					new RandomString().Build(),
					new RandomString().Build(),
					new[] {new RandomString().Build(), new RandomString().Build()});

				// Act
				using (var session = dataStore.OpenSession())
				{
					session.Store(user);
					session.SaveChanges();
				}

				// Assert
				using (var session = dataStore.OpenSession())
				{
					var retrievedUser = session.Load<User>(user.Id);
					retrievedUser.ShouldBeEquivalentTo(user);
				}
			}
		}

	}
}