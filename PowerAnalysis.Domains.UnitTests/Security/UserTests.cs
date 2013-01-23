﻿using System;
using FluentAssertions;
using HDC.PowerAnalysis.Security;
using NUnit.Framework;

namespace PowerAnalysis.Domains.UnitTests.Security
{
	[TestFixture]
	public class UserTests
	{
		[Test]
		public void Created_User_Has_Correct_State()
		{
			// Arrange
			string userName = new RandomString().Build();
			string password = new RandomString().Build();
			string[] roles = new string[]
			                 	{
			                 		new RandomString().Build(),
			                 		new RandomString().Build()
			                 	};

			// Act
			User user = new User(userName, password, roles);

			// Assert
			user.ShouldBeEquivalentTo(new
										{
											Id = (string)null,
											Username = userName,
											Password = password,
											Roles = roles
										});
		}

		[Test]
		[TestCase("userName", null, typeof(ArgumentNullException), TestName="Password cannot be null")]
		[TestCase(null, "password", typeof(ArgumentNullException), TestName = "UserName cannot be null")]
		[TestCase("ab", "password", typeof(ArgumentException), TestName = "UserName must be at least 3 chars long")]
		[TestCase("userName", "123456789", typeof(ArgumentException), TestName = "Password must have at least 10 chars long")]
		public void Invalid_User_Cannot_Be_Created(string userName, string password, Type exceptionType)
		{
			Assert.Throws(exceptionType, () => new User(userName, password, new string[0]));
		}
	}

	public class RandomString
	{
		public string Build()
		{
			return Guid.NewGuid().ToString();
		}
	}
}