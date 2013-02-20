using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Utility;
using HDC.PowerAnalysis.Web.Attributes;
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

			GlobalFilters.Filters.Add(new RavenSessionAttribute());
			InitializeRavenDb();
			string defaultSiteAdministratorPassword = ConfigurationManager.AppSettings["DefaultSiteAdministratorPassword"];
			if (string.IsNullOrWhiteSpace(defaultSiteAdministratorPassword))
				defaultSiteAdministratorPassword = "defaultSiteAdministratorPassword";
			SecuritySeeder.Run(RavenStore, defaultSiteAdministratorPassword);
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