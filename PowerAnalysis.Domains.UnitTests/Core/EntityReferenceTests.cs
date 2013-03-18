using System;
using FluentAssertions;
using HDC.PowerAnalysis.Core;
using NUnit.Framework;

namespace PowerAnalysis.Domains.UnitTests.Core
{
	[TestFixture]
	public class EntityReferenceTests
	{
		[Test]
		public void EntityReference_Constructor_Has_Correctly_Initialized_State()
		{
			// Arrange
			const string id = "abc/123";
			const string description = "entityDescription";
			// Act
			EntityReference entityReference = new EntityReference(id, description);

			// Assert
			entityReference.ShouldBeEquivalentTo(new
										{
											Id = id,
											Description = description,
										});
		}

		[Test]
		[TestCase(null, "desc", typeof(ArgumentNullException), TestName = "id cannot be null")]
		[TestCase("", "desc", typeof(ArgumentException), TestName = "id cannot be empty")]
		[TestCase(" ", "desc", typeof(ArgumentException), TestName = "id cannot be whitespace")]
		[TestCase("abc/123", null, typeof(ArgumentNullException), TestName = "description cannot be null")]
		[TestCase("abc/123", "", typeof(ArgumentException), TestName = "description cannot be empty")]
		[TestCase("abc/123", " ", typeof(ArgumentException), TestName = "description cannot be whitespace")]
		public void Invalid_EntityReference_Cannot_Be_Created(string id, string description, Type exceptionType)
		{
			Assert.Throws(exceptionType, () => new EntityReference(id, description));
		}
	}
}