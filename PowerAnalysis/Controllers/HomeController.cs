using System.Web.Mvc;

namespace PowerAnalysis.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Test Application";

			return View();
		}
		 
	}
}