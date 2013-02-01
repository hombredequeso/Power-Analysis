using System;
using System.Linq;
using System.Web.Mvc;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Utility;
using HDC.PowerAnalysis.Web.Attributes;
using HDC.PowerAnalysis.Web.ViewModels;

namespace HDC.PowerAnalysis.Web.Controllers
{
	[CustomAuthorize(Roles = "SiteAdministrator")]
	public class AdministrationController : RavenController
	{

		public ActionResult Index()
		{
			var users = this.RavenSession.Query<User>()
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
			var user = RavenSession.Query<User>().FirstOrDefault(x => x.Username == model.Username);
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
