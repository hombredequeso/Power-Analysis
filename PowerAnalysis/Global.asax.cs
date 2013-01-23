using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PowerAnalysis;
using Raven.Client;
using Raven.Client.Document;

namespace HDC.PowerAnalysis.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			InitializeRavenDb();
		}

		public static IDocumentStore RavenStore;

		private void InitializeRavenDb()
		{
			RavenStore = new DocumentStore { ConnectionStringName = "RavenDB" };
			RavenStore.Initialize();

			// IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), RavenStore);
		}

	}
}