using System.Linq;
using System.Web.Mvc;
using AppHarbor.Web.Security;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Web.ViewModels;
using Raven.Client;
using Encryption = HDC.PowerAnalysis.Utility.Encryption;

namespace HDC.PowerAnalysis.Web.Controllers
{
	public class UserController : Controller
	{
		private readonly IAuthenticator _authenticator;
		private readonly IDocumentSession _session;

		public UserController(IAuthenticator authenticator, IDocumentSession session)
		{
			_authenticator = authenticator;
			_session = session;
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
			if (_session.Query<User>().Any(x => x.Username == userInputModel.Username))
			{
				ModelState.AddModelError("Username", "Username is already in use");
			}

			if (ModelState.IsValid)
			{
				var company = new Company(userInputModel.Username + " Co.");
				_session.Store(company);

				var user = new User(
					userInputModel.Username, 
					Encryption.HashPassword(userInputModel.Password), 
					new string[0], 
					company);

				_session.Store(user);

				_authenticator.SetCookie(user.Id, false, user.Roles.ToArray());

				return RedirectToAction("Index", "Home");
			}

			return View("New", userInputModel);
		}

		[HttpGet]
		public ActionResult UpdatePassword()
		{
			string userName = User.Identity.Name;
			var user = _session.Query<User>().FirstOrDefault(x => x.Username == userName);
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
			var user = _session.Query<User>().FirstOrDefault(x => x.Username == userName);
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
