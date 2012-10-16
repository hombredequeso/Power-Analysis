using System.Web.Mvc;
using Raven.Client;

namespace PowerAnalysis.Controllers
{
	public abstract class RavenController : Controller
	{
		protected IDocumentSession RavenSession { get; set; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			RavenSession = MvcApplication.RavenStore.OpenSession();
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.IsChildAction)
				return;

			using (RavenSession)
			{
				if (filterContext.Exception != null)
					return;

				if (RavenSession != null)
					RavenSession.SaveChanges();
			}
		}
	}
}