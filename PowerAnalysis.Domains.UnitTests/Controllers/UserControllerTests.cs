using System.Linq;
using AppHarbor.Web.Security;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Web.Controllers;
using HDC.PowerAnalysis.Web.ViewModels;
using NSubstitute;
using NUnit.Framework;
using PowerAnalysis.Domains.UnitTests.TestInfrastructure;

namespace PowerAnalysis.Domains.UnitTests.Controllers
{
	[TestFixture]
	public class UserControllerTests : AAATestInfrastructure
	{
		[Test]
		public void Post_Create_AddsANewUserToTheDatabase()
		{
			var userViewModel = new UserInputModel()
			                               	{
			                               		Username = "test user name",
			                               		Password = "test password"
			                               	};

			Act(() =>
			    	{
			    		var controller = new UserController(Substitute.For<IAuthenticator>(), _session, null);
			    		controller.Create(userViewModel);
			    	});

			AssertThat(() =>
			           	{
			           		var newUser = _session.Query<User>()
			           			.SingleOrDefault(x => x.Username == userViewModel.Username);
			           		Assert.NotNull(newUser);
			           	});
		}
	}
}