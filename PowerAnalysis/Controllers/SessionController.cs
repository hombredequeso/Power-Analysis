﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppHarbor.Web.Security;
using PowerAnalysis.Models;
using PowerAnalysis.ViewModels;

namespace PowerAnalysis.Controllers
{
    public class SessionController : RavenController
    {
		private readonly IAuthenticator _authenticator;
		private const string errorMessage = "Invalid username or password";

		public SessionController()
		{
			_authenticator = new CookieAuthenticator(
				new ConfigFileAuthenticationConfiguration(),
				new HttpContextWrapper(System.Web.HttpContext.Current));
		}

		[HttpGet]
		public ActionResult New(string returnUrl)
		{
			return View(new SessionViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public ActionResult Create(SessionViewModel sessionViewModel)
		{
			User user = null;
			if (ModelState.IsValid)
			{
				user = RavenSession.Query<User>().SingleOrDefault(x => x.Username == sessionViewModel.Username);
				if (user == null)
				{
					ModelState.AddModelError(string.Empty, errorMessage);
				}
			}

			if (ModelState.IsValid)
			{
				if (!BCrypt.Net.BCrypt.Verify(sessionViewModel.Password, user.Password))
				{
					ModelState.AddModelError(string.Empty, errorMessage);
				}
			}

			if (ModelState.IsValid)
			{
				_authenticator.SetCookie(user.Username, false, new string[] { });
				var returnUrl = sessionViewModel.ReturnUrl;
				if (returnUrl != null)
				{
					Uri returnUri;
					if (Uri.TryCreate(returnUrl, UriKind.Relative, out returnUri))
					{
						return Redirect(sessionViewModel.ReturnUrl);
					}
				}

				return RedirectToAction("Index", "Home");
			}

			return View("New", sessionViewModel);
		}

		[HttpPost]
		public ActionResult Destroy()
		{
			_authenticator.SignOut();
			Session.Abandon();
			return RedirectToAction("Index", "Home");
		}
	}
}