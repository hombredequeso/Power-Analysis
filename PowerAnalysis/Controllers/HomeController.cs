using System.Web.Mvc;

namespace HDC.PowerAnalysis.Web.Controllers
{
	[AllowAnonymous]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}	 
	}
}