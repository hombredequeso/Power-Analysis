using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppHarbor.Web.Security;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Web.ViewModels;
using Encryption = HDC.PowerAnalysis.Utility.Encryption;

namespace HDC.PowerAnalysis.Web.Controllers
{
	[AppHarbor.Web.RequireHttps]
	public class UserController : RavenController
	{
		private readonly IAuthenticator _authenticator;

		public UserController()
		{
			_authenticator = new CookieAuthenticator(
				new ConfigFileAuthenticationConfiguration(),
				new HttpContextWrapper(System.Web.HttpContext.Current));
		}

		[HttpGet]
		public ActionResult New()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(UserInputModel userInputModel)
		{
			if (RavenSession.Query<User>().Any(x => x.Username == userInputModel.Username))
			{
				ModelState.AddModelError("Username", "Username is already in use");
			}

			if (ModelState.IsValid)
			{
				var user = new User(userInputModel.Username, Encryption.HashPassword(userInputModel.Password), new string[0]);

				RavenSession.Store(user);

				//_authenticator.SetCookie(user.Username);
				_authenticator.SetCookie(user.Username, false, new string[] { "Administrator" });

				return RedirectToAction("Index", "Home");
			}

			return View("New", userInputModel);
		}

		//[HttpGet]
		//[Authorize(Roles = "Administrator")]
		//public ActionResult Show()
		//{
		//    var user = _repository.GetAll<User>().SingleOrDefault(x => x.Username == User.Identity.Name);
		//    if (user == null)
		//    {
		//        throw new HttpException(404, "Not found");
		//    }

		//    return View(user);
		//}
	}
}
