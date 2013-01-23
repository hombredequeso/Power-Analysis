using System.Web.Mvc;

namespace HDC.PowerAnalysis.Web.Controllers
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