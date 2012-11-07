using System;
using System.Web.Mvc;

namespace PowerAnalysis.Controllers
{
	public class TestController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Test Application";

			return View();
		}

		public ActionResult Uncaughtexception(int id)
		{
			throw new Exception("This is an uncaught exception.");
		}

		[HttpPost]
		public ActionResult DoError()
		{
			throw new Exception("This is a post exception");
		}
	}
}