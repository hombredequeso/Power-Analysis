using FluentAssertions;
using HDC.PowerAnalysis.Security;
using NUnit.Framework;

namespace PowerAnalysis.Domains.UnitTests.Security
{
	[TestFixture]
	public class CompanyTests
	{
		[Test]
		public void Constructor_Correctly_Initializes_Object_State()
		{
			// Arrange
			const string description = "companyDescription";

			// Act
			Company company = new Company(description);
			
			// Assert
			company.ShouldBeEquivalentTo(new
			{
				Id = (string)null,
				Description = description
			});
		}
	}
}