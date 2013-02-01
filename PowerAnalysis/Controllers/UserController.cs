using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppHarbor.Web.Security;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Web.ViewModels;
using Encryption = HDC.PowerAnalysis.Utility.Encryption;

namespace HDC.PowerAnalysis.Web.Controllers
{
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
		[AllowAnonymous]
		public ActionResult New()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
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
				_authenticator.SetCookie(user.Username, false, user.Roles.ToArray());

				return RedirectToAction("Index", "Home");
			}

			return View("New", userInputModel);
		}

		[HttpGet]
		public ActionResult UpdatePassword()
		{
			string userName = User.Identity.Name;
			var user = RavenSession.Query<User>().FirstOrDefault(x => x.Username == userName);
			if (user != null)
			{
				UserUpdatePasswordModel model = new UserUpdatePasswordModel()
				{
					Username = user.Username
				};
				return View(model);
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public ActionResult UpdatePassword(UserUpdatePasswordModel model)
		{
			string userName = User.Identity.Name;
			var user = RavenSession.Query<User>().FirstOrDefault(x => x.Username == userName);
			if (!Encryption.Verify(model.OldPassword, user.Password))
			{
				ModelState.AddModelError("OldPassword", "Incorrect original password");
				return View("UpdatePassword", model);
			}

			if (model.NewPassword != model.ConfirmNewPassword)
			{
				ModelState.AddModelError("ConfirmNewPassword", "New password confirmation incorrect");
				return View("UpdatePassword", model);
			}

			user.ChangePassword(Encryption.HashPassword(model.NewPassword));
			return RedirectToAction("Index", "Home");
		}
	}
}
