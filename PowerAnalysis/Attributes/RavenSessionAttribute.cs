using System;
using System.Web.Mvc;
using Raven.Client;
using StructureMap;

namespace HDC.PowerAnalysis.Web.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class RavenSessionAttribute : FilterAttribute, IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.IsChildAction)
				return;

			var RavenSession = ObjectFactory.GetInstance<IDocumentSession>();
			if (filterContext.Exception != null)
				return;

			if (RavenSession != null)
				RavenSession.SaveChanges();
		}
	}
}