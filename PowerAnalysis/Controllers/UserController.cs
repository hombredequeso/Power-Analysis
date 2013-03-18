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
		private readonly IExecutionContext _executionContext;

		public UserController(
			IAuthenticator authenticator,
			IDocumentSession session,
			IExecutionContext executionContext)
		{
			_authenticator = authenticator;
			_session = session;
			_executionContext = executionContext;
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
			var user = _session.Load<User>(_executionContext.UserId);
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
			var user = _session.Load<User>(_executionContext.UserId);
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

		[ChildActionOnly]
		[AllowAnonymous]
		public PartialViewResult LogOnWidget()
		{
			string userName = null;
			if (!string.IsNullOrWhiteSpace(_executionContext.UserId))
			{
			}
			var user = HttpContext.User;
			bool isLoggedOn = (user != null && user.Identity != null && user.Identity.IsAuthenticated);
			if (isLoggedOn)
			{
				userName = _session.Load<User>(_executionContext.UserId).Username;
			}

			var vm = new LogOnWidgetViewModel() {IsLoggedOn = isLoggedOn, Username = userName};
			return PartialView(vm);
		}
	}
}