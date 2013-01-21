﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppHarbor.Web.Security;
using PowerAnalysis.Controllers;
using PowerAnalysis.Models;
using PowerAnalysis.ViewModels;

namespace AuthenticationExample.Web.Controllers
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
				var user = new User
				{
					Username = userInputModel.Username,
					Password = HashPassword(userInputModel.Password),
				};

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

		private static string HashPassword(string value)
		{
			string salt = BCrypt.Net.BCrypt.GenerateSalt();
			return BCrypt.Net.BCrypt.HashPassword(value, salt);
		}
	}
}