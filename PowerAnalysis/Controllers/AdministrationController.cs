using System;
using System.Linq;
using System.Web.Mvc;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Utility;
using HDC.PowerAnalysis.Web.Attributes;
using HDC.PowerAnalysis.Web.ViewModels;
using Raven.Client;

namespace HDC.PowerAnalysis.Web.Controllers
{
	[CustomAuthorize(Roles = "SiteAdministrator")]
	public class AdministrationController : Controller
	{
		private readonly IDocumentSession _session;

		public AdministrationController(IDocumentSession session)
		{
			_session = session;
		}


		public ActionResult Index()
		{
			var users = _session.Query<User>()
				.ToList()
				.Select(x => new UserAdminViewModel(x.Username));
			return View(users);
		}

		public ActionResult ResetPassword(string userName)
		{
			var model = new ResetUserPasswordViewModel(){Username = userName};
			return View(model);
		}

		[HttpPost]
		public ActionResult ResetPassword(ResetUserPasswordViewModel model)
		{
			var user = _session.Query<User>().FirstOrDefault(x => x.Username == model.Username);
			if (model.NewPassword != model.ConfirmNewPassword)
			{
				ModelState.AddModelError("ConfirmNewPassword", "New password confirmation incorrect");
				return View("ResetPassword", model);
			}

			user.ChangePassword(Encryption.HashPassword(model.NewPassword));
			return RedirectToAction("Index", "Administration");
		}
	}
}
